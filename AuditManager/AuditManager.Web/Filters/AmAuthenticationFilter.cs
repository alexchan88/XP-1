using AuditManager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using HttpApi = System.Web.Http.Filters;
using WebMvc = System.Web.Mvc;

namespace AuditManager.Web.Filters
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }

        public string ReasonPhrase { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            response.ReasonPhrase = ReasonPhrase;
            return response;
        }
    }
    public class AmMvcAuthorizationFilter : WebMvc.AuthorizeAttribute
    {
        public AmMvcAuthorizationFilter()
        {
            base.Users = AmUtil.GetActivityUserNameWithDomain();
        }

        protected override void HandleUnauthorizedRequest(WebMvc.AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult { ViewName = "Unauthorized" };
        }
    }

    public class AmMvcAuthenticationFilter : WebMvc.ActionFilterAttribute, WebMvc.Filters.IAuthenticationFilter
    {
        public void OnAuthentication(WebMvc.Filters.AuthenticationContext filterContext)
        {
            if (AuditManager.Common.ConfigUtility.IsUnderMaintenance() && !AmUtil.IsMaintenanceUser())
                return;

            if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Elmah", StringComparison.OrdinalIgnoreCase))
                return;

            //filterContext.Principal.Identity.Name 
            var usr = filterContext.Principal.Identity.Name.ToUserIdFromDnsName();
            var wsUser = AuditManager.Rep.AmUtility.GetWsUser(usr, Model.ImDbType.Active);
            if (wsUser == null)
            {
                AuditManager.Rep.WsCreate.RequestAccessToAuditManager(string.Format("Auto Mail - User \"{0}\" doesn't exist in WorkSite table.", usr), false);
                filterContext.Result = new ViewResult { ViewName = "Unauthorized" };
            }
        }

        public void OnAuthenticationChallenge(WebMvc.Filters.AuthenticationChallengeContext filterContext)
        {

        }
    }

    public class AmApiAuthenticationFilter : HttpApi.ActionFilterAttribute
    //, HttpApi.IAuthenticationFilter
    {
        //public override void OnActionExecuted(HttpApi.HttpActionExecutedContext actionExecutedContext)
        //{
        //    base.OnActionExecuted(actionExecutedContext);
        //}

        //System.Threading.Tasks.Task HttpApi.IAuthenticationFilter.AuthenticateAsync(HttpApi.HttpAuthenticationContext context, System.Threading.CancellationToken cancellationToken)
        //{
        //    var usr = context.Principal.Identity.Name.ToUserIdFromDnsName();
        //    var wsUser = AuditManager.Rep.AmUtility.GetWsUser(usr, Model.ImDbType.Active);
        //    if (wsUser == null)
        //    {

        //        context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", context.Request);
        //    }
        //    //throw new System.NotImplementedException();
        //    var httpUnauthorizedResponse = context.Request.CreateResponse(HttpStatusCode.Unauthorized, "You are not authorized.");
        //    context.ActionContext.Response = httpUnauthorizedResponse;
        //    context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", context.Request);
        //    return Task.FromResult(0);
        //}

        //System.Threading.Tasks.Task HttpApi.IAuthenticationFilter.ChallengeAsync(HttpApi.HttpAuthenticationChallengeContext context, System.Threading.CancellationToken cancellationToken)
        //{
        //    //throw new System.NotImplementedException();
        //    return Task.FromResult(0);
        //}

        //bool HttpApi.IFilter.AllowMultiple
        //{
        //    get { throw new System.NotImplementedException(); }
        //}
    }

    public class ApiAuthFilter : Attribute, IAuthenticationFilter
    {

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName.Equals("Workspace", StringComparison.OrdinalIgnoreCase)
                && (context.ActionContext.ActionDescriptor.ActionName.Equals("GetIsWSUnderPreservation", StringComparison.OrdinalIgnoreCase) ||
                context.ActionContext.ActionDescriptor.ActionName.Equals("GetNoRecordFiles", StringComparison.OrdinalIgnoreCase)))
                if (context.Principal.Identity.Name.ToUserIdFromDnsName().Equals("us-svcdeviwa1") ||
                    context.Principal.Identity.Name.ToUserIdFromDnsName().Equals("us-svcproddrms") ||
                    context.Principal.Identity.Name.ToUserIdFromDnsName().Equals("viveksingh1"))
                {
                    return;
                }

            if (context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName.Equals("Download", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            else if (context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName.Equals("Test", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string cookieToken = "";
            string formToken = "";

            IEnumerable<string> tokenHeaders;
            if (context.Request.Headers.TryGetValues("RequestVerificationToken", out tokenHeaders))
            {
                string[] tokens = tokenHeaders.First().Split(':');
                if (tokens.Length == 2)
                {
                    cookieToken = tokens[0].Trim();
                    formToken = tokens[1].Trim();
                }
            }

            AntiForgery.Validate(cookieToken, formToken);

            //throw new NotImplementedException();
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}