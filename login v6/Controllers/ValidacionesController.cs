using System.Linq;
using System.Web.Mvc;
using Negocio.Servicio;

namespace login_v6.Controllers
{
    /// <summary>
    /// Validaciones que el navegador pide mientras se completa un formulario (Scripts/app/sgei.js).
    /// </summary>
    [Authorize]
    public class ValidacionesController : Controller
    {
        /// <summary>
        /// ¿El nombre del equipo está libre? tipo: computadora, camara, switch, escaner o telefono; id: el del equipo que
        /// se edita (0 en un alta). Responde { ok: true } o { ok: false, mensaje }.
        /// </summary>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult NombreEquipo(string tipo, string nombre, int id = 0)
        {
            if (!NombresUnicos.TiposEquipo.Contains(tipo)) { return HttpNotFound(); }
            var existente = NombresUnicos.Equipo(tipo, nombre, id);
            return Json(existente == null
                ? (object)new { ok = true }
                : new { ok = false, mensaje = NombresUnicos.MensajeEquipo(tipo, existente) }, JsonRequestBehavior.AllowGet);
        }
    }
}
