using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Negocio.Dominio;
using Negocio.DTO;

namespace login_v6.Controllers
{
    [Authorize]
    public class IngresosController : Controller
    {
        // GET: Ingresos
        public ActionResult Index()
        {
            insumo_dominio insumo_impresora = new insumo_dominio();
            insumo_cam_dominio insumo_camara = new insumo_cam_dominio();
            insumo_pc_dominio insumo_computadora = new insumo_pc_dominio();

            ViewBag.insumoimpresora = insumo_impresora.Listar();
            ViewBag.insumocompu = insumo_computadora.Listar();
            ViewBag.insumocamara = insumo_camara.Listar();
            return View();
        }
        
        public ActionResult AgregarInsumoImpresora(int id)
        {


            return View();
        }
        
       
    }
}