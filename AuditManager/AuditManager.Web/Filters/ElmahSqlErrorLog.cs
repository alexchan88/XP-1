using Elmah;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AuditManager.Web.Filters
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

        public override ErrorLogEntry GetError(string id)
        {
            return base.GetError(id);
        }

        public override int GetErrors(int pageIndex, int pageSize, System.Collections.IList errorEntryList)
        {
            //return base.GetErrors(pageIndex, pageSize, errorEntryList);

            var count =  base.GetErrors(pageIndex, pageSize, errorEntryList);

            List<ErrorLogEntry> filterErrorEntryList = new List<ErrorLogEntry>();

            foreach (ErrorLogEntry item in errorEntryList)
            {
                if (item.Error.Type == "System.Runtime.InteropServices.COMException")
                {
                    filterErrorEntryList.Add(item);
                }
            }
            
            errorEntryList.Clear();
            filterErrorEntryList.ForEach(y => errorEntryList.Add(y));


            return errorEntryList.Count;
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