using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class switch_ubicacion_switch_dominio
    {

        public int ObtenerUltimo()
        {
            var det = new switch_detalle();
            try
            {
                using (var ctx = new SGEIContext())

                    det = ctx.switch_detalle.OrderByDescending(x => x.id_detalle_switch)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return det.id_detalle_switch;
        }


        public switch_ubicacion_switch ObtenerUbicacion(int id_detalle, int id_ubicacion)
        {
            var ubi = new switch_ubicacion_switch();
            try
            {
                using (var ctx = new SGEIContext())

                    ubi = ctx.switch_ubicacion_switch
                                                .Where(x => x.detalle_swich_id == id_detalle)
                                                .Where(x => x.ubicacion_switch_id == id_ubicacion)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return ubi;
        }

        public void Guardar(switch_ubicacion_switch sw)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (sw.id_switch_ubicacion_switch > 0) //Registro que ya existe
                    {
                        ctx.Entry(sw).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(sw).State = EntityState.Added;
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
