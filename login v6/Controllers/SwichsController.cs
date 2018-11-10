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
    public class SwichsController : Controller
    {
        stores store = new stores();

        // GET: Switch
        public ActionResult Index()
        {
            ubicacion_sw_dominio ubi = new ubicacion_sw_dominio();

            ViewBag.ubicacionsw = ubi.Listar();
            return View();

        }
        public ActionResult Buscar(int ubicacionsw)
        {
            var ubica = store.u_sw_01(ubicacionsw);
            return View(ubica);
        }



        public ActionResult Guardarincidente(switch_incidente_switch inci)
        {
            switch_incidente_switch incidente = new switch_incidente_switch();
            switch_incidente_switch_dominio incidenteDominio = new switch_incidente_switch_dominio();

            incidente.switch_id = inci.switch_id;
            incidente.fecha_incidente = inci.fecha_incidente;
            incidente.fecha_reparacion = inci.fecha_reparacion;
            incidente.reparacion = inci.reparacion;
            incidente.incidente_switch_id = inci.incidente_switch_id;

            incidenteDominio.Guardar(incidente);

            return View();
        }


        public ActionResult Editar(int id)
        {
            var det = store.u_detalle_sw(id);

            mmarca_sw_dominio modelo = new mmarca_sw_dominio();
            ubicacion_sw_dominio ubi = new ubicacion_sw_dominio();

            ViewBag.modelo = new SelectList(modelo.Listar(), "id_mmarca", "marca_modelo", det.id_mm);
            ViewBag.ubicacion = new SelectList(ubi.Listar(), "id_ubicacion_switch", "nombre", det.id_ubicacion_sw);

            return View(det);
        }


        public ActionResult GuardarSW(DTO_u_detalle_sw deta, bool test)
        {
            if (test)
            {
                switch_dominio sw = new switch_dominio();
                var switc = sw.Obtener(deta.id_sw);

                switc.descripcion = deta.descripcion;
                sw.Guardar(switc); //Cabecera

                detalle_sw_dominio dt = new detalle_sw_dominio();
                var detalle = dt.Obtener(deta.id_detalle);

                detalle.marca_modelo_id = deta.id_mm;

                detalle.nroip = deta.nro_ip; //Detalle
                detalle.interfaces = deta.interfaces;

                dt.Guardar(detalle);

                return View();
            }
            else
            {
                return Redirect("~/Swichs/Check");
            }
        }


        public ActionResult Incidente(int id)
        {
            incidente_sw_dominio inci = new incidente_sw_dominio();
            ViewBag.incidente = inci.Listar();
            ViewBag.detalle_id = id;

            return View(new switch_incidente_switch());
        }

        public ActionResult Baja(int id)
        {
            switch_dominio sw = new switch_dominio();
            var switc = sw.Obtener(id);
            switc.fecha_baja = DateTime.Now;
            sw.Guardar(switc);
            return View();
        }

        public ActionResult Check()
        {
            return View();
        }


    }
}