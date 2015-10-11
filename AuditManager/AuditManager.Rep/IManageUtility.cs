
namespace AuditManager.Rep
{
    public class IManageUtility
    {
        public static void UploadDocument(string engNum, string fileNameWithLocalPath, string iMFolderPath) {
            IM.Mgr.IManageUtility.UploadDocument(engNum, fileNameWithLocalPath, iMFolderPath);
        }


        public static void UploadDocument2(string wsId, string fileNameWithLocalPath, string iMFolderPath) {
            IM.Mgr.IManageUtility.UploadDocument(wsId, fileNameWithLocalPath, iMFolderPath);
        }
    }
}
