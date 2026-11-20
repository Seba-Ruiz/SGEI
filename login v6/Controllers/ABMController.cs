using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using login_v6.Helpers;
using Negocio.Dominio;
using Model;
using login_v6.Seguridad;
namespace login_v6.Controllers
{
    /// <summary>ABM de catálogos (sección "Gestión"): solo Administrador.</summary>
    [SoloAdministrador]
    public class ABMController : Controller
    {
        // GET: ABM
        public insumo_dominio insu = new insumo_dominio();
        public impresora_dominio imp = new impresora_dominio();
        public ubicacion_dominio ubi = new ubicacion_dominio();
        public ram_dominio ram = new ram_dominio();
        public disco_dominio disco = new disco_dominio();
        public so_dominio so = new so_dominio();
        public motherboard_dominio moth = new motherboard_dominio();
        public incidentepc_dominio incipc = new incidentepc_dominio();
        public tipopc_dominio tipocp = new tipopc_dominio();
        public ubicacionpc_dominio ubipc = new ubicacionpc_dominio();
        public motivobajapc_dominio motivopc = new motivobajapc_dominio();
        public procesador_dominio procesador = new procesador_dominio();
        public insumo_pc_dominio insumo_pc = new insumo_pc_dominio();
        public tipo_telefono_dominio tel = new tipo_telefono_dominio();
        public mmarca_dominio mm = new mmarca_dominio();
        public ubicacion_tel_dominio ut = new ubicacion_tel_dominio();
        public incidentes_tel_dominio incitel = new incidentes_tel_dominio();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Insumos()
        {
            return View(insu.Listar());
        }

        [HttpPost]
        public ActionResult EliminarInsumo(int id)
        {
            var registro = insu.Obtener(id);
            insu.Eliminar(id);
            this.AvisoEliminado("el insumo", registro != null ? registro.nombre : null);
            return Redirect("~/ABM/insumos");
        }
        [HttpPost]
        public ActionResult EliminarImpresora(int id)
        {
            var registro = imp.Obtener(id);
            imp.Eliminar(id);
            this.AvisoEliminado("el modelo de impresora", registro != null ? registro.marca_modelo : null);
            return Redirect("~/ABM/impresoras");
        }
        public ActionResult Crud(int id = 0) //Este es el ABM insumo
        {
            return View(id == 0 ? new insumo()
                        : insu.Obtener(id));
        }
        public ActionResult Editar(int id = 0) //Este es el ABM insumo
        {
            return View(id == 0 ? new insumo()
                        : insu.Obtener(id));
        }

        public ActionResult EditarImpresora(int id = 0)
        {
            return View(id == 0 ? new impresora()
                        : imp.Obtener(id));
        }

        public ActionResult Guardar(insumo model)
        {
            var nuevo = !(model.id > 0);
            insu.Guardar(model);
            this.AvisoGuardado(nuevo, "el insumo", model.nombre);
            return Redirect("~/ABM/Insumos");
        }

        public ActionResult GuardarImpresora(impresora model)
        {
            var nuevo = !(model.id > 0);
            imp.Guardar(model);
            this.AvisoGuardado(nuevo, "el modelo de impresora", model.marca_modelo);
            return Redirect("~/ABM/Impresoras");
        }

        public ActionResult Impresoras()
        {
            return View(imp.Listar());
        }

        public ActionResult CrudImpresora(int id = 0) //Este es el ABM impresora
        {
            return View(id == 0 ? new impresora()
                        : imp.Obtener(id));
        }

        public ActionResult Ubicacion()
        {
            return View(ubi.Listar());
        }

        public ActionResult CrudUbicacion(int id = 0) //Este es el ABM insumo
        {

            return View(id == 0 ? new ubicacion()
                        : ubi.Obtener(id));
        }

        [HttpPost]
        public ActionResult GuardarUbicacion(ubicacion model)
        {
            var nuevo = !(model.id > 0);
            ubi.Guardar(model);
            this.AvisoGuardado(nuevo, "la ubicación", model.nombreUbicacion);
            return Redirect("~/ABM/ubicacion");
        }


        public ActionResult EditarUbicacion(int id = 0)
        {
            return View(id == 0 ? new ubicacion()
                        : ubi.Obtener(id));
        }

        [HttpPost]
        public ActionResult EliminarUbicacion(int id)
        {
            var registro = ubi.Obtener(id);
            ubi.Eliminar(id);
            this.AvisoEliminado("la ubicación", registro != null ? registro.nombreUbicacion : null);
            return Redirect("~/ABM/ubicacion");
        }


        public ActionResult rams()
        {
            return View(ram.Listar());
        }
        public ActionResult CrudRam(int id = 0)
        {

            return View(id == 0 ? new ramPC()
                        : ram.Obtener(id));
        }


        public ActionResult GuardarRam(ramPC model)
        {
            var nuevo = !(model.id_ram > 0);
            ram.Guardar(model);
            this.AvisoGuardado(nuevo, "la memoria", model.descripcion);
            return Redirect("~/ABM/rams");
        }

        public ActionResult EditarRam(int id = 0)
        {
            return View(id == 0 ? new ramPC()
                        : ram.Obtener(id));
        }

        [HttpPost]
        public ActionResult EliminarRam(int id)
        {
            var registro = ram.Obtener(id);
            ram.Eliminar(id);
            this.AvisoEliminado("la memoria", registro != null ? registro.descripcion : null);
            return Redirect("~/ABM/rams");
        }

        public ActionResult discos()
        {
            return View(disco.Listar());
        }

        public ActionResult CrudDisco(int id = 0)
        {

            return View(id == 0 ? new discoPC()
                        : disco.Obtener(id));
        }


        public ActionResult GuardarDisco(discoPC model)
        {
            var nuevo = !(model.id_disco > 0);
            disco.Guardar(model);
            this.AvisoGuardado(nuevo, "el disco", model.descripcion);
            return Redirect("~/ABM/discos");
        }

        public ActionResult EditarDisco(int id = 0)
        {
            return View(id == 0 ? new discoPC()
                        : disco.Obtener(id));
        }

        [HttpPost]
        public ActionResult EliminarDisco(int id)
        {
            var registro = disco.Obtener(id);
            disco.Eliminar(id);
            this.AvisoEliminado("el disco", registro != null ? registro.descripcion : null);
            return Redirect("~/ABM/discos");
        }


        public ActionResult sos()
        {
            return View(so.Listar());
        }


        public ActionResult Crudso(int id = 0)
        {

            return View(id == 0 ? new soPC()
                        : so.Obtener(id));
        }


        public ActionResult Guardarso(soPC model)
        {
            var nuevo = !(model.id_so > 0);
            so.Guardar(model);
            this.AvisoGuardado(nuevo, "el sistema operativo", model.descripcion);
            return Redirect("~/ABM/sos");
        }

        public ActionResult Editarso(int id = 0)
        {
            return View(id == 0 ? new soPC()
                        : so.Obtener(id));
        }


        [HttpPost]
        public ActionResult Eliminarso(int id)
        {
            var registro = so.Obtener(id);
            so.Eliminar(id);
            this.AvisoEliminado("el sistema operativo", registro != null ? registro.descripcion : null);
            return Redirect("~/ABM/sos");
        }

        public ActionResult mothers()
        {
            return View(moth.Listar());
        }


        public ActionResult Crudmoth(int id = 0)
        {

            return View(id == 0 ? new motherPC()
                        : moth.Obtener(id));
        }

        public ActionResult Guardarmoth(motherPC model)
        {
            var nuevo = !(model.id_mother > 0);
            moth.Guardar(model);
            this.AvisoGuardado(nuevo, "la placa madre", model.descripcion);
            return Redirect("~/ABM/mothers");
        }

        public ActionResult Editarmoth(int id = 0)
        {
            return View(id == 0 ? new motherPC()
                        : moth.Obtener(id));
        }


        [HttpPost]
        public ActionResult Eliminarmoth(int id)
        {
            var registro = moth.Obtener(id);
            moth.Eliminar(id);
            this.AvisoEliminado("la placa madre", registro != null ? registro.descripcion : null);
            return Redirect("~/ABM/mothers");
        }


        public ActionResult incidentespc()
        {
            return View(incipc.Listar());
        }



        public ActionResult Crudincidentepc(int id = 0)
        {

            return View(id == 0 ? new incidentePC()
                        : incipc.Obtener(id));
        }


        public ActionResult Guardarincidentepc(incidentePC model)
        {
            var nuevo = !(model.id_incidentepc > 0);
            incipc.Guardar(model);
            this.AvisoGuardado(nuevo, "el incidente", model.nombre);
            return Redirect("~/ABM/incidentespc");
        }

        public ActionResult Editarincidentepc(int id = 0)
        {
            return View(id == 0 ? new incidentePC()
                        : incipc.Obtener(id));
        }


        [HttpPost]
        public ActionResult Eliminarincidentepc(int id)
        {
            var registro = incipc.Obtener(id);
            incipc.Eliminar(id);
            this.AvisoEliminado("el incidente", registro != null ? registro.nombre : null);
            return Redirect("~/ABM/incidentespc");
        }

        public ActionResult tipospc()
        {
            return View(tipocp.Listar());
        }


        public ActionResult Crudtipopc(int id = 0)
        {

            return View(id == 0 ? new tipoPC()
                        : tipocp.Obtener(id));
        }

        public ActionResult Guardartipopc(tipoPC model)
        {
            var nuevo = !(model.id_tipo > 0);
            tipocp.Guardar(model);
            this.AvisoGuardado(nuevo, "el tipo de computadora", model.nombre_tipo);
            return Redirect("~/ABM/tipospc");
        }

        public ActionResult Editartipopc(int id = 0)
        {
            return View(id == 0 ? new tipoPC()
                        : tipocp.Obtener(id));
        }

        [HttpPost]
        public ActionResult Eliminartipopc(int id)
        {
            var registro = tipocp.Obtener(id);
            tipocp.Eliminar(id);
            this.AvisoEliminado("el tipo de computadora", registro != null ? registro.nombre_tipo : null);
            return Redirect("~/ABM/tipospc");
        }

        public ActionResult ubicacionespc()
        {
            return View(ubipc.Listar());
        }


        public ActionResult Crudubipc(int id = 0)
        {

            return View(id == 0 ? new ubicacionPC()
                        : ubipc.Obtener(id));
        }


        public ActionResult Guardarubipc(ubicacionPC model)
        {
            var nuevo = !(model.id_ubicacion > 0);
            ubipc.Guardar(model);
            this.AvisoGuardado(nuevo, "la ubicación", model.nombre);
            return Redirect("~/ABM/ubicacionespc");
        }

        public ActionResult Editarubipc(int id = 0)
        {
            return View(id == 0 ? new ubicacionPC()
                        : ubipc.Obtener(id));
        }

        [HttpPost]
        public ActionResult Eliminarubipc(int id)
        {
            var registro = ubipc.Obtener(id);
            ubipc.Eliminar(id);
            this.AvisoEliminado("la ubicación", registro != null ? registro.nombre : null);
            return Redirect("~/ABM/ubicacionespc");
        }

        public ActionResult motivospc()
        {
            return View(motivopc.Listar());
        }


        public ActionResult Crudmotivopc(int id = 0)
        {

            return View(id == 0 ? new motivobajaPC()
                        : motivopc.Obtener(id));
        }

        public ActionResult Guardarmotivopc(motivobajaPC model)
        {
            var nuevo = !(model.id_motivobaja > 0);
            motivopc.Guardar(model);
            this.AvisoGuardado(nuevo, "el motivo de baja", model.descripcion);
            return Redirect("~/ABM/motivospc");
        }

        public ActionResult Editarmotivopc(int id = 0)
        {
            return View(id == 0 ? new motivobajaPC()
                        : motivopc.Obtener(id));
        }

        [HttpPost]
        public ActionResult Eliminarmotivopc(int id)
        {
            var registro = motivopc.Obtener(id);
            motivopc.Eliminar(id);
            this.AvisoEliminado("el motivo de baja", registro != null ? registro.descripcion : null);
            return Redirect("~/ABM/motivospc");
        }

        public ActionResult procesadorespc()
        {
            return View(procesador.Listar());
        }

        public ActionResult Crudprocesadorpc(int id = 0)
        {

            return View(id == 0 ? new procesadorPC()
                        : procesador.Obtener(id));
        }


        public ActionResult Guardarprocesadorpc(procesadorPC model)
        {
            var nuevo = !(model.id_procesador > 0);
            procesador.Guardar(model);
            this.AvisoGuardado(nuevo, "el procesador", model.descripcion);
            return Redirect("~/ABM/procesadorespc");
        }

        public ActionResult Editarprocesadorpc(int id = 0)
        {
            return View(id == 0 ? new procesadorPC()
                        : procesador.Obtener(id));
        }

        [HttpPost]
        public ActionResult Eliminarprocesadorpc(int id)
        {
            var registro = procesador.Obtener(id);
            procesador.Eliminar(id);
            this.AvisoEliminado("el procesador", registro != null ? registro.descripcion : null);
            return Redirect("~/ABM/procesadorespc");
        }

        public ActionResult insumospc()
        {
            return View(insumo_pc.Listar());
        }


        public ActionResult Crudinsumopc(int id = 0)
        {

            return View(id == 0 ? new insumo_pc()
                        : insumo_pc.Obtener(id));
        }


        public ActionResult Guardarinsumopc(insumo_pc model)
        {
            var nuevo = !(model.id_insumo > 0);
            insumo_pc.Guardar(model);
            this.AvisoGuardado(nuevo, "el insumo", model.nombre);
            return Redirect("~/ABM/insumospc");
        }


        public ActionResult Editarinsumopc(int id = 0)
        {
            return View(id == 0 ? new insumo_pc()
                        : insumo_pc.Obtener(id));
        }


        [HttpPost]
        public ActionResult Eliminarinsumopc(int id)
        {
            var registro = insumo_pc.Obtener(id);
            insumo_pc.Eliminar(id);
            this.AvisoEliminado("el insumo", registro != null ? registro.nombre : null);
            return Redirect("~/ABM/insumospc");
        }

        public ActionResult tipo_telefonos()
        {
            return View(tel.Listar());
        }


        public ActionResult Crudtipo_tel(int id = 0)
        {

            return View(id == 0 ? new tipo_tel()
                        : tel.Obtener(id));
        }


        public ActionResult Guardartipo_tel(tipo_tel model)
        {
            var nuevo = !(model.id_tipo > 0);
            tel.Guardar(model);
            this.AvisoGuardado(nuevo, "el tipo de teléfono", model.nombre_tipo);
            return Redirect("~/ABM/tipo_telefonos");
        }

        public ActionResult Editartipo_tel(int id = 0)
        {
            return View(id == 0 ? new tipo_tel()
                        : tel.Obtener(id));
        }


        [HttpPost]
        public ActionResult Eliminartipo_tel(int id)
        {
            var registro = tel.Obtener(id);
            tel.Eliminar(id);
            this.AvisoEliminado("el tipo de teléfono", registro != null ? registro.nombre_tipo : null);
            return Redirect("~/ABM/tipo_telefonos");
        }


        public ActionResult mmarcas()
        {
            return View(mm.Listar());
        }


        public ActionResult Crudmmarca(int id = 0)
        {

            return View(id == 0 ? new marca_modelo_cel()
                        : mm.Obtener(id));
        }


        public ActionResult Guardarmmarca(marca_modelo_cel model)
        {
            var nuevo = !(model.id_mm > 0);
            mm.Guardar(model);
            this.AvisoGuardado(nuevo, "el modelo de teléfono", model.descripcion);
            return Redirect("~/ABM/mmarcas");
        }


        public ActionResult Editarmmarca(int id = 0)
        {
            return View(id == 0 ? new marca_modelo_cel()
                        : mm.Obtener(id));
        }


        [HttpPost]
        public ActionResult Eliminarmmarca(int id)
        {
            var registro = mm.Obtener(id);
            mm.Eliminar(id);
            this.AvisoEliminado("el modelo de teléfono", registro != null ? registro.descripcion : null);
            return Redirect("~/ABM/mmarcas");
        }


        public ActionResult ubicacionestel()
        {
            return View(ut.Listar());
        }


        public ActionResult Crudubicaciontel(int id = 0)
        {

            return View(id == 0 ? new ubicacion_tel()
                        : ut.Obtener(id));
        }


        public ActionResult Guardarubicaciontel(ubicacion_tel model)
        {
            var nuevo = !(model.id_ubicacion_tel > 0);
            ut.Guardar(model);
            this.AvisoGuardado(nuevo, "la ubicación", model.nombre_ubi);
            return Redirect("~/ABM/ubicacionestel");
        }

        public ActionResult Editarubicaciontel(int id = 0)
        {
            return View(id == 0 ? new ubicacion_tel()
                        : ut.Obtener(id));
        }


        [HttpPost]
        public ActionResult Eliminarubicaciontel(int id)
        {
            var registro = ut.Obtener(id);
            ut.Eliminar(id);
            this.AvisoEliminado("la ubicación", registro != null ? registro.nombre_ubi : null);
            return Redirect("~/ABM/ubicacionestel");
        }



        public ActionResult incidentestel()
        {
            return View(incitel.Listar());
        }
        public ActionResult Crudincidentetel(int id = 0)
        {

            return View(id == 0 ? new incidente_tel()
                        : incitel.Obtener(id));
        }
        public ActionResult Guardarincidentetel(incidente_tel model)
        {
            var nuevo = !(model.id_incidente_tel > 0);
            incitel.Guardar(model);
            this.AvisoGuardado(nuevo, "el incidente", model.nombre_incidente);
            return Redirect("~/ABM/incidentestel");
        }
        public ActionResult Editarincidentetel(int id = 0)
        {
            return View(id == 0 ? new incidente_tel()
                        : incitel.Obtener(id));
        }
        [HttpPost]
        public ActionResult Eliminarincidentetel(int id)
        {
            var registro = incitel.Obtener(id);
            incitel.Eliminar(id);
            this.AvisoEliminado("el incidente", registro != null ? registro.nombre_incidente : null);
            return Redirect("~/ABM/incidentestel");
        }









        //DE ACA EN ADELANTE ARRANCA LA ACTUALIZACION DE LOS NUEVOS EQUIPOS



        //ABM INSUMO_CAMARAS
        public insumo_cam_dominio insumocam = new insumo_cam_dominio();

        public ActionResult insumosCam()
        {
            return View(insumocam.Listar());
        }


        public ActionResult Crudinsumocam(int id = 0)
        {

            return View(id == 0 ? new insumo_camara()
                        : insumocam.Obtener(id));
        }


        public ActionResult Guardarinsumocam(insumo_camara model)
        {
            var nuevo = !(model.id_insumo > 0);
            insumocam.Guardar(model);
            this.AvisoGuardado(nuevo, "el insumo", model.nombre);
            return Redirect("~/ABM/insumosCam");
        }


        public ActionResult EditarinsumoCam(int id = 0)
        {
            return View(id == 0 ? new insumo_camara()
                        : insumocam.Obtener(id));
        }


        [HttpPost]
        public ActionResult EliminarinsumoCam(int id)
        {
            var registro = insumocam.Obtener(id);
            insumocam.Eliminar(id);
            this.AvisoEliminado("el insumo", registro != null ? registro.nombre : null);
            return Redirect("~/ABM/insumosCam");
        }




        //ABM MODELOS_CAM
        public mmarca_camara_dominio modelo_cam = new mmarca_camara_dominio();

        public ActionResult modeloCam()
        {
            return View(modelo_cam.Listar());
        }


        public ActionResult Crudmmaarcacam(int id = 0)
        {

            return View(id == 0 ? new marca_modelo_camara()
                        : modelo_cam.Obtener(id));
        }


        public ActionResult Guardarmmarcacam(marca_modelo_camara model)
        {
            var nuevo = !(model.id_mmarca > 0);
            modelo_cam.Guardar(model);
            this.AvisoGuardado(nuevo, "el modelo de cámara", model.marca_modelo);
            return Redirect("~/ABM/modeloCam");
        }


        public ActionResult EditarmodeloCam(int id = 0)
        {
            return View(id == 0 ? new marca_modelo_camara()
                        : modelo_cam.Obtener(id));
        }


        [HttpPost]
        public ActionResult EliminarmodeloCam(int id)
        {
            var registro = modelo_cam.Obtener(id);
            modelo_cam.Eliminar(id);
            this.AvisoEliminado("el modelo de cámara", registro != null ? registro.marca_modelo : null);
            return Redirect("~/ABM/modeloCam");
        }




        //ABM UBICACIONES DE CAMARAS
        public ubicacion_camara_dominio ubi_cam = new ubicacion_camara_dominio();

        public ActionResult ubiCam()
        {
            return View(ubi_cam.Listar());
        }


        public ActionResult Crudubicam(int id = 0)
        {

            return View(id == 0 ? new ubicacion_camara()
                        : ubi_cam.Obtener(id));
        }


        public ActionResult Guardarubicam(ubicacion_camara model)
        {
            var nuevo = !(model.id_ubicacion_camara > 0);
            ubi_cam.Guardar(model);
            this.AvisoGuardado(nuevo, "la ubicación", model.nombre);
            return Redirect("~/ABM/ubiCam");
        }


        public ActionResult EditarubiCam(int id = 0)
        {
            return View(id == 0 ? new ubicacion_camara()
                        : ubi_cam.Obtener(id));
        }


        [HttpPost]
        public ActionResult EliminarubiCam(int id)
        {
            var registro = ubi_cam.Obtener(id);
            ubi_cam.Eliminar(id);
            this.AvisoEliminado("la ubicación", registro != null ? registro.nombre : null);
            return Redirect("~/ABM/ubiCam");
        }




        //ABM INCIDENTES DE CAMARAS
        public incidente_camara_dominio incidente_cam = new incidente_camara_dominio();

        public ActionResult incidenteCam()
        {
            return View(incidente_cam.Listar());
        }

        public ActionResult Crudincidentecam(int id = 0)
        {

            return View(id == 0 ? new incidente_camara()
                        : incidente_cam.Obtener(id));
        }

        public ActionResult Guardarincidentecam(incidente_camara model)
        {
            var nuevo = !(model.id_incidente > 0);
            incidente_cam.Guardar(model);
            this.AvisoGuardado(nuevo, "el incidente", model.descripcion);
            return Redirect("~/ABM/incidenteCam");
        }

        public ActionResult EditarincidenteCam(int id = 0)
        {
            return View(id == 0 ? new incidente_camara()
                        : incidente_cam.Obtener(id));
        }

        [HttpPost]
        public ActionResult EliminarincidenteCam(int id)
        {
            var registro = incidente_cam.Obtener(id);
            incidente_cam.Eliminar(id);
            this.AvisoEliminado("el incidente", registro != null ? registro.descripcion : null);
            return Redirect("~/ABM/incidenteCam");
        }




        //ABM UBICACIONES SWITCH
        public ubicacion_sw_dominio ubicacion_sw = new ubicacion_sw_dominio();

        public ActionResult ubicacionSW()
        {
            return View(ubicacion_sw.Listar());
        }

        public ActionResult Crudubicacionsw(int id = 0)
        {

            return View(id == 0 ? new ubicacion_switch()
                        : ubicacion_sw.Obtener(id));
        }

        public ActionResult Guardarubicacionsw(ubicacion_switch model)
        {
            var nuevo = !(model.id_ubicacion_switch > 0);
            ubicacion_sw.Guardar(model);
            this.AvisoGuardado(nuevo, "la ubicación", model.nombre);
            return Redirect("~/ABM/ubicacionSW");
        }

        public ActionResult Editarubicacionsw(int id = 0)
        {
            return View(id == 0 ? new ubicacion_switch()
                        : ubicacion_sw.Obtener(id));
        }

        [HttpPost]
        public ActionResult Eliminarubicacionsw(int id)
        {
            var registro = ubicacion_sw.Obtener(id);
            ubicacion_sw.Eliminar(id);
            this.AvisoEliminado("la ubicación", registro != null ? registro.nombre : null);
            return Redirect("~/ABM/ubicacionSW");
        }


        //ABM INCIDENTES SWITCH
        public incidente_sw_dominio incidente_sw = new incidente_sw_dominio();

        public ActionResult incidenteSW()
        {
            return View(incidente_sw.Listar());
        }

        public ActionResult Crudincidentesw(int id = 0)
        {

            return View(id == 0 ? new incidente_switch()
                        : incidente_sw.Obtener(id));
        }

        public ActionResult Guardarincidentesw(incidente_switch model)
        {
            var nuevo = !(model.id_incidente_switch > 0);
            incidente_sw.Guardar(model);
            this.AvisoGuardado(nuevo, "el incidente", model.descripcion);
            return Redirect("~/ABM/incidenteSW");
        }

        public ActionResult Editarincidentesw(int id = 0)
        {
            return View(id == 0 ? new incidente_switch()
                        : incidente_sw.Obtener(id));
        }

        [HttpPost]
        public ActionResult Eliminarincidentesw(int id)
        {
            var registro = incidente_sw.Obtener(id);
            incidente_sw.Eliminar(id);
            this.AvisoEliminado("el incidente", registro != null ? registro.descripcion : null);
            return Redirect("~/ABM/incidenteSW");
        }


        //ABM MODELOS SWITCH
        public modelo_sw_dominio modelo_sw = new modelo_sw_dominio();

        public ActionResult modeloSW()
        {
            return View(modelo_sw.Listar());
        }

        public ActionResult Crudmodelosw(int id = 0)
        {

            return View(id == 0 ? new marca_modelo_switch()
                        : modelo_sw.Obtener(id));
        }

        public ActionResult Guardarmodelosw(marca_modelo_switch model)
        {
            var nuevo = !(model.id_mmarca > 0);
            modelo_sw.Guardar(model);
            this.AvisoGuardado(nuevo, "el modelo de switch", model.marca_modelo);
            return Redirect("~/ABM/modeloSW");
        }

        public ActionResult Editarmodelosw(int id = 0)
        {
            return View(id == 0 ? new marca_modelo_switch()
                        : modelo_sw.Obtener(id));
        }

        [HttpPost]
        public ActionResult Eliminarmodeloswm(int id)
        {
            var registro = modelo_sw.Obtener(id);
            modelo_sw.Eliminar(id);
            this.AvisoEliminado("el modelo de switch", registro != null ? registro.marca_modelo : null);
            return Redirect("~/ABM/modeloSW");
        }


        //ABM UBICACION ESCANER
        public ubicacion_escaner_dominio ubi_es = new ubicacion_escaner_dominio();

        public ActionResult ubicacionES()
        {
            return View(ubi_es.Listar());
        }

        public ActionResult CrudubicacionES(int id = 0)
        {

            return View(id == 0 ? new ubicacion_escaner()
                        : ubi_es.Obtener(id));
        }

        public ActionResult GuardarubicacionES(ubicacion_escaner model)
        {
            var nuevo = !(model.id_ubicacion_escaner > 0);
            ubi_es.Guardar(model);
            this.AvisoGuardado(nuevo, "la ubicación", model.nombre);
            return Redirect("~/ABM/ubicacionES");
        }

        public ActionResult EditarubicacionES(int id = 0)
        {
            return View(id == 0 ? new ubicacion_escaner()
                        : ubi_es.Obtener(id));
        }

        [HttpPost]
        public ActionResult EliminarubicacionES(int id)
        {
            var registro = ubi_es.Obtener(id);
            ubi_es.Eliminar(id);
            this.AvisoEliminado("la ubicación", registro != null ? registro.nombre : null);
            return Redirect("~/ABM/ubicacionES");
        }




        //ABM MODELOS ESCANER
        public modelo_escaner_dominio modelo_es = new modelo_escaner_dominio();

        public ActionResult modeloES()
        {
            return View(modelo_es.Listar());
        }

        public ActionResult CrudmodeloES(int id = 0)
        {

            return View(id == 0 ? new marca_modelo_escaner()
                        : modelo_es.Obtener(id));
        }

        public ActionResult GuardarmodeloES(marca_modelo_escaner model)
        {
            var nuevo = !(model.id_marca_modelo_escaner > 0);
            modelo_es.Guardar(model);
            this.AvisoGuardado(nuevo, "el modelo de escáner", model.descripcion);
            return Redirect("~/ABM/modeloES");
        }

        public ActionResult EditarmodeloES(int id = 0)
        {
            return View(id == 0 ? new marca_modelo_escaner()
                        : modelo_es.Obtener(id));
        }

        [HttpPost]
        public ActionResult EliminarmodeloES(int id)
        {
            var registro = modelo_es.Obtener(id);
            modelo_es.Eliminar(id);
            this.AvisoEliminado("el modelo de escáner", registro != null ? registro.descripcion : null);
            return Redirect("~/ABM/modeloES");
        }






        //ABM INCIDENTE ESCANER
        public incidente_escaner_dominio incidente_es = new incidente_escaner_dominio();

        public ActionResult incidenteES()
        {
            return View(incidente_es.Listar());
        }

        public ActionResult CrudincidenteES(int id = 0)
        {

            return View(id == 0 ? new incidente_escaner()
                        : incidente_es.Obtener(id));
        }

        public ActionResult GuardarincidenteES(incidente_escaner model)
        {
            var nuevo = !(model.id_incidente_escaner > 0);
            incidente_es.Guardar(model);
            this.AvisoGuardado(nuevo, "el incidente", model.descripcion);
            return Redirect("~/ABM/incidenteES");
        }

        public ActionResult EditarincidenteES(int id = 0)
        {
            return View(id == 0 ? new incidente_escaner()
                        : incidente_es.Obtener(id));
        }

        [HttpPost]
        public ActionResult EliminarincidenteES(int id)
        {
            var registro = incidente_es.Obtener(id);
            incidente_es.Eliminar(id);
            this.AvisoEliminado("el incidente", registro != null ? registro.descripcion : null);
            return Redirect("~/ABM/incidenteES");
        }





        //ABM PROVEEDORES
        public proveedor_dominio proveedor = new proveedor_dominio();

        public ActionResult proveedores()
        {
            return View(proveedor.Listar());
        }

        public ActionResult CrudProveedor(int id = 0)
        {

            return View(id == 0 ? new Proveedor()
                        : proveedor.Obtener(id));
        }

        public ActionResult GuardarProveedor(Proveedor model)
        {
            var nuevo = !(model.id > 0);
            proveedor.Guardar(model);
            this.AvisoGuardado(nuevo, "el proveedor", model.nombre);
            return Redirect("~/ABM/proveedores");
        }

        public ActionResult EditarProveedor(int id = 0)
        {
            return View(id == 0 ? new Proveedor()
                        : proveedor.Obtener(id));
        }

        [HttpPost]
        public ActionResult EliminarProveedor(int id)
        {
            var registro = proveedor.Obtener(id);
            proveedor.Eliminar(id);
            this.AvisoEliminado("el proveedor", registro != null ? registro.nombre : null);
            return Redirect("~/ABM/proveedores");
        }

    }
}