using System;
using System.Collections.Generic;

namespace AuditManager.Model
{
    public class FileActivityModel
    {
        public string FAID { get; set; }
        public string EngagementNumber { get; set; }
        public string EngagementName { get; set; }
        public string RequestType { get; set; }
        public string Status { get; set; }
        public string ActionPerformedBy { get; set; }
        public DateTime? ActionPerformedOn { get; set; }
        public string FileName { get; set; }
        public string WorkBookName { get; set; }
        public string EngagementFileName { get; set; }
        public string NSTID { get; set; }
        //public bool IsS2 { get; set; }
        public string MAFGuid { get; set; }
        public string ReviewType { get; set; }
        //public string StatusUpdateUniqueId { get { return this.IsS2 ? this.FAID : "100"; } }
        //public StatusModel StatusModel { get; set; }
        public string DocNum { get; set; }
        //public string UniqueIdNDocNumString { get { return string.Format("{0}|{1}", FAID, string.IsNullOrWhiteSpace(DocNum) ? "0" : DocNum); } }
        //public bool CanTakeActivityAction { get; set; }
        
        public List<WsUserType> WsUserType { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
    }

    public class WsActivityModel
    {
        //[spCloseActivity]
        //@FAId int,
        //@success bit,
        //@comments varchar(max),
        //@activityEndDate datetime,
        //@userId varchar(50)

        //[spRemoveActivity]
        //@FAId int,
        //@comments varchar(255),
        //@userId varchar(50)
    }
}
