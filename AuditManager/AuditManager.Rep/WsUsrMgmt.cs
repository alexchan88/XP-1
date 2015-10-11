using AuditManager.Common;
using AuditManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuditManager.Rep
{
    public class WsUsrMgmt
    {
        public static void RemoveUsrFromGrp(string wsId, string grpName, string usrIdToRemove, ImDbType imDbType = ImDbType.Active)
        {
            if (CanTakeAction(wsId, grpName))
            {
                IM.Mgr.WsUsrMgmt.RemoveUsrFromGrp(wsId, grpName, usrIdToRemove, imDbType);
            }
            else
            {
                throw new Exception("You don't have rights to Remove user.");
            }
        }

        public static void AddUsrToGrp(string wsId, string grpName, string usrIdToAdd, ImDbType imDbType = ImDbType.Active)
        {
            if (CanTakeAction(wsId, grpName))
            {
                IM.Mgr.WsUsrMgmt.AddUsrToGrp(wsId, grpName, usrIdToAdd, imDbType);
            }
            else
            {
                throw new Exception("You don't have rights to Add user.");
            }
        }

        public static List<WsUser> SearchUsr(string searchStr, UsrSearchBy usrSearchBy, bool isExactSrch = false, ImDbType imDbType = ImDbType.Active)
        {
            return IM.Mgr.WsUsrMgmt.SearchUsr(searchStr, usrSearchBy, isExactSrch, imDbType);
        }

        private static bool CanTakeAction(string wsId, string grpName)
        {
            var wsModel = Workspace.GetEngByWsId(wsId, WsLoadType.Groups);

            if(wsModel.FirstOrDefault().WsProfile.EngNum.Equals(grpName.Split('_')[0], StringComparison.OrdinalIgnoreCase))
            {
                var actionGroup = wsModel.FirstOrDefault().WsGroups.Where(x =>
                 x.Name.Equals(string.Format("{0}_E_ADMIN", wsModel.FirstOrDefault().WsProfile.EngNum), StringComparison.OrdinalIgnoreCase) 
                //x.Name.Equals(string.Format("{0}_E_MEMBERS", wsModel.FirstOrDefault().WsProfile.EngNum), StringComparison.OrdinalIgnoreCase) 
                //|| x.Name.Equals(string.Format("{0}_E_READ_ONLY", wsModel.FirstOrDefault().WsProfile.EngNum), StringComparison.OrdinalIgnoreCase)
                );

                var canTakeAction = actionGroup.Where(x => x.GrpUsers.Exists(y => y.Name.Equals(AmUtil.GetCurrentUser, StringComparison.OrdinalIgnoreCase))).ToList();

                if (canTakeAction != null && canTakeAction.Count > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                throw new Exception("You don't have rights.");
            }
        }
    }
}
