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
    public class CamaraController : Controller
    {
        stores store = new stores();
        // GET: Camara
        public ActionResult Index()
        {

            ubicacion_camara_dominio ubi = new ubicacion_camara_dominio();

            ViewBag.ubicacioncam = ubi.Listar();
            return View();

        }

        public ActionResult Buscar(int ubicacioncam)
        {
            var ubica = store.u_camara_01(ubicacioncam);
            return View(ubica);
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



        public ActionResult GuardarCam(DTO_u_detalle_camara deta, bool test)
        {
            if (test)
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

                return View();
            }
            else
            {
                return Redirect("~/Camara/Check");
            }
        }

        public ActionResult Incidente(int id)
        {
            incidente_camara_dominio inci = new incidente_camara_dominio();
            ViewBag.incidente = inci.Listar();
            ViewBag.detalle_id = id;

            return View(new camara_incidente_camara());
        }

        public ActionResult Guardarincidente(camara_incidente_camara inci)
        {
            camara_incidente_camara incidente = new camara_incidente_camara();
            camara_incidente_camara_dominio incidenteDominio = new camara_incidente_camara_dominio();

            incidente.camara_id = inci.camara_id;
            incidente.fecha_incidente = inci.fecha_incidente;
            incidente.fecha_reparacion = inci.fecha_reparacion;
            incidente.reparacion = inci.reparacion;
            incidente.incidente_id = inci.incidente_id;

            incidenteDominio.Guardar(incidente);

            return View();
        }

        public ActionResult Baja(int id)
        {
            camara_dominio cam = new camara_dominio();
            var camara = cam.Obtener(id);
            camara.fecha_baja = DateTime.Now;
            cam.Guardar(camara);
            return View();
        }

        public ActionResult Check()
        {
            return View();
        }
    }

}