using Elmah;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace AuditManager.Web
{
    public class UsrIdentity : IIdentity
    {

        public string AuthenticationType
        {
            get { return ""; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return ""; }
        }
    }
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;

            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            GlobalConfiguration.Configuration.Formatters
            .JsonFormatter.SerializerSettings
            .PreserveReferencesHandling =
            Newtonsoft.Json.PreserveReferencesHandling.All;
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //Application_BeginRequest
        //Application_AuthenticateRequest
        //Application_PostAuthenticateRequest
        //Application_AuthorizeRequest
        //Session_Start
        //Application_EndRequest

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    var exception = Server.GetLastError();
        //    if (exception == null)
        //        return;

        //    var exMsg = exception.Message;

        //    // Clear the error
        //    Server.ClearError();

        //    // Redirect to a landing page
        //    //Response.Redirect("home/landing");
        //}

        protected void Session_Start(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                //var n = User.Identity.Name;
                //var id = ClaimsPrincipal.Current.Identities.First();

            }   
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // event is raised when a session is abandoned or expires
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            /* HttpContext.Current.Response.AddHeader(
                                                                                              "Access-Control-Allow-Origin", 
                                                                                              "http://AllowedDomain.com"); */
        }
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            
        }

        protected void Application_AuthorizeRequest(Object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {

        }

        protected void WindowsAuthentication_OnAuthenticate(object sender, WindowsAuthenticationEventArgs e)
        { 
        
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {

            //if (HttpContext.Current.User != null)
            //{
            //    if (HttpContext.Current.User.Identity.IsAuthenticated)
            //    {
            //        if (HttpContext.Current.User.Identity is WindowsIdentity)
            //        {
            //            var id = HttpContext.Current.User.Identity as WindowsIdentity;
                        
            //            UsrIdentity identity = new UsrIdentity();
            //            GenericPrincipal prin = new GenericPrincipal(identity, null);
            //            Thread.CurrentPrincipal = prin;
            //            HttpContext.Current.User = prin;

            //            //HttpContext.Current.Response.Cookies.Add(new HttpCookie(WindowsAuthentication);
            //        }
            //    }
            //}

            //if (Request.IsAuthenticated)
            //{
            //    var n = User.Identity.Name;
            //    var id = ClaimsPrincipal.Current.Identities.First();
                

            //    foreach (var group in Request.LogonUserIdentity.Groups)
            //    {
            //        var g = group.Value;

            //        var ClaimType = ClaimTypes.Role;
            //        var ClaimValue = new SecurityIdentifier(group.Value)
            //                           .Translate(typeof(NTAccount)).Value;
            //    }
            //}
        }

        void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            //if (e.Exception.GetBaseException() is HttpRequestValidationException)
            //    e.Dismiss();
        }

        void ErrorLog_Logged(object sender, ExceptionFilterEventArgs args)
        {
            //var elmahIoLog = new Elmah.Io.ErrorLog(new Logger(new Guid("insert your log id")));
            //elmahIoLog.Log(new Error(HttpContext.Current.Error, HttpContext.Current));
        }
    }
}
