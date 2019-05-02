using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class detallepc_dominio
    {
        public void Guardar(detallePC detalle)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (detalle.id_detalle > 0) //Registro que ya existe
                    {
                        ctx.Entry(detalle).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(detalle).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }

        public detallePC Obtener(int? id)
        {
            var dt = new detallePC();
            try
            {
                using (var ctx = new SGEIContext())

                    dt = ctx.detallePC.Where(x => x.id_detalle == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return dt;
        }

        public detallePC ObtenerByIdPc(int? id)
        {
            var dt = new detallePC();
            try
            {
                using (var ctx = new SGEIContext())

                    dt = ctx.detallePC.Where(x => x.pc_id == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return dt;
        }

        public void mover(int u_pc, int u_nueva_pc, int id_pc)
        {
            using (var ctx = new SGEIContext())
            {
                var resultado = ctx.pc_ubicacionpc.Where(x => x.ubicacionpc_id == u_pc).Where(x => x.pc_id == id_pc).FirstOrDefault();

                resultado.fecha_ubicacion = DateTime.Now;
                resultado.ubicacionpc_id = u_nueva_pc;

                ctx.SaveChanges();
            }
        }

        public void moverDetalle(int u_pc, int u_nueva_pc, int id_pc)
        {
            using (var ctx = new SGEIContext())
            {
                var resultado = ctx.pc_ubicacionpc.Where(x => x.ubicacionpc_id == u_pc).Where(x => x.pc_id == id_pc).FirstOrDefault();

                resultado.fecha_ubicacion = DateTime.Now;
                resultado.ubicacionpc_id = u_nueva_pc;

                ctx.SaveChanges();
            }
        }
    }
}
