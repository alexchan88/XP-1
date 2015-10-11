
using IM.Wrapper.Model;
namespace IM.Wrapper
{
    public class IMUtility
    {
        private IMInstance _IMInstance { get; set; }
        public IMUtility(IMInstance iMInstance)
        {
            _IMInstance = iMInstance;
        }

        public IMUtility(Env env, string userId = null, bool? useAdmin = false, bool? useConfig = false)
        {
            _IMInstance = new IMInstance(env, userId, useAdmin, useConfig);
        }

        public int? Upload(string engNum, string fileNameWithLocalPath, string iMFolderPath)
        {
            return IM.Wrapper.Operation.UploadFile.Upload(_IMInstance, engNum, fileNameWithLocalPath, iMFolderPath);
        }

        public string GetDocLocation(string engNum, double docNum)
        {
            return IM.Wrapper.Operation.DownloadFile.GetDocLocation(_IMInstance, engNum, docNum);
        }

        public void PutDocDownloadHistory(double docNum)
        {
            IM.Wrapper.Operation.DownloadFile.PutDocDownloadHistory(_IMInstance, docNum);
        }
    }
}
