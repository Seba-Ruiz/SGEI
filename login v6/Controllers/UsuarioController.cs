using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Model;
using Negocio.Dominio;
using login_v6.Seguridad;

namespace login_v6.Controllers
{
    /// <summary>
    /// Gestión de usuarios. Listar, ver y editar a otros usuarios (incluido su rol) es solo
    /// para el Administrador; cada usuario puede editar únicamente sus propios datos personales.
    /// </summary>
    public class UsuarioController : Controller
    {
        private readonly usuario_dominio user = new usuario_dominio();

        private ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        /// <summary>Lista los usuarios existentes.</summary>
        [SoloAdministrador]
        public ActionResult Index()
        {
            return View(user.Listar());
        }

        /// <summary>Detalle del usuario seleccionado en el Index.</summary>
        [SoloAdministrador]
        public ActionResult Detalle(string id)
        {
            var usuario = user.Obtener(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            var tipoRol = user.obtenerTipoRol(id);
            ViewBag.tipoRol = tipoRol != null ? tipoRol.Name : "Sin rol";
            return View(usuario);
        }

        /// <summary>Edición de datos y tipo de usuario (Administrador u Operario).</summary>
        [SoloAdministrador]
        public ActionResult Editar(string id)
        {
            var usuario = user.Obtener(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            CargarRoles(id);
            return View(usuario);
        }

        /// <summary>Cada usuario edita sus propios datos personales (sin rol).</summary>
        public ActionResult EditarSinRol(string id)
        {
            if (!EsUsuarioActual(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var usuario = user.Obtener(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            CargarRoles(id);
            return View(usuario);
        }

        /// <summary>Guarda los datos personales y, si cambió, el nuevo rol del usuario.</summary>
        [HttpPost]
        [SoloAdministrador]
        public ActionResult confirmarEditar(string id, string PhoneNumber, string nombre, string apellido, string tipoRol)
        {
            if (!user.ActualizarDatosPersonales(id, PhoneNumber, nombre, apellido))
            {
                return HttpNotFound();
            }
            login_v6.Helpers.UsuarioActual.Olvidar(id);

            var rolesDominio = new rol_dominio();
            var rolActual = user.obtenerTipoRol(id);
            var rolValido = rolesDominio.Listar().Any(r => r.Id == tipoRol);

            if (rolValido && (rolActual == null || rolActual.Id != tipoRol))
            {
                rolesDominio.Guardar(new AspNetUserRoles
                {
                    RoleId = tipoRol,
                    UserId = id,
                    fecha_rol = DateTime.Now
                });

                // Invalida la sesión del usuario para que el nuevo rol se aplique de inmediato.
                UserManager.UpdateSecurityStamp(id);
            }

            return View();
        }

        /// <summary>Guarda los datos personales del usuario logueado.</summary>
        [HttpPost]
        public ActionResult confirmarEditarSinRol(string id, string PhoneNumber, string nombre, string apellido)
        {
            if (!EsUsuarioActual(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (!user.ActualizarDatosPersonales(id, PhoneNumber, nombre, apellido))
            {
                return HttpNotFound();
            }
            login_v6.Helpers.UsuarioActual.Olvidar(id);

            return View();
        }

        private bool EsUsuarioActual(string id)
        {
            return !string.IsNullOrEmpty(id) && id == User.Identity.GetUserId();
        }

        private void CargarRoles(string id)
        {
            var tipoRol = user.obtenerTipoRol(id);
            ViewBag.tipo = new SelectList(new rol_dominio().Listar(), "Id", "Name", tipoRol != null ? tipoRol.Id : null);
        }
    }
}
