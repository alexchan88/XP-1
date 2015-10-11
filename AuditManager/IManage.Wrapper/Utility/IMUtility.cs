using IM.Wrapper.Model;

namespace IM.Wrapper.Utility
{
    internal class IMUtility
    {
        public static string GetWsObjectID(IMInstance iMInstance, IMSession iMSession, IMWSObjectType iMWSObjectType, double objNum)
        {
            switch (iMWSObjectType)
            {
                case IMWSObjectType.File:
                    return (string)string.Format("{0}!{1}:{2},1:",
                        iMInstance.UseAdmin.GetValueOrDefault(false) ? iMSession.AdminDb(IMDbType.Active).ObjectID : iMSession.UserDb(IMDbType.Active).ObjectID,
                        iMWSObjectType.ToEnumDesc<IMWSObjectType>(), objNum);
                case IMWSObjectType.Folder:
                    return (string)string.Format("{0}!{1}:{2}:",
                        iMInstance.UseAdmin.GetValueOrDefault(false) ? iMSession.AdminDb(IMDbType.Active).ObjectID : iMSession.UserDb(IMDbType.Active).ObjectID,
                        iMWSObjectType.ToEnumDesc<IMWSObjectType>(), objNum);
                case IMWSObjectType.Workspace:
                    return (string)string.Format("{0}!{1}:{2}:",
                        iMInstance.UseAdmin.GetValueOrDefault(false) ? iMSession.AdminDb(IMDbType.Active).ObjectID : iMSession.UserDb(IMDbType.Active).ObjectID,
                        iMWSObjectType.ToEnumDesc<IMWSObjectType>(), objNum);
                default:
                    return null;
            }
        }
    }
}
