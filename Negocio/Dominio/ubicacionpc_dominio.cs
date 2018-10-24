using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class ubicacionpc_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    ubicacionPC ubi = ctx.ubicacionPC.Find(id);
                    ubi.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }


        public List<ubicacionPC> Listar()
        {
            var ubi = new List<ubicacionPC>();
            try
            {
                using (var ctx = new SGEIContext())

                    ubi = ctx.ubicacionPC
                        .Where(x => x.fecha_baja == null)
                        .OrderBy(x => x.nombre)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return ubi;
        }

        public ubicacionPC Obtener(int id)
        {
            var ubi = new ubicacionPC();
            try
            {
                using (var ctx = new SGEIContext())

                    ubi = ctx.ubicacionPC.Where(x => x.id_ubicacion == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return ubi;
        }

        public void Guardar(ubicacionPC ubi)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (ubi.id_ubicacion > 0) //Registro que ya existe
                    {
                        ctx.Entry(ubi).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(ubi).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }
    }
}
