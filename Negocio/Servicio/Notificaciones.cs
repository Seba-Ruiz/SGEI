using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Negocio.DTO;

namespace Negocio.Servicio
{
    /// <summary>
    /// Tipos de notificación. Para un aviso nuevo del sistema alcanza con agregar acá su código e ícono y llamar a
    /// Notificador.Enviar (proyecto web) con un <see cref="NotificacionNueva"/>.
    /// </summary>
    public static class TiposNotificacion
    {
        /// <summary>Se creó un ticket, o se reasignó, a nombre del destinatario.</summary>
        public const string TicketAsignado = "TICKET_ASIGNADO";
        /// <summary>Cambió el estado de un ticket que creó el destinatario.</summary>
        public const string TicketEstado = "TICKET_ESTADO";

        public static string Icono(string tipo)
        {
            switch (tipo)
            {
                case TicketAsignado: return "bi-person-check";
                case TicketEstado: return "bi-arrow-left-right";
                default: return "bi-bell";
            }
        }
    }

    /// <summary>
    /// Notificaciones de cada usuario (tabla notificacion, script BaseDeDatos/2026-11-25_notificaciones.sql).
    /// Solo base de datos: el aviso en vivo (SignalR) y el mail los hace Notificador en el proyecto web.
    /// Todas las fechas se guardan con fecha y hora.
    /// </summary>
    public class Notificaciones
    {
        public const int PorPagina = 20;
        /// <summary>Cuántas sin leer se muestran en la campana.</summary>
        public const int MaximoCampana = 10;

        private const int LargoTitulo = 200;
        private const int LargoMensaje = 1000;
        private const int LargoUrl = 400;
        private const int LargoError = 500;

        /// <summary>Guarda la notificación y la devuelve como se muestra. Null si el destinatario no existe.</summary>
        public DTO_Notificacion Crear(NotificacionNueva n)
        {
            if (n == null || string.IsNullOrEmpty(n.UsuarioId) || string.IsNullOrEmpty(n.Tipo) || string.IsNullOrWhiteSpace(n.Titulo))
            {
                throw new ArgumentException("La notificación necesita destinatario, tipo y título.");
            }
            using (var ctx = new SGEIContext())
            {
                var usuarios = Tickets.Usuarios(ctx);
                if (!usuarios.ContainsKey(n.UsuarioId)) { return null; }

                var fila = new notificacion
                {
                    usuario_id = n.UsuarioId,
                    origen_usuario_id = n.OrigenUsuarioId != null && usuarios.ContainsKey(n.OrigenUsuarioId) ? n.OrigenUsuarioId : null,
                    tipo = n.Tipo,
                    titulo = Cortar(n.Titulo.Trim(), LargoTitulo),
                    mensaje = Cortar(n.Mensaje, LargoMensaje),
                    url = Cortar(n.Url, LargoUrl),
                    referencia_tipo = n.ReferenciaTipo,
                    referencia_id = n.ReferenciaId,
                    fecha = DateTime.Now
                };
                ctx.notificacion.Add(fila);
                ctx.SaveChanges();
                return Dto(fila, usuarios);
            }
        }

        public int CantidadSinLeer(string usuarioId)
        {
            using (var ctx = new SGEIContext())
            {
                return ctx.notificacion.Count(x => x.usuario_id == usuarioId && x.fecha_lectura == null);
            }
        }

        /// <summary>Las más nuevas sin leer (para la campana).</summary>
        public List<DTO_Notificacion> SinLeer(string usuarioId, int maximo = MaximoCampana)
        {
            using (var ctx = new SGEIContext())
            {
                var filas = ctx.notificacion
                    .Where(x => x.usuario_id == usuarioId && x.fecha_lectura == null)
                    .OrderByDescending(x => x.fecha).ThenByDescending(x => x.id_notificacion)
                    .Take(maximo).ToList();
                var usuarios = Tickets.Usuarios(ctx);
                return filas.Select(f => Dto(f, usuarios)).ToList();
            }
        }

        /// <summary>Todas las notificaciones del usuario, más nuevas primero, paginadas.</summary>
        public DTO_PaginaNotificaciones Listar(string usuarioId, bool soloSinLeer, int pagina)
        {
            using (var ctx = new SGEIContext())
            {
                var propias = ctx.notificacion.Where(x => x.usuario_id == usuarioId);
                var filtradas = soloSinLeer ? propias.Where(x => x.fecha_lectura == null) : propias;
                var total = filtradas.Count();
                var paginas = Math.Max(1, (total + PorPagina - 1) / PorPagina);
                pagina = Math.Min(Math.Max(1, pagina), paginas);

                var filas = filtradas
                    .OrderByDescending(x => x.fecha).ThenByDescending(x => x.id_notificacion)
                    .Skip((pagina - 1) * PorPagina).Take(PorPagina).ToList();
                var usuarios = Tickets.Usuarios(ctx);
                return new DTO_PaginaNotificaciones
                {
                    Items = filas.Select(f => Dto(f, usuarios)).ToList(),
                    Pagina = pagina,
                    PorPagina = PorPagina,
                    Total = total,
                    TotalSinLeer = soloSinLeer ? total : propias.Count(x => x.fecha_lectura == null),
                    SoloSinLeer = soloSinLeer
                };
            }
        }

        /// <summary>Una notificación del usuario, o null si no existe o es de otra persona.</summary>
        public DTO_Notificacion Obtener(int id, string usuarioId)
        {
            using (var ctx = new SGEIContext())
            {
                var f = ctx.notificacion.FirstOrDefault(x => x.id_notificacion == id && x.usuario_id == usuarioId);
                return f == null ? null : Dto(f, Tickets.Usuarios(ctx));
            }
        }

        /// <summary>Marca como leída (con fecha y hora) una notificación del usuario. False si no es suya.</summary>
        public bool MarcarLeida(int id, string usuarioId)
        {
            using (var ctx = new SGEIContext())
            {
                var f = ctx.notificacion.FirstOrDefault(x => x.id_notificacion == id && x.usuario_id == usuarioId);
                if (f == null) { return false; }
                if (f.fecha_lectura == null)
                {
                    f.fecha_lectura = DateTime.Now;
                    ctx.SaveChanges();
                }
                return true;
            }
        }

        /// <summary>Marca como leídas todas las del usuario. Devuelve cuántas cambió.</summary>
        public int MarcarTodasLeidas(string usuarioId)
        {
            using (var ctx = new SGEIContext())
            {
                var ahora = DateTime.Now;
                var filas = ctx.notificacion.Where(x => x.usuario_id == usuarioId && x.fecha_lectura == null).ToList();
                filas.ForEach(f => f.fecha_lectura = ahora);
                ctx.SaveChanges();
                return filas.Count;
            }
        }

        // ------------------------------------------------------------------ Mail

        /// <summary>Nombre y mail del destinatario, o null si no existe.</summary>
        public DTO_DestinatarioMail Destinatario(string usuarioId)
        {
            using (var ctx = new SGEIContext())
            {
                var u = ctx.AspNetUsers.Where(x => x.Id == usuarioId)
                    .Select(x => new { x.Email, x.UserName, x.nombre, x.apellido, x.fecha_baja })
                    .FirstOrDefault();
                if (u == null) { return null; }
                var nombre = ((u.nombre ?? "").Trim() + " " + (u.apellido ?? "").Trim()).Trim();
                return new DTO_DestinatarioMail
                {
                    Nombre = nombre.Length > 0 ? nombre : u.UserName,
                    Email = string.IsNullOrWhiteSpace(u.Email) ? null : u.Email.Trim(),
                    Activo = u.fecha_baja == null
                };
            }
        }

        /// <summary>Deja constancia del mail: fecha y hora de envío, o el motivo por el que no salió.</summary>
        public void RegistrarMail(int id, bool enviado, string error)
        {
            using (var ctx = new SGEIContext())
            {
                var f = ctx.notificacion.Find(id);
                if (f == null) { return; }
                f.fecha_mail = enviado ? DateTime.Now : (DateTime?)null;
                f.error_mail = enviado ? null : Cortar(error, LargoError);
                ctx.SaveChanges();
            }
        }

        // ------------------------------------------------------------------ Auxiliares

        private static DTO_Notificacion Dto(notificacion f, Dictionary<string, DTO_Usuario> usuarios)
        {
            DTO_Usuario origen = null;
            if (f.origen_usuario_id != null) { usuarios.TryGetValue(f.origen_usuario_id, out origen); }
            return new DTO_Notificacion
            {
                Id = f.id_notificacion,
                Tipo = f.tipo,
                Titulo = f.titulo,
                Mensaje = f.mensaje,
                Url = f.url,
                Origen = origen != null ? origen.Nombre : null,
                OrigenIniciales = origen != null ? origen.Iniciales : null,
                Fecha = f.fecha,
                FechaLectura = f.fecha_lectura,
                FechaMail = f.fecha_mail,
                ErrorMail = f.error_mail
            };
        }

        private static string Cortar(string texto, int largo)
        {
            if (string.IsNullOrEmpty(texto)) { return null; }
            return texto.Length <= largo ? texto : texto.Substring(0, largo - 1) + "…";
        }
    }
}
