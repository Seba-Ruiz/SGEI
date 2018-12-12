using System.Web;
using System.Web.Optimization;

namespace login_v6
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquerys").Include(
                
                        "~/assets/js/jquery.min.js",
                        "~/assets/js/detect.js",
                        "~/assets/js/fastclick.js",
                        "~/assets/js/jquery.slimscroll.js",
                        "~/assets/js/jquery.blockUI.js",
                        "~/assets/js/waves.js",
                        "~/assets/js/wow.min.js",
                        "~/assets/js/jquery.core.js",
                        "~/assets/js/jquery.app.js",
                        "~/assets/plugins/jquery.steps/js/jquery.steps.min.js",
                        "~/assets/pages/jquery.wizard-init.js",
                        "~/assets/js/jquery.nicescroll.js",
                        "~/assets/js/jquery.scrollTo.min.js",
                        "~/assets/plugins/peity/jquery.peity.min.js",
                        "~/assets/plugins/waypoints/lib/jquery.waypoints.js",
                        "~/assets/plugins/counterup/jquery.counterup.min.js",
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
                        "~/assets/plugins/datatables/jquery.dataTables.min.js",
                        "~/assets/plugins/datatables/dataTables.bootstrap.js",
                        "~/assets/plugins/bootstrap-sweetalert/sweet-alert.min.js",
                        "~/assets/pages/jquery.sweet-alert.init.js",
                        "~/assets/plugins/datatables/jquery.dataTables.min.js",
                        "~/assets/plugins/datatables/jszip.min.js",
                        "~/assets/plugins/datatables/dataTables.buttons.min.js",
                        "~/assets/plugins/datatables/buttons.bootstrap.min.js",
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
                        "~/assets/plugins/datatables/dataTables.fixedColumns.min.js",
                        "~/assets/pages/datatables.init.js",
                        "~/assets/plugins/moment/moment.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(

                        "~/assets/js/jquery.min.js"
                        
                        ));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/assets/js/modernizr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/assets/js/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/assets/coso").Include(
                        "~/assets/plugins/jquery.steps/css/jquery.steps.css",
                       "~/assets/css/bootstrap.min.css",
                      "~/assets/css/menu.css",
                      "~/assets/css/core.css",
                      "~/assets/css/components.css",
                      "~/assets/css/core.css",
                      "~/assets/css/icons.css",
                      "~/assets/css/pages.css",
                      "~/assets/css/responsive.css",
                      "~/assets/plugins/bootstrap-sweetalert/sweet-alert.css",
                      "~/assets/plugins/bootstrap-tagsinput/css/bootstrap-tagsinput.css",
                      "~/assets/plugins/switchery/css/switchery.min.css",
                      "~/assets/plugins/multiselect/css/multi-select.css",
                      "~/assets/plugins/select2/css/select2.min.css",
                      "~/assets/plugins/bootstrap-select/css/bootstrap-select.min.css",
                      "~/assets/plugins/bootstrap-touchspin/css/jquery.bootstrap-touchspin.min.css",
                      "~/assets/plugins/x-editable/css/bootstrap-editable.css",
                      "~/assets/plugins/datatables/jquery.dataTables.min.css",
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
