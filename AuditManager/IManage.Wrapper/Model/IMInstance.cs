
using IM.Wrapper.Utility;
namespace IM.Wrapper.Model
{
    public class IMInstance
    {
        public IMInstance()
        {

        }

        internal IMInstance(Env env, string userId = null, bool? useAdmin = false, bool? useConfig = false)
        {
            UserId = string.IsNullOrWhiteSpace(userId) ? IMConfigUtility.GetCurrentUser : userId.ToUpper();

            UseAdmin = useAdmin.GetValueOrDefault(false);
            UseConfig = useConfig.GetValueOrDefault(false);

            AppName_0 = "KPMGFileTransfer";
            AppName = "Audit Manager";

            switch (env)
            {
                case Env.Dev:
                    if (UseConfig.GetValueOrDefault(false))
                        IMInstance_Config();
                    else
                        IMInstance_Dev();
                    break;
                case Env.Stg:
                    if (UseConfig.GetValueOrDefault(false))
                        IMInstance_Config();
                    else
                        IMInstance_Stg();
                    break;
                case Env.Prod:
                    if (UseConfig.GetValueOrDefault(false))
                        IMInstance_Config();
                    else
                        IMInstance_Prod();
                    break;
            }
        }

        private void IMInstance_Config()
        {
            DataSource = IMConfigUtility.DataSource;
            InitialCatalog = IMConfigUtility.InitialCatalog;
            SqlUserID = IMConfigUtility.SqlUserID;
            SqlPassword = IMConfigUtility.SqlPassword;
            IntegratedSecurity = IMConfigUtility.IntegratedSecurity;
            IMServer = IMConfigUtility.IMServer;
            IMAdminUserID = IMConfigUtility.IMAdminUserID;
            IMAdminPassword = IMConfigUtility.IMAdminPassword;
            DownloadUserID = IMConfigUtility.DownloadUserID;
            DownloadPassword = IMConfigUtility.DownloadPassword;

        }

        private void IMInstance_Dev()
        {
            DataSource = IMResource_Dev.DataSource;
            InitialCatalog = IMResource_Dev.InitialCatalog;
            SqlUserID = IMResource_Dev.SqlUserID;
            SqlPassword = IMResource_Dev.SqlPassword;
            IntegratedSecurity = bool.Parse(IMResource_Dev.IntegratedSecurity);
            IMServer = IMResource_Dev.IMServer;
            IMAdminUserID = IMResource_Dev.IMAdminUserID;
            IMAdminPassword = IMResource_Dev.IMAdminPassword;
            DownloadUserID = IMResource_Dev.DownloadUserID;
            DownloadPassword = IMResource_Dev.DownloadPassword;

        }

        private void IMInstance_Stg()
        {
            DataSource = IMResource_Stg.DataSource;
            InitialCatalog = IMResource_Stg.InitialCatalog;
            SqlUserID = IMResource_Stg.SqlUserID;
            SqlPassword = IMResource_Stg.SqlPassword;
            IntegratedSecurity = bool.Parse(IMResource_Stg.IntegratedSecurity);
            IMServer = IMResource_Stg.IMServer;
            IMAdminUserID = IMResource_Stg.IMAdminUserID;
            IMAdminPassword = IMResource_Stg.IMAdminPassword;
            DownloadUserID = IMResource_Stg.DownloadUserID;
            DownloadPassword = IMResource_Stg.DownloadPassword;
        }

        private void IMInstance_Prod()
        {
            DataSource = IMResource_Prod.DataSource;
            InitialCatalog = IMResource_Prod.InitialCatalog;
            SqlUserID = IMResource_Prod.SqlUserID;
            SqlPassword = IMResource_Prod.SqlPassword;
            IntegratedSecurity = bool.Parse(IMResource_Prod.IntegratedSecurity);
            IMServer = IMResource_Prod.IMServer;
            IMAdminUserID = IMResource_Prod.IMAdminUserID;
            IMAdminPassword = IMResource_Prod.IMAdminPassword;
            DownloadUserID = IMResource_Prod.DownloadUserID;
            DownloadPassword = IMResource_Prod.DownloadPassword;
        }

        public string AppName_0 { get; set; }
        public string AppName { get; set; }

        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public string SqlUserID { get; set; }
        public string SqlPassword { get; set; }
        public bool IntegratedSecurity { get; set; }

        public string IMServer { get; set; }

        public string IMAdminUserID { get; set; }
        public string IMAdminPassword { get; set; }

        public string DownloadUserID { get; set; }
        public string DownloadPassword { get; set; }

        public string UserId { get; set; }
        public bool? UseAdmin { get; set; }

        public bool? UseConfig { get; set; }
    }
}
