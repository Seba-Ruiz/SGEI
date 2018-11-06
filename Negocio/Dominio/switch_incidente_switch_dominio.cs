using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class switch_incidente_switch_dominio
    {

        public void Guardar(switch_incidente_switch inci)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (inci.id_switch_incidente_switch > 0) //Registro que ya existe
                    {
                        ctx.Entry(inci).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(inci).State = EntityState.Added;
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
