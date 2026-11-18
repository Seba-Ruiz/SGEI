using System;
using System.Web;
using System.Web.Mvc;

namespace login_v6.Seguridad
{
    /// <summary>
    /// Restringe un controlador o acción al rol Administrador.
    /// Un usuario logueado sin permiso recibe la pantalla "Acceso denegado" (403);
    /// uno no logueado es enviado al login.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class SoloAdministradorAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.User.EsAdministrador();
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
                return;
            }

            filterContext.HttpContext.Response.StatusCode = 403;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            filterContext.Result = new ViewResult { ViewName = "AccesoDenegado" };
        }
    }
}
