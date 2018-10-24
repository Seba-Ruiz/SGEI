using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class motherboard_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    motherPC moth = ctx.motherPC.Find(id);
                    moth.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }

       
        public List<motherPC> Listar()
        {
            var moth = new List<motherPC>();
            try
            {
                using (var ctx = new SGEIContext())

                    moth = ctx.motherPC
                        .Where(x => x.fecha_baja == null)
                        .OrderBy(x => x.descripcion)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return moth;
        }

        public motherPC Obtener(int id)
        {
            var moth = new motherPC();
            try
            {
                using (var ctx = new SGEIContext())

                    moth = ctx.motherPC.Where(x => x.id_mother == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return moth;
        }

        public void Guardar(motherPC moth)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (moth.id_mother > 0) //Registro que ya existe
                    {
                        ctx.Entry(moth).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(moth).State = EntityState.Added;
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
