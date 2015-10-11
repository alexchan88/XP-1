using AuditManager.Model.EFModel.AM;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditManager.Model.EFConfig.AM
{
    public class WsFloatingFieldConfig : DbBaseEntityConfig<WsFloatingField>
    {
        public WsFloatingFieldConfig()
        {
            HasKey(x => x.WsFloatingFieldId);
            Property(x => x.WsFloatingFieldId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.EngNum).IsRequired().HasMaxLength(32);
            Property(x => x.IsUnderPreservation).IsOptional();
            Property(x => x.Comment).HasMaxLength(1000).IsOptional();
            Property(x => x.EventTrgDate).IsOptional();
            Property(x => x.IsServer2).IsOptional();
            Property(x => x.IsKDrive).IsOptional();

            Property(x => x.TPAns).IsRequired().IsMaxLength();

            Property(x => x.EnteredBy).IsRequired().HasMaxLength(64);
            Property(x => x.EnteredDate).IsRequired();
            Property(x => x.IsActive).IsRequired();
        }
    }
}
