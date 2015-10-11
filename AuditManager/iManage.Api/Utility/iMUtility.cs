using IManage;
using System;
using System.ComponentModel;
using System.Linq;
using System.Transactions;

namespace iManage.Api
{
    public class iMUtility
    {
        public static string IManageServer
        {
            get { return "useomapd1003"; }
        }

        public static double GetPrjIdForEngNum(string engNum)
        {
            using (new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                using (var db = new iMDbContext())
                {
                    //var result = db.Set<DOCMASTER>().Include("PROJECTS")
                    //    .Where(x => x.C2ALIAS.Equals(engNum, StringComparison.OrdinalIgnoreCase)
                    //    && x.C_ALIAS.Equals("WEBDOC", StringComparison.OrdinalIgnoreCase)
                    //    && x.DOCNAME.Equals(string.Empty, StringComparison.OrdinalIgnoreCase) 
                    //    ).FirstOrDefault().PROJECTS.FirstOrDefault().PRJ_ID;

                    var result = db.Set<PROJECT>().Include("DOCMASTER")
                        .Where(x => x.DOCMASTER.C2ALIAS.Equals(engNum, StringComparison.OrdinalIgnoreCase)
                        && x.DOCMASTER.C_ALIAS.Equals("WEBDOC", StringComparison.OrdinalIgnoreCase)
                        && x.DOCMASTER.DOCNAME.Equals(string.Empty, StringComparison.OrdinalIgnoreCase)
                        ).FirstOrDefault().PRJ_ID;

                    return result;
                }
            }
        }

        public static string GetObjectId(ObjectType objectType, IManDatabase db, double objNum)
        {
            switch (objectType)
            {
                case ObjectType.File:
                    return string.Format("{0}!{1}:{2},1:", db.ObjectID, objectType.ToEnumAttr<ObjectType, DescriptionAttribute>().Description, objNum);
                case ObjectType.Folder:
                    return string.Format("{0}!{1}:{2}:", db.ObjectID, objectType.ToEnumAttr<ObjectType, DescriptionAttribute>().Description, objNum);
                case ObjectType.Workspace:
                    return string.Format("{0}!{1}:{2}:", db.ObjectID, objectType.ToEnumAttr<ObjectType, DescriptionAttribute>().Description, objNum);
                default:
                    return null;
            }
        }

        public static void GetEngByEngNum(string engNum)
        {
            var prjId = GetPrjIdForEngNum(engNum);
            var us = new UserSession();
            var objId = GetObjectId(ObjectType.Workspace, us.DB(DbNameType.Active), prjId);

            //IManDMS imd = new ManDMS();
            //var e = (IManWorkspace)imd.GetObjectByID(objId);

            var eng = (IManWorkspace)us.Session().DMS.GetObjectByID(objId);

        }

        public static void GetObjectById(string objId)
        {

        }
    }

    public class WS
    {
        public DateTime CreationDate { get; set; }
        public DateTime DateModified { get; set; }
        public string Description { get; set; }
        public string EffectiveAccess { get; set; }
        public double FolderID { get; set; }
        public string ID { get; set; }
        public bool InUseBy { get; set; }
        public bool IsRoot { get; set; }
        public bool IsRootLevelFolder { get; set; }
        public string Name { get; set; }
        public string ObjectID { get; set; }
        public double Size { get; set; }
        public string SubType { get; set; }
        public string SubTypeEnum { get; set; }
        public double WorkspaceID { get; set; }
    }
}
