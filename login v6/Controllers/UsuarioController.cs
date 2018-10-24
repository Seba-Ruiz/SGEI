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
    [Authorize]
    public class UsuarioController : Controller
    {
        public usuario_dominio user = new usuario_dominio();
        
        
        public ActionResult Index()
        {
            var usuarios = user.Listar();

            

            return View(
                usuarios
                );
        }

        public ActionResult Detalle(string id)
        {


            var tipoRol = user.obtenerTipoRol(id);


            var usuario = user.Obtener(id);
            ViewBag.tipoRol = tipoRol.Name;


            return View(usuario);
        }
        public ActionResult Editar(string id)
        {
            var usuario = user.Obtener(id);


            var tipoRol = user.obtenerTipoRol(id);

            var roles_dominio = new rol_dominio();

            ViewBag.tipo = new SelectList(roles_dominio.Listar(), "Id", "Name", tipoRol.Id);

            

            return View(usuario);
        }
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