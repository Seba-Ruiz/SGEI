using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class escaner_incidente_escaner_dominio
    {
        public void Guardar(escaner_incidente_escaner inci)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (inci.id_escaner_incidente_escaner > 0) //Registro que ya existe
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
