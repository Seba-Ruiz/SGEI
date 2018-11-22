using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Negocio.Dominio;

namespace login_v6.Controllers
{
    [Authorize]
    public class InsumoController : Controller
    {
        public insumo_dominio insu = new insumo_dominio();
        public detalle_insumo_impresora_dominio deta = new detalle_insumo_impresora_dominio();
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult AgregarInsumo(int id)
        {
            ViewBag.id_ubicacion_impresora = id;
            ViewBag.insumos = insu.ListarSinCantidad();
            ViewBag.insu = insu.Listar();

            return View(new detalle_insumo_impresora());
        }
        [HttpPost]
        public ActionResult Guardar_detalle_insumo_impresora(detalle_insumo_impresora model)
        {
            deta.Guardar(model);

            int insu = Convert.ToInt32(model.insumo_id);

            deta.Restar(insu);

            //AUDITORIA
            Auditoria audit = new Auditoria();
            auditoria_dominio audit_dom = new auditoria_dominio();

            audit.fecha_hora = DateTime.Now;
            audit.tipo_equipo = "IMPRESORA";
            audit.id_equipo = model.ubicacion_impresora_id;
            audit.accion = "INSUMO";
            audit.usuario = User.Identity.Name;

            audit_dom.Guardar(audit);
            //---//

            return View();
        }


        public ActionResult AgregarInsumoPC(int id)
        {
            insumo_pc_dominio insu = new insumo_pc_dominio();

            ViewBag.id_detalle = id;
            ViewBag.insumos = insu.ListarSinCantidad();


            return View(new pc_insumo_pc());
        }


        public ActionResult Guardar_insumo_pc(pc_insumo_pc model)
        {
            pc_insumo_pc_dominio insu = new pc_insumo_pc_dominio();
            insumo_pc_dominio insumo = new insumo_pc_dominio();

            var detalle = new detallepc_dominio();


            insu.Guardar(model);

            var det = detalle.Obtener(model.detalle_pc_id);

            //AUDITORIA
            Auditoria audit = new Auditoria();
            auditoria_dominio audit_dom = new auditoria_dominio();

            audit.fecha_hora = DateTime.Now;
            audit.tipo_equipo = "COMPUTADORA";
            audit.id_equipo = det.pc_id;
            audit.accion = "INSUMO";
            audit.usuario = User.Identity.Name;

            audit_dom.Guardar(audit);
            //---//


            insumo.Restar(Convert.ToInt32(model.insumo_id));
            return View();
        }
    }
}