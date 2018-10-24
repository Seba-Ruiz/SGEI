using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class usuario_dominio
    {
        private SGEIContext db = new SGEIContext();
        
        public AspNetUsers Obtener(string id)
        {
            var user = new AspNetUsers();
            try
            {
                using (var ctx = new SGEIContext())

                    user = ctx.AspNetUsers
                                       .Where(x => x.Id == id)
                                       .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return user;
        }

        public List<AspNetUsers> Listar()
        {
            var user = new List<AspNetUsers>();
            try
            {
                using (var ctx = new SGEIContext())

                    user = ctx.AspNetUsers
                        .OrderBy(x => x.Email)
                        .ToList();
            }
            catch (Exception E)
            {
                throw;
            }

            return user;
        }



        public AspNetRoles obtenerTipoRol(string id)
        {
            var UsuarioRol = new AspNetUserRoles();

            var tipoRol = new AspNetRoles();

            try
            {
                using (var ctx = new SGEIContext())

                    UsuarioRol = ctx.AspNetUserRoles
                                .Where(x => x.UserId == id)
                                .OrderByDescending(x => x.fecha_rol)
                                .FirstOrDefault();
            }


            catch (Exception E)
            {
                throw;
            }

            try
            {
                using (var ctx = new SGEIContext())

                    tipoRol = ctx.AspNetRoles
                                .Find(UsuarioRol.RoleId);
                                
            }


            catch (Exception E)
            {
                throw;
            }

            return tipoRol;

        }

        public AspNetUsers ObtenerByEmail(string email)
        {
            var user = new AspNetUsers();
            try
            {
                using (var ctx = new SGEIContext())

                    user = ctx.AspNetUsers
                                       .Where(x => x.Email == email)
                                       .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return user;
        }


        public void Guardar(AspNetUsers user)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (user.Id != null) //Registro que ya existe
                    {
                        ctx.Entry(user).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(user).State = EntityState.Added;
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
