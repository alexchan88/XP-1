
using System.Configuration;
namespace IM.Wrapper.Utility
{
    internal class IMConfigUtility
    {
        public static string GetCurrentUser
        {
            get { return System.Environment.UserName.ToUpper(); }
        }
        
        public static string IMServer
        {
            get { return ConfigurationManager.AppSettings["IMServer"].ToString(); }
        }

        public static string IMAdminUserID
        {
            get { return ConfigurationManager.AppSettings["IMAdminUserID"].ToString(); }
        }

        public static string IMAdminPassword
        {
            get { return ConfigurationManager.AppSettings["IMAdminPassword"].ToString(); }
        }

        public static string DataSource
        {
            get { return ConfigurationManager.AppSettings["DataSource"].ToString(); }
        }

        public static string InitialCatalog
        {
            get { return ConfigurationManager.AppSettings["InitialCatalog"].ToString(); }
        }

        public static string SqlUserID
        {
            get { return ConfigurationManager.AppSettings["SqlUserID"].ToString(); }
        }

        public static string SqlPassword
        {
            get { return ConfigurationManager.AppSettings["SqlPassword"].ToString(); }
        }

        public static bool IntegratedSecurity
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["IntegratedSecurity"].ToString()); }
        }

        public static string DownloadUserID
        {
            get { return ConfigurationManager.AppSettings["DownloadUserID"].ToString(); }
        }

        public static string DownloadPassword
        {
            get { return ConfigurationManager.AppSettings["DownloadPassword"].ToString(); }
        }
    }
}
