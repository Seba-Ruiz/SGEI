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
    public class ImpresoraController : Controller
    {
        stores store = new stores();
        // GET: Impresora
        public ActionResult Index()
        {
            return View();
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





    }
}