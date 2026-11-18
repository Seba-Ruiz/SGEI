using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class incidentepc_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    incidentePC inci = ctx.incidentePC.Find(id);
                    inci.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }


        public List<incidentePC> Listar()
        {
            var inci = new List<incidentePC>();
            try
            {
                using (var ctx = new SGEIContext())

                    inci = ctx.incidentePC
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return inci;
        }

        public incidentePC Obtener(int id)
        {
            var inci = new incidentePC();
            try
            {
                using (var ctx = new SGEIContext())

                    inci = ctx.incidentePC.Where(x => x.id_incidentepc == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return inci;
        }

        public void Guardar(incidentePC inci)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (inci.id_incidentepc > 0) //Registro que ya existe
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
