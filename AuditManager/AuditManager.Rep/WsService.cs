using AuditManager.Model;
using System.Collections.Generic;
using System.Linq;

namespace AuditManager.Rep
{
    public class WsService
    {
        public static bool IsWSUnderPreservation(string engNum)
        {
            List<WsModel> wsModel = AuditManager.Rep.Workspace.GetEngByEngNum(engNum, WsLoadType.Profile, true);

            if (wsModel == null || wsModel.Count < 1)
            {
                return false;
            }
            else
            {
                return wsModel.FirstOrDefault().WsProfile.IsUnderPreservation;
            }
        }
    }
}
