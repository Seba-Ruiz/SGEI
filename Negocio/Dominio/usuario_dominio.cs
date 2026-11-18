using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Negocio.Dominio
{
    public class usuario_dominio
    {
        public AspNetUsers Obtener(string id)
        {
            using (var ctx = new SGEIContext())
            {
                return ctx.AspNetUsers.AsNoTracking().SingleOrDefault(x => x.Id == id);
            }
        }

        public List<AspNetUsers> Listar()
        {
            using (var ctx = new SGEIContext())
            {
                return ctx.AspNetUsers.AsNoTracking().OrderBy(x => x.Email).ToList();
            }
        }

        /// <summary>
        /// Rol vigente del usuario: el registro más reciente de AspNetUserRoles.
        /// Devuelve null si el usuario no tiene rol asignado.
        /// </summary>
        public AspNetRoles obtenerTipoRol(string id)
        {
            using (var ctx = new SGEIContext())
            {
                var rolId = ctx.AspNetUserRoles
                    .Where(x => x.UserId == id)
                    .OrderByDescending(x => x.fecha_rol)
                    .Select(x => x.RoleId)
                    .FirstOrDefault();

                return rolId == null ? null : ctx.AspNetRoles.Find(rolId);
            }
        }

        public AspNetUsers ObtenerByEmail(string email)
        {
            using (var ctx = new SGEIContext())
            {
                return ctx.AspNetUsers.AsNoTracking().SingleOrDefault(x => x.Email == email);
            }
        }

        /// <summary>
        /// Actualiza solo los datos personales editables. Nunca toca contraseña,
        /// SecurityStamp, bloqueo ni confirmación de correo.
        /// </summary>
        /// <returns>false si el usuario no existe.</returns>
        public bool ActualizarDatosPersonales(string id, string telefono, string nombre, string apellido)
        {
            using (var ctx = new SGEIContext())
            {
                var usuario = ctx.AspNetUsers.Find(id);
                if (usuario == null)
                {
                    return false;
                }

                usuario.PhoneNumber = telefono;
                usuario.nombre = nombre;
                usuario.apellido = apellido;
                ctx.SaveChanges();
                return true;
            }
        }

        public void RegistrarIngreso(string id)
        {
            using (var ctx = new SGEIContext())
            {
                var usuario = ctx.AspNetUsers.Find(id);
                if (usuario == null)
                {
                    return;
                }

                usuario.ultimo_ingreso = DateTime.Now;
                ctx.SaveChanges();
            }
        }
    }
}
