using AuditManager.Common;
using AuditManager.EF.AmDbCtx;
using AuditManager.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace AuditManager.Rep
{
    public class WsS2Guid
    {
        public static List<Mapped_WsS2GuidModel> GetMapped_WsS2Guid_ForEng(string engNum)
        {
            using (var db = new S2DbContext())
            {
                SqlParameter p1 = new SqlParameter("@EngagementNumber", engNum);

                var result = db.Database.SqlQuery<Mapped_WsS2GuidModel>("EXEC S2CLR.spGetServer2Links @EngagementNumber", p1).ToList();

                return result;
            }
        }

        public static List<WsS2GuidModel> Get_S2Detail_ForGuid(Guid mafGuid)
        {
            var engNum = Get_Linked_Ws_ForMafGuid(mafGuid);
            if (string.IsNullOrWhiteSpace(engNum))
            {
                List<S2MafService.Server2Object> result = null;
                S2MafService.MAFServiceClient obj = new S2MafService.MAFServiceClient();

                result = obj.GetClosureWBDetails(mafGuid.ToString());

                //try
                //{
                //    result = obj.GetClosureWBDetails(mafGuid.ToString());
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception(string.Format("Guid [{0}] does not exists on Server 2", mafGuid));
                //}

                if (result != null)
                {
                    List<WsS2GuidModel> lstResult = new List<WsS2GuidModel>();

                    foreach (var item in result)
                    {
                        lstResult.Add(new WsS2GuidModel
                        {
                            ClientName = item.ClientName,
                            ClientNumber = item.ClientNumber,
                            Description = item.Description,
                            EngName = item.EngagementName,
                            EngNum = item.EngagementNumber,
                            FileName = item.FileName,
                            MAFGUID = item.MafGuid,
                            Manager = item.Manager,
                            MasterAuditFileId = item.MafId,
                            Partner = item.Partner,
                            WBDescription = item.WbDescription,
                            WBStatus = item.WbStatus,
                            WorkbookName = item.WorkbookName
                        });
                    }

                    return lstResult;
                }
                else
                {
                    throw new Exception(string.Format("Guid [{0}] does not exists on Server 2", mafGuid));
                }
            }
            else
            {
                throw new Exception(string.Format("This eAudIT Engagement ID of the MAF on Server 2 is already linked to workspace {0}", engNum));
            }
        }

        private static string Get_Linked_Ws_ForMafGuid(Guid mafGuid)
        {
            using (var db = new S2DbContext())
            {
                var result = db.MasterAuditFile.Where(x => x.MAFGuid == mafGuid.ToString() 
                    && x.IsActive 
                    && !(x.EngagementNumber == null || x.EngagementNumber.Trim() == string.Empty))
                    .Select(y => y.EngagementNumber).FirstOrDefault();

                return result;
            }
        }

        private static bool ValidateIsEngS2(string engNum)
        {
            using (var db = new S2DbContext())
            {
                return db.MasterAuditFile.Where(x => x.EngagementNumber == engNum && x.IsActive).Count() > 0;
            }
        }

        private static void UpdateWs(string wsId, bool isS2)
        {
            var ws = AuditManager.Rep.Workspace.GetWs(wsId)[0];
            ws.WsProfile.IsServer2 = isS2;

            AuditManager.Rep.WsOperation.UpdateWs(new WsUpdateModel { WsModel = ws, PreservationComment = string.Format("IsServer2 set to {0} during Server 2 Guid Map.", isS2) }, UpdateProfileFrom.S2);
        }

        public static bool LinkMAF(Post_WsS2GuidModel post_WsS2GuidModel)
        {
            using (var db = new S2DbContext())
            {
                SqlParameter p1 = new SqlParameter("@MAFGuid", post_WsS2GuidModel.MafGuid);
                SqlParameter p2 = new SqlParameter("@EngagementNumber", post_WsS2GuidModel.EngNum);
                SqlParameter p3 = new SqlParameter("@EngagementName", post_WsS2GuidModel.EngName);
                SqlParameter p4 = new SqlParameter("@ClientNumber", post_WsS2GuidModel.Client);
                SqlParameter p5 = new SqlParameter("@ClientName", post_WsS2GuidModel.ClientDesc);
                SqlParameter p6 = new SqlParameter("@UpdatedBy", AmUtil.GetCurrentUser);
                SqlParameter p7 = new SqlParameter("@PartnerEmailId", post_WsS2GuidModel.PartnerEmail);
                SqlParameter p8 = new SqlParameter("@ManagerEmailId", post_WsS2GuidModel.ManagerEmail);
                
                db.Database.ExecuteSqlCommand("S2CLR.spMapEngagementToGuid @MAFGuid, @EngagementNumber, @EngagementName, @ClientNumber, @ClientName, @UpdatedBy, @PartnerEmailId, @ManagerEmailId", 
                    p1, p2, p3, p4, p5, p6, p7, p8);
            }

            if (!post_WsS2GuidModel.IsS2)
                UpdateWs(post_WsS2GuidModel.WsId, true);

            IM.Mgr.WsUtility.SaveGuidInfo_Link(post_WsS2GuidModel.MafGuid, post_WsS2GuidModel.EngNum);

            return true;
        }

        public static bool DeLinkMAF(Post_WsS2GuidModel post_WsS2GuidModel)
        {
            using (var db = new S2DbContext())
            {
                var original = db.MasterAuditFile.Find(post_WsS2GuidModel.MasterAuditFileId);

                original.IsActive = false;
                original.UpdateDate = DateTime.Now;
                original.UpdatedBy = AmUtil.GetCurrentUser;
                //original.EngagementNumber = null;//DBNull.Value;

                db.SaveChanges();
            }

            WsActivity.CallEmailActivities("UnlinkingRequest", "UnlinkingRequest_10",
                             "0", post_WsS2GuidModel.MasterAuditFileId,
                             string.Format("{0}#%1#{1}#%1#{2}", post_WsS2GuidModel.Comment, post_WsS2GuidModel.EngNum, post_WsS2GuidModel.EngName));

            var isEngS2 = ValidateIsEngS2(post_WsS2GuidModel.EngNum);

            if (!isEngS2)
                UpdateWs(post_WsS2GuidModel.WsId, false);

            IM.Mgr.WsUtility.SaveGuidInfo_DeLink(post_WsS2GuidModel.MafGuid, post_WsS2GuidModel.EngNum, 
                post_WsS2GuidModel.Comment, post_WsS2GuidModel.MasterAuditFileId);

            return isEngS2;
        }
    }
}
