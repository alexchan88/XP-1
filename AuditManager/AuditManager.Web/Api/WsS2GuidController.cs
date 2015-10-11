using AuditManager.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace AuditManager.Web.Api
{
    public class WsS2GuidController : ApiController
    {
        public JArray GetMapped_WsS2Guid_ForEng(string engNum)
        {
            var result = AuditManager.Rep.WsS2Guid.GetMapped_WsS2Guid_ForEng(engNum);

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public JArray Get_S2Detail_ForGuid(Guid mafGuid)
        {
            var result = AuditManager.Rep.WsS2Guid.Get_S2Detail_ForGuid(mafGuid);

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public bool LinkMAF(Post_WsS2GuidModel post_WsS2GuidModel)
        {
            return AuditManager.Rep.WsS2Guid.LinkMAF(post_WsS2GuidModel);
        }

        public bool DeLinkMAF(Post_WsS2GuidModel post_WsS2GuidModel)
        {
            return AuditManager.Rep.WsS2Guid.DeLinkMAF(post_WsS2GuidModel);
        }
    }
}