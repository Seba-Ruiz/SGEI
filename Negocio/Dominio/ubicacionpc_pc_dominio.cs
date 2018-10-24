using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class ubicacionpc_pc_dominio
    {
        public void Guardar(pc_ubicacionpc pcubi)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (pcubi.id_upc > 0) //Registro que ya existe
                    {
                        ctx.Entry(pcubi).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(pcubi).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }
        public pc_ubicacionpc Obtener(int id)
        {
            var ubicacion = new pc_ubicacionpc();
            try
            {
                using (var ctx = new SGEIContext())

                    ubicacion = ctx.pc_ubicacionpc.Where(x => x.id_upc == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return ubicacion;
        }

        public pc_ubicacionpc ObtenerUbicacion(int id)
        {
            var ubicacion = new pc_ubicacionpc();
            try
            {
                using (var ctx = new SGEIContext())

                    ubicacion = ctx.pc_ubicacionpc.Where(x => x.pc_id == id).OrderByDescending(x=>x.fecha_ubicacion)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return ubicacion;
        }

        public pc_ubicacionpc ObtenerUbicacionPC(int ubi, int id_pc)
        {
            var ubicacion = new pc_ubicacionpc();
            try
            {
                using (var ctx = new SGEIContext())

                    ubicacion = ctx.pc_ubicacionpc.Where(x => x.pc_id == id_pc).Where(x=>x.ubicacionpc_id==ubi).OrderByDescending(x => x.fecha_ubicacion)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return ubicacion;
        }

        public pc_ubicacionpc ObteneryCambiar(int id_pc)
        {
            var ubicacion = new pc_ubicacionpc();
            try
            {
                using (var ctx = new SGEIContext())

                    ubicacion = ctx.pc_ubicacionpc.Where(x => x.pc_id == id_pc).OrderByDescending(x => x.fecha_ubicacion)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return ubicacion;
        }
    }
}
