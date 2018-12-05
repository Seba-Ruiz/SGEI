using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        public ActionResult Index()
        {
            ubicacion_tel_dominio ubi = new ubicacion_tel_dominio();
            ViewBag.ubicaciontel = ubi.Listar();
            return View();
        }

        public ActionResult AltaTel(int id)
        {
            return View();
        }

        public ActionResult Buscar(int ubicaciontel)
        {
                var ubica = store.u_telefono_01(ubicaciontel);
                return View(ubica);   
        }

        public ActionResult Incidente(int id)
        {
            incidentes_tel_dominio inci = new incidentes_tel_dominio();
            ViewBag.incidente = inci.Listar();
            ViewBag.detalle_id = id;

            return View(new incidente_tel_tel());
        }


        public ActionResult Guardarincidente(incidente_tel_tel inci)
        {
            if (inci.fecha_incidente == null || inci.fecha_reparacion==null)
            {
                return RedirectToAction("Errorpantalla");
            }
            else
            {
                incidente_tel_tel incidente = new incidente_tel_tel();
                incidente_tel_tel_dominio incidenteDominio = new incidente_tel_tel_dominio();

                incidente.detalle_id = inci.detalle_id;
                incidente.fecha_incidente = inci.fecha_incidente;
                incidente.fecha_reparacion = inci.fecha_reparacion;
                incidente.reparacion = inci.reparacion;
                incidente.incidentetel_id = inci.incidentetel_id;

                incidenteDominio.Guardar(incidente);


                var detalle = new detalle_tel_dominio();
                var det = detalle.Obtener(inci.detalle_id);

                //AUDITORIA
                Auditoria audit = new Auditoria();
                auditoria_dominio audit_dom = new auditoria_dominio();

                audit.fecha_hora = DateTime.Now;
                audit.tipo_equipo = "TELEFONO";
                audit.id_equipo = det.telefono_id;
                audit.accion = "INCIDENTE";
                audit.usuario = User.Identity.Name;

                audit_dom.Guardar(audit);
                //---//

                return View();
            }
           
        }
        public ActionResult ErrorPantalla()
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

        public ActionResult GuardarTel(DTO_u_detalle_telefono deta, bool test)
        {
            if (test)
            {
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

                return View();
            }
            else
            {
                return Redirect("~/Computadora/Check");
            }
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
            return View();
        }

    }
}