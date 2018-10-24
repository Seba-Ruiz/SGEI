using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace AccessData.Dominio
{
    public class incidente_tel_tel_dominio
    {
        public void Guardar(incidente_tel_tel inci)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (inci.id_incidente_tel_tel > 0) //Registro que ya existe
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
