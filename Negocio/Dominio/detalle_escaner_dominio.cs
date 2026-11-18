using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class detalle_escaner_dominio
    {
        public detalle_escaner Obtener(int id)
        {
            var dt = new detalle_escaner();
            try
            {
                using (var ctx = new SGEIContext())

                    dt = ctx.detalle_escaner.Where(x => x.id_detalle_escaner == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return dt;
        }

        public void Guardar(detalle_escaner dt)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (dt.id_detalle_escaner > 0) //Registro que ya existe
                    {
                        ctx.Entry(dt).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(dt).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }
    }
}
