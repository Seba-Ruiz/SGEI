using Negocio.Servicio;
using System;
using System.Collections.Generic;
using System.Data;
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
        public ActionResult Sugeridos()
        {
            
            var a_mantenimiento = store.mantenimiento();

            return View(a_mantenimiento);
        }

        public ActionResult Dashboard()
        {

            

            return View();
        }

        [HttpPost]
        public JsonResult NewChart()
        {
            List<object> iData = new List<object>();
            //Creating sample data  
            DataTable dt = new DataTable();
            dt.Columns.Add("Employee", System.Type.GetType("System.String"));
            dt.Columns.Add("Credit", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["Employee"] = "Sam";
            dr["Credit"] = 123;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Employee"] = "Alex";
            dr["Credit"] = 456;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Employee"] = "Michael";
            dr["Credit"] = 587;
            dt.Rows.Add(dr);
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }


    }
}