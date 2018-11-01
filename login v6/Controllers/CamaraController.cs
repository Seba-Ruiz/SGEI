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



        public ActionResult Check()
        {
            return View();
        }
    }












}