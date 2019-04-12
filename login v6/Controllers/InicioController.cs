using Negocio.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace login_v6.Controllers
{
    [Authorize]
    public class InicioController : Controller
    {
        servicio serv = new servicio();
        stores store = new stores();

        public ActionResult Index()
        {
            ViewBag.impresoras = serv.sumaImpresora();
            ViewBag.computadoras = serv.sumaComputadora();
            ViewBag.telefonos = serv.sumaTelefono();
            ViewBag.camaras = serv.sumaCamara();
            ViewBag.sw = serv.sumaSW();
            ViewBag.es = serv.sumaES();

            var a_mantenimiento = store.mantenimiento();

            return View(a_mantenimiento);
        }
    }
}