using System.Linq;
using System.Web.Mvc;
using login_v6.Helpers;
using login_v6.Servicio;
using Microsoft.AspNet.Identity;
using Negocio.Servicio;

namespace login_v6.Controllers
{
    /// <summary>
    /// Notificaciones del usuario conectado: la campana de la barra superior (sin leer, por AJAX) y la pantalla con
    /// todas. Cada uno ve y marca solo las suyas. Se generan con Servicio/Notificador.cs.
    /// </summary>
    [Authorize]
    public class NotificacionesController : Controller
    {
        private readonly Notificaciones servicio = new Notificaciones();

        private string UsuarioId { get { return User.Identity.GetUserId(); } }

        /// <summary>Todas las notificaciones (o solo las sin leer), más nuevas primero.</summary>
        public ActionResult Index(string filtro, int pagina = 1)
        {
            var soloSinLeer = filtro == "sin-leer";
            return View(servicio.Listar(UsuarioId, soloSinLeer, pagina));
        }

        /// <summary>Contenido de la campana: cantidad sin leer y las más nuevas.</summary>
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Campana()
        {
            return Json(new
            {
                cantidad = servicio.CantidadSinLeer(UsuarioId),
                items = servicio.SinLeer(UsuarioId).Select(Notificador.ParaCampana).ToList()
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Marca la notificación como leída y lleva a la pantalla a la que se refiere.</summary>
        public ActionResult Abrir(int id)
        {
            var n = servicio.Obtener(id, UsuarioId);
            if (n == null) { return HttpNotFound(); }
            if (!n.Leida)
            {
                servicio.MarcarLeida(id, UsuarioId);
                Notificador.AvisarCantidad(UsuarioId);
            }
            var destino = Notificador.ResolverUrl(n.Url);
            return destino != null ? (ActionResult)Redirect(destino) : RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Leer(int id, string volver)
        {
            var ok = servicio.MarcarLeida(id, UsuarioId);
            if (ok) { Notificador.AvisarCantidad(UsuarioId); }
            if (Request.IsAjaxRequest())
            {
                return Json(new { ok, cantidad = servicio.CantidadSinLeer(UsuarioId) });
            }
            return Volver(volver);
        }

        [HttpPost]
        public ActionResult LeerTodas(string volver)
        {
            var cambiadas = servicio.MarcarTodasLeidas(UsuarioId);
            if (cambiadas > 0) { Notificador.AvisarCantidad(UsuarioId); }
            if (Request.IsAjaxRequest())
            {
                return Json(new { ok = true, cambiadas, cantidad = 0 });
            }
            if (cambiadas > 0)
            {
                this.AvisoExito(cambiadas == 1 ? "Se marcó 1 notificación como leída." : "Se marcaron " + cambiadas + " notificaciones como leídas.");
            }
            return Volver(volver);
        }

        private ActionResult Volver(string volver)
        {
            return !string.IsNullOrEmpty(volver) && Url.IsLocalUrl(volver) ? (ActionResult)Redirect(volver) : RedirectToAction("Index");
        }
    }
}
