using System.Web;
using System.Web.Optimization;

namespace SOP_IAA
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.bootstrap.wizard.js").Include(
                        "~/Scripts/jquery-1.11.0.js").Include(
                        "~/Scripts/plugins/metisMenu/metisMenu.min.js").Include(
                        "~/Scripts/admin_bootstrap.js").Include(
                        "~/Scripts/jquery-ui-1.11.1.js").Include(
                        "~/Scripts/jquery.json.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/pluggins").Include(
                       "~/Scripts/plugins/morris/raphael.min.js").Include(
                       "~/Scripts/plugins/morris/morris.min.js").Include(
                       "~/Scripts/plugins/morris/morris-data.js").Include(
                       "~/Scripts/sb-admin-2.js").Include(
                       "~/Content/datepicker/bootstrap-datepicker.js"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js").Include(
                      "~/Scripts/respond.js").Include(
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css").Include(
                      "~/Content/jquery-ui.css"));

            //Scripts para las tablas con paginación y demás adornos
            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                "~/Scripts/plugins/dataTables/dataTables.bootstrap.js").Include(
                "~/Scripts/plugins/dataTables/jquery.data.Tables.js"));

            // Para la depuración, establezca EnableOptimizations en false. Para obtener más información,
            // visite http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
