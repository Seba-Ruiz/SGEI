using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class detalle_insumo_impresora_dominio
    {

        public void Guardar(detalle_insumo_impresora deta)
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

        public void Restar(int insumo)
        {
            using (var ctx = new SGEIContext())
            {
                insumo insu = ctx.insumo.Find(insumo);
                insu.cantidad = insu.cantidad - 1;
                ctx.SaveChanges();
            }

        }
    }
}
