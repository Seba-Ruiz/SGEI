using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class detalle_camara_dominio
    {
        

        public detalle_camara Obtener(int id)
        {
            var dt = new detalle_camara();
            try
            {
                using (var ctx = new SGEIContext())

                    dt = ctx.detalle_camara.Where(x => x.id_detalle == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return dt;
        }

        public void Guardar(detalle_camara dt)
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
                throw E;
            }
        }

    }
}
