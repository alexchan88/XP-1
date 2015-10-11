using AuditManager.Web.Filters;
using System.Web.Http;

namespace AuditManager.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                //routeTemplate: "api/{controller}/{id}",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new ApiAuthFilter());
            config.Filters.Add(new AmApiAuthenticationFilter());
            config.Filters.Add(new Elmah.Contrib.WebApi.ElmahHandleErrorApiAttribute());
            config.Filters.Add(new AuditManager.Web.Filters.ApiExceptionWithElmahAttribute());

            //var json = config.Formatters.JsonFormatter;
            //json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            //config.Formatters.Remove(config.Formatters.XmlFormatter);

            //json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //GlobalConfiguration.Configuration.Formatters
            //.JsonFormatter.SerializerSettings
            //.PreserveReferencesHandling =
            //Newtonsoft.Json.PreserveReferencesHandling.All;
        }
    }
}
