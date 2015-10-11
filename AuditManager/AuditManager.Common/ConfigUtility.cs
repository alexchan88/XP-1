using System;
using System.Configuration;
using System.Web;

namespace AuditManager.Common
{
    public static class ConfigUtility
    {
        public static string GetKException { get { return ConfigurationManager.AppSettings["KException"].ToString(); } }
        public static string GetKExceptionMsg { get { return ConfigurationManager.AppSettings["KExceptionMsg"].ToString(); } }

        public static string EncKey
        {
            get { return "34989d15-50a0-46d3-9181-403b6bb66cf5"; }
        }

        public static string aM_conStr
        {
            get
            {
                return AMSec.DecryptText(ConfigurationManager.AppSettings["aM_conStr"].ToString());
            }
        }

        public static string ssc_conStr
        {
            get
            {
                return AMSec.DecryptText(ConfigurationManager.AppSettings["ssc_conStr"].ToString());
            }
        }

        public static string s2_conStr
        {
            get
            {
                return AMSec.DecryptText(ConfigurationManager.AppSettings["s2_conStr"].ToString());
            }
        }

        public static string active_conStr
        {
            get
            {
                return AMSec.DecryptText(ConfigurationManager.AppSettings["active_conStr"].ToString());
            }
        }

        public static string ref_conStr
        {
            get
            {
                return AMSec.DecryptText(ConfigurationManager.AppSettings["ref_conStr"].ToString());
            }
        }

        public static string errorLog_conStr
        {
            get
            {
                return AMSec.DecryptText(ConfigurationManager.AppSettings["errorLog_conStr"].ToString());
            }
        }

        public static string LogDrive
        {
            get { return ConfigurationManager.AppSettings["logDrive"].ToString(); }
        }

        public static string GetPrjNDcs_baseUri
        {
            get
            {
                return ConfigurationManager.AppSettings["PrjNDcs_baseUri"].ToString();
            }
        }

        public static string GetPrjNDcs_url_Prj
        {
            get
            {
                return ConfigurationManager.AppSettings["PrjNDcs_url_Prj"].ToString();
            }
        }

        public static string GetPrjNDcs_url_Dcs
        {
            get
            {
                return ConfigurationManager.AppSettings["PrjNDcs_url_Dcs"].ToString();
            }
        }

        public static string GetPrjNDcs_url_Dcs_All
        {
            get
            {
                return ConfigurationManager.AppSettings["PrjNDcs_url_Dcs_All"].ToString();
            }
        }

        public static string GetLauchSite_baseUri
        {
            get
            {
                return ConfigurationManager.AppSettings["LauchSite_baseUri"].ToString();
            }
        }

        public static string GetLauchSite_url_GetFoldersRollforward
        {
            get
            {
                return ConfigurationManager.AppSettings["LauchSite_url_GetFoldersRollforward"].ToString();
            }
        }

        //domainName
        public static string GetDomainName()
        {
            return ConfigurationManager.AppSettings["domainName"].ToString();
        }

        public static string GetAdminUser()
        {
            return ConfigurationManager.AppSettings["adminUser"].ToString();
        }

        public static string GetActivityUser()
        {
            return ConfigurationManager.AppSettings["activityUser"].ToString();
        }

        public static string GetSuperUser()
        {
            return ConfigurationManager.AppSettings["SuperUser"].ToString();
        }

        public static Tuple<string> GetImSrvr()
        {
            return new Tuple<string>(ConfigurationManager.AppSettings["imServer"].ToString());
        }

        public static bool IsImTLogin()
        {
            return ConfigurationManager.AppSettings["isImTLogin"].ToString().ToBool();
        }

        public static Tuple<string, string> GetImLoginInfo()
        {
            return new Tuple<string, string>(ConfigurationManager.AppSettings["imLoginId"].ToString(),
                ConfigurationManager.AppSettings["imPwd"].ToString());
        }

        public static Tuple<string, string> GetImAdminLoginInfo()
        {
            return new Tuple<string, string>(ConfigurationManager.AppSettings["imAdminLoginId"].ToString(),
                ConfigurationManager.AppSettings["imAdminPwd"].ToString());
        }

        public static string GetActiveDirectory()
        {
            return ConfigurationManager.ConnectionStrings["ActiveDirectory"].ToString();
        }

        public static bool IsAdTLogin()
        {
            return ConfigurationManager.AppSettings["isAdTLogin"].ToString().ToBool();
        }

        public static Tuple<string, string> GetAdLoginInfo()
        {
            return new Tuple<string, string>(ConfigurationManager.AppSettings["adLoginId"].ToString(),
                ConfigurationManager.AppSettings["adPwd"].ToString());
        }

        public static string GetSmptSrvr
        {
            get { return ConfigurationManager.AppSettings["SmptSrvr"].ToString(); }
        }

        public static Tuple<string, string, string> GetClosureConfig
        {
            get
            {
                return new Tuple<string, string, string>(
                ConfigurationManager.AppSettings["SupportMailbox"].ToString(),
                ConfigurationManager.AppSettings["Email_Closure_Sender"].ToString(),
                ConfigurationManager.AppSettings["Email_Closure_Subject"].ToString()
                );
            }
        }

        public static Tuple<string, string, string, string, string> GetAuditManagerRequestAccessConfig
        {
            get
            {
                return new Tuple<string, string, string, string, string>(
                ConfigurationManager.AppSettings["SupportMailbox"].ToString(),
                ConfigurationManager.AppSettings["Email_AuditManager_Access_Subject"].ToString(),
                ConfigurationManager.AppSettings["Email_AuditManager_Access_Sender"].ToString(),
                ConfigurationManager.AppSettings["Email_AuditManager_Access_Sender_From"].ToString(),
                ConfigurationManager.AppSettings["Email_AuditManager_Access_To"].ToString()
                );
            }
        }

        public static Tuple<string, string, string> GetPreservationConfig
        {
            get
            {
                return new Tuple<string, string, string>(
                ConfigurationManager.AppSettings["SupportMailbox"].ToString(),
                ConfigurationManager.AppSettings["Email_Preservation_Sender"].ToString(),
                ConfigurationManager.AppSettings["Email_Preservation_Sender_From"].ToString()
                );
            }
        }

        public static Tuple<string, string, string, string> GetWsCreateConfig
        {
            get
            {
                return new Tuple<string, string, string, string>(
                ConfigurationManager.AppSettings["SupportMailbox"].ToString(),
                ConfigurationManager.AppSettings["Email_WsCreate_Sender"].ToString(),
                ConfigurationManager.AppSettings["Email_WsCreate_Sender_From"].ToString(),
                ConfigurationManager.AppSettings["Email_WsCreate_Subject"].ToString()
                );
            }
        }

        public static string GetDownloadUrl
        {
            get { return ConfigurationManager.AppSettings["DownloadUrl"].ToString(); }
        }

        public static string GetUploadUrl
        {
            get { return ConfigurationManager.AppSettings["UploadUrl"].ToString(); }
        }

        public static string GetEnv
        {
            get { return ConfigurationManager.AppSettings["env"].ToString(); }
        }

        public static bool IncludeBcc
        {
            get
            {
                return ConfigurationManager.AppSettings["IncludeBcc"].ToString().ToBool();
            }
        }

        public static string EmailBcc
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailBcc"].ToString();
            }
        }

        public static int WsCreateTemplateFldrId
        {
            get
            {
                return int.Parse((ConfigurationManager.AppSettings["WsCreateTemplateFldrId"].ToString()));
            }
        }

        public static bool IsUnderMaintenance()
        {
            return ConfigurationManager.AppSettings["IsUnderMaintenance"].ToString().ToBool();
        }

        public static string MaintenanceUser()
        {
            return ConfigurationManager.AppSettings["MaintenanceUser"].ToString();
        }

        public static string GetTempDocLocation
        {
            get { return "\\tempDoc"; }
        }

        public static int SqlCommandTimeout
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["sqlCommandTimeout"].ToString()); }
        }

        public static string GetRootUrl()
        {
            //return ConfigurationManager.AppSettings["rootUrl"].ToString();
            return string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, HttpRuntime.AppDomainAppVirtualPath);
        }

        public static string GetKWebAppBaseUrl
        {
            get
            {
                if (bool.Parse(ConfigurationManager.AppSettings["OverrideWebApiPath"].ToString()))
                {
                    return ConfigurationManager.AppSettings["WebApiPath"].ToString();
                }
                else
                {
                    return string.Format("{0}{1}", GetRootUrl().Replace("https", "http"), "api/");
                }
            }
        }

        public static bool IsTestMail { get { return bool.Parse(ConfigurationManager.AppSettings["IsTestMail"].ToString()); } }
        public static string TestFromMailId { get { return ConfigurationManager.AppSettings["TestFromMailId"].ToString(); } }
        public static string TestToMailId { get { return ConfigurationManager.AppSettings["TestToMailId"].ToString(); } }
    }
}
