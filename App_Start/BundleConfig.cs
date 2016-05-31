using System.Web;
using System.Web.Optimization;

namespace MyOnlineClinic.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Scripts/jquery-ui-{version}.js",
            "~/Scripts/jquery-ui.unobtrusive-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            //bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
            //                              "~/Scripts/jquery.dataTables*",
            //                              "~/Scripts/bootstrap.datatables.js"
            //                            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Default.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-override.css"
        //               "~/Content/themes/base/jquery.ui.core.css",
        //"~/Content/themes/base/jquery.ui.datepicker.css",
        //"~/Content/themes/base/jquery.ui.theme.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/bonnet").Include(
                 "~/Scripts/jquery.bonnet.ajax-dropdownlist.js"
                ));

            bundles.Add(new StyleBundle("~/css").Include(
                     "~/css/bootstrap.css",
                     "~/css/featherlight.min.css",
                     "~/css/flexslider.css",
                     "~/css/owl.carousel.min.css",
                     "~/css/owl.theme.default.min.css",
                     "~/css/slimmenu.css",
                     "~/css/style.css",
                       "~/css/google_font.css", "~/AdminAssets/css/custom.css"            
                     ));
            bundles.Add(new StyleBundle("~/css/fonts/css").Include(
                   "~/css/fonts/css/font-awesome.css"
                   ));
            bundles.Add(new StyleBundle("~/js").Include(
                    "~/js/include_script.js",
                   // "~/js/owl.carousel.js",
                  //  "~/js/jquery.flexslider.js",
                    "~/js/jquery.Jcrop.js",
                    "~/js/pageScript.js", 
                    "~/js/daterangepicker.js",
                    // "~/js/bootstrap.js",
                    "~/js/daterangepicker.js",
                 //   "~/js/featherlight.min.js",
                    "~/js/jquery.color.js",
                  //  "~/js/jquery.easing.min.js",
                    "~/js/jquery.Jcrop.min.js",
                    "~/js/jquery.jcrop.preview.js",
                    "~/js/jquery.min.js"
                   // "~/js/jquery.slimmenu.js",
                  //  "~/js/jquery.unveilEffects.js"
                    ));


            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //"~/Content/themes/base/jquery.ui.core.css",
            // "~/Content/themes/base/jquery.ui.datepicker.css",
            // "~/Content/themes/base/jquery.ui.theme.css",
            //  "~/Content/themes/base/jquery.ui.dialog.css"));
        }
    }
}
