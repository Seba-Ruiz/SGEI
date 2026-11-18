using System.Web.Mvc;
using login_v6.Seguridad;

namespace login_v6
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            // Todo requiere usuario logueado; las excepciones usan [AllowAnonymous].
            filters.Add(new AuthorizeAttribute());

            // Todo POST requiere token antiforgery (formulario o header AJAX).
            filters.Add(new ValidarAntiforgeryEnPostAttribute());
        }
    }
}
