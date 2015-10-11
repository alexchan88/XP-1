using AuditManager.Common;
using Elmah;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KElmah
{
    public class ElmahSqlErrorLog : SqlErrorLog
    {
        public ElmahSqlErrorLog(IDictionary config)
            : base(config)
        {
            // Must define for Elmah to work
        }

        public ElmahSqlErrorLog(string connectString)
            : base(connectString)
        {
            // Must define for Elmah to work
        }

        public override string Log(Error error)
        {
            return base.Log(error);
        }

        public override int GetErrors(int pageIndex, int pageSize, System.Collections.IList errorEntryList)
        {
            if (AmUtil.GetCurrentUser.Equals("viveksingh1", StringComparison.OrdinalIgnoreCase))
                return base.GetErrors(pageIndex, pageSize, errorEntryList);
            else
                return GetErrors(pageIndex, errorEntryList, pageSize);
        }

        private int GetErrors(int pageIndex, System.Collections.IList errorEntryList, int pageSize = 15)
        {
            List<string> kEx = new List<string>();
            ConfigUtility.GetKException.Split(',').ToList().ForEach(x => kEx.Add(x.ToEnum<KExceptionType>().ToEnumDesc<KExceptionType>()));
            List<string> kExMsg = ConfigUtility.GetKExceptionMsg.Split(',').ToList();

            using (var db = new DbContextElmah())
            {
                using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    int errorCount = db.ELMAH_Error
                        .Where(x => kEx.Contains(x.Type) && !kExMsg.Any(y => x.Message.Contains(y)))
                        .Count();

                    var eLMAH_Error = db.ELMAH_Error
                        .Where(x => kEx.Contains(x.Type) && !kExMsg.Any(y => x.Message.Contains(y)))
                        .OrderByDescending(x => x.Sequence)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .ToList();

                    eLMAH_Error.ForEach(x =>
                    {
                        errorEntryList.Add(new ErrorLogEntry(this, x.ErrorId.ToString(), new Error
                        {
                            ApplicationName = x.Application,
                            Detail = x.AllXml,
                            HostName = x.Host,
                            Message = x.Message,
                            Source = x.Source,
                            StatusCode = x.StatusCode,
                            Time = x.TimeUtc,
                            Type = x.Type,
                            User = x.User,
                            WebHostHtmlMessage = ""

                        }));
                    });

                    return errorCount;
                }
            }
        }

        public override IAsyncResult BeginGetError(string id, AsyncCallback asyncCallback, object asyncState)
        {
            return base.BeginGetError(id, asyncCallback, asyncState);
        }

        public override ErrorLogEntry EndGetError(IAsyncResult asyncResult)
        {
            return base.EndGetError(asyncResult);
        }
    }
}