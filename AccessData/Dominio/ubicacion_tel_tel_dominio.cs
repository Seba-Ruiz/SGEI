using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace AccessData.Dominio
{
    public class ubicacion_tel_tel_dominio
    {
        
        public int ObtenerUltimo()
        {
            var det = new detalle_tel();
            try
            {
                using (var ctx = new SGEIContext())

                    det = ctx.detalle_tel.OrderByDescending(x => x.id_detalle)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return det.id_detalle;
        }


        public ubicacion_tel_tel ObtenerUbicacion(int id_detalle, int id_ubicacion)
        {
            var ubi = new ubicacion_tel_tel();
            try
            {
                using (var ctx = new SGEIContext())

                    ubi = ctx.ubicacion_tel_tel
                                                .Where(x => x.detalle_id == id_detalle)
                                                .Where(x => x.ubicacion_tel_id == id_ubicacion)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return ubi;
        }

        public void Guardar(ubicacion_tel_tel tel)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (tel.id_ubicacion_tel_tel > 0) //Registro que ya existe
                    {
                        ctx.Entry(tel).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(tel).State = EntityState.Added;
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
