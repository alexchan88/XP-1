using System;

namespace AuditManager.Model
{
    public class Post_WsS2GuidModel
    {
        public int MasterAuditFileId { get; set; }
        public string Comment { get; set; }
        public Guid MafGuid { get; set; }
        public string EngNum { get; set; }
        public string EngName { get; set; }
        public string Client { get; set; }
        public string ClientDesc { get; set; }
        public string PartnerEmail { get; set; }
        public string ManagerEmail { get; set; }
        public bool IsS2 { get; set; }
        public string WsId { get; set; }
    }
    
    public class WsS2GuidModel
    {
        public int MasterAuditFileId { get; set; }
        public string MAFGUID { get; set; }
        public string EngNum { get; set; }
        public string EngName { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string ClientNumber { get; set; }
        public string ClientName { get; set; }
        public string Partner { get; set; }
        public string Manager { get; set; }
        public string WorkbookName { get; set; }
        public string WBDescription { get; set; }
        public string WBStatus { get; set; }
    }

    public class Mapped_WsS2GuidModel
    {
        public int MasterAuditFileId { get; set; }
        public string MAFGuid { get; set; }
        public string FileName { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
