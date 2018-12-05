using Negocio.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace login_v6.Controllers
{
    public class BuscarController : Controller
    {
        stores store = new stores();
        // GET: Buscar
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Buscar_PC()
        {
            return View();
        }
        public ActionResult Buscar_Insu()
        {

            return View();
        }
        public ActionResult BuscarInsumoSerie(string nro_serie)
        {
            
            var dto = store.buscar_insumo_serie(nro_serie);

            if (dto == null)
            {

                return RedirectToAction("ErrorPantallaInsumo");

            }
            else
            {
                return View(dto);
            }

        }

        public ActionResult Buscar_PC_IP()
        {
            return View();
        }
        public ActionResult BuscarNumero(string numero)
        {
            var id = Convert.ToInt32(numero);
            var dto = store.buscar_pc(id);

            if (dto==null)
            {

                return RedirectToAction("ErrorPantalla");

            }
            else
            {
                return View(dto);
            }
        }

        public ActionResult BuscarIP(string numero)
        {
            
            var dto = store.buscar_pc_ip(numero);

            if (dto == null)
            {

                return RedirectToAction("ErrorPantalla");

            }
            else
            {
                return View(dto);
            }
        }

        public ActionResult ErrorPantalla()
        {

            return View();
        }

        public ActionResult ErrorPantallaIP()
        {

            return View();
        }
        public ActionResult ErrorPantallaInsumo()
        {

            return View();
        }


    }
}