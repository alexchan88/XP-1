using AuditManager.Common;
using AuditManager.EF.AmDbCtx;
using AuditManager.Model;
using AuditManager.Model.EFModel.AM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace AuditManager.Rep
{
    public class WsOperation
    {
        //Create,RET,RF
        //public static void UpdateWsProfile(string engNum, WsProfile_TP wsProfile_TP, UpdateProfileFrom updateProfileFrom)
        public static void UpdateWsProfile(string engNum, UpdateProfileFrom updateProfileFrom)
        {
            //IM.Mgr.WsUtility.UpdateWsProfile(engNum, wsProfile_TP, updateProfileFrom);
            IM.Mgr.WsUtility.UpdateWsProfile(engNum, updateProfileFrom);
        }

        //Profile,S2
        public static List<WsModel> UpdateWs(WsUpdateModel wsUpdateModel, UpdateProfileFrom updateProfileFrom)
        {
            return IM.Mgr.WsOperation.UpdateWs(wsUpdateModel, updateProfileFrom);
        }

        public static dynamic GetClosurePastDue(int days, WsActivityType mailTypeEnum)
        {
            using (var db = new AmDbContext())
            {
                using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    var result = db.Set<WsActivityHistory>().Where(
                        x => (
                            x.WsActivityTypeId == WsActivityType.CLOSURE &&
                            x.WsSubActivityTypeId == mailTypeEnum &&
                            x.DateCreated <= DbFunctions.AddDays(DateTime.Now, -(days))
                            )).Select(x => new
                            {
                                EngNum = x.EngNum
                                //, RecepientArray = x 
                            }).ToList();

                    var dynamicResult = result.Select(x => x.ToDynamic()).ToList();

                    return dynamicResult;
                }
            }
        }

        public static dynamic GetClosureComment(string engNum)
        {
            using (var db = new AmDbContext())
            {
                using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    var result = db.Set<WsActivityHistory>().Where(
                        x => (
                            x.EngNum.Equals(engNum, StringComparison.OrdinalIgnoreCase) &&
                            x.WsActivityTypeId == WsActivityType.CLOSURE &&
                            x.WsSubActivityTypeId == WsActivityType.CLOSURE_INITIATE &&
                            !(x.Comment == null || x.Comment.Trim() == string.Empty)
                            ))
                        .Select(x => new { Comment = x.Comment, UserId = x.UserId, DateCreated = x.DateCreated }).ToList();

                    var dynamicResult = result.Select(x => x.ToDynamic()).ToList();

                    return dynamicResult;
                }
            }
        }

        public static void SurveyRequest(WsSurveyModel wsSurveyModel)
        {
            using (var db = new SSCDbContext())
            {
                var lstParamName = new List<string>();
                var lstSqlParameter = new List<SqlParameter>();

                var lstSqlParam = new List<KeyValuePair<string, SqlParameter>>();

                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(wsSurveyModel.GetType()))
                {
                    var altPropName = property.Attributes.OfType<AltPropName>().FirstOrDefault();

                    if (altPropName != null)
                    {
                        lstParamName.Add(altPropName.Name);
                        lstSqlParameter.Add(new SqlParameter(altPropName.Name, property.GetValue(wsSurveyModel) ?? DBNull.Value));

                        lstSqlParam.Add(new KeyValuePair<string, SqlParameter>(
                                altPropName.Name,
                                new SqlParameter(altPropName.Name, property.GetValue(wsSurveyModel) ?? DBNull.Value)
                            ));
                    }
                }

                lstParamName.Add("@ActivityList");

                DataTable tblActivities = new DataTable();
                tblActivities.Columns.Add("ActivityName", typeof(string));
                tblActivities.Columns.Add("WorkbookNum", typeof(int));
                tblActivities.Columns.Add("WorkbookName", typeof(string));
                tblActivities.Columns.Add("RequiredDate", typeof(DateTime));

                if (wsSurveyModel.SurveyRequestType == SurveyRequestType.RF)
                {
                    //Error - More than one RF requested.
                    tblActivities.Rows.Add(new object[] { wsSurveyModel.SurveyRequestType.ToString(), 1, "", wsSurveyModel.RequiredDate });
                }
                else if (wsSurveyModel.SurveyRequestType == SurveyRequestType.RET)
                {
                    //Error - Workbook Name cannot be null/blank
                    int idx = 1;
                    wsSurveyModel.WorkBooks.Split(',').ToList().ForEach
                        (x => tblActivities.Rows.Add(new object[] { "PDF", idx++, x, wsSurveyModel.RequiredDate }));
                }

                var sqlParamActivityList = new SqlParameter("@ActivityList", SqlDbType.Structured);
                sqlParamActivityList.Value = tblActivities;
                sqlParamActivityList.TypeName = "udtTblSurveyActivityList";

                lstSqlParameter.Add(sqlParamActivityList);

                lstSqlParam.Add(new KeyValuePair<string, SqlParameter>(
                                "@ActivityList",
                                sqlParamActivityList
                            ));

                //
                lstParamName.Add("@Comments");
                //Comments
                //SequenceNumber
                DataTable tblComments = new DataTable();
                tblComments.Columns.Add("SequenceNumber", typeof(int));
                tblComments.Columns.Add("Comments", typeof(string));
                
                if (wsSurveyModel.SurveyRequestType == SurveyRequestType.RF)
                {
                    if (wsSurveyModel.IsRFInDiffWF.ToBool<char>())
                    {
                        tblComments.Rows.Add(new object[] { 1, "IsRFInDiffWF:CHANGE OF WORKFLOW – ADD “FOR GDC USE” IN THE FILE NAME" });
                    }

                    if (wsSurveyModel.IsPartilaRF.ToBool<char>())
                    {
                        tblComments.Rows.Add(new object[] { 2, "IsPartilaRF:PARTIAL RF – [" + wsSurveyModel.RFModificationType + "]" });
                    }

                    if (wsSurveyModel.IsSawEng)
                    {
                        tblComments.Rows.Add(new object[] { 3, "IsSawEng:SAW Engagement – Yes" });
                    }
                    else
                    {
                        tblComments.Rows.Add(new object[] { 3, "IsSawEng:SAW Engagement – No" });
                    }
                }

                var sqlParamComments = new SqlParameter("@Comments", SqlDbType.Structured);
                sqlParamComments.Value = tblComments;
                sqlParamComments.TypeName = "udtTblSurveyComments";
                lstSqlParameter.Add(sqlParamComments);

                lstSqlParam.Add(new KeyValuePair<string, SqlParameter>(
                                "@Comments",
                                sqlParamComments
                            ));
                //
                //SELECT @NSTID = ISNULL(MAX(SurveyRowId) + 1, '1') FROM Survey (NOLOCK)
                //INSERT INTO SurveyImport
                //SELECT @SurveyRowId = @@IDENTITY
                //SELECT @ActvityId = ActivityId FROM Activities WHERE ActivityType = 'SURVEYIMPORT' AND Enabled = 'Y';
                //SELECT @EmailActvityId = ActivityId FROM Activities WHERE ActivityType = 'SURVEYPDFEMAIL' AND Enabled = 'Y';
                //SELECT @EmailActvityId = ActivityId FROM Activities WHERE ActivityType = 'SURVEYRFEMAIL' AND Enabled = 'Y';
                //INSERT INTO FileActivities --Insert SURVEYIMPORT Activity for the Survey Records 
                //INSERT INTO FileActivities --Insert SURVEY RF/PDF EMAIL Activity for the Survey Record
                //INSERT INTO Survey
                //INSERT INTO SurveyImportHistory
                //DELETE FROM SurveyImport

                //RET--
                //INSERT INTO SurveyWorkbooks -- Check if WoorkName exists for SurveyId - and increment the WB number
                //Exec spProcessSIActivity @SurveyRowId, @SurveyRowId -- Crazy

                //db.Database.ExecuteSqlCommand("EXEC spRequestSurvey " + string.Join(",", lstParamName), lstSqlParameter.ToArray());
                //db.Database.ExecuteSqlCommand("EXEC uspRequestSurvey " + string.Join(",", lstParamName), lstSqlParameter.ToArray());


                db.Database.ExecuteSqlCommand("EXEC uspRequestSurvey " + string.Join(",", lstSqlParam.Select(x => x.Key)), lstSqlParam.Select(x => x.Value).ToArray());
            }
        }

        //public static WsFile DeleteDoc(string wsId, string wsLogFldrID, string docObjId, string comment)
        public static WsFile DeleteDoc(string wsId, string docObjId, string comment, bool toValidate = true)
        {
            var wsFile = IM.Mgr.WsOperation.DeleteDoc(wsId, docObjId, comment, toValidate);

            if ((wsFile != null) && (wsFile.WsFileType == WsFileType.Eng || wsFile.WsFileType == WsFileType.Ret))
            {
                //try
                //{
                    UpdateDocRemoveStatus(wsFile);
                //}
                //catch (Exception ex)
                //{
                //    DbLog.LogError("", ex);
                //}
            }

            return wsFile;
        }

        private static void UpdateDocRemoveStatus(WsFile wsFile)
        {

            //For Server 2
            using (var db = new S2DbContext())
            {
                SqlParameter p1 = new SqlParameter("@DocumentNumber", wsFile.Number);

                try
                {
                    db.Database.ExecuteSqlCommand("EXEC [S2CLR].spUpdateDRMSRemoveStatus @DocumentNumber", p1);
                }
                catch(SqlException ex)
                {
                    ex = null;
                }
            }

            //For SSC
            using (var db = new SSCDbContext())
            {
                SqlParameter p1 = new SqlParameter("@DocNumber", wsFile.Number);
                SqlParameter p2 = new SqlParameter("@IsRET", wsFile.WsFileType == WsFileType.Ret);
                SqlParameter p3 = new SqlParameter("@UserId", AmUtil.GetCurrentUser);

                try
                {
                    db.Database.ExecuteSqlCommand("EXEC spRemoveDRMSDocument @DocNumber, @IsRET, @UserId", p1, p2, p3);
                }
                catch (SqlException ex)
                {
                    ex = null;
                }
            }

            //if (wsFile.IsServer2)
            //{
            //    using (var db = new S2DbContext())
            //    {
            //        SqlParameter p1 = new SqlParameter("@DocumentNumber", wsFile.Number);

            //        db.Database.ExecuteSqlCommand("EXEC [S2CLR].spUpdateDRMSRemoveStatus @DocumentNumber", p1);
            //    }
            //}
            //else
            //{
            //    using (var db = new SSCDbContext())
            //    {
            //        SqlParameter p1 = new SqlParameter("@DocNumber", wsFile.Number);
            //        SqlParameter p2 = new SqlParameter("@IsRET", wsFile.WsFileType == WsFileType.Ret);
            //        SqlParameter p3 = new SqlParameter("@UserId", AmUtil.GetCurrentUser);

            //        db.Database.ExecuteSqlCommand("EXEC spRemoveDRMSDocument @DocNumber, @IsRET, @UserId", p1, p2, p3);
            //    }
            //}
        }

        public static List<WsModel> InitiateClosure(InitiateClosureWsModel initiateClosureWsModel)
        {
            IM.Mgr.WsOperation.InitiateClosure(initiateClosureWsModel);

            return AuditManager.Rep.Workspace.GetWs(initiateClosureWsModel.WsModel.ObjectID);
        }

        public static dynamic GetS2NonClosedWb(string engNum)
        {
            using (var db = new S2DbContext())
            {
                var result = db.MasterAuditFile.Where(x => x.EngagementNumber == engNum)
                            .Join(db.Workbook, t1 => t1.MasterAuditFileId, t2 => t2.MasterAuditFileId,
                            (t1, t2) => new { t2.WorkbookId, t2.WorkbookName, t2.StatusId })
                            .Join(db.Status, t3 => t3.StatusId, t4 => t4.StatusId,
                            (t3, t4) => new { WorkbookName = t3.WorkbookName, Status = t4.Status1 })
                            .Where(y => y.Status != "Closed")
                            .ToList();

                var dynamicResult = result.Select(x => x.ToDynamic()).ToList();

                return dynamicResult;

            }
        }

        public static List<int> GetRelated_S2_YrEnd_Audit_EngDoc_Num(double retFileNum)
        {
            using (var db = new S2DbContext())
            {
                var result = db.DRMSExportedFile.Where(x => x.WorkbookReviewId ==
                    (db.DRMSExportedFile.Where(y => y.DocumentNumber == retFileNum).Select(z => z.WorkbookReviewId).FirstOrDefault()))
                    .Where(x2 => x2.DRMSFolderPath == "2 - Period-end Audit >> ENG Files").Select(x3 => x3.DocumentNumber).ToList();

                return result;
            }
        }

        //if (retDocNum == 0)
        //        {
        //            //SELECT CAST(FA.FileNumber AS FLOAT) EngDocNum, CAST(DRMS.DocNumber AS FLOAT) RetDocNum FROM dbo.FileActivities FA (NOLOCK) 
        //            //JOIN DRMSPDFs DRMS (NOLOCK) ON FA.FAId = DRMS.FAId WHERE DRMS.FAId = 457847

        //            var result = db.Database.SqlQuery<AuditNonAuditModel>(
        //                    "SELECT CAST(FA.FileNumber AS FLOAT) EngDocNum, CAST(DRMS.DocNumber AS FLOAT) RetDocNum " +
        //                    "FROM dbo.FileActivities FA (NOLOCK) " +
        //                    "JOIN DRMSPDFs DRMS (NOLOCK) " +
        //                    "ON FA.FAId = DRMS.FAId " +
        //                    "WHERE DRMS.FAId = @FAID",
        //                    p2
        //                    ).ToList();

        //            return result;
        //        }
        //        else
        //        {
        //            //SELECT CAST(FA.FileNumber AS FLOAT) EngDocNum FROM dbo.FileActivities FA (NOLOCK)   
        //            //JOIN DRMSPDFs DRMS (NOLOCK) ON FA.FAId = DRMS.FAId WHERE DRMS.DocNumber = 488270

        //            var result = db.Database.SqlQuery<AuditNonAuditModel>(
        //                    "SELECT CAST(FA.FileNumber AS FLOAT) EngDocNum FROM dbo.FileActivities FA (NOLOCK) " +
        //                        "JOIN DRMSPDFs DRMS (NOLOCK) " +
        //                        "ON FA.FAId = DRMS.FAId " +
        //                        "WHERE DRMS.DocNumber = @DocNum",
        //                    p1
        //                    ).ToList();

        //            return result;
        //        }

        public static List<double?> GetRelated_SSC_YrEnd_Audit_EngDoc_Num(double retFileNum)
        {
            using (var db = new SSCDbContext())
            {
                var result = db.FileActivity.Join(db.DRMSPDF.Where(x => x.DocNumber == retFileNum), t1 => t1.FAId, t2 => t2.FAId, (t1, t2) => t1.FileNumber).ToList();

                return result;
            }
        }
    }
}
