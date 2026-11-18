using login_v6.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Negocio.Dominio;
using login_v6.Seguridad;

namespace login_v6.Controllers
{
    [SoloAdministrador]
    public class RoleController : Controller
    {

        public RoleController(){}

        public rol_dominio rol = new rol_dominio();


        public ActionResult Index()
        {
            var roles = rol.Listar();

            return View(roles);
        }

        public ActionResult Detalle()
        {
            return View();
        }

    }
}