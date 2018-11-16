using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Negocio.Dominio;
using Model;
using Negocio.Servicio;
using Negocio.DTO;

namespace login_v6.Controllers
{
    [Authorize]
    public class ComputadoraController : Controller
    {
        stores store = new stores();

        // GET: Computadora
        public ActionResult Index()
        {
            ubicacionpc_dominio ubi = new ubicacionpc_dominio();
            ViewBag.ubicacionpc = ubi.Listar();
            return View();
        }

        public ActionResult Buscar(int ubicacionpc)
        {
           

            var ubica = store.u_computadora(ubicacionpc);

            return View(ubica);


        }
        public ActionResult Detalle(int id)
        {

            var ubic = store.u_computadora_detalle(id);

            return View(ubic);
        }



        public ActionResult Incidente(int id)
        {
            detallepc_dominio dt = new detallepc_dominio();
            pc_dominio pc = new pc_dominio();
            incidentepc_dominio inci = new incidentepc_dominio();


            var detalle = dt.Obtener(id);
            var compu = pc.Obtener(Convert.ToInt32(detalle.pc_id));

            ViewBag.id_pc = compu.id_pc;
            ViewBag.incidentepc = inci.Listar();

            pc_incidentepc incidente = new pc_incidentepc();
            return View(incidente);

        }


        public ActionResult GuardarIncidente(pc_incidentepc inci, int incidentepc)
        {

            inci.incidentepc_id = incidentepc;
            pc_inidentepc_dominio incid = new pc_inidentepc_dominio();

            incid.Guardar(inci);

            return View();

        }

        public ActionResult Baja(int id)
        {
            detallepc_dominio dt = new detallepc_dominio();
            pc_dominio pc = new pc_dominio();
            motivobajapc_dominio motivo = new motivobajapc_dominio();
            ubicacionpc_dominio ubi = new ubicacionpc_dominio();

            var detalle = dt.Obtener(id);
            var compu = pc.Obtener(Convert.ToInt32(detalle.pc_id));

            TempData["id_pc"] = compu.id_pc;

            ViewBag.detalle_id = id;
            ViewBag.motivobaja = motivo.Listar();
            ViewBag.ubicacion = ubi.Listar();


            bajaPC baja = new bajaPC();

            return View(baja);

        }

        public ActionResult BajaAceptada(bajaPC baja)
        {
            var id_pc = TempData["id_pc"];

            bajapc_dominio baja_aceptada = new bajapc_dominio();
            pc_dominio compu = new pc_dominio();
            var pc = compu.Obtener(Convert.ToInt32(id_pc));

            pc.fecha_baja = DateTime.Now;
            compu.Guardar(pc);
            baja_aceptada.Guardar(baja);

            return View();
        }


        public ActionResult BuscarParaMover(int id)
        {


            detallepc_dominio detalle = new detallepc_dominio();
            var dt = detalle.Obtener(id);
            TempData["id_pc"] = dt.pc_id;

            var ubicacion_nueva = new ubicacionpc_dominio();
            ViewBag.ubicacion = ubicacion_nueva.Listar();

            return View();
        }


        public ActionResult Mover(int id_nueva_ubicacion)
        {

            int id_pc = Convert.ToInt32(TempData["id_pc"]);
            var ubicacion_pc = new ubicacionpc_pc_dominio();

            var ubicacion_actual_pc = ubicacion_pc.ObtenerUbicacion(id_pc);
            var id_ubicacion_actual = ubicacion_actual_pc.ubicacionpc_id;



            var dt = new detallepc_dominio();
            dt.mover(Convert.ToInt32(id_ubicacion_actual), id_nueva_ubicacion, id_pc);

            return View();

        }

        public ActionResult EditarPC(int id)
        {
            
                tipopc_dominio tipo = new tipopc_dominio();
                ram_dominio mem = new ram_dominio();
                disco_dominio hdd = new disco_dominio();
                so_dominio sisop = new so_dominio();
                motherboard_dominio mth = new motherboard_dominio();
                ubicacionpc_dominio ubi = new ubicacionpc_dominio();
                procesador_dominio procesador = new procesador_dominio();


                var ubic = store.u_computadora_detalle_id(id);

                ViewBag.tipo = new SelectList(tipo.Listar(), "id_tipo", "nombre_tipo", ubic.id_tipo);
                ViewBag.ram = new SelectList(mem.Listar(), "id_ram", "descripcion", ubic.id_ram);
                ViewBag.disco = new SelectList(hdd.Listar(), "id_disco", "descripcion", ubic.id_disco);
                ViewBag.so = new SelectList(sisop.Listar(), "id_so", "descripcion", ubic.id_so);
                ViewBag.pm = new SelectList(mth.Listar(), "id_mother", "descripcion", ubic.id_mother);
                ViewBag.ubi = new SelectList(ubi.Listar(), "id_ubicacion", "nombre", ubic.id_ubicacion);
                ViewBag.proce = new SelectList(procesador.Listar(), "id_procesador", "descripcion", ubic.id_procesador);

                return View(ubic);
            



        }

        public ActionResult ConfirmaEditar(DTO_u_computadora_detalle_id modelo, bool test)
        {
            if (test)
            {
                pc_dominio pc = new pc_dominio();
                var compu = pc.Obtener(modelo.id_pc);
                compu.ip = modelo.ip;
                compu.nombre = modelo.nombre_pc;
                compu.descripcion = modelo.descripcion_pc;
                compu.tipo_id = modelo.id_tipo;
                pc.Guardar(compu); // Guardo las modificaciones de la cabecera


                detallepc_dominio detalle = new detallepc_dominio();
                var dt = detalle.Obtener(modelo.id_detalle);
                dt.mother_id = modelo.id_mother;
                dt.disco_id = modelo.id_disco;
                dt.observacion = modelo.observacion;
                dt.procesador_id = modelo.id_procesador;
                dt.ram_id = modelo.id_ram;
                dt.responsablepc = modelo.responsablepc;
                dt.so_id = modelo.id_so;
                detalle.Guardar(dt); // Guardo las modificaciones del detalle
            }
            else
            {
                return RedirectToAction("Check");
            }

            return View();

        }

        public ActionResult Check()
        {
            return View();
        }
    }
}
