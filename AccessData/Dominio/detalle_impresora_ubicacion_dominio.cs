using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessData.Dominio
{
    public class detalle_impresora_ubicacion_dominio
    {
        public detalle_impresora_ubicacion Obtener(int id)
        {
            var detalles = new detalle_impresora_ubicacion();
            try
            {
                using (var ctx = new SGEIContext())

                    detalles = ctx.detalle_impresora_ubicacion.Where(x => x.id == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return detalles;
        }

        public void Guardar(detalle_impresora_ubicacion deta)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (deta.id > 0) //Registro que ya existe
                    {
                        ctx.Entry(deta).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(deta).State = EntityState.Added;
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
