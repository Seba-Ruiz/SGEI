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
    public class SwichsController : Controller
    {
        stores store = new stores();

        // GET: Switch
        /// <summary>
        /// Grilla con todos los switches activos. El combo de ubicación filtra la grilla en el
        /// navegador; ?ubicacion=N lo deja preseleccionado.
        /// </summary>
        public ActionResult Index(int? ubicacion)
        {
            var ubi = new ubicacion_sw_dominio();
            ViewBag.ubicaciones = new SelectList(ubi.Listar(), "id_ubicacion_switch", "nombre", ubicacion);
            return View(store.u_sw_01());
        }

        public ActionResult Editar(int id)
        {
            var det = store.u_detalle_sw(id);

            mmarca_sw_dominio modelo = new mmarca_sw_dominio();
            ubicacion_sw_dominio ubi = new ubicacion_sw_dominio();

            ViewBag.modelo = new SelectList(modelo.Listar(), "id_mmarca", "marca_modelo", det.id_mm);
            ViewBag.ubicacion = new SelectList(ubi.Listar(), "id_ubicacion_switch", "nombre", det.id_ubicacion_sw);

            return View(det);
        }

        public ActionResult GuardarSW(DTO_u_detalle_sw deta)
        {
            switch_dominio sw = new switch_dominio();
            var switc = sw.Obtener(deta.id_sw);

            switc.descripcion = deta.descripcion;
            sw.Guardar(switc); //Cabecera

            detalle_sw_dominio dt = new detalle_sw_dominio();
            var detalle = dt.Obtener(deta.id_detalle);

            detalle.marca_modelo_id = deta.id_mm;

            detalle.nroip = deta.nro_ip; //Detalle
            detalle.interfaces = deta.interfaces;

            dt.Guardar(detalle);

            //AUDITORIA
            Auditoria audit = new Auditoria();
            auditoria_dominio audit_dom = new auditoria_dominio();

            audit.fecha_hora = DateTime.Now;
            audit.tipo_equipo = "SWITCH";
            audit.id_equipo = deta.id_sw;
            audit.accion = "EDITAR";
            audit.usuario = User.Identity.Name;

            audit_dom.Guardar(audit);
            //---//
            this.AvisoExito("Se guardaron los cambios del switch" + Avisos.Nombre(deta.descripcion) + ".");
            return Redirect("~/Swichs/Index");
        }

        public ActionResult Baja(int id)
        {
            switch_dominio sw = new switch_dominio();
            var switc = sw.Obtener(id);
            switc.fecha_baja = DateTime.Now;
            sw.Guardar(switc);

            //AUDITORIA
            Auditoria audit = new Auditoria();
            auditoria_dominio audit_dom = new auditoria_dominio();

            audit.fecha_hora = DateTime.Now;
            audit.tipo_equipo = "SWITCH";
            audit.id_equipo = id;
            audit.accion = "BAJA";
            audit.usuario = User.Identity.Name;

            audit_dom.Guardar(audit);
            //---//
            this.AvisoExito("Se dio de baja el switch" + Avisos.Nombre(switc.descripcion) + ".");
            return Redirect("~/Swichs/Index");
        }

    }
}