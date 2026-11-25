using Negocio.Dominio;
using Negocio.DTO;
using Negocio.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using login_v6.Helpers;
using Model;

namespace login_v6.Controllers
{
    [Authorize]
    public class EscanerController : Controller
    {

        stores store = new stores();
        // GET: Escaner
        /// <summary>
        /// Grilla con todos los escáneres activos. El combo de ubicación filtra la grilla en el
        /// navegador; ?ubicacion=N lo deja preseleccionado.
        /// </summary>
        public ActionResult Index(int? ubicacion)
        {
            var ubi = new ubicacion_escaner_dominio();
            ViewBag.ubicaciones = new SelectList(ubi.Listar(), "id_ubicacion_escaner", "nombre", ubicacion);
            return View(store.u_es_01());
        }

        public ActionResult Editar(int id)
        {
            var det = store.u_detalle_escaner(id);

            mmarca_escaner_dominio modelo = new mmarca_escaner_dominio();
            ubicacion_escaner_dominio ubi = new ubicacion_escaner_dominio();

            ViewBag.modelo = new SelectList(modelo.Listar(), "id_marca_modelo_escaner", "descripcion", det.id_mm);
            ViewBag.ubicacion = new SelectList(ubi.Listar(), "id_ubicacion_escaner", "nombre", det.id_ubicacion_escaner);

            return View(det);
        }

        public ActionResult GuardarES(DTO_u_detalle_escaner deta)
        {
            // Nombre repetido: vuelve al formulario con el aviso (el navegador normalmente ya lo frenó).
            var repetido = this.VolverSiNombreRepetido("escaner", deta.descripcion, Convert.ToInt32(deta.id_escaner), "~/Escaner/Index");
            if (repetido != null) { return repetido; }

            escaner_dominio es = new escaner_dominio();
            var escaner = es.Obtener(deta.id_escaner);

            escaner.descripcion = deta.descripcion;
            es.Guardar(escaner); //Cabecera

            detalle_escaner_dominio dt = new detalle_escaner_dominio();
            var detalle = dt.Obtener(deta.id_detalle);

            detalle.marca_modelo_id = deta.id_mm;
            detalle.nroip = deta.nro_ip; //Detalle

            dt.Guardar(detalle);

            //AUDITORIA
            Auditoria audit = new Auditoria();
            auditoria_dominio audit_dom = new auditoria_dominio();

            audit.fecha_hora = DateTime.Now;
            audit.tipo_equipo = "ESCANER";
            audit.id_equipo = deta.id_escaner;
            audit.accion = "EDITAR";
            audit.usuario = User.Identity.Name;

            audit_dom.Guardar(audit);
            //---//

            this.AvisoExito("Se guardaron los cambios del escáner" + Avisos.Nombre(deta.descripcion) + ".");
            return Redirect("~/Escaner/Index");
        }

        public ActionResult Baja(int id)
        {
            escaner_dominio esc = new escaner_dominio();
            var escaner = esc.Obtener(id);
            escaner.fecha_baja = DateTime.Now;
            esc.Guardar(escaner);

            //AUDITORIA
            Auditoria audit = new Auditoria();
            auditoria_dominio audit_dom = new auditoria_dominio();

            audit.fecha_hora = DateTime.Now;
            audit.tipo_equipo = "ESCANER";
            audit.id_equipo = id;
            audit.accion = "BAJA";
            audit.usuario = User.Identity.Name;

            audit_dom.Guardar(audit);
            //---//
            this.AvisoExito("Se dio de baja el escáner" + Avisos.Nombre(escaner.descripcion) + ".");
            return Redirect("~/Escaner/Index");
        }

    }
}