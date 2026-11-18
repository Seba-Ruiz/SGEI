using System.Web.Optimization;

namespace login_v6
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        //
        // Estilos (en el <head>):
        //   ~/Content/css        -> Bootstrap 5, Bootstrap Icons, DataTables, Content/sgei.css
        //
        // Scripts (al final del <body>, en este orden):
        //   ~/bundles/jquery     -> jQuery (única copia en toda la app) + jQuery Migrate
        //   ~/bundles/bootstrap  -> Bootstrap 5 (incluye Popper)
        //   ~/bundles/plugins    -> DataTables, jquery.steps (wizard), Chart.js
        //   Scripts/app/sgei.js  -> comportamiento común (menú, confirmaciones, AJAX)
        //   Scripts/views/...    -> script propio de cada vista, si existe
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Por defecto, en modo debug se ignoran los *.min.js; algunos plugins solo vienen minificados
            // (chart.min.js, jquery.steps.min.js) y quedaban afuera al desarrollar.
            bundles.IgnoreList.Clear();
            bundles.IgnoreList.Ignore("*.intellisense.js");
            bundles.IgnoreList.Ignore("*-vsdoc.js");

            // CssRewriteUrlTransform corrige las rutas relativas (fuentes de los íconos) al servir el bundle.
            bundles.Add(new Bundle("~/Content/css", SinMinificarTransform.Css)
                        .Include("~/Content/lib/bootstrap/bootstrap.css")
                        .Include("~/Content/lib/bootstrap-icons/bootstrap-icons.css", new CssRewriteUrlTransform())
                        .Include("~/Content/lib/datatables/dataTables.bootstrap5.css")
                        .Include("~/assets/plugins/jquery.steps/css/jquery.steps.css", new CssRewriteUrlTransform())
                        .Include("~/Content/sgei.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        // Migrate mantiene funcionando a jquery.steps (2013), escrito para jQuery 1.x.
                        "~/Scripts/jquery-migrate-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"));

            // Bundle (sin ScriptBundle): el minificador de System.Web.Optimization (WebGrease) no entiende
            // JavaScript moderno y falla con Bootstrap 5 / DataTables 2. Estas librerías ya traen su .min.js,
            // que es el que se sirve en producción.
            bundles.Add(new Bundle("~/bundles/bootstrap", SinMinificarTransform.Js).Include(
                        "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new Bundle("~/bundles/plugins", SinMinificarTransform.Js).Include(
                        "~/Scripts/lib/datatables/dataTables.js",
                        "~/Scripts/lib/datatables/dataTables.bootstrap5.js",
                        "~/assets/plugins/jquery.steps/js/jquery.steps.min.js",
                        "~/assets/plugins/chart.js/chart.min.js"));

            // Portada (Home/Landing): conserva su propio tema.
            bundles.Add(new ScriptBundle("~/bundles/landing").Include(
                        "~/assets/landing/js/bootstrap.min.js",
                        "~/assets/landing/js/jquery.easing.1.3.min.js",
                        "~/assets/landing/js/owl.carousel.min.js",
                        "~/assets/landing/js/jquery.sticky.js",
                        "~/assets/landing/js/jquery.app.js"));
        }
    }
}
