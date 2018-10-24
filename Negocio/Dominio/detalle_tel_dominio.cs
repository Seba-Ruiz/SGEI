using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class detalle_tel_dominio
    {
        
       

        public detalle_tel Obtener(int id)
        {
            var dt = new detalle_tel();
            try
            {
                using (var ctx = new SGEIContext())

                    dt = ctx.detalle_tel.Where(x => x.id_detalle == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return dt;
        }

        public void Guardar(detalle_tel dt)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (dt.id_detalle > 0) //Registro que ya existe
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
                throw;
            }
        }
    }
}
