using AuditManager.Model.EFModel.S2;

namespace AuditManager.Model.EFConfig.S2
{
    public class MasterAuditFileConfig : DbBaseEntityConfig<MasterAuditFile>
    {
        public MasterAuditFileConfig()
        {
            Property(e => e.MAFGuid)
                .IsUnicode(false);

            Property(e => e.EngagementNumber)
            .IsUnicode(false);

            Property(e => e.EngagementName)
            .IsUnicode(false);

            Property(e => e.FileName)
            .IsUnicode(false);

            Property(e => e.Description)
            .IsUnicode(false);

            Property(e => e.ClientNumber)
            .IsUnicode(false);

            Property(e => e.ClientName)
            .IsUnicode(false);

            Property(e => e.Partner)
            .IsUnicode(false);

            Property(e => e.Manager)
            .IsUnicode(false);

            Property(e => e.InsertBy)
            .IsUnicode(false);

            Property(e => e.UpdatedBy)
            .IsUnicode(false);

            HasMany(e => e.Workbooks)
            .WithRequired(e => e.MasterAuditFile)
            .WillCascadeOnDelete(false);
        }
    }
}
