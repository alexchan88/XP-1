using AuditManager.Common;
using System.Net;
using System.Net.Http;

namespace AuditManager.Web.Filters
{
    public class ApiExceptionWithElmahAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        public override void OnException(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
        {
            var usr = actionExecutedContext.ActionContext.RequestContext.Principal.Identity.Name.ToUserIdFromDnsName();

            if (actionExecutedContext.Exception != null)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception);

                if (actionExecutedContext.Exception is System.AggregateException)
                {
                    if (actionExecutedContext.Exception.InnerException is AuditManager.Common.LogOnlyException)
                    {
                        //Elmah.ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception.InnerException);

                        var noErrorResponse =
                        actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, actionExecutedContext.Exception.Message);
                        actionExecutedContext.Response = noErrorResponse;

                    }
                }
                else if (actionExecutedContext.Exception is AuditManager.Common.LogOnlyException)
                {
                    //Elmah.ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception.InnerException);

                    //var noErrorResponse =
                    //actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, actionExecutedContext.Exception.Message);
                    //actionExecutedContext.Response = noErrorResponse;

                }
                else if (actionExecutedContext.Exception is System.Runtime.InteropServices.COMException)
                {
                    if (actionExecutedContext.Exception.Message == "[NRTDatabase ][GetFolderArtifactInfo ]GetFolderArtifactInfo : No record found  (0x800400ca)")
                    {
                        var errorResponse =
                        actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, "You don't have access to the workspace, please request access to the workspace using Request Access link.");
                        actionExecutedContext.Response = errorResponse;
                    }
                    else if (actionExecutedContext.Exception.Message == "[NRTSession ][TrustedLogin ]Access denied (0x8004012f)")
                    {
                        AuditManager.Rep.WsCreate.RequestAccessToAuditManager(string.Format("Auto Mail - [NRTSession ][TrustedLogin ]Access denied - user[{0}] record not in WorkSite table or incomplete.", usr), false);

                        var errorResponse =
                        actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, "You don't have access to Audit Manager.");
                        actionExecutedContext.Response = errorResponse;
                    }
                    else if (actionExecutedContext.Exception.Message == "[NRTSession ][TrustedLogin ]SSPI Authentication for client failed. (0x80040152)")
                    {
                        //[NRTSession ][TrustedLogin ]SSPI Authentication for client failed.  (0x80040152)
                        AuditManager.Rep.WsCreate.RequestAccessToAuditManager(string.Format("Auto Mail - [NRTSession ][TrustedLogin ]SSPI Authentication for client failed - user[{0}] record not in WorkSite table or incomplete.", usr), false);

                        var errorResponse =
                        actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, actionExecutedContext.Exception.Message);
                        actionExecutedContext.Response = errorResponse;
                    }
                    else
                    {
                        var errorResponse =
                        actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, actionExecutedContext.Exception.Message);
                        actionExecutedContext.Response = errorResponse;
                    }
                }
                else
                {
                    var errorResponse =
                    actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, actionExecutedContext.Exception.Message);
                    actionExecutedContext.Response = errorResponse;
                }
            }
            
            //Elmah.ErrorSignal.FromCurrentContext().Raise(new LogOnlyException(actionExecutedContext.Request.RequestUri.OriginalString));

            base.OnException(actionExecutedContext);
        }
    }

}