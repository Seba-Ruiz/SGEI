using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class incidente_dominio
    {
        public List<incidente> verIncidente(int id)
        {
            var incidentes = new List<incidente>();

            try
            {
                using (var ctx = new SGEIContext())

                     

                      incidentes = ctx.incidente  .Where(x => x.ubicacion_impresora_id == id)
                                                .ToList();

            }
            catch (Exception E)
            {
                throw E;
            }
            return incidentes;
        }

        public void Guardar(incidente inci)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (inci.id > 0) //Registro que ya existe
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
                throw E;
            }
        }
    }
}
