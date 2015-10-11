using System;

namespace iManage.Api
{
    public class UserSession : ISession, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IManage.IManSession Session()
        {
            IManage.ManDMS dms = new IManage.ManDMS();

            IManage.IManSession imSession = dms.Sessions.Add(iMUtility.IManageServer);

            var AccessToken = System.Security.Principal.WindowsIdentity.GetCurrent().Token.ToInt32();

            //imSession.TrustedLogin2(AccessToken);
            imSession.TrustedLogin();

            return imSession;
        }

        public IManage.IManDatabase DB(DbNameType dbNameType)
        {
            return Session().Databases.ItemByName(dbNameType.ToString());
        }
    }
}
