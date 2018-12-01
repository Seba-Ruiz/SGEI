using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
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
            ViewBag.insumoimpresora = insumo_impresora.Listar();

            proveedor_dominio proveedor = new proveedor_dominio();
            ViewBag.proveedor = proveedor.Listar();

            return View();
        }
        public ActionResult IndexCompu()
        {
            insumo_pc_dominio insumo_pc = new insumo_pc_dominio();
            ViewBag.insumopc = insumo_pc.Listar();

            proveedor_dominio proveedor = new proveedor_dominio();
            ViewBag.proveedor = proveedor.Listar();

            return View();
        }
        public ActionResult IndexCam()
        {
            insumo_cam_dominio insumo_cam = new insumo_cam_dominio();
            ViewBag.insumocam = insumo_cam.Listar();

            proveedor_dominio proveedor = new proveedor_dominio();
            ViewBag.proveedor = proveedor.Listar();

            return View();
        }

        [HttpPost]
        public ActionResult GuardarInsumoImpresora(int id, string od, string cantidad, int id_proveedor)
        {

            if (od=="0" || cantidad == "" || Convert.ToInt32(cantidad) <= 0 || Convert.ToInt32(od) <= 0)
            {
                return RedirectToAction("ErrorPantalla");
            }
            else
            {
                //AUDITORIA
                Auditoria audit = new Auditoria();
                auditoria_dominio audit_dom = new auditoria_dominio();

                audit.fecha_hora = DateTime.Now;
                audit.tipo_equipo = "INSUMO_IMP";
                audit.id_equipo = id;
                audit.accion = "INGRESO_INSUMO";
                audit.usuario = User.Identity.Name;

                audit_dom.Guardar(audit);
                //---//

                insumo_dominio insu = new insumo_dominio();
                var insumo = insu.Obtener(id);

                IngresoInsumo ingreso = new IngresoInsumo();
                ingreso_dominio ingreso_dom = new ingreso_dominio();

                ingreso.id_insumo = id;
                ingreso.fecha_ingreso = DateTime.Now;
                ingreso.id_proveedor = id_proveedor;
                ingreso.cantidad = Convert.ToInt32(cantidad);
                ingreso.tipo_equipo = "IMPRESORA";
                ingreso.nro_comprobante = Convert.ToInt32(od);
                ingreso.nombre_insumo = insumo.nombre;

                proveedor_dominio prov = new proveedor_dominio();
                var proveedor = prov.Obtener(id_proveedor);

                ingreso.proveedor = proveedor.nombre;

                ingreso_dom.Guardar(ingreso);

                insumo.cantidad = insumo.cantidad + Convert.ToInt32(cantidad);
                insu.Guardar(insumo);

                return View();
            }
           
        }


        [HttpPost]
        public ActionResult GuardarInsumoComputadora(int id, string od, string cantidad, int id_proveedor)
        {

            if (od == "0" || cantidad == "" || Convert.ToInt32(cantidad) <= 0 || Convert.ToInt32(od) <= 0)
            {
                return RedirectToAction("ErrorPantalla");
            }
            else
            {
                //AUDITORIA
                Auditoria audit = new Auditoria();
                auditoria_dominio audit_dom = new auditoria_dominio();

                audit.fecha_hora = DateTime.Now;
                audit.tipo_equipo = "INSUMO_COMP";
                audit.id_equipo = id;
                audit.accion = "INGRESO_INSUMO";
                audit.usuario = User.Identity.Name;

                audit_dom.Guardar(audit);
                //---//

                insumo_pc_dominio insu = new insumo_pc_dominio();
                var insumo = insu.Obtener(id);

                IngresoInsumo ingreso = new IngresoInsumo();
                ingreso_dominio ingreso_dom = new ingreso_dominio();

                ingreso.id_insumo = id;
                ingreso.fecha_ingreso = DateTime.Now;
                ingreso.id_proveedor = id_proveedor;
                ingreso.cantidad = Convert.ToInt32(cantidad);
                ingreso.tipo_equipo = "COMPUTADORA";
                ingreso.nro_comprobante = Convert.ToInt32(od);
                ingreso.nombre_insumo = insumo.nombre;

                proveedor_dominio prov = new proveedor_dominio();
                var proveedor = prov.Obtener(id_proveedor);

                ingreso.proveedor = proveedor.nombre;

                ingreso_dom.Guardar(ingreso);

                insumo.cantidad = insumo.cantidad + Convert.ToInt32(cantidad);
                insu.Guardar(insumo);

                return View();
            }

        }

        [HttpPost]
        public ActionResult GuardarInsumoCam(int id, string od, string cantidad, int id_proveedor)
        {

            if (od == "0" || cantidad == "" || Convert.ToInt32(cantidad) <= 0 || Convert.ToInt32(od) <= 0)
            {
                return RedirectToAction("ErrorPantalla");
            }
            else
            {
                //AUDITORIA
                Auditoria audit = new Auditoria();
                auditoria_dominio audit_dom = new auditoria_dominio();

                audit.fecha_hora = DateTime.Now;
                audit.tipo_equipo = "INSUMO_CAM";
                audit.id_equipo = id;
                audit.accion = "INGRESO_INSUMO";
                audit.usuario = User.Identity.Name;

                audit_dom.Guardar(audit);
                //---//

                insumo_cam_dominio insu = new insumo_cam_dominio();
                var insumo = insu.Obtener(id);

                IngresoInsumo ingreso = new IngresoInsumo();
                ingreso_dominio ingreso_dom = new ingreso_dominio();

                ingreso.id_insumo = id;
                ingreso.fecha_ingreso = DateTime.Now;
                ingreso.id_proveedor = id_proveedor;
                ingreso.cantidad = Convert.ToInt32(cantidad);
                ingreso.tipo_equipo = "CAMARA";
                ingreso.nro_comprobante = Convert.ToInt32(od);
                ingreso.nombre_insumo = insumo.nombre;

                proveedor_dominio prov = new proveedor_dominio();
                var proveedor = prov.Obtener(id_proveedor);

                ingreso.proveedor = proveedor.nombre;

                ingreso_dom.Guardar(ingreso);

                insumo.cantidad = insumo.cantidad + Convert.ToInt32(cantidad);
                insu.Guardar(insumo);

                return View();
            }

        }


        public ActionResult ErrorPantalla()
        {

            return View();

        }

        public ActionResult VerIngresoPC()
        {
            ingreso_dominio ingreso = new ingreso_dominio();
            var ing = ingreso.ListarIngresoPC();



            return View(ing);
        }
        public ActionResult VerIngresoImpresora()
        {
            ingreso_dominio ingreso = new ingreso_dominio();
            var ingImp = ingreso.ListarIngresoImpresoras();



            return View(ingImp);
        }

        public ActionResult VerIngresoCam()
        {
            ingreso_dominio ingreso = new ingreso_dominio();
            var ing = ingreso.ListarIngresoCam();



            return View(ing);
        }
       

    }
}