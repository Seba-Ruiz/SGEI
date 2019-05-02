using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Negocio.Dominio
{
    public class baja_dominio
    {
        public void Guardar(baja baja)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (baja.id > 0) //Registro que ya existe
                    {
                        ctx.Entry(baja).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(baja).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    baja baja = ctx.baja.Find(id);

                    ctx.Entry(baja).State = EntityState.Deleted;

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
