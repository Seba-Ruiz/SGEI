using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using login_v6.Helpers;
using login_v6.Seguridad;
using login_v6.Servicio;
using Microsoft.AspNet.Identity;
using Negocio.DTO;
using Negocio.Servicio;

namespace login_v6.Controllers
{
    /// <summary>
    /// Menú Tickets: board de trabajo por estado (Pendiente, En curso, Bloqueado, Terminado), grilla con todos los
    /// tickets, detalle con la conversación y el historial de cambios. Permisos:
    ///   alta, edición y eliminación: administradores · cambio de estado: cualquiera de los asignados o un administrador ·
    ///   comentarios: cualquiera; cada uno edita o elimina los suyos.
    /// Lógica en Negocio/Servicio/Tickets.cs; el HTML de Summernote se sanea al guardar (HtmlSeguro).
    /// </summary>
    [Authorize]
    public class TicketsController : Controller
    {
        /// <summary>Largo máximo del HTML de una descripción o un comentario (incluye las imágenes pegadas).</summary>
        private const int LargoMaximoHtml = 12 * 1024 * 1024;

        private readonly Tickets servicio = new Tickets();

        private string UsuarioId { get { return User.Identity.GetUserId(); } }

        // ================================================================== Board y grilla

        public ActionResult Index()
        {
            ViewBag.UsuarioId = UsuarioId;
            ViewBag.EsAdministrador = User.EsAdministrador();
            return View(servicio.Board());
        }

        public ActionResult Lista()
        {
            return View(servicio.Listar());
        }

        // ================================================================== Detalle

        public ActionResult Ver(int id)
        {
            var t = servicio.Detalle(id);
            if (t == null) { return HttpNotFound(); }
            ViewBag.UsuarioId = UsuarioId;
            ViewBag.EsAdministrador = User.EsAdministrador();
            ViewBag.PuedeMover = PuedeMover(t);
            return View(t);
        }

        /// <summary>Cambio de estado desde el board (AJAX, responde JSON) o desde el combo del detalle.</summary>
        [HttpPost]
        public ActionResult Estado(int id, byte estado)
        {
            var antes = servicio.Tarjeta(id);
            var error = servicio.CambiarEstado(id, estado, UsuarioId, User.EsAdministrador());
            if (error == null && antes != null && antes.Estado != estado) { NotificarCambioDeEstado(antes, estado); }
            if (Request.IsAjaxRequest())
            {
                if (error != null)
                {
                    Response.StatusCode = 400;
                    Response.TrySkipIisCustomErrors = true;
                    return Json(new { ok = false, mensaje = error });
                }
                var t = servicio.Tarjeta(id);
                return Json(new
                {
                    ok = true,
                    mensaje = "El ticket #" + id + " pasó a «" + EstadosTicket.Nombre(estado) + "».",
                    semaforo = t.Semaforo,
                    plazo = t.TextoPlazo
                });
            }

            if (error != null) { TempData["ticket.error"] = error; }
            else { this.AvisoExito("El ticket #" + id + " pasó a «" + EstadosTicket.Nombre(estado) + "»."); }
            return RedirectToAction("Ver", new { id });
        }

        // ================================================================== Alta, edición y eliminación (administradores)

        [SoloAdministrador]
        public ActionResult Nuevo()
        {
            return Formulario(new DTO_TicketFormulario { Prioridad = PrioridadesTicket.Media, FechaPrevista = DateTime.Today.AddDays(7) });
        }

        [SoloAdministrador]
        public ActionResult Editar(int id)
        {
            var f = servicio.Formulario(id);
            return f == null ? (ActionResult)HttpNotFound() : Formulario(f);
        }

        [HttpPost]
        [SoloAdministrador]
        [ValidateInput(false)] // La descripción es HTML de Summernote: se sanea en el servicio (HtmlSeguro).
        public ActionResult Guardar(DTO_TicketFormulario f)
        {
            f.Titulo = (f.Titulo ?? "").Trim();
            if (f.Titulo.Length == 0) { ModelState.AddModelError("Titulo", "Indicá el título."); }
            else if (f.Titulo.Length > Tickets.LargoTitulo) { ModelState.AddModelError("Titulo", "Máximo " + Tickets.LargoTitulo + " caracteres."); }

            var nuevo = f.Id == 0;
            var asignadosAntes = nuevo ? new List<string>() : (servicio.Formulario(f.Id)?.AsignadosIds ?? new List<string>());
            f.AsignadosIds = (f.AsignadosIds ?? new List<string>()).Where(a => !string.IsNullOrEmpty(a)).Distinct().ToList();
            var asignados = f.AsignadosIds.Select(a => servicio.Usuario(a)).ToList();
            if (asignados.Count == 0) { ModelState.AddModelError("AsignadosIds", "Elegí al menos una persona."); }
            else if (asignados.Any(a => a == null)) { ModelState.AddModelError("AsignadosIds", "Alguno de los usuarios elegidos ya no existe."); }
            else
            {
                // Uno dado de baja puede quedar si ya estaba asignado, pero no se puede sumar.
                var deBaja = asignados.Where(a => !a.Activo && !asignadosAntes.Contains(a.Id)).Select(a => a.Nombre).ToList();
                if (deBaja.Count > 0) { ModelState.AddModelError("AsignadosIds", "Dado de baja: " + string.Join(", ", deBaja) + "."); }
            }

            if (f.Prioridad == null || !PrioridadesTicket.EsValida(f.Prioridad.Value)) { ModelState.AddModelError("Prioridad", "Elegí la prioridad."); }

            if (f.FechaPrevista == null) { ModelState.AddModelError("FechaPrevista", "Indicá la fecha de finalización prevista."); }
            else if (f.Id == 0 && f.FechaPrevista.Value.Date < DateTime.Today) { ModelState.AddModelError("FechaPrevista", "No puede ser anterior a hoy."); }

            if (string.IsNullOrEmpty(f.TipoEquipo)) { f.TipoEquipo = null; f.EquipoId = null; }
            else if (!TiposEquipo.EsValido(f.TipoEquipo)) { ModelState.AddModelError("TipoEquipo", "Tipo de equipo desconocido."); }
            else if (f.EquipoId == null) { ModelState.AddModelError("EquipoId", "Elegí el equipo, o dejá el tipo en «Sin equipo»."); }

            if (f.Descripcion != null && f.Descripcion.Length > LargoMaximoHtml) { ModelState.AddModelError("Descripcion", "La descripción es demasiado grande (revisá el tamaño de las imágenes)."); }

            if (!ModelState.IsValid) { return Formulario(f); }

            var id = servicio.Guardar(f, UsuarioId);
            NotificarAsignacion(id, nuevo, f.AsignadosIds.Except(asignadosAntes).ToList());
            this.AvisoExito(nuevo
                ? "Se creó el ticket #" + id + Avisos.Nombre(f.Titulo) + " y se asignó a " + string.Join(", ", asignados.Select(a => a.Nombre)) + "."
                : "Se guardaron los cambios del ticket #" + id + Avisos.Nombre(f.Titulo) + ".");
            return nuevo ? RedirectToAction("Index") : RedirectToAction("Ver", new { id });
        }

        [HttpPost]
        [SoloAdministrador]
        public ActionResult Eliminar(int id)
        {
            var titulo = servicio.Eliminar(id, User.Identity.Name);
            if (titulo != null) { this.AvisoExito("Se eliminó el ticket #" + id + Avisos.Nombre(titulo) + "."); }
            return RedirectToAction("Index");
        }

        // ================================================================== Notificaciones (campana + mail)

        /// <summary>
        /// Al crear un ticket, o al sumar personas a uno existente, se le avisa a cada persona que se agregó (salvo a uno
        /// mismo). Cada una recibe su notificación y su mail.
        /// </summary>
        private void NotificarAsignacion(int id, bool nuevo, List<string> agregados)
        {
            var destinatarios = agregados.Where(a => a != UsuarioId).ToList();
            if (destinatarios.Count == 0) { return; }
            var t = servicio.Tarjeta(id);
            if (t == null) { return; }
            var autor = UsuarioActual.Datos(User).Nombre;
            foreach (var destinatario in destinatarios)
            {
                var companeros = t.Asignados.Where(a => a.Id != destinatario).Select(a => a.Nombre).ToList();
                Notificador.Enviar(new NotificacionNueva
                {
                    UsuarioId = destinatario,
                    OrigenUsuarioId = UsuarioId,
                    Tipo = TiposNotificacion.TicketAsignado,
                    Titulo = (nuevo ? "Se te asignó el ticket #" : "Te sumaron al ticket #") + t.Id,
                    Mensaje = autor + (nuevo ? " te asignó el ticket «" : " te sumó al ticket «") + t.Titulo + "», con prioridad "
                        + PrioridadesTicket.Nombre(t.Prioridad).ToLowerInvariant() + " y fecha prevista de finalización el "
                        + t.FechaPrevista.ToString("dd/MM/yyyy") + ".",
                    Url = "~/Tickets/Ver/" + t.Id,
                    ReferenciaTipo = "TICKET",
                    ReferenciaId = t.Id,
                    TextoBoton = "Ver ticket"
                }
                .Dato("Ticket", "#" + t.Id + " · " + t.Titulo)
                .Dato("Prioridad", PrioridadesTicket.Nombre(t.Prioridad))
                .Dato("Estado", EstadosTicket.Nombre(t.Estado))
                .Dato("Fecha prevista", t.FechaPrevista.ToString("dd/MM/yyyy"))
                .Dato("También asignados", companeros.Count == 0 ? null : string.Join(", ", companeros))
                .Dato("Equipo", t.Equipo));
            }
        }

        /// <summary>Al cambiar el estado se le avisa a quien creó el ticket (salvo que lo haya cambiado él mismo).</summary>
        private void NotificarCambioDeEstado(DTO_TicketTarjeta antes, byte estado)
        {
            if (antes.CreadoPorId == null || antes.CreadoPorId == UsuarioId) { return; }
            var autor = UsuarioActual.Datos(User).Nombre;
            var anterior = EstadosTicket.Nombre(antes.Estado);
            var nuevo = EstadosTicket.Nombre(estado);
            Notificador.Enviar(new NotificacionNueva
            {
                UsuarioId = antes.CreadoPorId,
                OrigenUsuarioId = UsuarioId,
                Tipo = TiposNotificacion.TicketEstado,
                Titulo = "El ticket #" + antes.Id + " pasó a «" + nuevo + "»",
                Mensaje = autor + " cambió el estado del ticket «" + antes.Titulo + "» de «" + anterior + "» a «" + nuevo + "».",
                Url = "~/Tickets/Ver/" + antes.Id,
                ReferenciaTipo = "TICKET",
                ReferenciaId = antes.Id,
                TextoBoton = "Ver ticket"
            }
            .Dato("Ticket", "#" + antes.Id + " · " + antes.Titulo)
            .Dato("Estado anterior", anterior)
            .Dato("Estado nuevo", nuevo)
            .Dato("Asignado a", antes.AsignadosTexto)
            .Dato("Prioridad", PrioridadesTicket.Nombre(antes.Prioridad))
            .Dato("Fecha prevista", antes.FechaPrevista.ToString("dd/MM/yyyy")));
        }

        private ActionResult Formulario(DTO_TicketFormulario f)
        {
            var equipos = TiposEquipo.Todos.ToDictionary(
                t => t,
                t => new ConsultasUnificadas().EquiposParaElegir(t, false, t == f.TipoEquipo ? f.EquipoId : null)
                         .Select(e => new { id = e.Id, texto = e.Texto }).ToList());
            ViewBag.Datos = new { equipos };
            ViewBag.Usuarios = servicio.UsuariosParaAsignar(f.AsignadosIds);
            return View("Formulario", f);
        }

        // ================================================================== Comentarios

        [HttpPost]
        [ValidateInput(false)] // HTML de Summernote: se sanea en el servicio (HtmlSeguro).
        public ActionResult Comentar(int id, string texto)
        {
            var error = ValidarComentario(texto);
            if (error != null)
            {
                TempData["ticket.error"] = error;
                TempData["ticket.borrador"] = texto != null && texto.Length <= LargoMaximoHtml ? texto : null;
                return RedirectToAction("Ver", new { id });
            }
            var idComentario = servicio.Comentar(id, UsuarioId, texto);
            if (idComentario == null) { return HttpNotFound(); }
            this.AvisoExito("Se agregó tu comentario al ticket #" + id + ".");
            return Redirect(Url.Action("Ver", new { id }) + "#comentario-" + idComentario);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditarComentario(int id, string texto)
        {
            var idTicket = servicio.TicketDeComentario(id);
            if (idTicket == null) { return RedirectToAction("Index"); }
            int ignorado;
            var error = ValidarComentario(texto) ?? servicio.EditarComentario(id, UsuarioId, texto, out ignorado);
            if (error != null)
            {
                TempData["ticket.error"] = error;
                return Redirect(Url.Action("Ver", new { id = idTicket }) + "#comentario-" + id);
            }
            this.AvisoExito("Se guardaron los cambios de tu comentario.");
            return Redirect(Url.Action("Ver", new { id = idTicket }) + "#comentario-" + id);
        }

        [HttpPost]
        public ActionResult EliminarComentario(int id)
        {
            int idTicket;
            var error = servicio.EliminarComentario(id, UsuarioId, out idTicket);
            if (idTicket == 0) { return RedirectToAction("Index"); }
            if (error != null) { TempData["ticket.error"] = error; }
            else { this.AvisoExito("Se eliminó tu comentario."); }
            return Redirect(Url.Action("Ver", new { id = idTicket }) + "#actividad");
        }

        private static string ValidarComentario(string texto)
        {
            if (HtmlSeguro.EstaVacio(texto)) { return "Escribí el comentario."; }
            if (texto.Length > LargoMaximoHtml) { return "El comentario es demasiado grande (revisá el tamaño de las imágenes)."; }
            return null;
        }

        private bool PuedeMover(DTO_TicketTarjeta t)
        {
            return User.EsAdministrador() || t.EstaAsignado(UsuarioId);
        }
    }
}
