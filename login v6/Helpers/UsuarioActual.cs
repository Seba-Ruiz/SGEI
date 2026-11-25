using System;
using System.Runtime.Caching;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Negocio.DTO;
using Negocio.Servicio;

namespace login_v6.Helpers
{
    /// <summary>
    /// Nombre y apellido del usuario conectado (barra superior). Se guarda unos minutos en memoria para no consultar
    /// la base en cada pantalla; si no tiene nombre cargado se muestra su usuario (mail).
    /// </summary>
    public static class UsuarioActual
    {
        private static readonly TimeSpan Duracion = TimeSpan.FromMinutes(5);

        public static DTO_Usuario Datos(IPrincipal usuario)
        {
            if (usuario == null || usuario.Identity == null || !usuario.Identity.IsAuthenticated) { return null; }
            var id = usuario.Identity.GetUserId();
            var clave = Clave(id);
            var datos = MemoryCache.Default.Get(clave) as DTO_Usuario;
            if (datos != null) { return datos; }

            try
            {
                datos = new Tickets().Usuario(id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("No se pudo leer el nombre del usuario {0}: {1}", id, ex);
            }
            if (datos == null)
            {
                var nombre = usuario.Identity.GetUserName() ?? "";
                return new DTO_Usuario { Id = id, Nombre = nombre, Iniciales = nombre.Length > 0 ? nombre.Substring(0, 1).ToUpperInvariant() : "?", Activo = true };
            }
            MemoryCache.Default.Set(clave, datos, DateTimeOffset.Now.Add(Duracion));
            return datos;
        }

        /// <summary>Para que se vea enseguida un cambio de nombre.</summary>
        public static void Olvidar(string id)
        {
            if (id != null) { MemoryCache.Default.Remove(Clave(id)); }
        }

        private static string Clave(string id)
        {
            return "sgei.usuario." + id;
        }
    }
}
