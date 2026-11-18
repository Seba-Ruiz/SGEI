using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace Negocio.Dominio
{
    public class rol_dominio
    {

        public List<AspNetRoles> Listar()
        {
            var roles = new List<AspNetRoles>();
            try
            {
                using (var ctx = new SGEIContext())

                    roles = ctx.AspNetRoles
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return roles;
        }


        public void Guardar( AspNetUserRoles usu_rol )
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (usu_rol.id > 0) //Registro que ya existe
                    {
                        ctx.Entry(usu_rol).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(usu_rol).State = EntityState.Added;
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
