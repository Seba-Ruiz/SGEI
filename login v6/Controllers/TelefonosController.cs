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
    [Authorize]
    public class TelefonosController : Controller
    {
        stores store = new stores();
        
        // GET: Telefonos
        /// <summary>
        /// Grilla con todos los teléfonos activos. El combo de ubicación filtra la grilla en el
        /// navegador; ?ubicacion=N lo deja preseleccionado.
        /// </summary>
        public ActionResult Index(int? ubicacion)
        {
            var ubi = new ubicacion_tel_dominio();
            ViewBag.ubicaciones = new SelectList(ubi.Listar(), "id_ubicacion_tel", "nombre_ubi", ubicacion);
            return View(store.u_telefono_01());
        }

        public ActionResult AltaTel(int id)
        {
            return View();
        }

        public ActionResult Editar(int id)
        {
                var det = store.u_detalle_telefono(id);

                mmarca_dominio modelo = new mmarca_dominio();
                tipo_telefono_dominio ttel = new tipo_telefono_dominio();
                ubicacion_tel_dominio ubi = new ubicacion_tel_dominio();

                ViewBag.modelo = new SelectList(modelo.Listar(), "id_mm", "descripcion", det.id_mm);
                ViewBag.tipo = new SelectList(ttel.Listar(), "id_tipo", "nombre_tipo", det.id_tipo);
                ViewBag.ubicacion = new SelectList(ubi.Listar(), "id_ubicacion_tel", "nombre_ubi", det.id_ubicacion_tel);

                return View(det);
        }

        public ActionResult GuardarTel(DTO_u_detalle_telefono deta)
        {
            // Nombre repetido: vuelve al formulario con el aviso (el navegador normalmente ya lo frenó).
            var repetido = this.VolverSiNombreRepetido("telefono", deta.descripcion, Convert.ToInt32(deta.id_telefono), "~/Telefonos/Index");
            if (repetido != null) { return repetido; }

            telefono_dominio tel = new telefono_dominio();
            var telefono = tel.Obtener(deta.id_telefono);

            telefono.descripcion = deta.descripcion;
            tel.Guardar(telefono); //Cabecera

            detalle_tel_dominio dt = new detalle_tel_dominio();
            var detalle = dt.Obtener(deta.id_detalle);

            detalle.marca_modelo_id = deta.id_mm;
            detalle.nro_interno = deta.nro_interno;
            detalle.tipo_tel_id = deta.id_tipo; //Detalle

            dt.Guardar(detalle);

            //AUDITORIA
            Auditoria audit = new Auditoria();
            auditoria_dominio audit_dom = new auditoria_dominio();

            audit.fecha_hora = DateTime.Now;
            audit.tipo_equipo = "TELEFONO";
            audit.id_equipo = deta.id_telefono;
            audit.accion = "EDITAR";
            audit.usuario = User.Identity.Name;

            audit_dom.Guardar(audit);
            //---//

            this.AvisoExito("Se guardaron los cambios del teléfono" + Avisos.Nombre(deta.descripcion) + ".");
            return Redirect("~/Telefonos/Index");
        }
        public ActionResult Baja(int id)
        {
            telefono_dominio tel = new telefono_dominio();
            var telefono = tel.Obtener(id);
            telefono.fecha_baja = DateTime.Now;
            tel.Guardar(telefono);

            //AUDITORIA
            Auditoria audit = new Auditoria();
            auditoria_dominio audit_dom = new auditoria_dominio();

            audit.fecha_hora = DateTime.Now;
            audit.tipo_equipo = "TELEFONO";
            audit.id_equipo = id;
            audit.accion = "BAJA";
            audit.usuario = User.Identity.Name;

            audit_dom.Guardar(audit);
            //---//
            this.AvisoExito("Se dio de baja el teléfono" + Avisos.Nombre(telefono.descripcion) + ".");
            return Redirect("~/Telefonos/Index");
        }

    }
}