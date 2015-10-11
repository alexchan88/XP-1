using System;

namespace AuditManager.Common
{
    public class LogOnlyException : Exception
    {
        public string ErrorMsg { get; set; }

        public LogOnlyException(string msg)
        {
            ErrorMsg = msg;
        }
    }
}
