using System.Web;
using System.Web.Optimization;
namespace Property
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
                      /* Scripts for Web-Site */
            bundles.Add(new ScriptBundle("~/bundles/assets/jquery").Include(
                "~/assets-frontend/js/jquery-1.12.4.min.js",
                "~/assets-frontend/js/plugins.js",

                
                
                "~/assets-frontend/js/custom.js"
                 //,"~/assets-frontend/js/Custom_GooleMap.js"
            

                ));

            /* Style for Web-Site */
            bundles.Add(new StyleBundle("~/styles/web-themes/base/css").Include(

                        "~/assets-frontend/css/plugins.css",
                        "~/assets-frontend/css/style.css"));




           


            // Code removed for clarity.
            //BundleTable.EnableOptimizations = true;

        }
    }
}