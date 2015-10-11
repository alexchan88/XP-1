using AuditManager.Common;
using AuditManager.Web.Common;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AuditManager.Web.Api
{
    public class IManageUtilityController : ApiController
    {
        public JArray GetTest()
        {
            //LOCAL_ADDR	Returns the server address on which the request came in - Differ
            //HttpContext.Current.Request.UserHostAddress - Same
            //REMOTE_ADDR	Returns the IP address of the remote host making the request - Same
            //REMOTE_HOST	Returns the name of the host making the request - Same

            List<dynamic> x = new List<dynamic>();

            x.Add(new { a = "UserHostAddress", b = HttpContext.Current.Request.UserHostAddress });
            x.Add(new { a = "HTTP_CLIENT_IP", b = HttpContext.Current.Request.Params["HTTP_CLIENT_IP"] });
            x.Add(new { a = "LOCAL_ADDR", b = HttpContext.Current.Request.Params["LOCAL_ADDR"] });
            x.Add(new { a = "REMOTE_ADDR", b = HttpContext.Current.Request.Params["REMOTE_ADDR"] });
            x.Add(new { a = "REMOTE_HOST", b = HttpContext.Current.Request.Params["REMOTE_HOST"] });
            x.Add(new { a = "LOGON_USER", b = HttpContext.Current.Request.Params["LOGON_USER"] });
            x.Add(new { a = "SERVER_NAME", b = HttpContext.Current.Request.Params["SERVER_NAME"] });
            x.Add(new { a = "GetIP", b = AmUtil.GetIP });

            return JArray.FromObject(x);
        }
        
        public JObject GetHash(int? fileNum = null)
        {
            return JObject.FromObject(Utility.GetMd5Hash(fileNum));
        }

        private string GetIp()
        {
            return GetClientIp();
        }

        private string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties != null && request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            //else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            //{
            //    RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
            //    return prop.Address;
            //}
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }

        //[HttpGet]
        //public void UploadDocument(string engNum, string fileNameWithLocalPath, string iMFolderPath) {
        //    AuditManager.Rep.IManageUtility.UploadDocument(engNum, fileNameWithLocalPath, iMFolderPath);
        //}

        //public void UploadDocument2(string wsId, string fileNameWithLocalPath, string iMFolderPath) {
        //    AuditManager.Rep.IManageUtility.UploadDocument2(wsId, fileNameWithLocalPath, iMFolderPath);
        //}
    }
}
