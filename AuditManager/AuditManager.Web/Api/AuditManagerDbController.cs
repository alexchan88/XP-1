using AuditManager.Model;
using AuditManager.Web.Common;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace AuditManager.Web.Api
{
    public class ProxyContent
    {
        public string contentType { get; set; }
        public string base64 { get; set; }
        public string fileName { get; set; }
    }

    public class AuditManagerDbController : ApiController
    {
        public JArray GetTableData(string tableName)
        {
            if (tableName.Equals("C4", StringComparison.OrdinalIgnoreCase))
            {
                var result = AuditManager.Rep.AuditManagerDb.GetTableData_C4(tableName);

                if (result == null)
                    return null;

                var retModel = JArray.FromObject(result);

                return retModel;
            }
            else if (tableName.Equals("C6", StringComparison.OrdinalIgnoreCase))
            {
                var result = AuditManager.Rep.AuditManagerDb.GetTableData_C6(tableName);

                if (result == null)
                    return null;

                var retModel = JArray.FromObject(result);

                return retModel;
            }

            return null;
        }
        
        public JArray GetWsActivityHistory(string engNum, string usrId, WsActivityType wsActivityType, WsActivityType? wsActivitySubType = null)
        {
            var result = AuditManager.Rep.AuditManagerDb.GetWsActivityHistory(engNum, usrId, wsActivityType, wsActivitySubType);

            if (result == null)
                return null;

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public JArray GetClosureHistory(string engNum, string usrId, WsActivityType? wsActivityType = null, WsActivityType? wsActivitySubType = null)
        {
            var result = AuditManager.Rep.AuditManagerDb.GetClosureHistory(engNum, usrId, wsActivityType, wsActivitySubType);

            if (result == null)
                return null;

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public JArray GetClosureReport(DateTime fromD, DateTime toD, WsActivityType? wsActivityType = null, WsActivityType? wsActivitySubType = null)
        {
            var result = AuditManager.Rep.AuditManagerDb.GetClosureReport(fromD, toD, wsActivityType, wsActivitySubType);

            if (result == null)
                return null;

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        //excel.proxyURL String (default: null)

        //The URL of the server side proxy which will stream the file to the end user.

        //A proxy will be used when the browser isn't capable of saving files locally. Such browsers are IE version 9 and lower and Safari.

        //The developer is responsible for implementing the server-side proxy.

        //The proxy will receive a POST request with the following parameters in the request body:

        //contentType: The MIME type of the file
        //base64: The base-64 encoded file content
        //fileName: The file name, as requested by the caller.
        //The proxy should return the decoded file with set "Content-Disposition" header.

        public HttpResponseMessage PostClosureReportProxy(ProxyContent proxyContent)
        {
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            byte[] byteArray = Convert.FromBase64String(proxyContent.base64);
            MemoryStream stream = new MemoryStream(byteArray);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentDisposition =
               new ContentDispositionHeaderValue("attachment") { FileName = proxyContent.fileName };

            return response;
        }

        public JArray GetProfileHistory(string engNum, string usrId, WsActivityType? wsActivityType = null, WsActivityType? wsActivitySubType = null)
        {
            var result = AuditManager.Rep.AuditManagerDb.GetProfileHistory(engNum, usrId, wsActivityType, wsActivitySubType);

            if (result == null)
                return null;

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public JArray GetEngLog(string engNum)
        {
            var result = AuditManager.Rep.AuditManagerDb.GetEngLog(engNum);

            if (result == null)
                return null;

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public void ProcessKPMGOnly()
        {
            ExtApi.ProcessKPMGOnly();
        }
    }
}
