using AuditManager.Common;
using AuditManager.EF.AmDbCtx;
using AuditManager.Model;
using AuditManager.Model.EFModel.Active;
using AuditManager.Model.EFModel.AM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Hosting;

namespace AuditManager.Rep
{
    public class AuditManagerDb
    {
        public static List<CUSTOM4> GetTableData_C4(string tableName)
        {
            using (new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                using (var db = new ActiveDbContext())
                {
                    return db.CUSTOM4.ToList();
                }
            }
        }

        public static List<CUSTOM6> GetTableData_C6(string tableName)
        {
            using (new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                using (var db = new ActiveDbContext())
                {
                    return db.CUSTOM6.ToList();
                }
            }
        }

        public static List<WsActivityHistory> GetWsActivityHistory(string engNum, string usrId, WsActivityType wsActivityType, WsActivityType? wsActivitySubType = null)
        {
            using (var db = new AmDbContext())
            {
                using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    List<WsActivityHistory> actHist = null;

                    if (string.IsNullOrWhiteSpace(engNum) && string.IsNullOrWhiteSpace(usrId))
                    {
                        actHist = db.WsActivityHistory.Include("WsActivity").Include("WsSubActivity").Where(x =>
                            x.WsActivityTypeId == wsActivityType &&
                                //x.WsSubActivityTypeId == wsActivitySubType &&
                            x.EngNum.Equals(engNum, StringComparison.OrdinalIgnoreCase) &&
                            x.UserId.Equals(usrId, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                    else if (!string.IsNullOrWhiteSpace(engNum) && !string.IsNullOrWhiteSpace(usrId))
                    {
                        actHist = db.WsActivityHistory.Include("WsActivity").Include("WsSubActivity").Where(x =>
                            x.WsActivityTypeId == wsActivityType
                            //&& x.WsSubActivityTypeId == wsActivitySubType
                            ).ToList();
                    }
                    else if (!string.IsNullOrWhiteSpace(engNum))
                    {
                        actHist = db.WsActivityHistory.Include("WsActivity").Include("WsSubActivity").Where(x =>
                            x.WsActivityTypeId == wsActivityType &&
                                //x.WsSubActivityTypeId == wsActivitySubType &&
                            x.EngNum.Equals(engNum, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                    else if (!string.IsNullOrWhiteSpace(usrId))
                    {
                        actHist = db.WsActivityHistory.Include("WsActivity").Include("WsSubActivity").Where(x =>
                            x.WsActivityTypeId == wsActivityType &&
                                //x.WsSubActivityTypeId == wsActivitySubType &&
                            x.UserId.Equals(usrId, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    //return actHist.OrderByDescending(x => x.DateCreated).ThenByDescending(x => x.EngNum).ToList();

                    return actHist.OrderByDescending(x => x.DateCreated).ToList();

                }


            }
        }

        public static List<WsActivityHistory> GetClosureHistory(string engNum, string usrId, WsActivityType? wsActivityType = null, WsActivityType? wsActivitySubType = null)
        {

            using (var db = new AmDbContext())
            {
                using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    List<WsActivityHistory> actHist = null;

                    if (string.IsNullOrWhiteSpace(engNum) && string.IsNullOrWhiteSpace(usrId))
                    {
                        actHist = db.WsActivityHistory.Include("WsActivity").Include("WsSubActivity").Where(x =>
                            x.WsActivityTypeId == WsActivityType.CLOSURE &&
                            x.WsSubActivityTypeId == WsActivityType.CLOSURE_INITIATE &&
                            x.EngNum.Equals(engNum, StringComparison.OrdinalIgnoreCase) &&
                            x.UserId.Equals(usrId, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                    else if (!string.IsNullOrWhiteSpace(engNum) && !string.IsNullOrWhiteSpace(usrId))
                    {
                        actHist = db.WsActivityHistory.Include("WsActivity").Include("WsSubActivity").Where(x =>
                            x.WsActivityTypeId == WsActivityType.CLOSURE &&
                            x.WsSubActivityTypeId == WsActivityType.CLOSURE_INITIATE
                            ).ToList();
                    }
                    else if (!string.IsNullOrWhiteSpace(engNum))
                    {
                        actHist = db.WsActivityHistory.Include("WsActivity").Include("WsSubActivity").Where(x =>
                            x.WsActivityTypeId == WsActivityType.CLOSURE &&
                            x.WsSubActivityTypeId == WsActivityType.CLOSURE_INITIATE &&
                            x.EngNum.Equals(engNum, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                    else if (!string.IsNullOrWhiteSpace(usrId))
                    {
                        actHist = db.WsActivityHistory.Include("WsActivity").Include("WsSubActivity").Where(x =>
                            x.WsActivityTypeId == WsActivityType.CLOSURE &&
                            x.WsSubActivityTypeId == WsActivityType.CLOSURE_INITIATE &&
                            x.UserId.Equals(usrId, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    //return actHist.OrderByDescending(x => x.DateCreated).ThenByDescending(x => x.EngNum).ToList();

                    return actHist.OrderByDescending(x => x.DateCreated).ToList();
                }
            }
        }

        public static List<WsActivityHistory> GetClosureReport(DateTime fromD, DateTime toD, WsActivityType? wsActivityType = null, WsActivityType? wsActivitySubType = null)
        {

            using (var db = new AmDbContext())
            {
                using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    List<WsActivityHistory> actHist = null;

                    toD = toD.AddDays(1);

                    actHist = db.WsActivityHistory.Include("WsActivity").Include("WsSubActivity").Where(x =>
                            x.WsActivityTypeId == WsActivityType.CLOSURE &&
                            x.WsSubActivityTypeId == WsActivityType.CLOSURE_INITIATE &&
                            x.DateCreated >= fromD &&
                            x.DateCreated <= toD).ToList();


                    return actHist.OrderByDescending(x => x.DateCreated).ToList();
                }
            }
        }

        public static List<WsFloatingField> GetProfileHistory(string engNum, string usrId, WsActivityType? wsActivityType = null, WsActivityType? wsActivitySubType = null)
        {
            using (var db = new AmDbContext())
            {
                using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    List<WsFloatingField> profHist = null;

                    if (string.IsNullOrWhiteSpace(engNum) && string.IsNullOrWhiteSpace(usrId))
                    {
                        profHist = db.WsFloatingField.Where(x => x.EngNum.Equals(engNum, StringComparison.OrdinalIgnoreCase) &&
                            x.EnteredBy.Equals(usrId, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                    else if (!string.IsNullOrWhiteSpace(engNum) && !string.IsNullOrWhiteSpace(usrId))
                    {
                        profHist = db.WsFloatingField.ToList();
                    }
                    else if (!string.IsNullOrWhiteSpace(engNum))
                    {
                        profHist = db.WsFloatingField.Where(x => x.EngNum.Equals(engNum, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                    else if (!string.IsNullOrWhiteSpace(usrId))
                    {
                        profHist = db.WsFloatingField.Where(x => x.EnteredBy.Equals(usrId, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    //return profHist.OrderByDescending(x => x.EngNum).ThenByDescending(x => x.EnteredDate).ToList();

                    return profHist.OrderByDescending(x => x.EnteredDate).ToList();
                }
            }
        }

        public static List<WsFile> GetEngLog(string engNum)
        {

            var eng = AuditManager.Rep.Workspace.GetEngByEngNum(engNum, WsLoadType.Fldrs, isAdmin: true).FirstOrDefault();

            if (eng != null)
            {
                var fldr = eng.WsFldrs.Where(x => x.Name.Equals("Workspace Log", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (fldr != null)
                {
                    return fldr.WsFiles.OrderByDescending(x => x.Number).ToList();
                }
            }

            return null;
        }

        public static List<KeyValuePair<string, bool>> GetKPMGOnlyForAllEng_Monika()
        {
            var obj = File.ReadAllLines(HostingEnvironment.ApplicationPhysicalPath + "//ENG.txt").ToList().Distinct();

            List<KeyValuePair<string, bool>> kPMGOnlyForEng = new List<KeyValuePair<string, bool>>();

            List<KeyValuePair<string, bool?>> kPMGOnlyForEng_1 = new List<KeyValuePair<string, bool?>>();
            List<KeyValuePair<string, bool?>> kPMGOnlyForEng_2 = new List<KeyValuePair<string, bool?>>();
            List<KeyValuePair<string, bool?>> kPMGOnlyForEng_3 = new List<KeyValuePair<string, bool?>>();

            using (var db = new AmDbContext())
            {
                using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    var wsFloatingField = db.WsFloatingField.Where(x => x.IsActive).ToList();

                    wsFloatingField.ForEach(x =>
                    {
                        if (obj.Contains(x.EngNum))
                        {
                            if (!string.IsNullOrWhiteSpace(x.TPAns))
                            {
                                var tpAns = x.TPAns.Split('|');

                                var tp_1 = tpAns[0].Split('=')[1];
                                var tp_2 = tpAns[1].Split('=')[1];
                                var tp_3 = tpAns[2].Split('=')[1];

                                if (string.IsNullOrWhiteSpace(tp_1) || string.IsNullOrWhiteSpace(tp_2) || string.IsNullOrWhiteSpace(tp_3))
                                {
                                    kPMGOnlyForEng.Add(new KeyValuePair<string, bool>(x.EngNum, true));
                                    kPMGOnlyForEng_1.Add(new KeyValuePair<string, bool?>(x.EngNum, null));
                                }
                                else if (tp_1.ToBool<string>() && (string.Equals("NA", tp_2, StringComparison.OrdinalIgnoreCase) || tp_2.ToBool<string>() && !tp_3.ToBool<string>()))
                                {
                                    kPMGOnlyForEng.Add(new KeyValuePair<string, bool>(x.EngNum, false));
                                    kPMGOnlyForEng_2.Add(new KeyValuePair<string, bool?>(x.EngNum, false));
                                }
                                else
                                {
                                    kPMGOnlyForEng.Add(new KeyValuePair<string, bool>(x.EngNum, true));
                                    kPMGOnlyForEng_3.Add(new KeyValuePair<string, bool?>(x.EngNum, true));
                                }
                            }
                        }
                    }
                  );
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Workspace for which ThirdParty question was never answered, it only created in our system[Note: Workspace created after our new system implemented]");
            sb.AppendLine(string.Format("Count = {0}", kPMGOnlyForEng_1.Count));
            kPMGOnlyForEng_1.ForEach(x => sb.AppendLine(x.ToString()));

            sb.AppendLine("KPMG Flag = false/Contractor Allowed = true");
            sb.AppendLine(string.Format("Count = {0}", kPMGOnlyForEng_2.Count));
            kPMGOnlyForEng_2.ForEach(x => sb.AppendLine(x.ToString()));

            sb.AppendLine("KPMG Flag = true/Contractor Allowed = false");
            sb.AppendLine(string.Format("Count = {0}", kPMGOnlyForEng_3.Count));
            kPMGOnlyForEng_3.ForEach(x => sb.AppendLine(x.ToString()));

            return kPMGOnlyForEng;
        }

        public static List<KeyValuePair<string, bool>> GetKPMGOnlyForAllEng()
        {
            List<KeyValuePair<string, bool>> kPMGOnlyForEng = new List<KeyValuePair<string, bool>>();

            using (var db = new AmDbContext())
            {
                using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    var wsFloatingField = db.WsFloatingField.Where(x => x.IsActive).ToList();

                    wsFloatingField.ForEach(x =>
                    {
                        if (!string.IsNullOrWhiteSpace(x.TPAns))
                        {
                            var tpAns = x.TPAns.Split('|');

                            var tp_1 = tpAns[0].Split('=')[1];
                            var tp_2 = tpAns[1].Split('=')[1];
                            var tp_3 = tpAns[2].Split('=')[1];

                            if (string.IsNullOrWhiteSpace(tp_1) || string.IsNullOrWhiteSpace(tp_2) || string.IsNullOrWhiteSpace(tp_3))
                            {
                                kPMGOnlyForEng.Add(new KeyValuePair<string, bool>(x.EngNum, true));
                            }
                            else if (tp_1.ToBool<string>() && (string.Equals("NA", tp_2, StringComparison.OrdinalIgnoreCase) || tp_2.ToBool<string>() && !tp_3.ToBool<string>()))
                            {
                                kPMGOnlyForEng.Add(new KeyValuePair<string, bool>(x.EngNum, false));
                            }
                            else
                            {
                                kPMGOnlyForEng.Add(new KeyValuePair<string, bool>(x.EngNum, true));
                            }
                        }

                    }
                  );
                }
            }

            return kPMGOnlyForEng;
        }
    }
}
