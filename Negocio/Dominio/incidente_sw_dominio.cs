using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class incidente_sw_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    incidente_switch inci = ctx.incidente_switch.Find(id);
                    inci.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }


        public List<incidente_switch> Listar()
        {
            var inci = new List<incidente_switch>();
            try
            {
                using (var ctx = new SGEIContext())

                    inci = ctx.incidente_switch
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return inci;
        }

        public incidente_switch Obtener(int id)
        {
            var inci = new incidente_switch();
            try
            {
                using (var ctx = new SGEIContext())

                    inci = ctx.incidente_switch.Where(x => x.id_incidente_switch == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return inci;
        }

        public void Guardar(incidente_switch inci)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (inci.id_incidente_switch > 0) //Registro que ya existe
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
