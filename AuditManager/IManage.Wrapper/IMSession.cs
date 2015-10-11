using IM.Wrapper.Model;
using System.Collections.Generic;

namespace IM.Wrapper
{
    internal class IMSession
    {
        private IMInstance _IMInstance { get; set; }
        private static Dictionary<string, IManage.IManSession> dictIManSession = new Dictionary<string, IManage.IManSession>();
        private Dictionary<string, IManage.IManSession> dictIManSession_User = new Dictionary<string, IManage.IManSession>();

        public IMSession(IMInstance iMInstance)
        {
            _IMInstance = iMInstance;
        }

        public  IManage.IManSession AdminSession()
        {
            var adminId = _IMInstance.IMAdminUserID;

            if (!dictIManSession.ContainsKey(adminId))
            {
                IManage.ManDMS dms = new IManage.ManDMS();

                IManage.IManSession imSession = dms.Sessions.Add(_IMInstance.IMServer);

                imSession.Login(adminId, _IMInstance.IMAdminPassword);

                dictIManSession.Add(adminId, imSession);

                return imSession;
            }
            else
            {
                IManage.IManSession imSession = dictIManSession[adminId];
                if (imSession != null && imSession.Connected)
                    return imSession;
                else
                {
                    dictIManSession.Remove(adminId);
                    return AdminSession();
                }
            }
        }

        public IManage.IManDatabase AdminDb(IMDbType imDbType)
        {
            return AdminSession().Databases.ItemByName(imDbType.ToString());
        }

        public IManage.IManSession UserSession()
        {
            if (!dictIManSession_User.ContainsKey(_IMInstance.UserId))
            {
                IManage.ManDMS dms = new IManage.ManDMS();

                IManage.IManSession imSession = dms.Sessions.Add(_IMInstance.IMServer);

                imSession.TrustedLogin();

                dictIManSession_User.Add(_IMInstance.UserId, imSession);

                return imSession;
            }
            else
            {
                IManage.IManSession imSession = dictIManSession_User[_IMInstance.UserId];
                if (imSession != null && imSession.Connected)
                    return imSession;
                else
                {
                    dictIManSession_User.Remove(_IMInstance.UserId);
                    return UserSession();
                }
            }
        }

        public IManage.IManDatabase UserDb(IMDbType imDbType)
        {
            return UserSession().Databases.ItemByName(imDbType.ToString());
        }
    }
}
