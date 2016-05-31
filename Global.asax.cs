using MyOnlineClinic.Migrator;
using MyOnlineClinic.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MyOnlineClinic.RepositoryService;
using System.Web.Security;

namespace MyOnlineClinic.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //asa
        protected void Application_Start()
        {
            if (Database.Exists("EntitiesDB"))
            {  //Recreates the DB if the model changes
              //Database.SetInitializer(new DbIntializer<DatabaseNotFoundInitializer>());
               Database.SetInitializer(new MigrateDatabaseToLatestVersion<EntitiesDB, Configuration>());
            }
            else
            //This is the default strategy.  It creates the DB only if it doesn't exist
            {
                Database.SetInitializer(new DbIntializer<DatabaseNotFoundInitializer>());
            }
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.GetConfiguredContainer();
        }

        protected void Application_Error()
        {
            var error = Server.GetLastError();
            var cryptoEx = error as CryptographicException;
            if (cryptoEx != null)
            {
                FormsAuthentication.SignOut();
                Server.ClearError();
                //HttpContext.Current.Response.Redirect("/Home/Index");
                Response.Redirect("/Home/Index");
            }
        }
    }
}
