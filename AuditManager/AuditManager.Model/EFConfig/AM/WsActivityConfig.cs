using AuditManager.Model.EFModel.AM;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditManager.Model.EFConfig.AM
{
    public class WsActivityHistoryConfig : DbBaseEntityConfig<WsActivityHistory>
    {
        public WsActivityHistoryConfig()
        {
            HasKey(x => x.WsActivityHistoryId);
            Property(x => x.WsActivityHistoryId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.EngNum).IsRequired().HasMaxLength(32);
            Property(x => x.UserId).IsRequired().HasMaxLength(64);
            Property(x => x.DocNum).IsOptional();

            HasRequired(x => x.WsActivity)
                .WithMany()
                .HasForeignKey(x => x.WsActivityTypeId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.WsSubActivity)
                .WithMany()
                .HasForeignKey(x => x.WsSubActivityTypeId)
                .WillCascadeOnDelete(false);

            Property(x => x.Comment).IsOptional().HasMaxLength(1000);
            Property(x => x.WsActivityInfo).IsOptional().IsMaxLength();
            Property(x => x.DateCreated).IsRequired();
        }
    }
    
    public class ClosureFileConfig : DbBaseEntityConfig<Closure>
    {
        public ClosureFileConfig()
        {
            HasKey(x => x.ClosureId);
            Property(x => x.ClosureId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.EngNum).IsRequired().HasMaxLength(32);
            Property(x => x.ClosureDate).IsRequired();
            Property(x => x.IsClosed).IsRequired();
            Property(x => x.LastMailSentDate).IsRequired();
        }
    }
}
