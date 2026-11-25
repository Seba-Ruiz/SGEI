using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using login_v6.Helpers;
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
        /// <summary>
        /// Grilla con todas las computadoras activas. El combo de ubicación filtra la grilla en el
        /// navegador; ?ubicacion=N lo deja preseleccionado.
        /// </summary>
        public ActionResult Index(int? ubicacion)
        {
            ubicacionpc_dominio ubi = new ubicacionpc_dominio();
            ViewBag.ubicaciones = new SelectList(ubi.Listar(), "id_ubicacion", "nombre", ubicacion);
            return View(store.u_computadora());
        }

        public ActionResult Preventivo()
        {
            mantenimiento_dominio mante = new mantenimiento_dominio();
            List<DTO_preventivo_pc> lista = new List<DTO_preventivo_pc>();
            var m = mante.Listar();

            foreach (var a in m)
            {
                DTO_preventivo_pc dto = new DTO_preventivo_pc();
                var nombre = store.buscar_pc_00(a.pc_id);

                dto.nombre = nombre.nombre;
                dto.fecha_mantenimiento = a.fecha_mantenimiento;
                dto.pospuesto_por = a.pospuesto_por;
                dto.fecha_pospuso = a.fecha_pospuesto;
                dto.descripcion = a.descripcion;
                dto.realizado_por = a.realizado_por;
                dto.proximo_mantenimiento = a.proximo_mantenimiento;

                lista.Add(dto);
            }

            return View(lista);
        }
        public ActionResult PosponerMante(int id, int dias)
        {
            mantenimiento_dominio mante = new mantenimiento_dominio();
            var mantenimiento = mante.ObtenerMantePC(id);

            mantenimiento.proximo_mantenimiento = DateTime.Now.AddDays(dias);
            mantenimiento.fecha_pospuesto = DateTime.Now;
            mantenimiento.pospuesto = true;
            mantenimiento.pospuesto_por = User.Identity.Name;

            mante.Guardar(mantenimiento);

            return Redirect("~/Inicio/Index");
        }
        public ActionResult ManteCompu(int id_pc, string descripcion)
        {
            mantenimiento_dominio mante = new mantenimiento_dominio();
            var mantenimiento = mante.ObtenerMantePC(id_pc);

            mantenimiento.realizado = true;
            mantenimiento.realizado_por = User.Identity.Name;
            mantenimiento.descripcion = descripcion;
            mantenimiento.fecha_mantenimiento = DateTime.Now;
            mantenimiento.proximo_mantenimiento = DateTime.Now;

            mante.Guardar(mantenimiento); 

            var proximo = new mantenimiento();

            proximo.pc_id = id_pc;
            proximo.pospuesto = false;

            var ubicacion = store.buscar_pc(id_pc) ;

            if (ubicacion.ubicacion == "GUARDIA" || ubicacion.ubicacion == "ADMISION")
            {
                proximo.proximo_mantenimiento = DateTime.Now.AddMonths(5); 
            }
            else
            {
                proximo.proximo_mantenimiento = DateTime.Now.AddMonths(8); 
            }

            proximo.fecha_mantenimiento = DateTime.Now;
            proximo.descripcion = "A Realizarse";
            proximo.realizado = false;

            mante.Guardar(proximo);

            return Redirect("~/Inicio/Index");
        }

        public ActionResult Detalle(int id)
        {
            var ubic = store.u_computadora_detalle(id);
            return View(ubic);
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

            //AUDITORIA
            Auditoria audit = new Auditoria();
            auditoria_dominio audit_dom = new auditoria_dominio();

            audit.fecha_hora = DateTime.Now;
            audit.tipo_equipo = "COMPUTADORA";
            audit.id_equipo = pc.id_pc;
            audit.accion = "BAJA";
            audit.usuario = User.Identity.Name;

            audit_dom.Guardar(audit);
            //---//

            this.AvisoExito("Se dio de baja la computadora" + Avisos.Nombre(pc.nombre) + ".");
            return Redirect("~/Computadora/Index");
        }

        /// <summary>
        /// Mueve la computadora a otra ubicación de computadoras desde la modal de la grilla
        /// (Views/Shared/_MoverUbicacion.cshtml). id: id_detalle de la fila. Responde JSON.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoverUbicacion(int id, int idUbicacion)
        {
            var detalle = new detallepc_dominio().Obtener(id);
            if (detalle == null || detalle.pc_id == null) { return this.ErrorJson(404, "La computadora ya no existe."); }
            var idPc = detalle.pc_id.Value;

            var destino = new ubicacionpc_dominio().Obtener(idUbicacion);
            if (destino == null || destino.fecha_baja != null) { return this.ErrorJson(400, "Elegí una ubicación válida."); }

            var actual = new ubicacionpc_pc_dominio().ObtenerUbicacion(idPc);
            if (actual == null || actual.ubicacionpc_id == null) { return this.ErrorJson(400, "La computadora no tiene una ubicación cargada."); }
            if (actual.ubicacionpc_id == idUbicacion) { return this.ErrorJson(400, "La computadora ya está en " + destino.nombre + "."); }

            new detallepc_dominio().mover(actual.ubicacionpc_id.Value, idUbicacion, idPc);
            new auditoria_dominio().Guardar(new Auditoria
            {
                fecha_hora = DateTime.Now, tipo_equipo = "COMPUTADORA", id_equipo = idPc, accion = "MOVER", usuario = User.Identity.Name
            });

            var equipo = new pc_dominio().Obtener(idPc);
            return Json(new
            {
                ok = true,
                mensaje = "Se movió la computadora" + Avisos.Nombre(equipo != null ? equipo.nombre : null) + " a " + destino.nombre + ".",
                ubicacion = destino.nombre,
                fecha = ((DateTime?)DateTime.Today).ToString()
            });
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

            //AUDITORIA
            Auditoria audit = new Auditoria();
            auditoria_dominio audit_dom = new auditoria_dominio();

            audit.fecha_hora = DateTime.Now;
            audit.tipo_equipo = "COMPUTADORA";
            audit.id_equipo = id_pc;
            audit.accion = "MOVER";
            audit.usuario = User.Identity.Name;

            audit_dom.Guardar(audit);
            //---//

            var equipo = new pc_dominio().Obtener(id_pc);
            var destino = new ubicacionpc_dominio().Obtener(id_nueva_ubicacion);
            this.AvisoExito("Se movió la computadora" + Avisos.Nombre(equipo != null ? equipo.nombre : null) + (destino != null ? " a " + destino.nombre : "") + ".");
            return Redirect("~/Computadora/Index");

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

        public ActionResult ConfirmaEditar(DTO_u_computadora_detalle_id modelo)
        {
            // Nombre repetido: vuelve al formulario con el aviso (el navegador normalmente ya lo frenó).
            var repetido = this.VolverSiNombreRepetido("computadora", modelo.nombre_pc, Convert.ToInt32(modelo.id_pc), "~/Computadora/Index");
            if (repetido != null) { return repetido; }

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

            //AUDITORIA
            Auditoria audit = new Auditoria();
            auditoria_dominio audit_dom = new auditoria_dominio();

            audit.fecha_hora = DateTime.Now;
            audit.tipo_equipo = "COMPUTADORA";
            audit.id_equipo = modelo.id_pc;
            audit.accion = "EDITAR";
            audit.usuario = User.Identity.Name;

            audit_dom.Guardar(audit);
            //---//

            this.AvisoExito("Se guardaron los cambios de la computadora" + Avisos.Nombre(modelo.nombre_pc) + ".");
            return Redirect("~/Computadora/Index");

        }
    }
}
