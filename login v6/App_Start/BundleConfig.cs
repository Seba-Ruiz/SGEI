using System.Web.Optimization;

namespace login_v6
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        //
        // Orden de carga en el layout (todo al final del <body>):
        //   ~/bundles/jquery     -> jQuery (única copia en toda la app) + jQuery Migrate
        //   ~/bundles/bootstrap  -> Bootstrap 3
        //   ~/bundles/plugins    -> plugins del tema (DataTables, select2, sweetalert, etc.)
        //   Scripts/app/sgei.js  -> comportamiento común (menú, confirmaciones, AJAX)
        //   Scripts/views/...    -> script propio de cada vista, si existe
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        // Migrate mantiene funcionando a los plugins del tema escritos para jQuery 1.x/2.x
                        // (por ejemplo DataTables Buttons usa .andSelf()). Se puede quitar al actualizar los plugins.
                        "~/Scripts/jquery-migrate-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/assets/js/modernizr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/assets/js/bootstrap.min.js"));

            // Pantallas de cuenta sin layout (registro, recuperar contraseña, etc.): tema "login".
            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                        "~/Scripts/app/tema-login.js",
                        "~/assets/login/js/detect.js",
                        "~/assets/login/js/fastclick.js",
                        "~/assets/login/js/jquery.slimscroll.js",
                        "~/assets/login/js/jquery.blockUI.js",
                        "~/assets/login/js/waves.js",
                        "~/assets/login/js/wow.min.js",
                        "~/assets/login/js/jquery.nicescroll.js",
                        "~/assets/login/js/jquery.scrollTo.min.js",
                        "~/assets/login/js/jquery.core.js",
                        "~/assets/login/js/jquery.app.js"));

            // Portada (Home/Landing), tiene su propio tema.
            bundles.Add(new ScriptBundle("~/bundles/landing").Include(
                        "~/assets/landing/js/bootstrap.min.js",
                        "~/assets/landing/js/jquery.easing.1.3.min.js",
                        "~/assets/landing/js/owl.carousel.min.js",
                        "~/assets/landing/js/jquery.sticky.js",
                        "~/assets/landing/js/jquery.app.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                        "~/assets/js/detect.js",
                        "~/assets/js/fastclick.js",
                        "~/assets/js/jquery.slimscroll.js",
                        "~/assets/js/jquery.blockUI.js",
                        "~/assets/js/waves.js",
                        "~/assets/js/wow.min.js",

                        "~/assets/plugins/chart.js/chart.min.js",

                        "~/assets/js/jquery.core.js",
                        "~/assets/js/jquery.app.js",

                        "~/assets/plugins/counterup/jquery.counterup.min.js",

                        "~/assets/plugins/datatables/jquery.dataTables.min.js",
                        "~/assets/plugins/datatables/dataTables.bootstrap.js",
                        "~/assets/plugins/datatables/dataTables.buttons.min.js",
                        "~/assets/plugins/datatables/buttons.bootstrap.min.js",

                        "~/assets/plugins/jquery.steps/js/jquery.steps.min.js",
                        "~/assets/js/jquery.nicescroll.js",

                        "~/assets/js/jquery.scrollTo.min.js",
                        "~/assets/plugins/peity/jquery.peity.min.js",
                        "~/assets/plugins/waypoints/lib/jquery.waypoints.js",

                        "~/assets/plugins/raphael/raphael-min.js",
                        "~/assets/plugins/jquery-knob/jquery.knob.js",
                        "~/assets/plugins/bootstrap-tagsinput/js/bootstrap-tagsinput.min.js",
                        "~/assets/plugins/switchery/js/switchery.min.js",
                        "~/assets/plugins/multiselect/js/jquery.multi-select.js",
                        "~/assets/plugins/jquery-quicksearch/jquery.quicksearch.js",
                        "~/assets/plugins/select2/js/select2.min.js",
                        "~/assets/plugins/bootstrap-select/js/bootstrap-select.min.js",
                        "~/assets/plugins/bootstrap-filestyle/js/bootstrap-filestyle.min.js",
                        "~/assets/plugins/bootstrap-touchspin/js/jquery.bootstrap-touchspin.min.js",
                        "~/assets/plugins/bootstrap-maxlength/bootstrap-maxlength.min.js",

                        "~/assets/plugins/bootstrap-sweetalert/sweet-alert.min.js",
                        "~/assets/pages/datatables.init.js",
                        "~/assets/plugins/datatables/jszip.min.js",

                        "~/assets/plugins/datatables/pdfmake.min.js",
                        "~/assets/plugins/datatables/vfs_fonts.js",
                        "~/assets/plugins/datatables/buttons.html5.min.js",
                        "~/assets/plugins/datatables/buttons.print.min.js",
                        "~/assets/plugins/datatables/dataTables.fixedHeader.min.js",
                        "~/assets/plugins/datatables/dataTables.keyTable.min.js",
                        "~/assets/plugins/datatables/dataTables.responsive.min.js",
                        "~/assets/plugins/datatables/responsive.bootstrap.min.js",
                        "~/assets/plugins/datatables/dataTables.scroller.min.js",
                        "~/assets/plugins/datatables/dataTables.colVis.js",
                        "~/assets/plugins/datatables/dataTables.fixedColumns.min.js"));

            bundles.Add(new StyleBundle("~/assets/coso").Include(
                        "~/assets/plugins/jquery.steps/css/jquery.steps.css",
                       "~/assets/css/bootstrap.min.css",
                      "~/assets/css/menu.css",
                      "~/assets/css/core.css",
                      "~/assets/css/components.css",
                      "~/assets/css/icons.css",
                      "~/assets/css/pages.css",
                      "~/assets/css/responsive.css",

                     "~/assets/plugins/morris/morris.css",

                      "~/assets/plugins/datatables/jquery.dataTables.min.css",
                      "~/assets/plugins/bootstrap-sweetalert/sweet-alert.css",
                      "~/assets/plugins/bootstrap-tagsinput/css/bootstrap-tagsinput.css",
                      "~/assets/plugins/switchery/css/switchery.min.css",
                      "~/assets/plugins/multiselect/css/multi-select.css",
                      "~/assets/plugins/select2/css/select2.min.css",
                      "~/assets/plugins/bootstrap-select/css/bootstrap-select.min.css",
                      "~/assets/plugins/bootstrap-touchspin/css/jquery.bootstrap-touchspin.min.css",
                      "~/assets/plugins/x-editable/css/bootstrap-editable.css",

                      "~/assets/plugins/datatables/buttons.bootstrap.min.css",
                      "~/assets/plugins/datatables/fixedHeader.bootstrap.min.css",
                      "~/assets/plugins/datatables/responsive.bootstrap.min.css",
                      "~/assets/plugins/datatables/dataTables.colVis.css",
                      "~/assets/plugins/datatables/dataTables.bootstrap.min.css",
                      "~/assets/plugins/datatables/fixedColumns.dataTables.min.css",
                      "~/assets/plugins/datatables/scroller.bootstrap.min.css"
                      ));
        }
    }
}
