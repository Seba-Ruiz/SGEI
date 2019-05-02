using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class ubicacion_sw_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    ubicacion_switch ubi = ctx.ubicacion_switch.Find(id);
                    ubi.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }


        public List<ubicacion_switch> Listar()
        {
            var ubi = new List<ubicacion_switch>();
            try
            {
                using (var ctx = new SGEIContext())

                    ubi = ctx.ubicacion_switch
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return ubi;
        }

        public ubicacion_switch Obtener(int id)
        {
            var ubi = new ubicacion_switch();
            try
            {
                using (var ctx = new SGEIContext())

                    ubi = ctx.ubicacion_switch.Where(x => x.id_ubicacion_switch == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return ubi;
        }

        public void Guardar(ubicacion_switch ubi)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (ubi.id_ubicacion_switch > 0) //Registro que ya existe
                    {
                        ctx.Entry(ubi).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(ubi).State = EntityState.Added;
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
