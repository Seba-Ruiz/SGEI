using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using login_v6.Helpers;
using Model;
using Negocio.Dominio;
using Negocio.DTO;
using Negocio.Servicio;

namespace login_v6.Controllers
{
    public class CamaraController : Controller
    {
        stores store = new stores();
        // GET: Camara
        /// <summary>
        /// Grilla con todas las cámaras activas. El combo de ubicación filtra la grilla en el
        /// navegador; ?ubicacion=N lo deja preseleccionado.
        /// </summary>
        public ActionResult Index(int? ubicacion)
        {
            var ubi = new ubicacion_camara_dominio();
            ViewBag.ubicaciones = new SelectList(ubi.Listar(), "id_ubicacion_camara", "nombre", ubicacion);
            return View(store.u_camara_01());
        }

        public ActionResult Editar(int id)
        {
            var det = store.u_detalle_camara(id);

            mmarca_camara_dominio modelo = new mmarca_camara_dominio();
            ubicacion_camara_dominio ubi = new ubicacion_camara_dominio();

            ViewBag.modelo = new SelectList(modelo.Listar(), "id_mmarca", "marca_modelo", det.id_mm);
            ViewBag.ubicacion = new SelectList(ubi.Listar(), "id_ubicacion_camara", "nombre", det.id_ubicacion_camara);

            return View(det);
        }

        public ActionResult GuardarCam(DTO_u_detalle_camara deta)
        {
            camara_dominio cam = new camara_dominio();
            var camara = cam.Obtener(deta.id_camara);

            camara.descripcion = deta.descripcion;
            cam.Guardar(camara); //Cabecera

            detalle_camara_dominio dt = new detalle_camara_dominio();
            var detalle = dt.Obtener(deta.id_detalle);

            detalle.marca_model_id = deta.id_mm;
            detalle.nroip = deta.nro_ip; //Detalle

            dt.Guardar(detalle);

            //AUDITORIA
            Auditoria audit = new Auditoria();
            auditoria_dominio audit_dom = new auditoria_dominio();

            audit.fecha_hora = DateTime.Now;
            audit.tipo_equipo = "CAMARA";
            audit.id_equipo = deta.id_camara;
            audit.accion = "EDITAR";
            audit.usuario = User.Identity.Name;

            audit_dom.Guardar(audit);
            //---//

            this.AvisoExito("Se guardaron los cambios de la cámara" + Avisos.Nombre(deta.descripcion) + ".");
            return Redirect("~/Camara/Index");
        }

        public ActionResult Baja(int id)
        {
            camara_dominio cam = new camara_dominio();
            var camara = cam.Obtener(id);
            camara.fecha_baja = DateTime.Now;
            cam.Guardar(camara);

            //AUDITORIA
            Auditoria audit = new Auditoria();
            auditoria_dominio audit_dom = new auditoria_dominio();

            audit.fecha_hora = DateTime.Now;
            audit.tipo_equipo = "CAMARA";
            audit.id_equipo = id;
            audit.accion = "BAJA";
            audit.usuario = User.Identity.Name;

            audit_dom.Guardar(audit);
            //---//
            this.AvisoExito("Se dio de baja la cámara" + Avisos.Nombre(camara.descripcion) + ".");
            return Redirect("~/Camara/Index");
        }
    }

}