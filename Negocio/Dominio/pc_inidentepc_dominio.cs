using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Negocio.Dominio;
using System.Data.Entity;

namespace Negocio.Dominio
{
    public class pc_inidentepc_dominio
    {

        public void Guardar(pc_incidentepc incidente)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (incidente.id_pc_incidente > 0) //Registro que ya existe
                    {
                        ctx.Entry(incidente).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(incidente).State = EntityState.Added;
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
