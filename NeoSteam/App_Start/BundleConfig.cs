using System.Web;
using System.Web.Optimization;

namespace NeoSteam
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // js 包
            bundles.Add(new ScriptBundle("~/Content/js").Include(
                        "~/Content/js/jquery-1.7.1.min.js",
                        "~/Content/plugins/jimgareaselect/jquery.imgareaselect.min.js",
                        "~/Content/plugins/jquery.dualListBox-1.3.min.js",
                        "~/Content/plugins/jgrowl/jquery.jgrowl.js",
                        "~/Content/plugins/jquery.filestyle.js",
                        "~/Content/plugins/fullcalendar/fullcalendar.min.js",
                        "~/Content/plugins/jquery.dataTables.js",
                        "~/Content/plugins/flot/jquery.flot.min.js",
                        "~/Content/plugins/flot/jquery.flot.pie.min.js",
                        "~/Content/plugins/flot/jquery.flot.stack.min.js",
                        "~/Content/plugins/flot/jquery.flot.resize.min.js",
                        "~/Content/plugins/colorpicker/colorpicker.js",
                        "~/Content/plugins/tipsy/jquery.tipsy.js",
                        "~/Content/plugins/sourcerer/Sourcerer-1.2.js",
                        "~/Content/plugins/jquery.placeholder.js",
                        "~/Content/plugins/jquery.validate.js", "~/Content/plugins/jquery.mousewheel.js",
                        "~/Content/plugins/spinner/ui.spinner.js",
                        "~/Content/js/jquery-ui.js",
                        "~/Content/js/mws.js",
                        "~/Content/js/demo.js",
                        "~/Content/js/themer.js"));
            //css 包
            bundles.Add(new StyleBundle("~/Content").Include(
                        "~/Content/css/style.css",
                        "~/Content/css/fonts.css",
                        "~/Content/css/960.min.css",
                        "~/Content/css/flexslider.css",
                        "~/Content/css/diapo.css",
                        "~/Content/css/prettyPhoto.css",
                        "~/Content/plugins/spinner/spinner.css", "~/Content/css/mws.theme.css"));

        }
    }
}