using System.Web.Mvc;
using System.Web.Routing;

namespace AuditManager.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Admin_elmah",
            //    url: "Admin/elmah/{type}",
            //    defaults: new { action = "Index", controller = "Elmah", type = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Workspace", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Offline",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Workspace", action = "Maintenance", id = UrlParameter.Optional }
            );
        }
    }
}
