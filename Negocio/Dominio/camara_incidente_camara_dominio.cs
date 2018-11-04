using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class camara_incidente_camara_dominio
    {
        public void Guardar(camara_incidente_camara inci)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (inci.id_camara_incidente_camara > 0) //Registro que ya existe
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
