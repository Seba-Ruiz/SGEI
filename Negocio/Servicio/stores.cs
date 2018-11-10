using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Negocio.DTO;

namespace Negocio.Servicio
{
    public class stores
    {
        public List<DTO_u_computadora> u_computadora(int ubicacionpc)
        {
            var dto_u_computadora = new List<DTO_u_computadora>();


            using (var ctx = new SGEIContext())
            {
                var ubica = (from a in ctx.pc
                             from b in ctx.ubicacionPC
                             from c in ctx.pc_ubicacionpc
                             from d in ctx.detallePC
                             from e in ctx.tipoPC
                             from f in ctx.ramPC
                             from g in ctx.discoPC
                             from h in ctx.soPC
                             from i in ctx.motherPC
                             from j in ctx.procesadorPC

                             where
                             c.ubicacionpc_id == ubicacionpc &&
                             c.pc_id == a.id_pc &&
                             c.ubicacionpc_id == b.id_ubicacion &&
                             a.id_pc == d.pc_id &&
                             a.tipo_id == e.id_tipo &&
                             d.disco_id == g.id_disco &&
                             d.mother_id == i.id_mother &&
                             d.so_id == h.id_so &&
                             d.ram_id == f.id_ram &&
                             d.procesador_id == j.id_procesador &&
                             a.fecha_baja == null

                             select new
                             {
                                 nombre = a.nombre,
                                 ip = a.ip,
                                 descripcion = a.descripcion,
                                 id_pc = a.id_pc,
                                 ubicacion = b.nombre,
                                 fecha_ubicacion = c.fecha_ubicacion,
                                 responsablepc = d.responsablepc,
                                 observacion = d.observacion,
                                 nombre_tipo = e.nombre_tipo,
                                 ram = f.descripcion,
                                 disco = g.descripcion,
                                 so = h.descripcion,
                                 procesador = j.descripcion,
                                 mother = i.descripcion,
                                 id_detalle = d.id_detalle

                             }).ToList();

                foreach (var item in ubica)
                {
                    var dto = new DTO_u_computadora();

                    dto.nombre = item.nombre;
                    dto.ip = item.ip;
                    dto.descripcion = item.descripcion;
                    dto.id_pc = item.id_pc;
                    dto.ubicacion = item.ubicacion;
                    dto.fecha_ubicacion = item.fecha_ubicacion;
                    dto.responsablepc = item.responsablepc;
                    dto.observacion = item.observacion;
                    dto.nombre_tipo = item.nombre_tipo;
                    dto.ram = item.ram;
                    dto.disco = item.disco;
                    dto.so = item.so;
                    dto.procesador = item.procesador;
                    dto.mother = item.mother;
                    dto.id_detalle = item.id_detalle;

                    dto_u_computadora.Add(dto);

                }



                return dto_u_computadora;
            }

        }

        public DTO_u_computadora_detalle u_computadora_detalle(int id_detalle)
        {
            var dto_u_computadora_detalle = new List<DTO_u_computadora_detalle>();


            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.pc
                               from b in ctx.ubicacionPC
                               from c in ctx.pc_ubicacionpc
                               from d in ctx.detallePC
                               from e in ctx.tipoPC
                               from f in ctx.ramPC
                               from g in ctx.discoPC
                               from h in ctx.soPC
                               from i in ctx.motherPC
                               from j in ctx.procesadorPC

                               where

                               d.id_detalle == id_detalle &&
                               c.pc_id == a.id_pc &&
                               c.ubicacionpc_id == b.id_ubicacion &&
                               a.id_pc == d.pc_id &&
                               a.tipo_id == e.id_tipo &&
                               d.disco_id == g.id_disco &&
                               d.mother_id == i.id_mother &&
                               d.so_id == h.id_so &&
                               d.ram_id == f.id_ram &&
                               d.procesador_id == j.id_procesador &&
                               a.fecha_baja == null

                               select new
                               {
                                   nombre = a.nombre,
                                   id_pc = a.id_pc,
                                   ip = a.ip,
                                   descripcion = a.descripcion,
                                   ubicacion = b.nombre,
                                   fecha_ubicacion = c.fecha_ubicacion,
                                   responsablepc = d.responsablepc,
                                   observacion = d.observacion,
                                   nombre_tipo = e.nombre_tipo,
                                   ram = f.descripcion,
                                   disco = g.descripcion,
                                   so = h.descripcion,
                                   procesador = j.descripcion,
                                   mother = i.descripcion,
                                   id_detalle = d.id_detalle

                               }).FirstOrDefault();


                var dto = new DTO_u_computadora_detalle();

                dto.nombre = detalle.nombre;
                dto.ip = detalle.ip;
                dto.descripcion = detalle.descripcion;
                dto.id_pc = detalle.id_pc;
                dto.ubicacion = detalle.ubicacion;
                dto.fecha_ubicacion = detalle.fecha_ubicacion;
                dto.responsablepc = detalle.responsablepc;
                dto.observacion = detalle.observacion;
                dto.nombre_tipo = detalle.nombre_tipo;
                dto.ram = detalle.ram;
                dto.disco = detalle.disco;
                dto.so = detalle.so;
                dto.procesador = detalle.procesador;
                dto.mother = detalle.mother;
                dto.id_detalle = detalle.id_detalle;

                return dto;
            }


        }

        public DTO_u_computadora_detalle_id u_computadora_detalle_id(int id_detalle)
        {
            var dto_u_computadora_detalle = new List<DTO_u_computadora_detalle_id>();


            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.pc
                               from b in ctx.ubicacionPC
                               from c in ctx.pc_ubicacionpc
                               from d in ctx.detallePC
                               from e in ctx.tipoPC
                               from f in ctx.ramPC
                               from g in ctx.discoPC
                               from h in ctx.soPC
                               from i in ctx.motherPC
                               from j in ctx.procesadorPC

                               where

                               d.id_detalle == id_detalle &&
                               c.pc_id == a.id_pc &&
                               c.ubicacionpc_id == b.id_ubicacion &&
                               a.id_pc == d.pc_id &&
                               a.tipo_id == e.id_tipo &&
                               d.disco_id == g.id_disco &&
                               d.mother_id == i.id_mother &&
                               d.so_id == h.id_so &&
                               d.ram_id == f.id_ram &&
                               d.procesador_id == j.id_procesador &&
                               a.fecha_baja == null

                               select new
                               {

                                   id_pc = a.id_pc,
                                   nombre_pc = a.nombre,
                                   ip = a.ip,
                                   descripcion_pc = a.descripcion,
                                   responsablepc = d.responsablepc,
                                   observacion = d.observacion,
                                   id_ubicacion = b.id_ubicacion,

                                   id_tipo = e.id_tipo,
                                   id_ram = f.id_ram,
                                   id_disco = g.id_disco,
                                   id_so = h.id_so,
                                   id_procesador = j.id_procesador,
                                   id_mother = i.id_mother,
                                   id_detalle = d.id_detalle

                               }).FirstOrDefault();


                var dto = new DTO_u_computadora_detalle_id();

                dto.nombre_pc = detalle.nombre_pc;

                dto.ip = detalle.ip;
                dto.descripcion_pc = detalle.descripcion_pc;
                dto.id_pc = detalle.id_pc;
                dto.responsablepc = detalle.responsablepc;
                dto.observacion = detalle.observacion;

                dto.id_tipo = detalle.id_tipo;
                dto.id_ram = detalle.id_ram;
                dto.id_disco = detalle.id_disco;
                dto.id_so = detalle.id_so;
                dto.id_procesador = detalle.id_procesador;
                dto.id_mother = detalle.id_mother;
                dto.id_detalle = detalle.id_detalle;

                return dto;
            }


        }

        public List<DTO_u_impresora_01> u_impresora_01(int ubicacion)
        {
            var dto_u_impresora_01 = new List<DTO_u_impresora_01>();


            using (var ctx = new SGEIContext())
            {
                var ubica = (from a in ctx.impresora
                             from b in ctx.ubicacion
                             from c in ctx.ubicacion_impresora
                             from d in ctx.detalle_impresora_ubicacion

                             where

                             c.ubicacion_id == ubicacion &&
                             c.impresora_id == a.id &&
                             c.ubicacion_id == b.id &&
                             c.id == d.ubicacion_impresora_id

                             select new
                             {

                                 id_imp = c.id,
                                 marca_modelo = a.marca_modelo,
                                 tipo = a.tipo,
                                 descripcion = c.descripcion,
                                 fecha_ubicacion = c.fecha_ubicacion,
                                 responsable = b.responsable,
                                 ip = d.ip,

                                 pc_dondeseconecta = d.pc_dondeseconecta,
                                 id = c.id
                             }).ToList();


                foreach (var item in ubica)
                {
                    var dto = new DTO_u_impresora_01();

                    dto.id_imp = item.id_imp;
                    dto.marca_modelo = item.marca_modelo;
                    dto.tipo = item.tipo;
                    dto.descripcion = item.descripcion;
                    dto.fecha_ubicacion = item.fecha_ubicacion;
                    dto.responsable = item.responsable;
                    dto.ip = item.ip;
                    dto.pc_dondeseconecta = item.pc_dondeseconecta;
                    dto.id = item.id;

                    dto_u_impresora_01.Add(dto);

                }

                return dto_u_impresora_01;

            }


        }

        public List<DTO_consulta_01a> consulta_01a(int id, DateTime fdesde, DateTime fhasta)
        {
            var dto_consulta_01a = new List<DTO_consulta_01a>();


            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.detalle_insumo_impresora
                               from b in ctx.insumo

                               where

                               a.ubicacion_impresora_id == id &&
                               a.fechacambioinsumo <= fhasta &&
                               a.fechacambioinsumo >= fdesde &&
                               a.insumo_id == b.id


                               select new
                               {

                                   fechacambioinsumo = a.fechacambioinsumo,
                                   nombre = b.nombre


                               }).ToList();


                foreach (var item in detalle)
                {
                    var dto = new DTO_consulta_01a();

                    dto.fechacambioinsumo = item.fechacambioinsumo;
                    dto.nombre = item.nombre;


                    dto_consulta_01a.Add(dto);

                }



                return dto_consulta_01a;
            }


        }

        public List<DTO_consulta_pc> consulta_pc(int id, DateTime fdesde, DateTime fhasta)
        {
            var dto_consulta_pc = new List<DTO_consulta_pc>();


            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.pc_insumo_pc
                               from b in ctx.insumo_pc

                               where

                               a.detalle_pc_id == id &&
                               a.fecha_insumo <= fhasta &&
                               a.fecha_insumo >= fdesde &&
                               a.insumo_id == b.id_insumo


                               select new
                               {

                                   fecha_insumo = a.fecha_insumo,
                                   nombre = b.nombre


                               }).ToList();


                foreach (var item in detalle)
                {
                    var dto = new DTO_consulta_pc();

                    dto.fecha_insumo = item.fecha_insumo;
                    dto.nombre = item.nombre;


                    dto_consulta_pc.Add(dto);

                }
                return dto_consulta_pc;
            }
        }

        public List<DTO_consulta_pc_detalle> consulta_pc_detalle(int? id, DateTime fdesde, DateTime fhasta)
        {
            var dto_consulta_pc_detalle = new List<DTO_consulta_pc_detalle>();


            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.pc_incidentepc
                               from b in ctx.incidentePC

                               where

                               a.pc_id == id &&
                               a.fecha_incidente >= fdesde &&
                               a.fecha_incidente <= fhasta &&
                               a.incidentepc_id == b.id_incidentepc


                               select new
                               {

                                   fecha_incidente = a.fecha_incidente ,
                                   fecha_reparacion  = a.fecha_reparacion,
                                   reparacion = a.reparacion,
                                   nombre = b.nombre


                               }).ToList();


                foreach (var item in detalle)
                {
                    var dto = new DTO_consulta_pc_detalle();

                    dto.fecha_incidente = item.fecha_incidente;
                    dto.fecha_reparacion = item.fecha_reparacion;
                    dto.nombre = item.nombre;
                    dto.reparacion = item.reparacion;


                    dto_consulta_pc_detalle.Add(dto);

                }
                return dto_consulta_pc_detalle;
            }
        }

        public List<DTO_u_telefono_01> u_telefono_01(int ubicacion)
        {
            var dto_u_telefono_01 = new List<DTO_u_telefono_01>();


            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.detalle_tel
                               from b in ctx.telefono
                               from c in ctx.marca_modelo_cel
                               from d in ctx.ubicacion_tel_tel
                               from e in ctx.ubicacion_tel
                               from f in ctx.tipo_tel


                               where

                               d.ubicacion_tel_id == ubicacion &&
                               e.id_ubicacion_tel == d.ubicacion_tel_id &&
                               d.detalle_id == a.id_detalle &&
                               a.marca_modelo_id == c.id_mm &&
                               a.tipo_tel_id == f.id_tipo &&
                               a.telefono_id == b.id_telefono &&
                               a.tipo_tel_id == f.id_tipo &&
                               b.fecha_baja == null


                               select new
                               {

                                   nro_interno = a.nro_interno,
                                   descripcion = b.descripcion,
                                   id_telefono = b.id_telefono,
                                   marca = c.descripcion,
                                   fecha_ubicacion = d.fecha_ubicacion,
                                   nombre_ubi = e.nombre_ubi,
                                   nombre_tipo = f.nombre_tipo,
                                   id_detalle = a.id_detalle


                               }).ToList();


                foreach (var item in detalle)
                {
                    var dto = new DTO_u_telefono_01();

                    dto.nro_interno = item.nro_interno;
                    dto.descripcion = item.descripcion;
                    dto.id_telefono = item.id_telefono;
                    dto.mmarca = item.marca;
                    dto.fecha_ubicacion = item.fecha_ubicacion;
                    dto.nombre_ubi = item.nombre_ubi;
                    dto.nombre_tipo = item.nombre_tipo;
                    dto.id_detalle = item.id_detalle;


                    dto_u_telefono_01.Add(dto);

                }
                return dto_u_telefono_01;
            }
        }

        public List<DTO_consulta_tel_incidente> consulta_tel_incidente(int? id, DateTime fdesde, DateTime fhasta)
        {
            var dto_consulta_tel_incidente = new List<DTO_consulta_tel_incidente>();


            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.incidente_tel_tel
                               from b in ctx.incidente_tel


                               where

                               a.detalle_id == id &&
                               a.fecha_incidente >= fdesde &&
                               a.fecha_incidente <= fhasta &&
                               a.incidentetel_id == b.id_incidente_tel


                               select new
                               {

                                   fecha_incidente = a.fecha_incidente,
                                   fecha_reparacion = a.fecha_reparacion,
                                   reparacion = a.reparacion,
                                   nombre_incidente = b.nombre_incidente,
                                   


                               }).ToList();


                foreach (var item in detalle)
                {
                    var dto = new DTO_consulta_tel_incidente();

                    dto.fecha_incidente = item.fecha_incidente;
                    dto.fecha_reparacion = item.fecha_reparacion;
                    dto.reparacion = item.reparacion;
                    dto.nombre_incidente = item.nombre_incidente;


                    dto_consulta_tel_incidente.Add(dto);

                }
                return dto_consulta_tel_incidente;
            }
        }

        public DTO_u_detalle_telefono u_detalle_telefono(int ubicacion)
        {
            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.detalle_tel
                               from b in ctx.telefono
                               from c in ctx.marca_modelo_cel
                               from d in ctx.ubicacion_tel_tel
                               from e in ctx.ubicacion_tel
                               from f in ctx.tipo_tel


                               where

                               a.id_detalle == ubicacion &&
                               a.marca_modelo_id == c.id_mm &&
                               a.telefono_id == b.id_telefono &&
                               a.tipo_tel_id == f.id_tipo &&
                               a.id_detalle == d.detalle_id &&
                               d.ubicacion_tel_id == e.id_ubicacion_tel


                               select new
                               {

                                   nro_interno = a.nro_interno,
                                   id_telefono = b.id_telefono,
                                   descripcion = b.descripcion,
                                   id_mm = c.id_mm,
                                   fecha_ubicacion = d.fecha_ubicacion,
                                   id_ubicacion_tel = e.id_ubicacion_tel,
                                   id_tipo = f.id_tipo,
                                   id_detalle = a.id_detalle


                               }).FirstOrDefault();


                
                    var dto = new DTO_u_detalle_telefono();

                    dto.nro_interno = detalle.nro_interno;
                    dto.id_telefono = detalle.id_telefono;
                    dto.descripcion = detalle.descripcion;
                    dto.id_mm = detalle.id_mm;
                    dto.fecha_ubicacion = detalle.fecha_ubicacion;
                    dto.id_ubicacion_tel = detalle.id_ubicacion_tel;
                    dto.id_tipo = detalle.id_tipo;
                    dto.id_detalle = detalle.id_detalle;


                return dto;
            }
                
            }

        public List<DTO_u_impresora_04> u_impresora_04()
        {
            var dto_u_impresora_04 = new List<DTO_u_impresora_04>();


            using (var ctx = new SGEIContext())
            {
                var ubica = (from a in ctx.impresora
                             from b in ctx.ubicacion
                             from c in ctx.ubicacion_impresora
                             from d in ctx.detalle_impresora_ubicacion
                             from e in ctx.baja

                             where

                             c.impresora_id == a.id &&
                             c.ubicacion_id == b.id &&
                             c.id == d.ubicacion_impresora_id &&
                             c.id == e.ubicacion_impresora_id &&
                             c.fecha_baja != null

                             select new
                             {

                                 marca_modelo = a.marca_modelo,
                                 tipo = a.tipo,
                                 descripcion = c.descripcion,
                                 responsable = b.responsable,
                                 ip = d.ip,
                                 pc_dondeseconecta = d.pc_dondeseconecta,
                                 fecha_baja = c.fecha_baja,
                                 motivo_baja = e.motivo_baja,
                                 ultima_ubicacion = e.ultima_ubicacion,
                                 id = c.id
                             }).ToList();


                foreach (var item in ubica)
                {
                    var dto = new DTO_u_impresora_04();

                   
                    dto.marca_modelo = item.marca_modelo;
                    dto.tipo = item.tipo;
                    dto.descripcion = item.descripcion;
                    dto.responsable = item.responsable;
                    dto.ip = item.ip;
                    dto.pc_dondeseconecta = item.pc_dondeseconecta;
                    dto.fecha_baja = item.fecha_baja;
                    dto.motivo_baja = item.motivo_baja;
                    dto.ultima_ubicacion = item.ultima_ubicacion;
                    dto.id = item.id;

                    dto_u_impresora_04.Add(dto);

                }

                return dto_u_impresora_04;

            }


        }

        public DTO_u_detalle_camara u_detalle_camara(int ubicacion)
        {
            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.detalle_camara
                               from b in ctx.camara
                               from c in ctx.marca_modelo_camara
                               from d in ctx.ubicacion_camara_camara
                               from e in ctx.ubicacion_camara


                               where

                               a.id_detalle == ubicacion &&
                               a.marca_model_id == c.id_mmarca &&
                               a.camara_id == b.id_camara &&
                               a.id_detalle == d.detalle_camara_id &&
                               d.ubicacion_camara_id == e.id_ubicacion_camara


                               select new
                               {

                                   nro_ip = a.nroip,
                                   id_camara = b.id_camara,
                                   descripcion = b.descripcion,
                                   id_mm = c.id_mmarca,
                                   fecha_ubicacion = d.fehca_ubicacion,
                                   id_ubicacion_cam = e.id_ubicacion_camara,
                                   id_detalle = a.id_detalle


                               }).FirstOrDefault();



                var dto = new DTO_u_detalle_camara();

                dto.nro_ip = detalle.nro_ip;
                dto.id_camara = detalle.id_camara;
                dto.descripcion = detalle.descripcion;
                dto.id_mm = detalle.id_mm;
                dto.fecha_ubicacion = detalle.fecha_ubicacion;
                dto.id_ubicacion_camara = detalle.id_ubicacion_cam;
                dto.id_detalle = detalle.id_detalle;


                return dto;
            }

        }

        public List<DTO_u_camara_01> u_camara_01(int ubicacion)
        {
            var dto_u_camara_01 = new List<DTO_u_camara_01>();


            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.detalle_camara
                               from b in ctx.camara
                               from c in ctx.marca_modelo_camara
                               from d in ctx.ubicacion_camara_camara
                               from e in ctx.ubicacion_camara


                               where

                               d.ubicacion_camara_id == ubicacion &&
                               e.id_ubicacion_camara == d.ubicacion_camara_id &&
                               d.detalle_camara_id == a.id_detalle &&
                               a.marca_model_id == c.id_mmarca &&
                               a.camara_id == b.id_camara &&
                               b.fecha_baja == null


                               select new
                               {

                                   nro_interno = a.nroip,
                                   descripcion = b.descripcion,
                                   id_camara = b.id_camara,
                                   marca = c.marca_modelo,
                                   fecha_ubicacion = d.fehca_ubicacion,
                                   nombre_ubi = e.nombre,
                                   id_detalle = a.id_detalle


                               }).ToList();


                foreach (var item in detalle)
                {
                    var dto = new DTO_u_camara_01();

                    dto.nro_ip = item.nro_interno;
                    dto.descripcion = item.descripcion;
                    dto.id_camara = item.id_camara;
                    dto.mmarca = item.marca;
                    dto.fecha_ubicacion = item.fecha_ubicacion;
                    dto.nombre_ubi = item.nombre_ubi;
                    dto.id_detalle = item.id_detalle;


                    dto_u_camara_01.Add(dto);

                }
                return dto_u_camara_01;
            }
        }

        public List<DTO_u_sw_01> u_sw_01(int ubicacion)
        {
            var dto_u_sw_01 = new List<DTO_u_sw_01>();


            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.switch_detalle
                               from b in ctx.swicth
                               from c in ctx.marca_modelo_switch
                               from d in ctx.switch_ubicacion_switch
                               from e in ctx.ubicacion_switch


                               where

                               d.ubicacion_switch_id == ubicacion &&
                               e.id_ubicacion_switch == d.ubicacion_switch_id &&
                               d.detalle_swich_id == a.id_detalle_switch &&
                               a.marca_modelo_id == c.id_mmarca &&
                               a.switch_id == b.id_switch &&
                               b.fecha_baja == null


                               select new
                               {

                                   nro_ip = a.nroip,
                                   descripcion = b.descripcion,
                                   id_sw = b.id_switch,
                                   marca = c.marca_modelo,
                                   fecha_ubicacion = d.fecha_ubicacion,
                                   nombre_ubi = e.nombre,
                                   id_detalle = a.id_detalle_switch,
                                   interfaces = a.interfaces


                               }).ToList();


                foreach (var item in detalle)
                {
                    var dto = new DTO_u_sw_01();

                    dto.nro_ip = item.nro_ip;
                    dto.descripcion = item.descripcion;
                    dto.id_sw = item.id_sw;
                    dto.mmarca = item.marca;
                    dto.fecha_ubicacion = item.fecha_ubicacion;
                    dto.nombre_ubi = item.nombre_ubi;
                    dto.id_detalle = item.id_detalle;
                    dto.interfaces = item.interfaces;


                    dto_u_sw_01.Add(dto);

                }
                return dto_u_sw_01;
            }
        }


        public DTO_u_detalle_sw u_detalle_sw(int ubicacion)
        {
            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.switch_detalle
                               from b in ctx.swicth
                               from c in ctx.marca_modelo_switch
                               from d in ctx.switch_ubicacion_switch
                               from e in ctx.ubicacion_switch


                               where

                               a.id_detalle_switch == ubicacion &&
                               a.marca_modelo_id == c.id_mmarca &&
                               a.switch_id == b.id_switch &&
                               a.id_detalle_switch == d.detalle_swich_id &&
                               d.ubicacion_switch_id == e.id_ubicacion_switch


                               select new
                               {

                                   nro_ip = a.nroip,
                                   id_sw = b.id_switch,
                                   descripcion = b.descripcion,
                                   id_mm = c.id_mmarca,
                                   fecha_ubicacion = d.fecha_ubicacion,
                                   id_ubicacion_sw = e.id_ubicacion_switch,
                                   id_detalle = a.id_detalle_switch,
                                   interfaces = a.interfaces


                               }).FirstOrDefault();



                var dto = new DTO_u_detalle_sw();

                dto.nro_ip = detalle.nro_ip;
                dto.id_sw = detalle.id_sw;
                dto.descripcion = detalle.descripcion;
                dto.id_mm = detalle.id_mm;
                dto.fecha_ubicacion = detalle.fecha_ubicacion;
                dto.id_ubicacion_sw = detalle.id_ubicacion_sw;
                dto.id_detalle = detalle.id_detalle;
                dto.interfaces = detalle.interfaces;


                return dto;
            }

        }

        public List<DTO_u_es_01> u_es_01(int ubicacion)
        {
            var dto_u_es_01 = new List<DTO_u_es_01>();


            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.detalle_escaner
                               from b in ctx.escaner
                               from c in ctx.marca_modelo_escaner
                               from d in ctx.escaner_ubicacion_escaner
                               from e in ctx.ubicacion_escaner


                               where

                               d.ubicacion_escaner_id == ubicacion &&
                               e.id_ubicacion_escaner == d.ubicacion_escaner_id &&
                               d.detalle_escaner_id == a.id_detalle_escaner &&
                               a.marca_modelo_id == c.id_marca_modelo_escaner &&
                               a.escaner_id == b.id_escaner &&
                               b.fecha_baja == null


                               select new
                               {

                                   nro_ip = a.nroip,
                                   descripcion = b.descripcion,
                                   id_es = b.id_escaner,
                                   marca = c.descripcion,
                                   fecha_ubicacion = d.fecha_ubicacion,
                                   nombre_ubi = e.nombre,
                                   id_detalle = a.id_detalle_escaner


                               }).ToList();


                foreach (var item in detalle)
                {
                    var dto = new DTO_u_es_01();

                    dto.nro_ip = item.nro_ip;
                    dto.descripcion = item.descripcion;
                    dto.id_es = item.id_es;
                    dto.mmarca = item.marca;
                    dto.fecha_ubicacion = item.fecha_ubicacion;
                    dto.nombre_ubi = item.nombre_ubi;
                    dto.id_detalle = item.id_detalle;


                    dto_u_es_01.Add(dto);

                }
                return dto_u_es_01;
            }
        }

        public DTO_u_detalle_escaner u_detalle_escaner(int ubicacion)
        {
            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.detalle_escaner
                               from b in ctx.escaner
                               from c in ctx.marca_modelo_escaner
                               from d in ctx.escaner_ubicacion_escaner
                               from e in ctx.ubicacion_escaner


                               where

                               a.id_detalle_escaner == ubicacion &&
                               a.marca_modelo_id == c.id_marca_modelo_escaner &&
                               a.escaner_id == b.id_escaner &&
                               a.id_detalle_escaner == d.detalle_escaner_id &&
                               d.ubicacion_escaner_id == e.id_ubicacion_escaner


                               select new
                               {

                                   nro_ip = a.nroip,
                                   id_escaner = b.id_escaner,
                                   descripcion = b.descripcion,
                                   id_mm = c.id_marca_modelo_escaner,
                                   fecha_ubicacion = d.fecha_ubicacion,
                                   id_ubicacion_escaner = e.id_ubicacion_escaner,
                                   id_detalle = a.id_detalle_escaner


                               }).FirstOrDefault();



                var dto = new DTO_u_detalle_escaner();

                dto.nro_ip = detalle.nro_ip;
                dto.id_escaner = detalle.id_escaner;
                dto.descripcion = detalle.descripcion;
                dto.id_mm = detalle.id_mm;
                dto.fecha_ubicacion = detalle.fecha_ubicacion;
                dto.id_ubicacion_escaner = detalle.id_ubicacion_escaner;
                dto.id_detalle = detalle.id_detalle;


                return dto;
            }

        }


        public List<DTO_consultacam_01a> consultacam_01a(int id, DateTime fdesde, DateTime fhasta)
        {
            var dto_consultacam_01a = new List<DTO_consultacam_01a>();


            using (var ctx = new SGEIContext())
            {
                var detalle = (from a in ctx.detalle_insumo_camara
                               from b in ctx.insumo_camara

                               where

                               a.detalle_id == id &&
                               a.fecha_insumo <= fhasta &&
                               a.fecha_insumo >= fdesde &&
                               a.insumo_id == b.id_insumo


                               select new
                               {

                                   fechacambioinsumo = a.fecha_insumo,
                                   nombre = b.nombre


                               }).ToList();


                foreach (var item in detalle)
                {
                    var dto = new DTO_consultacam_01a();

                    dto.fechacambioinsumo = item.fechacambioinsumo;
                    dto.nombre = item.nombre;


                    dto_consultacam_01a.Add(dto);

                }



                return dto_consultacam_01a;
            }


        }

    }

}

