using System;

namespace AuditManager.Model.EFModel.AM
{
    public class Engagement : PostFix
    {
        public int Id {get; set;} 
        public long WorkspaceId {get; set;} 
        public string Name {get; set;} 
        public int ManagerId {get; set;}
        public virtual User Manager { get; set; }
        public int PartnerId {get; set;}
        public virtual User Partner { get; set; }
        public int ClientId {get; set;}
        public virtual Client Client { get; set; }
    }

    public class EngagementProfile : PostFix
    {
        public int Id {get; set;} 
        public int EngagementId {get; set;}
        public virtual Engagement Engagement { get; set; }
        public DateTime EventTriggerDate {get; set;} 
        public bool? IsUnderPreservation {get; set;} 
        public string PreservationComment {get; set;} 
        public bool? IsOnRetentionServer {get; set;}  
        public bool? IsServer2 {get; set;}  
        public bool? TP1 {get; set;}
        public ResponseTypeEnum TP2 { get; set; }
        public virtual ResponseType ResponseType { get; set; }
        public bool? TP3 {get; set;}  
        public string TP3Comment {get; set;}
        public ProfileUpdateActionTypeEnum ProfileUpdateActionTypeId { get; set; }
        public virtual ProfileUpdateActionType ProfileUpdateActionType { get; set; }
    }

    public class EngagementGuid : PostFix
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public int EngagementProfileId { get; set; }
        public virtual EngagementProfile EngagementProfile { get; set; }
    }
}
