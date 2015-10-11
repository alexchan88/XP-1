using System;
using System.IO;
using System.Web;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace AuditManager.Common
{
    public class DbLog
    {
        //private static string log4NetConfig = AppDomain.CurrentDomain.BaseDirectory + "Config/Log/log4net.config";

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
                (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static DbLog()
        {
            ConfigureLog4Net();
        }

        private static void ConfigureLog4Net()
        {
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(log4NetConfig));
            log4net.GlobalContext.Properties["LogDrive"] = ConfigUtility.LogDrive;
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Config/Log/log4net.config"));
        }

        public static void LogDbInfo(string message)
        {
            LogInfo(message);
        }

        public static void LogInfo(string message)
        {
            log.Info(message);
        }

        //public static void LogError(string message, Exception ex = null)
        //{
        //    log.Error(message, ex);
        //}

        public static void LogToElmah(string msg, HttpContext httpContext)
        {
            Elmah.ErrorSignal.FromContext(httpContext).Raise(new LogOnlyException(msg));
        }

        public static void LogToElmah(Exception ex, HttpContext httpContext)
        {
            Elmah.ErrorSignal.FromContext(httpContext).Raise(new LogOnlyException(ex.Message));
        }
    }
}
