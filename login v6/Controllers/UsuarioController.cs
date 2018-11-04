using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Negocio.Servicio;
using Negocio.Dominio;


namespace login_v6.Controllers
{
    /// <summary>
    /// Este ees el controlador del usuario
    /// </summary>
    [Authorize]
    public class UsuarioController : Controller
    {
        //variable de la capa dominio de la aplicacion para gesion con la bd
        public usuario_dominio user = new usuario_dominio();
        
        /// <summary>
        /// El metodo index es el que lista los usuarios que existen
        /// </summary>
        public ActionResult Index()
        {
            var usuarios = user.Listar();
            return View(usuarios);
        }

        /// <summary>
        /// El metodo Detalle es el que obtiene y muestra en pantalla el detalle del usuario seleccionado en el Index
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retoran el usuario con todo su detalle</returns>
        public ActionResult Detalle(string id)
        {
            var tipoRol = user.obtenerTipoRol(id);
            
            var usuario = user.Obtener(id);
            ViewBag.tipoRol = tipoRol.Name;
            
            return View(usuario);
        }

        /// <summary>
        /// Aqui podremos editar la informacion del usuario como asi tambien el tipo de usuario al que pertenece (Administrador u Operario)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna el usuario que deseamos editar</returns>
        public ActionResult Editar(string id)
        {
            var usuario = user.Obtener(id);
            
            var tipoRol = user.obtenerTipoRol(id);

            var roles_dominio = new rol_dominio();

            ViewBag.tipo = new SelectList(roles_dominio.Listar(), "Id", "Name", tipoRol.Id);
            
            return View(usuario);
        }

        /// <summary>
        /// Este es la confirmacion del la edicion del usuario guardando los cambios seleccionados anteriormente.
        /// </summary>
        /// <param name="usu"></param>
        /// <param name="tipoRol"></param>
        [HttpPost]
        public ActionResult confirmarEditar(AspNetUsers usu, string tipoRol)
        {

            var roles_dominio = new rol_dominio();

            var usuarioRol = new AspNetUserRoles();

            usuarioRol.RoleId = tipoRol;
            usuarioRol.UserId = usu.Id;
            usuarioRol.fecha_rol = DateTime.Now;

            roles_dominio.Guardar(usuarioRol);
            
            user.Guardar(usu);
            
            return View();
        }



    }
}