using DDA.Business;
using DDA.Web.Controllers;
using DDA.Web.Helpers;
using SSSFramework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DDA.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Dependency.Register();
            var binder = new UTCDateTimeModelBinder();
            ModelBinders.Binders.Add(typeof(DateTime), binder);
            ModelBinders.Binders.Add(typeof(DateTime?), binder);
        }
        protected void Application_Error()
        {
            RouteData routeData = new RouteData();

            Exception ex = Server.GetLastError().GetBaseException();
            Server.ClearError();

            LoggingHelper.LogError(ex);

            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "Error");
            routeData.Values.Add("Description", ex.Message);
            routeData.Values.Add("Name", ex.GetType().Name);

            Response.Headers.Add("Content-Type", "text/html");

            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);

            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
        }


        protected void Application_EndRequest()
        {
            Response.Headers["X-Content-Type-Options"] = "nosniff";
            Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
            Response.Headers["X-XSS-Protection"] = "1; mode=block";
            if (Request.IsSecureConnection)
                Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";

            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
        }
    }
}
