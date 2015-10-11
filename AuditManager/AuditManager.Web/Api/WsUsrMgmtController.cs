using AuditManager.Model;
using AuditManager.Web.Common;
using Newtonsoft.Json.Linq;
using System.Web.Http;

namespace AuditManager.Web.Api
{
    public class WsUsrMgmtController : ApiController
    {
        public void PutRemoveUsrFromGrp(string num, string wsId, string grpName, string usrIdToRemove, ImDbType imDbType = ImDbType.Active)
        {
            AuditManager.Rep.WsUsrMgmt.RemoveUsrFromGrp(wsId, grpName, usrIdToRemove, imDbType);
            ExtApi.KWSLogAddDeleteUser(num, grpName, usrIdToRemove, "DeleteUser");
        }

        public void PostAddUsrToGrp(string num, string wsId, string grpName, string usrIdToAdd, ImDbType imDbType = ImDbType.Active)
        {
            AuditManager.Rep.WsUsrMgmt.AddUsrToGrp(wsId, grpName, usrIdToAdd, imDbType);
            ExtApi.KWSLogAddDeleteUser(num, grpName, usrIdToAdd, "AddUser");
        }

        public JArray GetSearchUsr(string searchStr, UsrSearchBy usrSearchBy, bool isExactSrch = false, ImDbType imDbType = ImDbType.Active)
        {
            var result = AuditManager.Rep.WsUsrMgmt.SearchUsr(searchStr, usrSearchBy, isExactSrch, imDbType);

            if (result == null)
                return null;

            var retModel = JArray.FromObject(result);

            return retModel;
        }
    }
}
