using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class incidentes_tel_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    incidente_tel inci = ctx.incidente_tel.Find(id);
                    inci.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }


        public List<incidente_tel> Listar()
        {
            var inci = new List<incidente_tel>();
            try
            {
                using (var ctx = new SGEIContext())

                    inci = ctx.incidente_tel
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return inci;
        }

        public incidente_tel Obtener(int id)
        {
            var inci = new incidente_tel();
            try
            {
                using (var ctx = new SGEIContext())

                    inci = ctx.incidente_tel.Where(x => x.id_incidente_tel == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return inci;
        }

        public void Guardar(incidente_tel inci)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (inci.id_incidente_tel > 0) //Registro que ya existe
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
