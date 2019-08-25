using System.Web;
using System.Web.Optimization;

namespace fftoolkit.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/DataTables/datatables.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-cards.css",
                      "~/Content/site.css",
                      "~/DataTables/datatables.css"));

            bundles.Add(new ScriptBundle("~/DataTables/scripts").Include(
                     "~/DataTables/datatables.js"));

            //bundles.Add(new ScriptBundle("~/QrCodes/scripts").Include(
            //    "~/Scripts/webcodecamjs/qrcodelib.js",
            //    "~/Scripts/webcodecamjs/webcodecamjs.js"));

            //bundles.Add(new ScriptBundle("~/Scripts/MyScripts").Include(
            //         "~/Scripts/trade.js"));

            bundles.Add(new StyleBundle("~/DataTables/css").Include(
                      "~/DataTables/datatables.css"));
        }
    }
}
