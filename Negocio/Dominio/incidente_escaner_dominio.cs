using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class incidente_escaner_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    incidente_escaner inci = ctx.incidente_escaner.Find(id);
                    inci.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }


        public List<incidente_escaner> Listar()
        {
            var inci = new List<incidente_escaner>();
            try
            {
                using (var ctx = new SGEIContext())

                    inci = ctx.incidente_escaner
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return inci;
        }

        public incidente_escaner Obtener(int id)
        {
            var inci = new incidente_escaner();
            try
            {
                using (var ctx = new SGEIContext())

                    inci = ctx.incidente_escaner.Where(x => x.id_incidente_escaner == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return inci;
        }

        public void Guardar(incidente_escaner inci)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (inci.id_incidente_escaner > 0) //Registro que ya existe
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
