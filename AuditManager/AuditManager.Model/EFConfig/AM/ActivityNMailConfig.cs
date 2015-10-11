using AuditManager.Model.EFModel.AM;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditManager.Model.EFConfig.AM
{
    public class ActivityConfig : BaseEntityConfig<Activity>
    {
        public ActivityConfig()
        {
            ToTable("Activity");

            HasKey(t => t.Id);
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(t => t.ActivityType).WithMany().HasForeignKey(t => t.ActivityTypeId).WillCascadeOnDelete(false);
            HasRequired(t => t.EngagementProfile).WithMany().HasForeignKey(t => t.EngagementProfileId).WillCascadeOnDelete(false);
            Property(t => t.Comment).IsOptional().HasMaxLength(1000);
            Property(t => t.ActivityInfo).IsOptional().IsMaxLength();
            HasRequired(t => t.Actor).WithMany().HasForeignKey(t => t.ActorId).WillCascadeOnDelete(false);
            Property(t => t.DateCreated).IsRequired();
        }
    }

    public class MailConfig : BaseEntityConfig<Mail>
    {
        public MailConfig()
        {
            ToTable("Mail");

            HasKey(t => t.ActivityId);
            HasRequired(t => t.Activity).WithMany().HasForeignKey(t => t.ActivityId).WillCascadeOnDelete(false);
            Property(t => t.MailBody).IsRequired().IsMaxLength();
            Property(t => t.DateCreated).IsRequired();
        }
    }
}
