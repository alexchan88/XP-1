using System;

namespace AuditManager.Model.EFModel.AM
{   
    public class WsActivityHistory : AmDbEntityModel
    {
        public int WsActivityHistoryId { get; set; }

        public string EngNum { get; set; }
        public string UserId { get; set; }
        public double DocNum { get; set; }

        public WsActivityType WsActivityTypeId { get; set; }
        //[JsonIgnore]
        //public virtual WsActivity WsActivity { get; set; }
        public virtual WsActivity WsActivity { get; set; }

        public WsActivityType WsSubActivityTypeId { get; set; }
        //[JsonIgnore]
        public virtual WsActivity WsSubActivity { get; set; }

        public string Comment { get; set; }
        public string WsActivityInfo { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class Closure : AmDbEntityModel
    {
        public int ClosureId { get; set; }

        public string EngNum { get; set; }
        public DateTime ClosureDate { get; set; }
        public bool IsClosed { get; set; }
        public DateTime LastMailSentDate { get; set; }
    }
}
