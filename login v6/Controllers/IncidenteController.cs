using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Negocio.Dominio;

namespace login_v6.Controllers
{
    [Authorize]
    public class IncidenteController : Controller
    {
        public incidente_dominio inci = new incidente_dominio();
        public ActionResult Index(int id)
        {
            ViewBag.ubicacion_impresora_id = id;
            return View(new incidente());
        }

        public ActionResult Guardar(incidente model)
        {
            if (model.fecha_accion==null || model.fecha_incidente==null)
            {
                return RedirectToAction("ErrorPantalla");
            }
            else
            {
                inci.Guardar(model);

                //AUDITORIA
                Auditoria audit = new Auditoria();
                auditoria_dominio audit_dom = new auditoria_dominio();

                audit.fecha_hora = DateTime.Now;
                audit.tipo_equipo = "IMPRESORA";
                audit.id_equipo = model.ubicacion_impresora_id;
                audit.accion = "INCIDENTE";
                audit.usuario = User.Identity.Name;

                audit_dom.Guardar(audit);
                //---//

                TempData["id_incidente"] = model.id;
                return RedirectToAction("AgregarImagen");
            }
            
        }

        public ActionResult ErrorPantalla()
        {

            return View();
        }
        public ActionResult Volver()
        {

            return RedirectToAction("Index", "Home");
        }

        public ActionResult AgregarImagen()
        {
            imagen ima = new imagen();

            ViewBag.mensajito = false;

            ViewBag.id_incidente = TempData["id_incidente"];
            return View(ima);
        }

        [HttpPost]
        public ActionResult FileUpload(imagen model, HttpPostedFileBase file)
        {
            ViewBag.mensajito = true;

            ViewBag.mensajeExito = "Exito!";
            ViewBag.mensajeExito1 = " Imagen subida correctamente";
            if (file != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();

                    var db = new SGEIContext();
                    db.imagen.Add(new Model.imagen()
                    {
                        incidente_id = model.incidente_id,
                        imagen1 = array,
                    });
                    db.SaveChanges();
                }

            }

            return RedirectToAction("Index", "Home");
        }

    }
}