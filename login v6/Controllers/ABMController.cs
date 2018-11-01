using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Negocio.Dominio;
using Model;
namespace login_v6.Controllers
{
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
        [Authorize]
        public ActionResult Insumos()
        {
            return View(insu.Listar());
        }

        [Authorize]
        public ActionResult EliminarInsumo(int id)
        {
            insu.Eliminar(id);
            return Redirect("~/ABM/insumos");
        }
        [Authorize]
        public ActionResult EliminarImpresora(int id)
        {
            imp.Eliminar(id);
            return Redirect("~/ABM/impresoras");
        }
        [HttpPost]
        [Authorize]
        public ActionResult Crud(int id = 0) //Este es el ABM insumo
        {
            return View(id == 0 ? new insumo()
                        : insu.Obtener(id));
        }
        [Authorize]
        public ActionResult Editar(int id = 0) //Este es el ABM insumo
        {
            return View(id == 0 ? new insumo()
                        : insu.Obtener(id));
        }

        [Authorize]
        public ActionResult EditarImpresora(int id = 0)
        {
            return View(id == 0 ? new impresora()
                        : imp.Obtener(id));
        }

        [Authorize]
        public ActionResult Guardar(insumo model)
        {
            insu.Guardar(model);
            return Redirect("~/ABM/Insumos");
        }

        [Authorize]
        public ActionResult GuardarImpresora(impresora model)
        {
            imp.Guardar(model);
            return Redirect("~/ABM/Impresoras");
        }

        [Authorize]
        public ActionResult Impresoras()
        {
            return View(imp.Listar());
        }

        [Authorize]
        [HttpPost]
        public ActionResult CrudImpresora(int id = 0) //Este es el ABM impresora
        {
            return View(id == 0 ? new impresora()
                        : imp.Obtener(id));
        }

        [Authorize]
        public ActionResult Ubicacion()
        {
            return View(ubi.Listar());
        }

        [Authorize]
        public ActionResult CrudUbicacion(int id = 0) //Este es el ABM insumo
        {

            return View(id == 0 ? new ubicacion()
                        : ubi.Obtener(id));
        }

        [Authorize]
        [HttpPost]
        public ActionResult GuardarUbicacion(ubicacion model)
        {
            ubi.Guardar(model);
            return Redirect("~/ABM/ubicacion");
        }


        [Authorize]
        public ActionResult EditarUbicacion(int id = 0)
        {
            return View(id == 0 ? new ubicacion()
                        : ubi.Obtener(id));
        }

        [Authorize]
        public ActionResult EliminarUbicacion(int id)
        {

            ubi.Eliminar(id);
            return Redirect("~/ABM/ubicacion");
        }


        [Authorize]
        public ActionResult rams()
        {
            return View(ram.Listar());
        }
        [Authorize]
        public ActionResult CrudRam(int id = 0)
        {

            return View(id == 0 ? new ramPC()
                        : ram.Obtener(id));
        }


        [Authorize]
        public ActionResult GuardarRam(ramPC model)
        {
            ram.Guardar(model);
            return Redirect("~/ABM/rams");
        }

        [Authorize]
        public ActionResult EditarRam(int id = 0)
        {
            return View(id == 0 ? new ramPC()
                        : ram.Obtener(id));
        }

        [Authorize]
        public ActionResult EliminarRam(int id)
        {

            ram.Eliminar(id);
            return Redirect("~/ABM/rams");
        }

        [Authorize]
        public ActionResult discos()
        {
            return View(disco.Listar());
        }

        [Authorize]
        public ActionResult CrudDisco(int id = 0)
        {

            return View(id == 0 ? new discoPC()
                        : disco.Obtener(id));
        }


        [Authorize]
        public ActionResult GuardarDisco(discoPC model)
        {
            disco.Guardar(model);
            return Redirect("~/ABM/discos");
        }

        [Authorize]
        public ActionResult EditarDisco(int id = 0)
        {
            return View(id == 0 ? new discoPC()
                        : disco.Obtener(id));
        }



        [Authorize]
        public ActionResult EliminarDisco(int id)
        {

            disco.Eliminar(id);
            return Redirect("~/ABM/discos");
        }


        [Authorize]
        public ActionResult sos()
        {
            return View(so.Listar());
        }


        [Authorize]
        public ActionResult Crudso(int id = 0)
        {

            return View(id == 0 ? new soPC()
                        : so.Obtener(id));
        }


        [Authorize]
        public ActionResult Guardarso(soPC model)
        {
            so.Guardar(model);
            return Redirect("~/ABM/sos");
        }

        [Authorize]
        public ActionResult Editarso(int id = 0)
        {
            return View(id == 0 ? new soPC()
                        : so.Obtener(id));
        }


        [Authorize]
        public ActionResult Eliminarso(int id)
        {

            so.Eliminar(id);
            return Redirect("~/ABM/sos");
        }

        [Authorize]
        public ActionResult mothers()
        {
            return View(moth.Listar());
        }


        [Authorize]
        public ActionResult Crudmoth(int id = 0)
        {

            return View(id == 0 ? new motherPC()
                        : moth.Obtener(id));
        }

        [Authorize]
        public ActionResult Guardarmoth(motherPC model)
        {
            moth.Guardar(model);
            return Redirect("~/ABM/mothers");
        }

        [Authorize]
        public ActionResult Editarmoth(int id = 0)
        {
            return View(id == 0 ? new motherPC()
                        : moth.Obtener(id));
        }


        [Authorize]
        public ActionResult Eliminarmoth(int id)
        {

            moth.Eliminar(id);
            return Redirect("~/ABM/mothers");
        }


        [Authorize]
        public ActionResult incidentespc()
        {
            return View(incipc.Listar());
        }



        [Authorize]
        public ActionResult Crudincidentepc(int id = 0)
        {

            return View(id == 0 ? new incidentePC()
                        : incipc.Obtener(id));
        }


        [Authorize]
        public ActionResult Guardarincidentepc(incidentePC model)
        {
            incipc.Guardar(model);
            return Redirect("~/ABM/incidentespc");
        }

        [Authorize]
        public ActionResult Editarincidentepc(int id = 0)
        {
            return View(id == 0 ? new incidentePC()
                        : incipc.Obtener(id));
        }


        [Authorize]
        public ActionResult Eliminarincidentepc(int id)
        {

            incipc.Eliminar(id);
            return Redirect("~/ABM/incidentespc");
        }

        [Authorize]
        public ActionResult tipospc()
        {
            return View(tipocp.Listar());
        }


        [Authorize]
        public ActionResult Crudtipopc(int id = 0)
        {

            return View(id == 0 ? new tipoPC()
                        : tipocp.Obtener(id));
        }

        [Authorize]
        public ActionResult Guardartipopc(tipoPC model)
        {
            tipocp.Guardar(model);
            return Redirect("~/ABM/tipospc");
        }

        [Authorize]
        public ActionResult Editartipopc(int id = 0)
        {
            return View(id == 0 ? new tipoPC()
                        : tipocp.Obtener(id));
        }

        [Authorize]
        public ActionResult Eliminartipopc(int id)
        {

            tipocp.Eliminar(id);
            return Redirect("~/ABM/tipospc");
        }

        [Authorize]
        public ActionResult ubicacionespc()
        {
            return View(ubipc.Listar());
        }


        [Authorize]
        public ActionResult Crudubipc(int id = 0)
        {

            return View(id == 0 ? new ubicacionPC()
                        : ubipc.Obtener(id));
        }


        [Authorize]
        public ActionResult Guardarubipc(ubicacionPC model)
        {
            ubipc.Guardar(model);
            return Redirect("~/ABM/ubicacionespc");
        }

        [Authorize]
        public ActionResult Editarubipc(int id = 0)
        {
            return View(id == 0 ? new ubicacionPC()
                        : ubipc.Obtener(id));
        }

        [Authorize]
        public ActionResult Eliminarubipc(int id)
        {

            ubipc.Eliminar(id);
            return Redirect("~/ABM/ubicacionespc");
        }

        [Authorize]
        public ActionResult motivospc()
        {
            return View(motivopc.Listar());
        }


        [Authorize]
        public ActionResult Crudmotivopc(int id = 0)
        {

            return View(id == 0 ? new motivobajaPC()
                        : motivopc.Obtener(id));
        }

        [Authorize]
        public ActionResult Guardarmotivopc(motivobajaPC model)
        {
            motivopc.Guardar(model);
            return Redirect("~/ABM/motivospc");
        }

        [Authorize]
        public ActionResult Editarmotivopc(int id = 0)
        {
            return View(id == 0 ? new motivobajaPC()
                        : motivopc.Obtener(id));
        }

        [Authorize]
        public ActionResult Eliminarmotivopc(int id)
        {

            motivopc.Eliminar(id);
            return Redirect("~/ABM/motivospc");
        }

        [Authorize]
        public ActionResult procesadorespc()
        {
            return View(procesador.Listar());
        }

        [Authorize]
        public ActionResult Crudprocesadorpc(int id = 0)
        {

            return View(id == 0 ? new procesadorPC()
                        : procesador.Obtener(id));
        }


        [Authorize]
        public ActionResult Guardarprocesadorpc(procesadorPC model)
        {
            procesador.Guardar(model);
            return Redirect("~/ABM/procesadorespc");
        }

        [Authorize]
        public ActionResult Editarprocesadorpc(int id = 0)
        {
            return View(id == 0 ? new procesadorPC()
                        : procesador.Obtener(id));
        }

        [Authorize]
        public ActionResult Eliminarprocesadorpc(int id)
        {

            procesador.Eliminar(id);
            return Redirect("~/ABM/procesadorespc");
        }

        [Authorize]
        public ActionResult insumospc()
        {
            return View(insumo_pc.Listar());
        }


        [Authorize]
        public ActionResult Crudinsumopc(int id = 0)
        {

            return View(id == 0 ? new insumo_pc()
                        : insumo_pc.Obtener(id));
        }


        [Authorize]
        public ActionResult Guardarinsumopc(insumo_pc model)
        {
            insumo_pc.Guardar(model);
            return Redirect("~/ABM/insumospc");
        }


        [Authorize]
        public ActionResult Editarinsumopc(int id = 0)
        {
            return View(id == 0 ? new insumo_pc()
                        : insumo_pc.Obtener(id));
        }


        [Authorize]
        public ActionResult Eliminarinsumopc(int id)
        {

            insumo_pc.Eliminar(id);
            return Redirect("~/ABM/insumospc");
        }

        [Authorize]
        public ActionResult tipo_telefonos()
        {
            return View(tel.Listar());
        }


        [Authorize]
        public ActionResult Crudtipo_tel(int id = 0)
        {

            return View(id == 0 ? new tipo_tel()
                        : tel.Obtener(id));
        }


        [Authorize]
        public ActionResult Guardartipo_tel(tipo_tel model)
        {
            tel.Guardar(model);
            return Redirect("~/ABM/tipo_telefonos");
        }

        [Authorize]
        public ActionResult Editartipo_tel(int id = 0)
        {
            return View(id == 0 ? new tipo_tel()
                        : tel.Obtener(id));
        }


        [Authorize]
        public ActionResult Eliminartipo_tel(int id)
        {
            tel.Eliminar(id);
            return Redirect("~/ABM/tipo_telefonos");
        }


        [Authorize]
        public ActionResult mmarcas()
        {
            return View(mm.Listar());
        }


        [Authorize]
        public ActionResult Crudmmarca(int id = 0)
        {

            return View(id == 0 ? new marca_modelo_cel()
                        : mm.Obtener(id));
        }


        [Authorize]
        public ActionResult Guardarmmarca(marca_modelo_cel model)
        {
            mm.Guardar(model);
            return Redirect("~/ABM/mmarcas");
        }


        [Authorize]
        public ActionResult Editarmmarca(int id = 0)
        {
            return View(id == 0 ? new marca_modelo_cel()
                        : mm.Obtener(id));
        }


        [Authorize]
        public ActionResult Eliminarmmarca(int id)
        {
            mm.Eliminar(id);
            return Redirect("~/ABM/mmarcas");
        }


        [Authorize]
        public ActionResult ubicacionestel()
        {
            return View(ut.Listar());
        }


        [Authorize]
        public ActionResult Crudubicaciontel(int id = 0)
        {

            return View(id == 0 ? new ubicacion_tel()
                        : ut.Obtener(id));
        }


        [Authorize]
        public ActionResult Guardarubicaciontel(ubicacion_tel model)
        {
            ut.Guardar(model);
            return Redirect("~/ABM/ubicacionestel");
        }

        [Authorize]
        public ActionResult Editarubicaciontel(int id = 0)
        {
            return View(id == 0 ? new ubicacion_tel()
                        : ut.Obtener(id));
        }


        [Authorize]
        public ActionResult Eliminarubicaciontel(int id)
        {
            ut.Eliminar(id);
            return Redirect("~/ABM/ubicacionestel");
        }



        [Authorize]
        public ActionResult incidentestel()
        {
            return View(incitel.Listar());
        }
        [Authorize]
        public ActionResult Crudincidentetel(int id = 0)
        {

            return View(id == 0 ? new incidente_tel()
                        : incitel.Obtener(id));
        }
        [Authorize]
        public ActionResult Guardarincidentetel(incidente_tel model)
        {
            incitel.Guardar(model);
            return Redirect("~/ABM/incidentestel");
        }
        [Authorize]
        public ActionResult Editarincidentetel(int id = 0)
        {
            return View(id == 0 ? new incidente_tel()
                        : incitel.Obtener(id));
        }
        [Authorize]
        public ActionResult Eliminarincidentetel(int id)
        {
            incitel.Eliminar(id);
            return Redirect("~/ABM/incidentestel");
        }









        //DE ACA EN ADELANTE ARRANCA LA ACTUALIZACION DE LOS NUEVOS EQUIPOS



        //ABM INSUMO_CAMARAS
        public insumo_cam_dominio insumocam = new insumo_cam_dominio();

        [Authorize]
        public ActionResult insumosCam()
        {
            return View(insumocam.Listar());
        }


        [Authorize]
        public ActionResult Crudinsumocam(int id = 0)
        {

            return View(id == 0 ? new insumo_camara()
                        : insumocam.Obtener(id));
        }


        [Authorize]
        public ActionResult Guardarinsumocam(insumo_camara model)
        {
            insumocam.Guardar(model);
            return Redirect("~/ABM/insumosCam");
        }


        [Authorize]
        public ActionResult EditarinsumoCam(int id = 0)
        {
            return View(id == 0 ? new insumo_camara()
                        : insumocam.Obtener(id));
        }


        [Authorize]
        public ActionResult EliminarinsumoCam(int id)
        {

            insumocam.Eliminar(id);
            return Redirect("~/ABM/insumosCam");
        }




        //ABM MODELOS_CAM
        public mmarca_camara_dominio modelo_cam = new mmarca_camara_dominio();

        [Authorize]
        public ActionResult modeloCam()
        {
            return View(modelo_cam.Listar());
        }


        [Authorize]
        public ActionResult Crudmmaarcacam(int id = 0)
        {

            return View(id == 0 ? new marca_modelo_camara()
                        : modelo_cam.Obtener(id));
        }


        [Authorize]
        public ActionResult Guardarmmarcacam(marca_modelo_camara model)
        {
            modelo_cam.Guardar(model);
            return Redirect("~/ABM/modeloCam");
        }


        [Authorize]
        public ActionResult EditarmodeloCam(int id = 0)
        {
            return View(id == 0 ? new marca_modelo_camara()
                        : modelo_cam.Obtener(id));
        }


        [Authorize]
        public ActionResult EliminarmodeloCam(int id)
        {

            modelo_cam.Eliminar(id);
            return Redirect("~/ABM/modeloCam");
        }




        //ABM UBICACIONES DE CAMARAS
        public ubicacion_camara_dominio ubi_cam = new ubicacion_camara_dominio();

        [Authorize]
        public ActionResult ubiCam()
        {
            return View(ubi_cam.Listar());
        }


        [Authorize]
        public ActionResult Crudubicam(int id = 0)
        {

            return View(id == 0 ? new ubicacion_camara()
                        : ubi_cam.Obtener(id));
        }


        [Authorize]
        public ActionResult Guardarubicam(ubicacion_camara model)
        {
            ubi_cam.Guardar(model);
            return Redirect("~/ABM/ubiCam");
        }


        [Authorize]
        public ActionResult EditarubiCam(int id = 0)
        {
            return View(id == 0 ? new ubicacion_camara()
                        : ubi_cam.Obtener(id));
        }


        [Authorize]
        public ActionResult EliminarubiCam(int id)
        {

            ubi_cam.Eliminar(id);
            return Redirect("~/ABM/ubiCam");
        }




        //ABM INCIDENTES DE CAMARAS
        public incidente_camara_dominio incidente_cam = new incidente_camara_dominio();

        [Authorize]
        public ActionResult incidenteCam()
        {
            return View(incidente_cam.Listar());
        }

        [Authorize]
        public ActionResult Crudincidentecam(int id = 0)
        {

            return View(id == 0 ? new incidente_camara()
                        : incidente_cam.Obtener(id));
        }

        [Authorize]
        public ActionResult Guardarincidentecam(incidente_camara model)
        {
            incidente_cam.Guardar(model);
            return Redirect("~/ABM/incidenteCam");
        }

        [Authorize]
        public ActionResult EditarincidenteCam(int id = 0)
        {
            return View(id == 0 ? new incidente_camara()
                        : incidente_cam.Obtener(id));
        }

        [Authorize]
        public ActionResult EliminarincidenteCam(int id)
        {

            ubi_cam.Eliminar(id);
            return Redirect("~/ABM/incidenteCam");
        }




        //ABM UBICACIONES SWITCH
        public ubicacion_sw_dominio ubicacion_sw = new ubicacion_sw_dominio();

        [Authorize]
        public ActionResult ubicacionSW()
        {
            return View(ubicacion_sw.Listar());
        }

        [Authorize]
        public ActionResult Crudubicacionsw(int id = 0)
        {

            return View(id == 0 ? new ubicacion_switch()
                        : ubicacion_sw.Obtener(id));
        }

        [Authorize]
        public ActionResult Guardarubicacionsw(ubicacion_switch model)
        {
            ubicacion_sw.Guardar(model);
            return Redirect("~/ABM/ubicacionSW");
        }

        [Authorize]
        public ActionResult Editarubicacionsw(int id = 0)
        {
            return View(id == 0 ? new ubicacion_switch()
                        : ubicacion_sw.Obtener(id));
        }

        [Authorize]
        public ActionResult Eliminarubicacionsw(int id)
        {

            ubicacion_sw.Eliminar(id);
            return Redirect("~/ABM/ubicacionSW");
        }


        //ABM INCIDENTES SWITCH
        public incidente_sw_dominio incidente_sw = new incidente_sw_dominio();

        [Authorize]
        public ActionResult incidenteSW()
        {
            return View(incidente_sw.Listar());
        }

        [Authorize]
        public ActionResult Crudincidentesw(int id = 0)
        {

            return View(id == 0 ? new incidente_switch()
                        : incidente_sw.Obtener(id));
        }

        [Authorize]
        public ActionResult Guardarincidentesw(incidente_switch model)
        {
            incidente_sw.Guardar(model);
            return Redirect("~/ABM/incidenteSW");
        }

        [Authorize]
        public ActionResult Editarincidentesw(int id = 0)
        {
            return View(id == 0 ? new incidente_switch()
                        : incidente_sw.Obtener(id));
        }

        [Authorize]
        public ActionResult Eliminarincidentesw(int id)
        {

            incidente_sw.Eliminar(id);
            return Redirect("~/ABM/incidenteSW");
        }


        //ABM MODELOS SWITCH
        public modelo_sw_dominio modelo_sw = new modelo_sw_dominio();

        [Authorize]
        public ActionResult modeloSW()
        {
            return View(modelo_sw.Listar());
        }

        [Authorize]
        public ActionResult Crudmodelosw(int id = 0)
        {

            return View(id == 0 ? new marca_modelo_switch()
                        : modelo_sw.Obtener(id));
        }

        [Authorize]
        public ActionResult Guardarmodelosw(marca_modelo_switch model)
        {
            modelo_sw.Guardar(model);
            return Redirect("~/ABM/modeloSW");
        }

        [Authorize]
        public ActionResult Editarmodelosw(int id = 0)
        {
            return View(id == 0 ? new marca_modelo_switch()
                        : modelo_sw.Obtener(id));
        }

        [Authorize]
        public ActionResult Eliminarmodeloswm(int id)
        {

            modelo_sw.Eliminar(id);
            return Redirect("~/ABM/modeloSW");
        }


        //ABM UBICACION ESCANER
        public ubicacion_escaner_dominio ubi_es = new ubicacion_escaner_dominio();

        [Authorize]
        public ActionResult ubicacionES()
        {
            return View(ubi_es.Listar());
        }

        [Authorize]
        public ActionResult CrudubicacionES(int id = 0)
        {

            return View(id == 0 ? new ubicacion_escaner()
                        : ubi_es.Obtener(id));
        }

        [Authorize]
        public ActionResult GuardarubicacionES(ubicacion_escaner model)
        {
            ubi_es.Guardar(model);
            return Redirect("~/ABM/ubicacionES");
        }

        [Authorize]
        public ActionResult EditarubicacionES(int id = 0)
        {
            return View(id == 0 ? new ubicacion_escaner()
                        : ubi_es.Obtener(id));
        }

        [Authorize]
        public ActionResult EliminarubicacionES(int id)
        {

            ubi_es.Eliminar(id);
            return Redirect("~/ABM/ubicacionES");
        }




        //ABM MODELOS ESCANER
        public modelo_escaner_dominio modelo_es = new modelo_escaner_dominio();

        [Authorize]
        public ActionResult modeloES()
        {
            return View(modelo_es.Listar());
        }

        [Authorize]
        public ActionResult CrudmodeloES(int id = 0)
        {

            return View(id == 0 ? new marca_modelo_escaner()
                        : modelo_es.Obtener(id));
        }

        [Authorize]
        public ActionResult GuardarmodeloES(marca_modelo_escaner model)
        {
            modelo_es.Guardar(model);
            return Redirect("~/ABM/modeloES");
        }

        [Authorize]
        public ActionResult EditarmodeloES(int id = 0)
        {
            return View(id == 0 ? new marca_modelo_escaner()
                        : modelo_es.Obtener(id));
        }

        [Authorize]
        public ActionResult EliminarmodeloES(int id)
        {

            modelo_es.Eliminar(id);
            return Redirect("~/ABM/modeloES");
        }






        //ABM INCIDENTE ESCANER
        public incidente_escaner_dominio incidente_es = new incidente_escaner_dominio();

        [Authorize]
        public ActionResult incidenteES()
        {
            return View(incidente_es.Listar());
        }

        [Authorize]
        public ActionResult CrudincidenteES(int id = 0)
        {

            return View(id == 0 ? new incidente_escaner()
                        : incidente_es.Obtener(id));
        }

        [Authorize]
        public ActionResult GuardarincidenteES(incidente_escaner model)
        {
            incidente_es.Guardar(model);
            return Redirect("~/ABM/incidenteES");
        }

        [Authorize]
        public ActionResult EditarincidenteES(int id = 0)
        {
            return View(id == 0 ? new incidente_escaner()
                        : incidente_es.Obtener(id));
        }

        [Authorize]
        public ActionResult EliminarincidenteES(int id)
        {

            incidente_es.Eliminar(id);
            return Redirect("~/ABM/incidenteES");
        }













    }
}