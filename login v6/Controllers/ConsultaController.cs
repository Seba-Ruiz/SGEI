using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Negocio.Dominio;
using Negocio.Servicio;

namespace login_v6.Controllers
{
    [Authorize]
    public class ConsultaController : Controller
    {
        public ubicacion_dominio ubi = new ubicacion_dominio();
        public insumo_dominio insu = new insumo_dominio();
        public incidente_dominio inci = new incidente_dominio();
        public insumo_pc_dominio insupc = new insumo_pc_dominio();
        public ubicacionpc_dominio ubipc = new ubicacionpc_dominio();
        public ubicacion_camara_dominio ubicam = new ubicacion_camara_dominio();
        public ubicacion_sw_dominio ubisw = new ubicacion_sw_dominio();
        public ubicacion_escaner_dominio ubies = new ubicacion_escaner_dominio();
        public insumo_cam_dominio insucam = new insumo_cam_dominio();
        stores store = new stores();


        // CONSULTAS DE CAMARA
        public ActionResult Camara()
        {
            ViewBag.ubicaciones = ubicam.Listar();
            ViewBag.insucam = insucam.Listar();
            return View();
        }

        public ActionResult BuscarCam(int ubicacion)
        {

            var datos = store.u_camara_01(ubicacion);

            return View(datos);

        }
        public ActionResult ConsultarFechaCam(int id)
        {

            TempData["id_ubicacion_camara"] = id;

            return View();

        }
        public ActionResult ConsultarRangoCam(DateTime? FechaDesde, DateTime? FechaHasta)
        {
            if (FechaDesde > FechaHasta || FechaHasta == null || FechaDesde == null)
            {
                return RedirectToAction("ErrorPantallaCam");
            }
            else
            {
                var ubi_camara = TempData["id_ubicacion_camara"];
                int u_cam_id = Convert.ToInt32(ubi_camara);

                var datos = store.consultacam_01a(u_cam_id, FechaDesde, FechaHasta);

                return View(datos);
            }
        }
        public ActionResult BuscarIncidenteCam(int ubicacion)
        {

            var datos = store.u_camara_01(ubicacion);

            return View(datos);

        }

        public ActionResult VerIncidenteCam(int id)
        {


            var consulta = store.incidente_camara(id);

            return View(consulta);

        }
        //-----------------------------------//



        // CONSULTAS DE SWITCHS 
        public ActionResult switchs()
        {
            ViewBag.ubicaciones = ubisw.Listar();
            return View();
        }

       
        public ActionResult ConsultarSW(int id)
        {
            TempData["id_ubicacion_camara"] = id;
            return View();
        }
        public ActionResult ConsultarRangoSW(DateTime? FechaDesde, DateTime? FechaHasta)
        {
            if (FechaDesde > FechaHasta || FechaHasta == null || FechaDesde == null)
            {
                return RedirectToAction("ErrorPantallaSW");
            }
            else
            {
                var ubi_camara = TempData["id_ubicacion_camara"];
            int u_cam_id = Convert.ToInt32(ubi_camara);

            var datos = store.consultacam_01a(u_cam_id, FechaDesde, FechaHasta);

            return View(datos);
            }
        }
        public ActionResult BuscarIncidenteSW(int ubicacion)
        {

            var datos = store.u_switch_01(ubicacion);

            return View(datos);

        }

        public ActionResult VerIncidenteSW(int id)
        {
            var consulta = store.incidente_sw(id);

            return View(consulta);
        }
        //-----------------------------------//



        // CONSULTAS DE ESCANER 
        public ActionResult escaners()
        {
            ViewBag.ubicaciones = ubies.Listar();
            return View();
        }

        public ActionResult BuscarIncidenteES(int ubicacion)
        {
            var datos = store.u_escaner_01(ubicacion);

            return View(datos);

        }

        public ActionResult VerIncidenteES(int id)
        {
            var consulta = store.incidente_es(id);

            return View(consulta);
        }
        //-----------------------------------//











        public ActionResult Index()
        {
            ViewBag.ubicaciones = ubi.Listar();
            ViewBag.insumos = insu.Listar();
            return View();
        }

        public ActionResult Buscar(int ubicacion)
        {
           
            var datos = store.u_impresora_01(ubicacion);

            return View(datos);

        }

        public ActionResult BuscarIncidente(int ubicacion)
        {

            var datos = store.u_impresora_01(ubicacion); 

            return View(datos);

        }

        public ActionResult VerIncidente(int id)
        {

            incidente_dominio datos = new incidente_dominio();

            var consulta = datos.verIncidente(id);

            return View(consulta);

        }
        public ActionResult ConsultarFecha(int id)
        {

            TempData["id_ubicacion_impresora"] = id;

            return View();

        }

        public ActionResult VerImagen(int id)
        {
            var context = new SGEIContext();
            byte[] imageData = context.imagen.FirstOrDefault(i => i.incidente_id == id)?.imagen1;
            if (imageData != null)
            {
                //ViewBag.coso = File(imageData, "image/png");
                return File(imageData, "image/png"); // Might need to adjust the content type based on your actual image type

            }
            return null;
        }


        public ActionResult ConsultarRango(DateTime? FechaDesde, DateTime? FechaHasta)
        {
            if (FechaDesde > FechaHasta || FechaHasta == null || FechaDesde == null)
            {
                return RedirectToAction("ErrorPantallaImp");
            }
            else
            {

                var ubi_impresora = TempData["id_ubicacion_impresora"];
                int u_imp_id = Convert.ToInt32(ubi_impresora);

                var datos = store.consulta_01a(u_imp_id, FechaDesde, FechaHasta);

                return View(datos);
            }

        }

        public ActionResult Computadoras()
        {
            ViewBag.insumos = insupc.Listar();
            ViewBag.ubicaciones = ubipc.Listar();

            return View();
        }

        public ActionResult InsumosPCporFecha(int ubicacion)
        {
            
                var ubica = store.u_computadora(ubicacion);
                return View(ubica);
            

        }

        public ActionResult ConsultarRangoFechaPC(int id)
        {
            TempData["id_detalle_pc"] = id;

            return View();
        }
        public ActionResult ConsultarRangoPC(DateTime? FechaDesde, DateTime? FechaHasta)
        {
            if (FechaDesde>FechaHasta || FechaHasta==null ||FechaDesde==null)
            {
                return RedirectToAction("ErrorPantalla");
            }
            else
            {
                var det_pc = TempData["id_detalle_pc"];
                int detalle_id = Convert.ToInt32(det_pc);
                var datos = store.consulta_pc(detalle_id, FechaDesde, FechaHasta);

                return View(datos);
            }

        }

        public ActionResult ErrorPantalla()
        {

            return View();
        }
        public ActionResult ErrorPantallaCam()
        {

            return View();
        }
        public ActionResult ErrorPantallaSW()
        {

            return View();
        }
        public ActionResult ErrorPantallaImp()
        {

            return View();
        }

        public ActionResult IncidentesPcporFecha(int ubicacion)
        {
           
                var ubica = store.u_computadora(ubicacion);
                return View(ubica);
            
        }

        public ActionResult ConsultarRangoFechaPCIncidente(int id)
        {
            TempData["id_detalle_pc_incidente"] = id;

            return View();
        }

        public ActionResult ConsultarRangoPCIncidente(DateTime? FechaDesde, DateTime? FechaHasta)
        {
            if (FechaDesde > FechaHasta || FechaHasta == null || FechaDesde == null)
            {
                return RedirectToAction("ErrorPantalla");
            }
            else
            {
                var det_pc = TempData["id_detalle_pc_incidente"];

                int detalle_id = Convert.ToInt32(det_pc);

                var detalle_pc_dominio = new detallepc_dominio();
                var consulta = detalle_pc_dominio.Obtener(detalle_id);
                var id_pc = consulta.pc_id;

                var datos = store.consulta_pc_detalle(id_pc, FechaDesde, FechaHasta);

                return View(datos);
            }
        }

        public ActionResult compuBaja()
        {

            servicios serv = new servicios();
            var pcsdebaja = serv.computadoras_de_baja();



            return View(pcsdebaja);
        }

        public ActionResult AltaPc(int id)
        {
            ubicacionpc_dominio ubicacion = new ubicacionpc_dominio();
            ViewBag.ubicaciones = ubicacion.Listar();

            ubicacionpc_pc_dominio ubi = new ubicacionpc_pc_dominio();
            var ubica = ubi.ObteneryCambiar(id);

            return View(ubica);
        }

        public ActionResult Alta_aceptada_pc(pc_ubicacionpc coso)
        {

            ubicacionpc_pc_dominio ubi = new ubicacionpc_pc_dominio();
            ubi.Guardar(coso);  //Guardo la Nueva Ubicacion

            var idpc = coso.pc_id;
            pc_dominio pc = new pc_dominio();
            var compu = pc.Obtener(Convert.ToInt32(idpc)); //Busco la PC para hacer Null la fecha de Baja

            compu.fecha_baja = null;

            pc.Guardar(compu);   //Guardo el cambio de la fecha


            return View();
        }


        public ActionResult Telefonos()
        {
            ubicacion_tel_dominio ubi = new ubicacion_tel_dominio();
            ViewBag.ubicaciontel = ubi.Listar();

            mmarca_dominio tel = new mmarca_dominio();
            ViewBag.telefonos = tel.Listar();

            return View();
        }

        public ActionResult BuscarTel(int ubicaciontel)
        {
            
                var ubica = store.u_telefono_01(ubicaciontel);
                return View(ubica);
            

        }

        public ActionResult ConsultarRangoFechaTelIncidente(int id)
        {
            TempData["id_detalle_tel_incidente"] = id;

            return View();
        }

        public ActionResult ConsultarRangoTelIncidente(DateTime? FechaDesde, DateTime? FechaHasta)
        {
            if (FechaDesde > FechaHasta || FechaHasta == null || FechaDesde == null)
            {
                return RedirectToAction("ErrorPantalla");
            }
            else
            {
                var det_tel = TempData["id_detalle_tel_incidente"];

                var datos = store.consulta_tel_incidente(Convert.ToInt32(det_tel), FechaDesde, FechaHasta);

                return View(datos);
            }
        }
    }
}