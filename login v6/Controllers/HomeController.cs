﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Negocio;
using Negocio.Dominio;
using Negocio.Servicio;
using Negocio.DTO;
using System.Web.Security;

namespace SGEI.Controllers
{
    
    public class HomeController : Controller
    {
        public insumo_dominio insu = new insumo_dominio();
        public impresora_dominio imp = new impresora_dominio();
        public ubicacion_dominio ubi = new ubicacion_dominio();
        public detalle_impresora_ubicacion_dominio deta = new detalle_impresora_ubicacion_dominio();
        public ubicacion_impresora_dominio ubi_imp = new ubicacion_impresora_dominio();

        public ActionResult Index()
        {
            var user = User.Identity.Name;

            ViewBag.ubicaciones = ubi.Listar();
            ViewBag.insumos = insu.Listar();

            return View();
        }

        public ActionResult Landing()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AltaTel()
        {
            tipo_telefono_dominio tipot = new tipo_telefono_dominio();
            mmarca_dominio mmarca = new mmarca_dominio();
            ubicacion_tel_dominio ubitel = new ubicacion_tel_dominio();

            ViewBag.modelo = mmarca.Listar();
            ViewBag.tipo = tipot.Listar();
            ViewBag.ubicacion = ubitel.Listar();


            DTO_Telefono dto = new DTO_Telefono();

            return View(dto);
        }

        public ActionResult GuardarTel(DTO_Telefono dto, bool test)
        {
            if (test)
            {
                telefono_dominio tel = new telefono_dominio();
                telefono tele = new telefono();

                tele.descripcion = dto.descripcion;
                tel.Guardar(tele); //Cabecera

                detalle_tel_dominio dt = new detalle_tel_dominio();
                detalle_tel detalle = new detalle_tel();

                int id_telefono = tel.ObtenerUltimo();

                detalle.nro_interno = Convert.ToString(dto.interno);
                detalle.marca_modelo_id = dto.mmarca;
                detalle.tipo_tel_id = dto.tipo_tel;
                detalle.fecha_detalle = DateTime.Now;
                detalle.telefono_id = id_telefono;

                dt.Guardar(detalle);  //Detalle

                ubicacion_tel_tel_dominio ubi = new ubicacion_tel_tel_dominio();
                ubicacion_tel_tel ubicacion = new ubicacion_tel_tel();

                int id_detalle = ubi.ObtenerUltimo();

                ubicacion.ubicacion_tel_id = dto.ubicacion;
                ubicacion.fecha_ubicacion = DateTime.Now;
                ubicacion.detalle_id = id_detalle;

                ubi.Guardar(ubicacion); //Guardo la ubicacion

                return RedirectToAction("AltaExitosaTel");
            }
            else
            {
                return Redirect("~/Computadora/Check");
            }

        }
        public ActionResult AltaExitosaTel()
        {
            return View();
        }

        public ActionResult AltaCompu(int id = 0)
        {
            tipopc_dominio tipo = new tipopc_dominio();
            ram_dominio mem = new ram_dominio();
            disco_dominio hdd = new disco_dominio();
            so_dominio sisop = new so_dominio();
            motherboard_dominio mth = new motherboard_dominio();
            ubicacionpc_dominio ubi = new ubicacionpc_dominio();
            procesador_dominio procesador = new procesador_dominio();

            DTO_PC dto = new DTO_PC();

            ViewBag.tipo = tipo.Listar();
            ViewBag.ram = mem.Listar();
            ViewBag.disco = hdd.Listar();
            ViewBag.so = sisop.Listar();
            ViewBag.pm = mth.Listar();
            ViewBag.ubi = ubi.Listar();
            ViewBag.proce = procesador.Listar();

            return View(dto);
        }

        public ActionResult GuardarCompu(DTO_PC dto, int tipo, int ram, int disco, int so, int moth, int ubicacion, int proce, bool test)
        {
            if (test)
            {
                pc_dominio compu = new pc_dominio();

                pc pc = new pc();

                pc.ip = dto.ip;
                pc.nombre = dto.nombrepc;
                pc.descripcion = dto.descripcion;
                pc.tipo_id = tipo;

                compu.Guardar(pc);      // Guardo la cabecera

                int id_compu = compu.ObtenerUltimo(); // obtengo el id de la cabecera

                detallepc_dominio det = new detallepc_dominio();

                detallePC detalle = new detallePC();

                detalle.observacion = dto.observacion;
                detalle.responsablepc = dto.responsable;
                detalle.pc_id = id_compu;
                detalle.ram_id = ram;
                detalle.so_id = so;
                detalle.disco_id = disco;
                detalle.mother_id = moth;
                detalle.procesador_id = proce;
                detalle.fecha_detalle = DateTime.Now;

                det.Guardar(detalle); // guardo el detalle de la cabecera

                ubicacionpc_pc_dominio ubipc = new ubicacionpc_pc_dominio();
                pc_ubicacionpc pcubicacion = new pc_ubicacionpc();

                pcubicacion.pc_id = id_compu;
                pcubicacion.ubicacionpc_id = ubicacion;
                pcubicacion.fecha_ubicacion = DateTime.Now;

                ubipc.Guardar(pcubicacion); // guardo la ubicacion de la pc


                return Redirect("~/home/AltaExitosa");
            }
            else
            {
                return Redirect("~/Computadora/Check");
            }

        }



        public ActionResult AltaExitosa()
        {
            return View();
        }


        public ActionResult Ver(int id)
        {

            return View(imp.Obtener(id));
        }

        [HttpPost]
        public ActionResult Buscar(int ubicacion)
        {
            stores store = new stores();
            var datos = store.u_impresora_01(ubicacion);

            return View(datos);

        }

        public ActionResult Baja(int id)
        {
            ViewBag.ubicacion = id;
            var baja = new baja();

            return View(baja);
        }

        public ActionResult Baja_aceptada(baja baja)
        {

            var baja_aceptada = new baja_dominio();
            var ubi = new ubicacion_impresora_dominio();

            var ubicacion_impr = ubi.Obtener(Convert.ToInt32(baja.ubicacion_impresora_id));
            ubicacion_impr.fecha_baja = DateTime.Now;

            ubi.Guardar(ubicacion_impr);

            baja_aceptada.Guardar(baja);

            return Redirect("~/home");

        }

        public ActionResult Crud(int id = 0) //Este es el ABM impresora
        {
            ViewBag.impresoras = imp.Listar();
            return View(id == 0 ? new impresora()
                        : imp.Obtener(id));
        }

        public ActionResult BuscarParaMover(int id)
        {

            TempData["impresora"] = id;
            var ubicacion_nueva = new ubicacion_dominio();
            ViewBag.ubicacion = ubicacion_nueva.Listar();

            return View();
        }

        public ActionResult Mover(int id_nueva_ubicacion)
        {
            int id_ubicacion_impresora = Convert.ToInt32(TempData["impresora"]);
            var impresora = new u_impresora_dominio();

            impresora.mover(id_ubicacion_impresora, id_nueva_ubicacion);
            return Redirect("~/home");
        }

        public ActionResult CrudDetalle_Impresora_Ubicacion(int id = 0) //Este es el ABM detalle_impresora
        {
            ViewBag.id_ubicacion = TempData["id_ubicacion"];
            return View(id == 0 ? new detalle_impresora_ubicacion()
                        : deta.Obtener(id));
        }

        public ActionResult CrudUbicacion_impresora(int id = 0) //Este es el ABM ubicacion_impresora
        {
            ViewBag.id_impr = TempData["id"];

            ViewBag.ubicaciones = ubi.Listar();

            return View(id == 0 ? new ubicacion_impresora()
                        : ubi_imp.Obtener(id));
        }

        public ActionResult Guardar(int impresora)
        {
            //model.Guardar();
            TempData["id"] = impresora;
            return RedirectToAction("CrudUbicacion_impresora");
        }

        public ActionResult GuardarDetalle(detalle_impresora_ubicacion model)
        {
            deta.Guardar(model);
            //TempData["id_impre"] = model.impresora_id;
            return Redirect("~/home");
        }

        public ActionResult GuardarUbicacion(ubicacion_impresora model)
        {
            ubi_imp.Guardar(model);
            TempData["id_ubicacion"] = model.id;

            return RedirectToAction("CrudDetalle_Impresora_Ubicacion");
        }

        public ActionResult Eliminar(impresora model)
        {
            imp.Eliminar(model.id);
            return Redirect("~/home");
        }

        public ActionResult impresoras_baja()
        {
            stores store = new stores();

            var datos = store.u_impresora_04();

            return View(datos);
        }

        public ActionResult Alta(int id)
        {
            ViewBag.ubicaciones = ubi.Listar();

            var datos = new ubicacion_impresora_dominio();
            var ub_impr = datos.Obtener(id);


            return View(ub_impr);
        }

        public ActionResult Alta_aceptada(ubicacion_impresora ubi)
        {

            ubi.fecha_baja = null;

            var ubi_impr = new ubicacion_impresora_dominio();
            var alta = new baja_dominio();

            ubi_imp.Guardar(ubi);

            //alta.Eliminar(ubi.id);

            return Redirect("~/home");
        }




    }
}