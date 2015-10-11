using AuditManager.Common;
using AuditManager.Model;
using System;
using System.Collections.Generic;

namespace IM.Mgr
{
    public class IManageSession
    {
        private static Dictionary<string, IManage.ManDMS> dictIManageDms = new Dictionary<string, IManage.ManDMS>();

        private static IManage.IManSession GetUsrSession(IManage.IManSession imSession)
        {
            if (ConfigUtility.IsImTLogin())
            {
                //imSession.TrustedLogin2(System.Security.Principal.WindowsIdentity.GetCurrent().Token.ToInt32());
                imSession.TrustedLogin();
            }
            else
            {
                imSession.Login(ConfigUtility.GetImLoginInfo().Item1, ConfigUtility.GetImLoginInfo().Item2);
            }

            return imSession;
        }
        
        public static IManage.IManSession UsrSession(bool newSession = false)
        {
            IManage.IManSession imSession = null;
            var currentUser = AmUtil.GetCurrentUser;

            if (!dictIManageDms.ContainsKey(currentUser))
            {
                IManage.ManDMS dms = new IManage.ManDMS();

                dictIManageDms.Add(currentUser, dms);

                imSession = dms.Sessions.Add(ConfigUtility.GetImSrvr().Item1);

                return GetUsrSession(imSession);
            }
            else
            {
                var dms = dictIManageDms[currentUser];
                imSession = dms.Sessions.ItemByName(ConfigUtility.GetImSrvr().Item1);
                
                if (newSession)
                {
                    if (imSession != null && imSession.Connected)
                    {
                        imSession.Logout();
                    }
                }
                else
                {
                    if (imSession != null && imSession.Connected)
                    {
                        return imSession;
                    }
                }

                return GetUsrSession(imSession);
            }
        }

        public static IManage.IManSession AdminSession()
        {
            IManage.IManSession imSession = null;
            var currentUser = ConfigUtility.GetImAdminLoginInfo().Item1;

            if (!dictIManageDms.ContainsKey(currentUser))
            {
                IManage.ManDMS dms = new IManage.ManDMS();

                dictIManageDms.Add(currentUser, dms);

                imSession = dms.Sessions.Add(ConfigUtility.GetImSrvr().Item1);

                imSession.Login(ConfigUtility.GetImAdminLoginInfo().Item1, ConfigUtility.GetImAdminLoginInfo().Item2);

                return imSession;
            }
            else
            {
                var dms = dictIManageDms[currentUser];
                imSession = dms.Sessions.ItemByName(ConfigUtility.GetImSrvr().Item1);

                if (imSession != null && imSession.Connected)
                    return imSession;
                else
                {
                    imSession.Login(ConfigUtility.GetImAdminLoginInfo().Item1, ConfigUtility.GetImAdminLoginInfo().Item2);
                    return imSession;
                }
            }
        }

        public static IManage.IManDatabase Db(ImDbType imDbType, bool newSession = false)
        {
            IManage.IManDatabase iMDb = UsrSession(newSession).Databases.ItemByName(imDbType.ToString());

            return iMDb;
        }

        public static IManage.IManDatabase AdminDb(ImDbType imDbType, bool newSession = false)
        {
            IManage.IManDatabase iMDb = AdminSession().Databases.ItemByName(imDbType.ToString());

            return iMDb;
        }
    }

    public class NrtSession2
    {
        private static Dictionary<string, IMANADMIN.NRTDMS> dictNrtDms = new Dictionary<string, IMANADMIN.NRTDMS>();

        private static IMANADMIN.NRTSession GetUsrSession(IMANADMIN.NRTSession nrtSession)
        {
            if (ConfigUtility.IsImTLogin())
            {
                nrtSession.TrustedLogin();
            }
            else
            {
                nrtSession.Login(ConfigUtility.GetImLoginInfo().Item1, ConfigUtility.GetImLoginInfo().Item2);
            }

            return nrtSession;
        }

        public static IMANADMIN.NRTSession UsrSession()
        {
            IMANADMIN.NRTSession nrtSession = null;
            var currentUser = AmUtil.GetCurrentUser;

            if (!dictNrtDms.ContainsKey(currentUser))
            {
                IMANADMIN.NRTDMS dms = new IMANADMIN.NRTDMS();

                dictNrtDms.Add(currentUser, dms);

                nrtSession = dms.Sessions.Add(ConfigUtility.GetImSrvr().Item1);

                return GetUsrSession(nrtSession);
            }
            else
            {
                var dms = dictNrtDms[currentUser];
                nrtSession = dms.Sessions.Item(ConfigUtility.GetImSrvr().Item1);

                if (nrtSession != null && nrtSession.Connected)
                    return nrtSession;

                return GetUsrSession(nrtSession);
            }
        }

        public static IMANADMIN.NRTSession AdminSession()
        {
            IMANADMIN.NRTSession nrtSession = null;
            var currentUser = ConfigUtility.GetImAdminLoginInfo().Item1;

            if (!dictNrtDms.ContainsKey(currentUser))
            {
                IMANADMIN.NRTDMS dms = new IMANADMIN.NRTDMS();

                dictNrtDms.Add(currentUser, dms);

                nrtSession = dms.Sessions.Add(ConfigUtility.GetImSrvr().Item1);

                nrtSession.Login(ConfigUtility.GetImAdminLoginInfo().Item1, ConfigUtility.GetImAdminLoginInfo().Item2);

                return nrtSession;
            }
            else
            {
                var dms = dictNrtDms[currentUser];
                nrtSession = dms.Sessions.Item(ConfigUtility.GetImSrvr().Item1);

                if (nrtSession != null && nrtSession.Connected)
                    return nrtSession;

                nrtSession.Login(ConfigUtility.GetImAdminLoginInfo().Item1, ConfigUtility.GetImAdminLoginInfo().Item2);

                return nrtSession;
            }
        }

        public static IMANADMIN.NRTDatabase Db(ImDbType imDbType)
        {
            IMANADMIN.NRTDatabase db = null;

            foreach (IMANADMIN.NRTDatabase item in UsrSession().Databases)
            {
                if (item.Name.Equals(imDbType.ToString(), StringComparison.OrdinalIgnoreCase))
                    return item;
            }

            return db;
        }

        public static IMANADMIN.NRTDatabase AdminDb(ImDbType imDbType)
        {
            IMANADMIN.NRTDatabase db = null;

            foreach (IMANADMIN.NRTDatabase item in AdminSession().Databases)
            {
                if (item.Name.Equals(imDbType.ToString(), StringComparison.OrdinalIgnoreCase))
                    return item;
            }

            return db;
        }
    }
}
