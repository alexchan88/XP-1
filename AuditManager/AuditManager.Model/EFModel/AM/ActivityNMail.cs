using System;

namespace AuditManager.Model.EFModel.AM
{
    public class Activity : BaseEntity
    {
        public int Id { get; set; }
        public ActivityTypeEnum ActivityTypeId { get; set; }
        public virtual ActivityType ActivityType { get; set; }
        public int EngagementProfileId { get; set; }
        public virtual EngagementProfile EngagementProfile { get; set; }
        public string Comment { get; set; }
        public string ActivityInfo { get; set; }

        public int ActorId { get; set; }
        public virtual User Actor { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class Mail : BaseEntity
    {
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        public string MailBody { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
