using System;
using System.Collections.Generic;

namespace AuditManager.IM
{

    public class IMSession
    {
        ISessionInfo _ISessionInfo = null;
        DbName? _dbName = null;

        public IMSession(ISessionInfo iSessionInfo, DbName dbName)
        {
            _ISessionInfo = iSessionInfo;
            _dbName = dbName;
        }

        public IManage.IManSession GetSession()
        {
            return IMSession<IManage.IManSession, IManage.IManDatabase, IManage.ManDMS>.GetInstance().Login(_ISessionInfo);
        }

        public IManage.IManDatabase GetDb()
        {
            return IMSession<IManage.IManSession, IManage.IManDatabase, IManage.ManDMS>.GetInstance().GetDb(GetSession(), _dbName.GetValueOrDefault());
        }
    }
    
    public class NRTSession
    {
        public IMANADMIN.NRTSession GetSession(ISessionInfo iSessionInfo)
        {
            return NRTSession<IMANADMIN.NRTSession, IMANADMIN.NRTDatabase, IMANADMIN.NRTDMS>.GetInstance().Login(iSessionInfo);
        }

        public IMANADMIN.NRTDatabase GetDb(ISessionInfo iSessionInfo, DbName dbName)
        {
            return NRTSession<IMANADMIN.NRTSession, IMANADMIN.NRTDatabase, IMANADMIN.NRTDMS>.GetInstance().GetDb(GetSession(iSessionInfo), dbName);
        }
    }

    public enum DbName : byte
    {
        Active,
    }

    public interface ISessionInfo
    {
        string UserId { get; set; }
        string Password { get; set; }
        string IManageServer { get; set; }
        bool IsTrusted { get; set; }
    }

    public class SessionInfo : ISessionInfo
    {
        public string UserId { get; set; }

        public string Password { get; set; }

        public string IManageServer { get; set; }

        public bool IsTrusted { get; set; }
    }

    public interface ISession<T, U, V>
    {
        T Login(ISessionInfo iSessionInfo, bool isNewSession = false);

        void Logout(T t);

        U GetDb(T t, DbName dbName);
    }

    public class IMSession<T, U, V> : ISession<T, U, V>
        where T : IManage.IManSession
        where U : IManage.IManDatabase
        where V : IManage.ManDMS
    {
        private static IMSession<T, U, V> _IMSession = null;

        private IMSession() {
            _IMSession = new IMSession<T, U, V>();
        }

        public static IMSession<T, U, V> GetInstance()
        {
            return _IMSession;
        }
        
        private static Dictionary<string, V> iManageSessionDictionary = new Dictionary<string, V>();

        private T Login(ISessionInfo iSessionInfo, T imSession)
        {
            if (iSessionInfo.IsTrusted)
            {
                imSession.TrustedLogin();
            }
            else
            {
                imSession.Login(iSessionInfo.UserId, iSessionInfo.Password);
            }

            return (T)imSession;
        }

        public T Login(ISessionInfo iSessionInfo, bool isNewSession = false)
        {
            V dms = default(V);
            T imSession = default(T);

            if (iManageSessionDictionary.TryGetValue(iSessionInfo.UserId, out dms))
            {
                imSession = (T)dms.Sessions.ItemByName(iSessionInfo.IManageServer);

                if (imSession == null)
                    imSession = (T)dms.Sessions.Add(iSessionInfo.IManageServer);

                if (!imSession.Connected)
                    imSession = Login(iSessionInfo, (T)imSession);
            }
            else
            {
                imSession = (T)dms.Sessions.Add(iSessionInfo.IManageServer);

                imSession = Login(iSessionInfo, (T)imSession);

                iManageSessionDictionary.Add(iSessionInfo.UserId, dms);
            }

            return (T)imSession;
        }

        public U GetDb(T t, DbName dbName)
        {
            return (U)t.Databases.ItemByName(dbName.ToString());
        }

        public void Logout(T t)
        {
            if (t != null)
            {
                if (t.Connected)
                    t.Logout();

                t.DMS.Close();
            }
        }
    }

    public class NRTSession<T, U, V> : ISession<T, U, V>
        where T : IMANADMIN.NRTSession
        where U : IMANADMIN.NRTDatabase
        where V : IMANADMIN.NRTDMS
    {
        private static NRTSession<T, U, V> _NRTSession = null;

        private NRTSession() {
            _NRTSession = new NRTSession<T, U, V>();
        }

        public static NRTSession<T, U, V> GetInstance()
        {
            return _NRTSession;
        }

        private static Dictionary<string, V> nrtSessionDictionary = new Dictionary<string, V>();

        private T Login(ISessionInfo iSessionInfo, T imSession)
        {
            if (iSessionInfo.IsTrusted)
            {
                imSession.TrustedLogin();
            }
            else
            {
                imSession.Login(iSessionInfo.UserId, iSessionInfo.Password);
            }

            return (T)imSession;
        }

        public T Login(ISessionInfo iSessionInfo, bool isNewSession = false)
        {
            V dms = default(V);
            T imSession = default(T);

            if (nrtSessionDictionary.TryGetValue(iSessionInfo.UserId, out dms))
            {
                imSession = (T)dms.Sessions.Item(iSessionInfo.IManageServer);

                if (imSession == null)
                    imSession = (T)dms.Sessions.Add(iSessionInfo.IManageServer);

                if (!imSession.Connected)
                    imSession = Login(iSessionInfo, (T)imSession);
            }
            else
            {
                imSession = (T)dms.Sessions.Add(iSessionInfo.IManageServer);

                imSession = Login(iSessionInfo, (T)imSession);

                nrtSessionDictionary.Add(iSessionInfo.UserId, dms);
            }

            return (T)imSession;
        }

        public U GetDb(T t, DbName dbName)
        {
            foreach (U item in t.Databases)
            {
                if (item.Name.Equals(dbName.ToString(), StringComparison.OrdinalIgnoreCase))
                    return item;
            }

            return default(U);
        }

        public void Logout(T t)
        {
            if (t != null)
            {
                if (t.Connected)
                    t.Logout();

                t.NRTDMS.CloseApplication();
            }
        }
    }
}
