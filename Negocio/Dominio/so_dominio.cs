using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class so_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    soPC so = ctx.soPC.Find(id);
                    so.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }


        public List<soPC> Listar()
        {
            var so = new List<soPC>();
            try
            {
                using (var ctx = new SGEIContext())

                    so = ctx.soPC
                        .Where(x => x.fecha_baja == null)
                        .OrderBy(x => x.descripcion)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return so;
        }

        public soPC Obtener(int id)
        {
            var so = new soPC();
            try
            {
                using (var ctx = new SGEIContext())

                    so = ctx.soPC.Where(x => x.id_so == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return so;
        }

        public void Guardar(soPC so)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (so.id_so > 0) //Registro que ya existe
                    {
                        ctx.Entry(so).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(so).State = EntityState.Added;
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
