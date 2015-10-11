using AuditManager.Model;
using AuditManager.Web.Common;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Http;

namespace AuditManager.Web.Api
{
    public class PrjNDCSController : ApiController
    {
        public JArray GetPrjCode(string clientCode, string engNum)
        {
            var result = ExtApi.GetResponse(ExtApiType.Prj, new List<string> { clientCode, engNum });
            if (result == null)
                return null;
            return JArray.FromObject(result);
        }

        public JArray GetDCSServer(string prjCode)
        {
            dynamic result = null;
            if (string.IsNullOrWhiteSpace(prjCode))
                result = ExtApi.GetResponse(ExtApiType.DcsAll, new List<string> { });
            else
                result = ExtApi.GetResponse(ExtApiType.Dcs, new List<string> { prjCode });
            if (result == null)
                return null;
            return JArray.FromObject(result);
        }

        public JArray GetPrjCode0(string clientCode, string engNum)
        {
            var result = new List<string> { "PrjCode_1", "PrjCode_2", "PrjCode_3", "PrjCode_4", "PrjCode_5" };
            return JArray.FromObject(result);
        }

        public JArray GetDCSServer0(string prjCode)
        {
            var result = new List<string>();
            if (!string.IsNullOrWhiteSpace(prjCode))
            {
                switch (prjCode)
                {
                    case "PrjCode_1":
                        result.AddRange(new List<string> { "1_DCSServer_PrjCode_1", "2_DCSServer_PrjCode_1", "3_DCSServer_PrjCode_1", "4_DCSServer_PrjCode_1", "5_DCSServer_PrjCode_1" });
                        break;
                    case "PrjCode_2":
                        result.AddRange(new List<string> { "1_DCSServer_PrjCode_2", "2_DCSServer_PrjCode_2", "3_DCSServer_PrjCode_2", "4_DCSServer_PrjCode_2", "5_DCSServer_PrjCode_2" });
                        break;
                    case "PrjCode_3":
                        result.AddRange(new List<string> { "1_DCSServer_PrjCode_3", "2_DCSServer_PrjCode_3", "3_DCSServer_PrjCode_3", "4_DCSServer_PrjCode_3", "5_DCSServer_PrjCode_3" });
                        break;
                    case "PrjCode_4":
                        result.AddRange(new List<string> { "1_DCSServer_PrjCode_4", "2_DCSServer_PrjCode_4", "3_DCSServer_PrjCode_4", "4_DCSServer_PrjCode_4", "5_DCSServer_PrjCode_4" });
                        break;
                    case "PrjCode_5":
                        result.AddRange(new List<string> { "1_DCSServer_PrjCode_5", "2_DCSServer_PrjCode_5", "3_DCSServer_PrjCode_5", "4_DCSServer_PrjCode_5", "5_DCSServer_PrjCode_5" });
                        break;
                }
            }

            if (result.Count == 0)
            {
                result.AddRange(new List<string> { "1_DCSServer_PrjCode_1", "2_DCSServer_PrjCode_1", "3_DCSServer_PrjCode_1", "4_DCSServer_PrjCode_1", "5_DCSServer_PrjCode_1" });
                result.AddRange(new List<string> { "1_DCSServer_PrjCode_2", "2_DCSServer_PrjCode_2", "3_DCSServer_PrjCode_2", "4_DCSServer_PrjCode_2", "5_DCSServer_PrjCode_2" });
                result.AddRange(new List<string> { "1_DCSServer_PrjCode_3", "2_DCSServer_PrjCode_3", "3_DCSServer_PrjCode_3", "4_DCSServer_PrjCode_3", "5_DCSServer_PrjCode_3" });
                result.AddRange(new List<string> { "1_DCSServer_PrjCode_4", "2_DCSServer_PrjCode_4", "3_DCSServer_PrjCode_4", "4_DCSServer_PrjCode_4", "5_DCSServer_PrjCode_4" });
                result.AddRange(new List<string> { "1_DCSServer_PrjCode_5", "2_DCSServer_PrjCode_5", "3_DCSServer_PrjCode_5", "4_DCSServer_PrjCode_5", "5_DCSServer_PrjCode_5" });
            }

            return JArray.FromObject(result);
        }
    }
}
