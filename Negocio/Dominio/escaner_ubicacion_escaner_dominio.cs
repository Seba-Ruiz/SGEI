using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class escaner_ubicacion_escaner_dominio
    {
        public int ObtenerUltimo()
        {
            var det = new detalle_escaner();
            try
            {
                using (var ctx = new SGEIContext())

                    det = ctx.detalle_escaner.OrderByDescending(x => x.id_detalle_escaner)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return det.id_detalle_escaner;
        }


        public escaner_ubicacion_escaner ObtenerUbicacion(int id_detalle, int id_ubicacion)
        {
            var ubi = new escaner_ubicacion_escaner();
            try
            {
                using (var ctx = new SGEIContext())

                    ubi = ctx.escaner_ubicacion_escaner
                                                .Where(x => x.detalle_escaner_id == id_detalle)
                                                .Where(x => x.ubicacion_escaner_id == id_ubicacion)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return ubi;
        }

        public void Guardar(escaner_ubicacion_escaner es)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (es.id_escaner_ubicacion_escaner > 0) //Registro que ya existe
                    {
                        ctx.Entry(es).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(es).State = EntityState.Added;
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
