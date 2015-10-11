using AuditManager.Common;
using AuditManager.Model;
using System;
using System.DirectoryServices;

namespace AuditManager.Rep
{
    public class AmUtility
    {
        public static KUsr GetKUsr(string userId)
        {
            
            var kUsr = new KUsr();

            if (string.IsNullOrWhiteSpace(userId))
                return kUsr;

            kUsr.UserId = userId;//AmUtil.GetCurrentUser;

            SearchResult result = null;

            using (DirectoryEntry AdEntry = ConfigUtility.IsAdTLogin() ?
                new DirectoryEntry(ConfigUtility.GetActiveDirectory()) :
                new DirectoryEntry(ConfigUtility.GetActiveDirectory(), ConfigUtility.GetAdLoginInfo().Item1, ConfigUtility.GetAdLoginInfo().Item2))
            {
                using (DirectorySearcher AdSearcher = new DirectorySearcher(AdEntry))
                {
                    AdSearcher.Filter = "(CN=" + kUsr.UserId + ")";
                    AdSearcher.PropertiesToLoad.Add(kUsr.GetPropAttr<KUsr, AltPropName>(x => x.Domain).Name);
                    AdSearcher.PropertiesToLoad.Add(kUsr.GetPropAttr<KUsr, AltPropName>(x => x.FName).Name);
                    AdSearcher.PropertiesToLoad.Add(kUsr.GetPropAttr<KUsr, AltPropName>(x => x.LName).Name);
                    AdSearcher.PropertiesToLoad.Add(kUsr.GetPropAttr<KUsr, AltPropName>(x => x.EmailId).Name);
                    //AdSearcher.PropertiesToLoad.Add(kUsr.GetPropAttr<KUsr, AltPropName>(x => x.UserInitials).Name);
                    AdSearcher.PropertiesToLoad.Add(kUsr.GetPropAttr<KUsr, AltPropName>(x => x.Location).Name);
                    AdSearcher.PropertiesToLoad.Add(kUsr.GetPropAttr<KUsr, AltPropName>(x => x.Department).Name);

                    result = AdSearcher.FindOne();
                }

                //AdEntry.Close();
            }

            if (result != null)
            {
                DirectoryEntry usrEntry = result.GetDirectoryEntry();

                kUsr.Domain = usrEntry.Properties[kUsr.GetPropAttr<KUsr, AltPropName>(x => x.Domain).Name].Value == null ? null :
                    usrEntry.Properties[kUsr.GetPropAttr<KUsr, AltPropName>(x => x.Domain).Name].Value.ToString();

                kUsr.FName = usrEntry.Properties[kUsr.GetPropAttr<KUsr, AltPropName>(x => x.FName).Name].Value == null ? null :
                    usrEntry.Properties[kUsr.GetPropAttr<KUsr, AltPropName>(x => x.FName).Name].Value.ToString();

                kUsr.LName = usrEntry.Properties[kUsr.GetPropAttr<KUsr, AltPropName>(x => x.LName).Name].Value == null ? null :
                    usrEntry.Properties[kUsr.GetPropAttr<KUsr, AltPropName>(x => x.LName).Name].Value.ToString();

                kUsr.EmailId = usrEntry.Properties[kUsr.GetPropAttr<KUsr, AltPropName>(x => x.EmailId).Name].Value == null ? null :
                    usrEntry.Properties[kUsr.GetPropAttr<KUsr, AltPropName>(x => x.EmailId).Name].Value.ToString();

                //kUsr.UserInitials = usrEntry.Properties[kUsr.GetPropAttr<KUsr, AltPropName>(x => x.UserInitials).Name].Value == null ? null :
                //    usrEntry.Properties[kUsr.GetPropAttr<KUsr, AltPropName>(x => x.UserInitials).Name].Value.ToString();

                kUsr.Location = usrEntry.Properties[kUsr.GetPropAttr<KUsr, AltPropName>(x => x.Location).Name].Value == null ? null :
                    usrEntry.Properties[kUsr.GetPropAttr<KUsr, AltPropName>(x => x.Location).Name].Value.ToString();

                kUsr.Department = usrEntry.Properties[kUsr.GetPropAttr<KUsr, AltPropName>(x => x.Department).Name].Value == null ? null :
                    usrEntry.Properties[kUsr.GetPropAttr<KUsr, AltPropName>(x => x.Department).Name].Value.ToString();
            }

            return kUsr;
        }

        public static bool IsWSExists(string engNum)
        {
            return AuditManager.Rep.Workspace.IsWSExists(engNum);
        }

        //public static WsProfile_TP GetWsProfile_TP(string engNum)
        //{
        //    return IM.Mgr.Workspace.GetWsProfile_TP(engNum);
        //}

        public static WsUser GetWsUser(string usrId, ImDbType imDbType)
        {
            return IM.Mgr.WsUtility.GetWsUser(usrId, imDbType);
        }
    }
}
