using Negocio.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Negocio.Dominio;
using Model;
using Negocio.DTO;

namespace login_v6.Controllers
{
    [Authorize]
    public class ImpresoraController : Controller
    {
        stores store = new stores();
        
        // GET: Impresora
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Preventivo()
        {
            mantenimiento_dominio mante = new mantenimiento_dominio();
            List<DTO_preventivo_impre> lista = new List<DTO_preventivo_impre>();
            var m = mante.Listari();

            foreach (var a in m)
            {
                DTO_preventivo_impre dto = new DTO_preventivo_impre();
                var nombre = store.nombreImpresora(a.id_impresora);
                var ubi = store.nombreUbicacionImpresora(a.id_impresora);

                dto.nombre = nombre;
                dto.fecha_mantenimiento = a.fecha_mantenimiento;
                dto.pospuesto_por = a.pospuesto_por;
                dto.fecha_pospuso = a.fecha_pospuesto;
                dto.descripcion = a.descripcion;
                dto.realizado_por = a.realizado_por;
                dto.proximo_mantenimiento = a.proximo_mantenimiento;
                dto.ubicacion = ubi.nombre;
                dto.detalle = ubi.descripcion;

                lista.Add(dto);
            }

            return View(lista);
        }

        public ActionResult Editar(int id)
        {
            var det = store.u_impresora(id);

            impresora_dominio modelo = new impresora_dominio();
            ubicacion_dominio ubi = new ubicacion_dominio();

            ViewBag.modelo = new SelectList(modelo.Listar(), "id", "marca_modelo", det.mmarca);
            ViewBag.ubicacion = new SelectList(ubi.Listar(), "id", "nombreUbicacion", det.ubicacion);

            return View(det);
        }

        public ActionResult GuardarImp(DTO_Impresora_Editar dto, bool test)
        {
            if (test)
            {
                ubicacion_impresora_dominio im = new ubicacion_impresora_dominio();
                var impresora = im.Obtener(dto.id_ubicacion_impresora);

                impresora.descripcion = dto.descripcion;
                impresora.fecha_ubicacion = dto.fecha_ubicacion;

                im.Guardar(impresora); //cabecera



                detalle_impresora_ubicacion_dominio imp = new detalle_impresora_ubicacion_dominio();
                var imp_deta = imp.Obtener(dto.id_detalle_ubicacion_impresora);

                imp_deta.ip = dto.ip;
                imp_deta.pc_dondeseconecta = dto.pc_donde_se_conecta;

                imp.Guardar(imp_deta); //detalle


                

                //AUDITORIA
                Auditoria audit = new Auditoria();
                auditoria_dominio audit_dom = new auditoria_dominio();

                audit.fecha_hora = DateTime.Now;
                audit.tipo_equipo = "IMPRESORA";
                audit.id_equipo = impresora.id;
                audit.accion = "EDITAR";
                audit.usuario = User.Identity.Name;

                audit_dom.Guardar(audit);
                //---//
                return View();
            }
            else
            {
                return Redirect("~/Home/Check");
            }

        }


        [HttpPost]
        public ActionResult PosponerMante(int id1, int dias)
        {
            mantenimiento_dominio mante = new mantenimiento_dominio();
            var mantenimiento = mante.ObtenerManteImpre(id1);

            mantenimiento.proximo_mantenimiento = DateTime.Now.AddDays(dias);
            mantenimiento.fecha_pospuesto = DateTime.Now;
            mantenimiento.pospuesto = true;
            mantenimiento.pospuesto_por = User.Identity.Name;

            mante.Guardari(mantenimiento);
            return Redirect("~/Inicio/Index");
        }

        public ActionResult ManteImpre(int id_impre, string descripcion1)
        {
            mantenimiento_dominio mante = new mantenimiento_dominio();
            var mantenimiento = mante.ObtenerManteImpre(id_impre);

            mantenimiento.realizado = true;
            mantenimiento.realizado_por = User.Identity.Name;
            mantenimiento.descripcion = descripcion1;
            mantenimiento.fecha_mantenimiento = DateTime.Now;
            mantenimiento.proximo_mantenimiento = DateTime.Now;

            mante.Guardari(mantenimiento);

            var proximo = new mantenimiento_impre();

            proximo.id_impresora = id_impre;
            proximo.pospuesto = false;

            var ubicacion = store.nombreUbicacionImpresora(id_impre);

            if (ubicacion.nombre == "GUARDIA" || ubicacion.nombre == "ADMISION")
            {
                proximo.proximo_mantenimiento = DateTime.Now.AddMonths(4);
            }
            else
            {
                proximo.proximo_mantenimiento = DateTime.Now.AddMonths(6);
            }

            
            proximo.fecha_mantenimiento = DateTime.Now;
            proximo.descripcion = "A realizarse";
            proximo.realizado = false;

            mante.Guardari(proximo);

            return Redirect("~/Inicio/Index");
        }




    }
}