
namespace AuditManager.IM
{
    public class Workspace
    {
        IMSession _iMSession = null;
        
        public Workspace(IMSession iMSession)
        {
            _iMSession = iMSession;
        }

        public IManage.IManWorkspaces GetAllWs()
        {
            return _iMSession.GetDb().Workspaces;
        }

        public T GetObjectByObjectId<T>(string objectId)
        {
            return (T)_iMSession.GetSession().DMS.GetObjectByID(objectId);
        }
    }
}
