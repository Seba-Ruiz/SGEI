using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class bajapc_dominio
    {
        public void Guardar(bajaPC baja)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (baja.id_baja > 0) //Registro que ya existe
                    {
                        ctx.Entry(baja).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(baja).State = EntityState.Added;
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
