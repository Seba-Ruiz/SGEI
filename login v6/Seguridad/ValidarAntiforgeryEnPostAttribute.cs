using System;
using System.Web.Helpers;
using System.Web.Mvc;

namespace login_v6.Seguridad
{
    /// <summary>
    /// Filtro global: todo POST debe traer un token antiforgery válido, ya sea en el
    /// formulario (@Html.AntiForgeryToken()) o en el header "RequestVerificationToken"
    /// para pedidos AJAX. Evita ataques CSRF sin tener que decorar cada acción.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ValidarAntiforgeryEnPostAttribute : FilterAttribute, IAuthorizationFilter
    {
        public const string HeaderToken = "RequestVerificationToken";

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            if (!string.Equals(request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var headerToken = request.Headers[HeaderToken];
            if (!string.IsNullOrEmpty(headerToken))
            {
                var cookie = request.Cookies[AntiForgeryConfig.CookieName];
                AntiForgery.Validate(cookie != null ? cookie.Value : null, headerToken);
                return;
            }

            AntiForgery.Validate();
        }
    }
}
