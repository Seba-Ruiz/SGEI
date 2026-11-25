using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using login_v6.Hubs;
using login_v6.Models;
using Microsoft.AspNet.SignalR;
using Negocio.DTO;
using Negocio.Servicio;

namespace login_v6.Servicio
{
    /// <summary>
    /// Punto de entrada para avisarle algo a un usuario. Para cualquier notificación del sistema:
    /// <code>
    /// Notificador.Enviar(new NotificacionNueva { UsuarioId = ..., OrigenUsuarioId = ..., Tipo = TiposNotificacion.X,
    ///     Titulo = "...", Mensaje = "...", Url = "~/Controlador/Accion/1", TextoBoton = "Ver ..." }.Dato("Campo", "valor"));
    /// </code>
    /// 1. La guarda en la base (tabla notificacion), con fecha y hora.
    /// 2. La manda en vivo a la campana del destinatario (SignalR, <see cref="NotificacionesHub"/>), en todas sus pestañas.
    /// 3. Si en Web.config SendMail = true, le manda un mail (en segundo plano, sin demorar la pantalla). Si el mail falla,
    ///    la notificación queda igual y el motivo se guarda en notificacion.error_mail; el usuario no ve ningún error.
    /// Nunca lanza excepciones: un problema al notificar no debe impedir la operación que la generó.
    /// </summary>
    public static class Notificador
    {
        /// <summary>Web.config: &lt;add key="SendMail" value="true" /&gt;</summary>
        public static bool EnviaMails
        {
            get
            {
                bool valor;
                return bool.TryParse(ConfigurationManager.AppSettings["SendMail"], out valor) && valor;
            }
        }

        /// <summary>Guarda, avisa en vivo y manda el mail. Devuelve la notificación guardada, o null si no se pudo.</summary>
        public static DTO_Notificacion Enviar(NotificacionNueva n)
        {
            DTO_Notificacion guardada;
            var servicio = new Notificaciones();
            try
            {
                guardada = servicio.Crear(n);
                if (guardada == null) { return null; }
            }
            catch (Exception ex)
            {
                Trace.TraceError("No se pudo guardar la notificación {0} para {1}: {2}", n == null ? null : n.Tipo, n == null ? null : n.UsuarioId, ex);
                return null;
            }

            try
            {
                Hub().Clients.User(n.UsuarioId).nueva(ParaCampana(guardada), servicio.CantidadSinLeer(n.UsuarioId));
            }
            catch (Exception ex)
            {
                Trace.TraceError("No se pudo avisar en vivo la notificación {0}: {1}", guardada.Id, ex);
            }

            if (EnviaMails) { ProgramarMail(n, guardada); }
            return guardada;
        }

        /// <summary>Actualiza el número de la campana en todas las pestañas abiertas del usuario.</summary>
        public static void AvisarCantidad(string usuarioId)
        {
            try
            {
                Hub().Clients.User(usuarioId).cantidad(new Notificaciones().CantidadSinLeer(usuarioId));
            }
            catch (Exception ex)
            {
                Trace.TraceError("No se pudo actualizar la campana de {0}: {1}", usuarioId, ex);
            }
        }

        /// <summary>Datos de una notificación para la campana (Scripts/app/notificaciones.js).</summary>
        public static object ParaCampana(DTO_Notificacion n)
        {
            return new
            {
                id = n.Id,
                titulo = n.Titulo,
                mensaje = n.Mensaje,
                origen = n.Origen,
                iniciales = n.OrigenIniciales,
                icono = n.Icono,
                fecha = n.Fecha.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                fechaTexto = n.Fecha.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                url = UrlDeAbrir(n.Id)
            };
        }

        /// <summary>Abrir una notificación la marca como leída y lleva a su pantalla.</summary>
        public static string UrlDeAbrir(int id)
        {
            return VirtualPathUtility.ToAbsolute("~/Notificaciones/Abrir/" + id);
        }

        /// <summary>"~/Tickets/Ver/15" → "/SGEI/Tickets/Ver/15". Solo rutas dentro del sistema.</summary>
        public static string ResolverUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) { return null; }
            url = url.Trim();
            if (url.StartsWith("~/", StringComparison.Ordinal)) { return VirtualPathUtility.ToAbsolute(url); }
            if (url.StartsWith("/", StringComparison.Ordinal) && !url.StartsWith("//", StringComparison.Ordinal)) { return url; }
            return null;
        }

        // ------------------------------------------------------------------ Mail

        private static void ProgramarMail(NotificacionNueva n, DTO_Notificacion guardada)
        {
            // La dirección del sistema se toma ahora, mientras hay un pedido HTTP; el mail sale después, en segundo plano.
            var raiz = RaizDelSistema();
            var urlBoton = raiz == null ? null : raiz + UrlDeAbrir(guardada.Id);
            var urlTodas = raiz == null ? null : raiz + VirtualPathUtility.ToAbsolute("~/Notificaciones/Index");
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(ct => EnviarMail(n, guardada, urlBoton, urlTodas));
            }
            catch (Exception ex)
            {
                // Fuera de ASP.NET (por ejemplo, en una prueba) no hay cola en segundo plano.
                Trace.TraceError("No se pudo programar el mail de la notificación {0}: {1}", guardada.Id, ex);
                RegistrarMail(guardada.Id, false, "No se pudo programar el envío: " + ex.Message);
            }
        }

        private static async Task EnviarMail(NotificacionNueva n, DTO_Notificacion guardada, string urlBoton, string urlTodas)
        {
            string error;
            try
            {
                var destinatario = new Notificaciones().Destinatario(n.UsuarioId);
                if (destinatario == null || string.IsNullOrEmpty(destinatario.Email))
                {
                    error = "El usuario no tiene un mail cargado.";
                }
                else
                {
                    var mail = new MailNotificacion(n, guardada, destinatario.Nombre, urlBoton, urlTodas);
                    error = await new MailSender().Enviar(destinatario.Email, mail.Asunto, mail.Html, mail.TextoPlano);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error al armar o enviar el mail de la notificación {0}: {1}", guardada.Id, ex);
                error = ex.Message;
            }
            RegistrarMail(guardada.Id, error == null, error);
        }

        private static void RegistrarMail(int id, bool enviado, string error)
        {
            try
            {
                new Notificaciones().RegistrarMail(id, enviado, error);
            }
            catch (Exception ex)
            {
                Trace.TraceError("No se pudo registrar el resultado del mail de la notificación {0}: {1}", id, ex);
            }
        }

        /// <summary>
        /// "http://servidor" (sin barra final) para armar los links del mail. Web.config puede fijarla con la clave
        /// UrlSistema, solo protocolo y servidor, sin la carpeta de la aplicación (por ejemplo "http://192.168.10.198");
        /// si no está, se usa la del pedido actual.
        /// </summary>
        private static string RaizDelSistema()
        {
            var configurada = ConfigurationManager.AppSettings["UrlSistema"];
            if (!string.IsNullOrWhiteSpace(configurada)) { return configurada.Trim().TrimEnd('/'); }
            var contexto = HttpContext.Current;
            if (contexto == null) { return null; }
            try
            {
                return contexto.Request.Url.GetLeftPart(UriPartial.Authority);
            }
            catch (HttpException)
            {
                return null;
            }
        }

        private static IHubContext Hub()
        {
            return GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
        }
    }
}
