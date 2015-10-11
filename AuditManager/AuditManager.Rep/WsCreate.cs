using AuditManager.Common;
using AuditManager.EF.AmDbCtx;
using AuditManager.Model;
using AuditManager.Model.EFModel.Active;
using AuditManager.Model.EFModel.AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;


namespace AuditManager.Rep
{
    public class WsCreate
    {
        public static void RequestAccessToAuditManager(string comment, bool toIncludeCurrentUser)
        {
            //Task task = new Task(() => AmUtil.SendMail_RequestAccessToAuditManager(comment, toIncludeCurrentUser));
            //task.Start();

            AmUtil.SendMail_RequestAccessToAuditManager(comment, toIncludeCurrentUser);

            //AmUtil.SendMail_RequestAccessToAuditManager(comment);
        }

        public static void RequestAccess(string engNum, WsUserType wsUserType, string emailUsers, string comment = null)
        {
            var dtNow = DateTime.Now;

            var wsModel = Workspace.GetEngByEngNum(engNum, WsLoadType.Groups, true).FirstOrDefault();
            var recepients = Workspace.GetEmailRecepientsForEng(wsModel, EmailRecepientType.ADMIN, true);

            emailUsers = emailUsers.ToUpper();
            var sp_emailUsers = emailUsers.Split(',').ToList();

            var filterRecepients = recepients.Where(x => (sp_emailUsers.Contains((x.Item2 + "@kpmg.com").ToUpper()) || (x.Item2.Equals(AuditManager.Common.AmUtil.GetCurrentUser, StringComparison.OrdinalIgnoreCase)))).ToList();

            var mailText = AmUtil.SendMail_RequestAccess(WsActivityType.Workspace_RequestAccess.ToString(), wsModel.WsProfile.EngNum, wsModel.Name, filterRecepients, wsUserType.ToString(), comment);
            var recepientArray = string.Join(",", filterRecepients.Select(x => x.Item2).ToList());

            //INSERT INTO [AuditManager].[dbo].[WsActivity] VALUES (25, 'Workspace')
            //INSERT INTO [AuditManager].[dbo].[WsActivity] VALUES (26, 'Workspace_RequestAccess')

            using (var db = new AmDbContext())
            {
                var wsActivityHistory = db.WsActivityHistory.Add(
                    new WsActivityHistory
                    {
                        EngNum = engNum,
                        UserId = AmUtil.GetCurrentUser,
                        WsActivityTypeId = WsActivityType.Workspace,
                        WsSubActivityTypeId = WsActivityType.Workspace_RequestAccess,
                        Comment = comment,
                        WsActivityInfo = wsUserType.ToString(),
                        DateCreated = dtNow
                    }
                    );

                db.SaveChanges();

                var wsMail = db.WsMail.Add(new WsMail
                {
                    WsActivityHistoryId = wsActivityHistory.WsActivityHistoryId,
                    WsMailStatusTypeId = WsMailStatusType.Success,
                    MailText = mailText,
                    RecepientArray = recepientArray
                });

                db.SaveChanges();
            }
        }

        public static bool DoesUserHaveAccessToEng(string engNum)
        {
            var userWithWsUserType = IM.Mgr.WsUtility.GetAllWsForUserWithWsUserType(null);

            List<Model.WsUserType> usrType = null;

            var yes = userWithWsUserType.TryGetValue(engNum, out usrType);

            return yes;
        }

        public static List<dynamic> GetEngDetails(string engNum)
        {
            using (var db = new RefDbContext())
            {
                using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    //var xRes = (from c in db.Set<Engagement>()
                    //           where c.EngagementNumber.StartsWith(engNum)
                    //            where c.Manager == null
                    //            where c.Partner == null
                    //            //where c.Manager.DisplayName == "vivek"
                    //            //where c.EPEmployeeId != null
                    //           select c).Take(10).ToList();

                    var result = db.Set<AuditManager.Model.EFModel.Ref.Engagement>().Where
                        (
                            x =>
                                (
                                x.EngagementNumber.StartsWith(engNum)
                                && !(x.EMEmployeeId == null || x.EMEmployeeId.Trim() == string.Empty)
                                && !(x.EPEmployeeId == null || x.EPEmployeeId.Trim() == string.Empty)
                                //&& x.Manager == null
                                //&& x.Partner == null
                                //&& x.EMEmployeeId == "2586527"
                                //&& x.EPEmployeeId == "2499942"
                            )
                            )
                            .ToList();

                    //var result2 = result.Where(x => x.Manager != null && x.Partner != null).Take(10).ToList();

                    var result2 = result.Take(10).ToList();

                    List<dynamic> retList = new List<dynamic>();

                    foreach (AuditManager.Model.EFModel.Ref.Engagement e in result2)
                    {
                        retList.Add(

                        new AuditManager.Model.WsCreate
                        {
                            EngagementNumber = e.EngagementNumber,
                            EngagementDescription = e.EngagementDescription,

                            ClientId = e.Customer == null ? "" : e.Customer.CustomerNumber,
                            ClientName = e.Customer == null ? "" : e.Customer.CustomerName,

                            ManagerId = e.Manager == null ? "" : e.Manager.SamAccountName,
                            ManagerEmpId = e.Manager == null ? "" : e.Manager.EmplID,
                            ManagerName = e.Manager == null ? "" : e.Manager.DisplayName,
                            ManagerFunction = e.Manager == null ? "" : e.Manager.Function == null ? "" : e.Manager.Function.FunctionName,

                            PartnerId = e.Partner == null ? "" : e.Partner.SamAccountName,
                            PartnerEmpId = e.Partner == null ? "" : e.Partner.EmplID,
                            PartnerName = e.Partner == null ? "" : e.Partner.DisplayName,
                            PartnerFunction = e.Partner == null ? "" : e.Partner.Function == null ? "" : e.Partner.Function.FunctionName,

                            ParentCompany = e.Customer == null ? "" : e.Customer.SentinelId,

                            PartnerAssistanceId = e.AdminAssistant == null ? "" : string.IsNullOrWhiteSpace(e.AdminAssistant.AssistantEmployeeId) ? "" :
                            e.AdminAssistant.Assistant == null ? "" : e.AdminAssistant.Assistant.SamAccountName,
                            PartnerAssistanceEmpId = e.AdminAssistant == null ? "" : string.IsNullOrWhiteSpace(e.AdminAssistant.AssistantEmployeeId) ? "" :
                            e.AdminAssistant.Assistant == null ? "" : e.AdminAssistant.Assistant.EmplID,
                            PartnerAssistanceName = e.AdminAssistant == null ? "" : string.IsNullOrWhiteSpace(e.AdminAssistant.AssistantEmployeeId) ? "" :
                            e.AdminAssistant.Assistant == null ? "" : e.AdminAssistant.Assistant.DisplayName
                        });
                    }

                    return retList;
                }
            }
        }

        public static void GetEngNumWhereC4OrC6IsNull()
        {
            List<DOCMASTER> docMaster = null;

            using (new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                using (var db = new ActiveDbContext())
                {
                    //using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    //{ 


                    //}
                    docMaster = db.DOCMASTER.Where(x => (x.C6ALIAS == null) &&
                        (x.SUBCLASS_ALIAS.Equals("AUDIT", StringComparison.OrdinalIgnoreCase) ||
                        x.SUBCLASS_ALIAS.Equals("EAUDIT ENGAGEMENT", StringComparison.OrdinalIgnoreCase))).ToList();
                }
            }

            using (var db = new ActiveDbContext())
            {
                foreach (var dM in docMaster)
                {
                    dM.C6ALIAS = dM.C4ALIAS;
                }

                db.SaveChanges();
            }
        }

        public static void CreateWS(AuditManager.Model.WsCreate wsCreate)
        {
            //IM.Mgr.WsUtility.CreateWS(wsCreate)

            //using (new TransactionScope(
            //        TransactionScopeOption.Required,
            //        new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            //{
            using (var db = new ActiveDbContext())
            {
                var p = wsCreate.PartnerId.ToUserIdFromDnsName();
                var m = wsCreate.ManagerId.ToUserIdFromDnsName();

                var c4 = db.CUSTOM4.Where(x => x.CUSTOM_ALIAS.Equals(p)).FirstOrDefault();
                if (c4 == null)
                {
                    var p_db = WsUsrMgmt.SearchUsr(p, Model.UsrSearchBy.Email, true);

                    if (p_db != null && p_db.Count > 0)
                    {
                        var p_2_In = p_db.FirstOrDefault();

                        var p_In = db.CUSTOM4.Add(new CUSTOM4
                        {
                            CUSTOM_ALIAS = p_2_In.Name,
                            C_DESCRIPT = p_2_In.FullName,
                            EDITWHEN = DateTime.Now,
                            ENABLED = "Y",
                            IS_HIPAA = "N"
                        });

                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception(string.Format("Failure creating workspace: Partner => {0} not defined in [DOCUSERS] table.", p));
                    }
                }

                var c6 = db.CUSTOM6.Where(x => x.CUSTOM_ALIAS.Equals(m)).FirstOrDefault();
                if (c6 == null)
                {
                    var m_db = WsUsrMgmt.SearchUsr(m, Model.UsrSearchBy.Email, true);

                    if (m_db != null && m_db.Count > 0)
                    {
                        var m_2_In = m_db.FirstOrDefault();

                        var m_In = db.CUSTOM6.Add(new CUSTOM6
                        {
                            CUSTOM_ALIAS = m_2_In.Name,
                            C_DESCRIPT = m_2_In.FullName,
                            EDITWHEN = DateTime.Now,
                            ENABLED = "Y",
                            IS_HIPAA = "N"
                        });

                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception(string.Format("Failure creating workspace: Manager => {0} not defined in [DOCUSERS] table.", m));
                    }
                }
            }
            //}

            //var cur_Eng = AuditManager.Rep.Workspace.GetEngByEngNum(wsCreate.EngagementNumber, Model.WsLoadType.None, true);

            //if (cur_Eng == null
            //    || cur_Eng.Count == 0)
            //{
            //    IM.Mgr.WsUtility.CreateWS(wsCreate);
            //    AuditManager.Rep.WsOperation.UpdateWsProfile(wsCreate.EngagementNumber, wsCreate.WsProfile_TP, Model.UpdateProfileFrom.CreateWs);
            //    return;
            //}
            //else
            //{
            //    throw new Exception(string.Format("Workspace => {0} already exists.", wsCreate.EngagementNumber));
            //}

            IM.Mgr.WsUtility.CreateWS(wsCreate);

            //AuditManager.Rep.WsOperation.UpdateWsProfile(wsCreate.EngagementNumber, wsCreate.WsProfile_TP, Model.UpdateProfileFrom.CreateWs);

            return;
        }
    }
}


//SELECT DISTINCT 
//        Ref.Customer.CustomerNumber AS ClientID,
//        Ref.Customer.CustomerName AS [Client Name],

//        Ref.Engagement.EngagementNumber AS EngagementID,
//        Ref.Engagement.EngagementDescription AS [Engagement Name],

//        RIGHT(DRMSUsers_1.SamAccountName, LEN(DRMSUsers_1.SamAccountName) - 3) AS [Business Owner],
//        DRMSUsers_1.DisplayName AS [Business Owner Name],
//        RIGHT(Ref.DRMSUsers.SamAccountName, LEN(Ref.DRMSUsers.SamAccountName) - 3) AS Manager,
//        DRMSUsers.DisplayName as [Manager Name],

//        ISNULL(Ref.[Function].FunctionName, 'AUDIT') AS [Function],
//        Ref.Customer.SentinelId  AS [Parent Company],

//        RIGHT(DRMSUsers_2.SamAccountName, LEN(DRMSUsers_2.SamAccountName) - 3) AS AdminUserID 

//FROM Ref.Engagement 
//    INNER JOIN Ref.Customer WITH (NOLOCK) 
//    ON  Ref.Engagement.CustomerNumber = Ref.Customer.CustomerNumber 
//    LEFT OUTER JOIN Ref.DRMSUsers as DRMSUsers_1 ON ISNULL(DRMSUsers_1.EmplID, '') <> '' 
//    AND DRMSUsers_1.EmplID = Ref.Engagement.EPEmployeeId 
//    LEFT OUTER JOIN Ref.DRMSUsers WITH (NOLOCK) 
//    ON Ref.DRMSUsers.EmplID = Ref.Engagement.EMEmployeeId 

//    LEFT OUTER JOIN Ref.[Function] WITH (NOLOCK) 
//    ON Ref.[Function].FucntionID = DRMSUsers_1.GoFunction 
//    LEFT OUTER JOIN Ref.AdminAssistant WITH (NOLOCK) 
//    ON Ref.Engagement.EPEmployeeId = Ref.AdminAssistant.PartnerEmployeeID 			
//    LEFT OUTER JOIN Ref.DRMSUsers as DRMSUsers_2 ON ISNULL(DRMSUsers_2.EmplID, '') <> ''
//    AND DRMSUsers_2.EmplID = Ref.AdminAssistant.AssistantEmployeeId 	
//WHERE ISNULL(Ref.DRMSUsers.EmplID, '') <> '' 
//AND Ref.Engagement.EngagementNumber IS NOT NULL 
//AND Ref.Engagement.EngagementNumber like '6666666860%' 
//ORDER BY Ref.Engagement.EngagementNumber


