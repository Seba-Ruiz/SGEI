using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class usuario_rol_dominio
    {


        public void Guardar(AspNetUserRoles model)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (model.id > 0) //Registro que ya existe
                    {
                        ctx.Entry(model).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(model).State = EntityState.Added;
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
