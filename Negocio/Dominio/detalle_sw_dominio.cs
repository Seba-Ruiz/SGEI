using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class detalle_sw_dominio
    {

        public switch_detalle Obtener(int id)
        {
            var dt = new switch_detalle();
            try
            {
                using (var ctx = new SGEIContext())

                    dt = ctx.switch_detalle.Where(x => x.id_detalle_switch == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return dt;
        }

        public void Guardar(switch_detalle dt)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (dt.id_detalle_switch > 0) //Registro que ya existe
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
