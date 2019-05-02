using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class ubicacion_camara_camara_dominio
    {
        public int ObtenerUltimo()
        {
            var det = new detalle_camara();
            try
            {
                using (var ctx = new SGEIContext())

                    det = ctx.detalle_camara.OrderByDescending(x => x.id_detalle)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return det.id_detalle;
        }


        public ubicacion_camara_camara ObtenerUbicacion(int id_detalle, int id_ubicacion)
        {
            var ubi = new ubicacion_camara_camara();
            try
            {
                using (var ctx = new SGEIContext())

                    ubi = ctx.ubicacion_camara_camara
                                                .Where(x => x.detalle_camara_id == id_detalle)
                                                .Where(x => x.ubicacion_camara_id == id_ubicacion)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return ubi;
        }

        public void Guardar(ubicacion_camara_camara cam)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (cam.id_ubicacion_camara_camara > 0) //Registro que ya existe
                    {
                        ctx.Entry(cam).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(cam).State = EntityState.Added;
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
