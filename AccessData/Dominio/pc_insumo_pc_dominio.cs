using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace AccessData.Dominio
{
    public class pc_insumo_pc_dominio
    {
        public void Guardar(pc_insumo_pc insu)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (insu.id_ipc > 0) //Registro que ya existe
                    {
                        ctx.Entry(insu).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(insu).State = EntityState.Added;
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
