using AuditManager.Common;
using AuditManager.EF.AmDbCtx;
using AuditManager.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace AuditManager.Rep
{
    public class WsActivity
    {
        public static List<EngDocByRetModel> GetENGDocByRETDocNumber_2(double retFileNum)
        {
            using (var db = new S2DbContext())
            {
                SqlParameter p1 = new SqlParameter("@DocumentNumber", retFileNum);

                var result = db.Database.SqlQuery<EngDocByRetModel>("EXEC [S2CLR].spGetENGDocByRETDocNumber @DocumentNumber"
                    , p1).ToList();

                return result;
            }
        }

        public static WsFile GetENGDocByRETDocNumber(double retFileNum)
        {
            using (var db = new S2DbContext())
            {
                SqlParameter p1 = new SqlParameter("@DocumentNumber", retFileNum);

                var result = db.Database.SqlQuery<EngDocByRetModel>("EXEC [S2CLR].spGetENGDocByRETDocNumber @DocumentNumber"
                    , p1).ToList();

                if (result != null && result.Count() > 0)
                {
                    var engDocNum = result.FirstOrDefault().DocumentNumber;

                    var wsFile = IM.Mgr.WsOperation.GetWsFile(engDocNum);

                    return wsFile;
                }
                else
                {
                    return null;
                }
            }
        }

        //public static void CallEmailActivities(string activityName, string templateName, int workbookReviewId, int masterAuditFileID, string comment)
        public static void CallEmailActivities(string activityName, string templateName, string workbookReviewId, int masterAuditFileID, string comment)
        {
            using (var db = new S2DbContext())
            {
                SqlParameter p1 = new SqlParameter("@ActivityName", activityName);
                SqlParameter p2 = new SqlParameter("@WorkbookReviewId", workbookReviewId);
                SqlParameter p3 = new SqlParameter("@MasterAuditFileID", masterAuditFileID);
                SqlParameter p4 = new SqlParameter("@TemplateName", templateName);
                SqlParameter p5 = new SqlParameter("@Priority", 1);
                SqlParameter p6 = new SqlParameter("@Comments", comment);
                SqlParameter p7 = new SqlParameter("@InsertBy", AmUtil.GetCurrentUser);

                db.Database.ExecuteSqlCommand("EXEC [S2CLR].spAddEmailActivities @ActivityName, @WorkbookReviewId, @MasterAuditFileID, @TemplateName, @Priority, @Comments, @InsertBy"
                    , p1, p2, p3, p4, p5, p6, p7);
            }
        }

        public static string UpdateFileActivity(FileActivity_UpdateModel activityUpdateModel)
        {
            string returnStatus = null;

            if (activityUpdateModel.FileIn == FileIn.S2)
            {
                using (var db = new S2DbContext())
                {
                    if (activityUpdateModel.WsActivityType == WsActivityType.Activity_Reprocess)
                    {
                        WsActivity.CallEmailActivities("RepreocessServer2RETFile", "ReprocessRequested_14",
                            activityUpdateModel.FileUniqueId, 0, activityUpdateModel.Comment);
                    }

                    if (activityUpdateModel.WsActivityType == WsActivityType.Activity_Acknowledge && activityUpdateModel.NonAuditFlag.HasValue
                        ? activityUpdateModel.NonAuditFlag.Value : false)
                    {
                        var relatedEngFiles = WsOperation.GetRelated_S2_YrEnd_Audit_EngDoc_Num(activityUpdateModel.FileNum);
                        relatedEngFiles.ForEach(x => Workspace.Move_YrEnd_Audit_RET_N_ENG_To_ElecWp(activityUpdateModel.FileNum, x, activityUpdateModel.EngNum));
                    }

                    SqlParameter p1 = new SqlParameter("@WorkbookReviewId", activityUpdateModel.FileUniqueId);
                    SqlParameter p2 = new SqlParameter("@Status", (activityUpdateModel.WsActivityType == WsActivityType.Activity_Acknowledge)
                        ? "Acknowledged" : "Removed");
                    SqlParameter p3 = new SqlParameter("@UpdatedBy", string.IsNullOrWhiteSpace(activityUpdateModel.logAs) ? AmUtil.GetCurrentUser : activityUpdateModel.logAs);

                    SqlParameter p4;
                    if (activityUpdateModel.NonAuditFlag.HasValue)
                    { p4 = new SqlParameter("@NonAuditFlag", activityUpdateModel.NonAuditFlag.Value); }
                    else
                    { p4 = new SqlParameter("@NonAuditFlag", DBNull.Value); }

                    SqlParameter p5 = new SqlParameter("@docNum", activityUpdateModel.FileNum);

                    db.Database.ExecuteSqlCommand("EXEC [S2CLR].spUpdateReviewStatus @WorkbookReviewId, @Status, @UpdatedBy, @NonAuditFlag, @docNum", p1, p2, p3, p4, p5);
                }

                var docStatus = AuditManager.Rep.Workspace.GetFileStatus_S2(new List<int> { (int)activityUpdateModel.FileNum });
                returnStatus = docStatus.FirstOrDefault().Status;
            }
            else if (activityUpdateModel.FileIn == FileIn.SSC)
            {
                if (activityUpdateModel.WsActivityType == WsActivityType.Activity_Acknowledge
                    || activityUpdateModel.WsActivityType == WsActivityType.Activity_Reprocess)
                {
                    using (var db = new SSCDbContext())
                    {
                        SqlParameter p1 = new SqlParameter("@FAId", activityUpdateModel.FileUniqueId);
                        SqlParameter p2 = new SqlParameter("@success", (activityUpdateModel.WsActivityType == WsActivityType.Activity_Acknowledge));

                        SqlParameter p3;
                        if (string.IsNullOrWhiteSpace(activityUpdateModel.Comment))
                        { p3 = new SqlParameter("@comments", DBNull.Value); }
                        else
                        { p3 = new SqlParameter("@comments", activityUpdateModel.Comment); }

                        SqlParameter p4 = new SqlParameter("@activityEndDate", DateTime.Now.ToUniversalTime());
                        SqlParameter p5 = new SqlParameter("@userId", string.IsNullOrWhiteSpace(activityUpdateModel.logAs) ? AmUtil.GetCurrentUser : activityUpdateModel.logAs);
                        SqlParameter p6 = new SqlParameter("@docNum", activityUpdateModel.FileNum);

                        db.Database.ExecuteSqlCommand("spCloseActivity @FAId, @success, @comments, @activityEndDate, @userId", p1, p2, p3, p4, p5);
                    }
                }
                else if (activityUpdateModel.WsActivityType == WsActivityType.Activity_Remove)
                {
                    using (var db = new SSCDbContext())
                    {
                        SqlParameter p1 = new SqlParameter("@FAId", activityUpdateModel.FileUniqueId);

                        SqlParameter p2;
                        if (string.IsNullOrWhiteSpace(activityUpdateModel.Comment))
                        { p2 = new SqlParameter("@comments", DBNull.Value); }
                        else
                        { p2 = new SqlParameter("@comments", activityUpdateModel.Comment); }

                        SqlParameter p3 = new SqlParameter("@userId", string.IsNullOrWhiteSpace(activityUpdateModel.logAs) ? AmUtil.GetCurrentUser : activityUpdateModel.logAs);
                        SqlParameter p4 = new SqlParameter("@docNum", activityUpdateModel.FileNum);

                        db.Database.ExecuteSqlCommand("spRemoveActivity @FAId, @comments, @userId", p1, p2, p3);
                    }
                }

                var docStatus = AuditManager.Rep.Workspace.GetFileStatus_SSC(new List<int> { (int)activityUpdateModel.FileNum });
                returnStatus = docStatus.FirstOrDefault().Status;
            }

            IM.Mgr.WsUtility.SaveActivityInfo(activityUpdateModel, returnStatus);
             
            return returnStatus;
        }

        public static List<FileActivityModel> GetActivity(DateTime? fDate, DateTime tDate, ActivityFilterType activityFilterType, string engNum = null, string usrId = null)
        {
            var userWithWsUserType = IM.Mgr.WsUtility.GetAllWsForUserWithWsUserType(usrId);

            var engNums = string.IsNullOrWhiteSpace(engNum) ? string.Join(",", userWithWsUserType.Select(x => x.Key).ToList()) : engNum;

            List<FileActivityModel> result = new List<FileActivityModel>();


            if (activityFilterType == ActivityFilterType.ALL
                || activityFilterType == ActivityFilterType.RET
                || activityFilterType == ActivityFilterType.RF)
            {
                var ssc_Activity = GetActivity_SSC(fDate, tDate, activityFilterType, engNums);

                ssc_Activity.ForEach(x =>
                {
                    x.WsUserType = userWithWsUserType.Where(y => y.Key == x.EngagementNumber).Select(z => z.Value).FirstOrDefault();

                    x.DocumentStatus = new DocumentStatus
                    {
                        ActionBy = x.ActionPerformedBy,
                        ActionDate = x.ActionPerformedOn,
                        DocumentNumber = x.DocNum,
                        DocumentType = x.RequestType,
                        FileIn = FileIn.SSC,
                        Status = x.Status,
                        UniqueId = x.FAID,
                    };
                });

                result.AddRange(ssc_Activity);
            }

            if (activityFilterType == ActivityFilterType.ALL
                || activityFilterType == ActivityFilterType.S2
                || activityFilterType == ActivityFilterType.S2_RET
                || activityFilterType == ActivityFilterType.S2_RF)
            {
                var s2_Activity = GetActivity_S2(fDate, tDate, activityFilterType, engNums);

                s2_Activity.ForEach(x =>
                    {
                        x.WsUserType = userWithWsUserType.Where(y => y.Key == x.EngagementNumber).Select(z => z.Value).FirstOrDefault();

                        x.DocumentStatus = new DocumentStatus
                        {
                            ActionBy = x.ActionPerformedBy,
                            ActionDate = x.ActionPerformedOn,
                            DocumentNumber = x.DocNum,
                            DocumentType = x.RequestType,
                            FileIn = FileIn.S2,
                            Status = x.Status,
                            UniqueId = x.FAID,
                        };
                    });

                result.AddRange(s2_Activity);
            }

            return result;
        }

        private static List<FileActivityModel> GetActivity_SSC(DateTime? fDate, DateTime tDate, ActivityFilterType activityFilterType, string engNums)
        {
            using (var db = new SSCDbContext())
            {
                SqlParameter p1;
                if (activityFilterType == ActivityFilterType.ALL)
                {
                    p1 = new SqlParameter("@ActivityType", DBNull.Value);
                }
                else
                {
                    p1 = new SqlParameter("@ActivityType", activityFilterType == ActivityFilterType.RET ? activityFilterType.ToEnumDesc() : activityFilterType.ToString());
                }

                SqlParameter p2 = new SqlParameter("@EngagementNumber", engNums);

                SqlParameter p3 = new SqlParameter("@From_Dt", fDate.GetValueOrDefault().ToUTCAdjustment());
                SqlParameter p4 = new SqlParameter("@To_Dt", tDate.ToUTCAdjustment());

                var result = db.Database.SqlQuery<FileActivityModel>("EXEC spGetAllActivitiesByEngagements @ActivityType, @EngagementNumber, @From_Dt, @To_Dt",
                    p1, p2, p3, p4).ToList();

                return result;
            }
        }

        private static List<FileActivityModel> GetActivity_S2(DateTime? fDate, DateTime tDate, ActivityFilterType activityFilterType, string engNums)
        {
            using (var db = new S2DbContext())
            {
                SqlParameter p1 = new SqlParameter("@ActivityType", DBNull.Value);
                SqlParameter p2 = new SqlParameter("@EngagementNumber", engNums);
                SqlParameter p3 = new SqlParameter("@From_Dt", fDate.GetValueOrDefault().ToUTCAdjustment());
                SqlParameter p4 = new SqlParameter("@To_Dt", tDate.ToUTCAdjustment());

                var result = db.Database.SqlQuery<FileActivityModel>("EXEC [S2CLR].spGetS2CActivitiesByEngagements @ActivityType, @EngagementNumber, @From_Dt, @To_Dt",
                    p1, p2, p3, p4).ToList();


                return result;
            }
        }
    }
}
