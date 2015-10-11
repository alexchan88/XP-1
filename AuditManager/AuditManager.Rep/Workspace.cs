using AuditManager.Common;
using AuditManager.EF.AmDbCtx;
using AuditManager.Model;
using AuditManager.Model.EFModel.Active;
using IM.Mgr;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Transactions;

namespace AuditManager.Rep
{
    public class Workspace
    {
        public static dynamic GetProjectIdForEngNum(List<string> engNums)
        {
            using (new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel =  System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                using (var db = new ActiveDbContext())
                {
                    var result = (
                            from dm in db.Set<DOCMASTER>()
                            join prj in db.Set<PROJECT>() on
                            dm.DOCNUM equals prj.DOCNUM
                            where
                            engNums.Contains(dm.C2ALIAS) &&
                            dm.C_ALIAS.Equals("WEBDOC", StringComparison.OrdinalIgnoreCase) &&
                            dm.DOCNAME.Equals("", StringComparison.OrdinalIgnoreCase)

                            select new { EngNum = dm.C2ALIAS, EngId = prj.PRJ_ID }).ToList();

                    return result;
                }
            }
        }

        private static List<string> PathToIgnore_NoRecord = new List<string>
        {
            "/4 - Roll Forward ENG Files",
            "/Workspace Log",
            "/6 - Backups",
        };

        public static List<DocumentStatus> GetFileStatus_SSC(List<int> fileNums)
        {
            if (fileNums.Count() == 0)
                return new List<DocumentStatus>();

            using (var db = new SSCDbContext())
            {
                SqlParameter p1 = new SqlParameter("@DocumentNumber", string.Join(",", fileNums));
                var lstResult = db.Database.SqlQuery<DocumentStatus>("EXEC spGetStatusByDocNumber @DocumentNumber", p1).ToList();
                return lstResult;
            }
        }

        public static List<DocumentStatus> GetFileStatus_S2(List<int> fileNums)
        {
            if (fileNums.Count() == 0)
                return new List<DocumentStatus>();

            using (var db = new S2DbContext())
            {
                SqlParameter p1 = new SqlParameter("@DocumentNumber", string.Join(",", fileNums));
                var lstResult = db.Database.SqlQuery<DocumentStatus>("EXEC [S2CLR].spGetStatusByDocNumber @DocumentNumber", p1).ToList();
                return lstResult;
            }
        }

        public static bool IsWSExists(string engNum)
        {
            var result = GetProjectIdForEngNum(new List<string> { engNum });

            if (result == null || result.Count == 0)
                return false;
            else
                return true;

            //using (new TransactionScope(
            //        TransactionScopeOption.Required,
            //        new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            //{
            //    using (var db = new ActiveDbContext())
            //    {
            //        //var result = db.Set<DOCMASTER>().Where(x => x.C2ALIAS.Equals(engNum, StringComparison.OrdinalIgnoreCase)).Take(1);

            //        //var result = db.Set<DOCMASTER>().Where(x => x.C2ALIAS.Equals(engNum, StringComparison.OrdinalIgnoreCase)).ToList();

            //        var result = db.Set<DOCMASTER>().Where(x => x.C2ALIAS.Equals(engNum, StringComparison.OrdinalIgnoreCase)).ToList();

            //        if (result == null || result.Count() == 0)
            //            return false;
            //        else
            //            return true;
            //    }
            //}
        }

        public static List<Tuple<string, string>> GetEmailRecepientsForEng(WsModel wsModel, EmailRecepientType emailRecepientType, bool includeRequester = true)
        {
            return IM.Mgr.WsUtility.GetEmailRecepients(wsModel, emailRecepientType, includeRequester);
        }

        public static List<Tuple<string, string>> GetEmailRecepientsForEngByEngNum(string engNum, EmailRecepientType emailRecepientType, bool includeRequester = true)
        {
            var wsModel = GetEngByEngNum(engNum, WsLoadType.Groups, true).FirstOrDefault();
            return IM.Mgr.WsUtility.GetEmailRecepients(wsModel, emailRecepientType, includeRequester);
        }

        public static List<WsModel> GetEngByEngNum(string engNum, WsLoadType wsLoadType, bool isAdmin = false)
        {
            var projectIdForEngNum = GetProjectIdForEngNum(new List<string> { engNum });

            if (projectIdForEngNum.Count == 0)
                return null;

            var wsId = WsUtility.GetWsObjectTypeId(WsObjectType.Workspace, projectIdForEngNum[0].EngId);
            //return GetWs(wsId);
            var result = IM.Mgr.Workspace.GetWs(wsId, wsLoadType, isAdmin: isAdmin);
            
            return result;
        }

        public static List<WsModel> GetEngByWsId(string wsId, WsLoadType wsLoadType, bool newSession = false)
        {
            //var result = IM.Mgr.Workspace.GetWs(wsId, wsLoadType);
            var result = GetWs(wsId, newSession);

            return result;
        }

        public static List<WsModel> GetWs(string wsId = null, bool newSession = false)
        {
            var result = IM.Mgr.Workspace.GetWs(wsId, WsLoadType.ALL, newSession);

            if (wsId != null)
            {
                List<WsFile> wsFiles = IM.Mgr.WsOperation.GetAllFilesFromWs(result.FirstOrDefault());

                List<int> allFileNums = wsFiles.Select(x => x.Number).ToList();

                var sscFileStatus = GetFileStatus_SSC(allFileNums);
                var sscFiles = sscFileStatus.Where(x => x.IsPresent.ToBool()).ToList();
                sscFiles.ForEach(x =>
                {
                    x.FileIn = FileIn.SSC;
                    wsFiles.First(y => y.Number == x.DocumentNumber.ToInt()).DocumentStatus = x;
                    //fileStatus.Add(x.DocumentNumber.ToInt(), x);
                });

                var s2FileNums = sscFileStatus.Where(x => !(x.IsPresent.ToBool())).Select(x => x.DocumentNumber.ToInt()).ToList();

                var s2FileStatus = GetFileStatus_S2(s2FileNums);
                var s2Files = s2FileStatus.Where(x => x.IsPresent.ToBool()).ToList();

                s2Files.ForEach(x =>
                {
                    x.FileIn = FileIn.S2;
                    wsFiles.First(y => y.Number == x.DocumentNumber.ToInt()).DocumentStatus = x;
                    //fileStatus.Add(x.DocumentNumber.ToInt(), x);
                });
            }

            return result;
        }
        
        public static void Move_YrEnd_Audit_RET_N_ENG_To_ElecWp(double retDocNum, double engDocNum, string engNum)
        {
            var projectIdForEngNum = GetProjectIdForEngNum(new List<string> { engNum });

            IM.Mgr.WsOperation.Move_YrEnd_Audit_RET_N_ENG_To_ElecWp(retDocNum, engDocNum, projectIdForEngNum[0].EngId);
        }

        public static dynamic GetNoRecordFiles(List<string> engNums)
        {
            var projectIdForEngNum = GetProjectIdForEngNum(engNums);

            dynamic noRcdEngFldrNFiles = new List<ExpandoObject>();

            foreach (var eng in projectIdForEngNum)
            {
                var fileWithPath = IM.Mgr.WsUtility.GetFileWithPath(WsUtility.GetWsObjectTypeId(WsObjectType.Workspace, eng.EngId));

                dynamic engNFldrs = new ExpandoObject();
                engNFldrs.EngNum = eng.EngNum;
                engNFldrs.Folders = new List<ExpandoObject>();

                foreach (var path in fileWithPath)
                {
                    var fldrPath = path.Key;

                    if (PathToIgnore_NoRecord.Exists(x => fldrPath.StartsWith(x, StringComparison.OrdinalIgnoreCase)))
                        continue;

                    if (path.Value == null || path.Value.Count == 0)
                        continue;

                    dynamic fldrNFiles = new ExpandoObject();
                    fldrNFiles.Path = fldrPath;
                    fldrNFiles.Files = new List<ExpandoObject>();

                    foreach (var file in path.Value)
                    {
                        if (file.IsRecord || file.IsDeleted)
                            continue;

                        string strFileDesc = file.Description;
                        string strFileExtn = file.Extn;
                        dynamic f = new ExpandoObject();
                        f.Number = file.Number;
                        f.Name = strFileDesc.FileNameWithExtn(strFileExtn);

                        fldrNFiles.Files.Add(f);
                    }

                    if (fldrNFiles.Files != null && fldrNFiles.Files.Count > 0)
                        engNFldrs.Folders.Add(fldrNFiles);
                }

                //if (engNFldrs.Folders != null && engNFldrs.Folders.Count > 0)
                noRcdEngFldrNFiles.Add(engNFldrs);
            }

            return noRcdEngFldrNFiles;
        }

        public static List<dynamic> GetNoRecordFiles_NIU(string wsId)
        {
            var fileWithPath = IM.Mgr.WsUtility.GetFileWithPath(wsId);

            var fWPs = new List<dynamic>();

            foreach (var path in fileWithPath)
            {
                var fldrPath = path.Key;

                if (PathToIgnore_NoRecord.Exists(x => fldrPath.StartsWith(x, StringComparison.OrdinalIgnoreCase)))
                    continue;

                if (path.Value == null || path.Value.Count == 0)
                    continue;

                var fWP = new
                {
                    Path = fldrPath,
                    Files = new List<dynamic>()
                };

                foreach (var file in path.Value)
                {
                    if (file.IsRecord || file.IsDeleted)
                        continue;

                    fWP.Files.Add(new
                    {
                        Number = file.Number,
                        Name = file.Description
                    });
                }

                if (fWP.Files != null && fWP.Files.Count > 0)
                    fWPs.Add(fWP);
            }

            return fWPs;
        }

        public static List<WsGroup> GetGrpNUser(ImDbType imDbType)
        {
            return WsUtility.GetGrpNUser(imDbType);
        }
    }
}
