using Negocio.Dominio;
using Negocio.DTO;
using Negocio.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;

namespace login_v6.Controllers
{
    [Authorize]
    public class EscanerController : Controller
    {

        stores store = new stores();
        // GET: Escaner
        public ActionResult Index()
        {
            ubicacion_escaner_dominio ubi = new ubicacion_escaner_dominio();

            ViewBag.ubicaciones = ubi.Listar();
            return View();
        }

        public ActionResult Buscar(int ubicaciones)
        {
            var ubica = store.u_es_01(ubicaciones);
            return View(ubica);
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

        public ActionResult Check()
        {
            return View();
        }



        public ActionResult GuardarES(DTO_u_detalle_escaner deta, bool test)
        {
            if (test)
            {
                escaner_dominio es = new escaner_dominio();
                var escaner = es.Obtener(deta.id_escaner);

                escaner.descripcion = deta.descripcion;
                es.Guardar(escaner); //Cabecera

                detalle_escaner_dominio dt = new detalle_escaner_dominio();
                var detalle = dt.Obtener(deta.id_detalle);

                detalle.marca_modelo_id = deta.id_mm;
                detalle.nroip = deta.nro_ip; //Detalle

                dt.Guardar(detalle);

                return View();
            }
            else
            {
                return Redirect("~/Escaner/Check");
            }
        }

        public ActionResult Incidente(int id)
        {
            incidente_escaner_dominio inci = new incidente_escaner_dominio();
            ViewBag.incidente = inci.Listar();
            ViewBag.detalle_id = id;

            return View(new escaner_incidente_escaner());
        }

        public ActionResult Guardarincidente(escaner_incidente_escaner inci)
        {
            escaner_incidente_escaner incidente = new escaner_incidente_escaner();
            escaner_incidente_escaner_dominio incidenteDominio = new escaner_incidente_escaner_dominio();

            incidente.escaner_id = inci.escaner_id;
            incidente.fecha_incidente = inci.fecha_incidente;
            incidente.fecha_reparacion = inci.fecha_reparacion;
            incidente.reparacion = inci.reparacion;
            incidente.escaner_id = inci.escaner_id;
            incidente.incidente_escaner_id = inci.incidente_escaner_id;

            incidenteDominio.Guardar(incidente);

            return View();
        }


        public ActionResult Baja(int id)
        {
            escaner_dominio esc = new escaner_dominio();
            var escaner = esc.Obtener(id);
            escaner.fecha_baja = DateTime.Now;
            esc.Guardar(escaner);
            return View();
        }





    }
}