using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class escaner_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    escaner esc = ctx.escaner.Find(id);
                    esc.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }
        public int ObtenerUltimo()
        {
            var esc = new escaner();
            try
            {
                using (var ctx = new SGEIContext())

                    esc = ctx.escaner.OrderByDescending(x => x.id_escaner)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return esc.id_escaner;
        }

        public List<escaner> Listar()
        {
            var esc = new List<escaner>();
            try
            {
                using (var ctx = new SGEIContext())

                    esc = ctx.escaner
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return esc;
        }

        public escaner Obtener(int id)
        {
            var esc = new escaner();
            try
            {
                using (var ctx = new SGEIContext())

                    esc = ctx.escaner.Where(x => x.id_escaner == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return esc;
        }

        public void Guardar(escaner esc)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (esc.id_escaner > 0) //Registro que ya existe
                    {
                        ctx.Entry(esc).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(esc).State = EntityState.Added;
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
