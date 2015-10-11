
namespace AuditManager.Model.EFModel.AM
{
    public class WsMail : AmDbEntityModel
    {
        public int WsActivityHistoryId { get; set; }
        public virtual WsActivityHistory WsActivityHistory { get; set; }

        public string RecepientArray { get; set; }
        public string MailText { get; set; }
        
        public WsMailStatusType WsMailStatusTypeId { get; set; }
        public virtual WsMailStatus WsMailStatus { get; set; }
    }
}
