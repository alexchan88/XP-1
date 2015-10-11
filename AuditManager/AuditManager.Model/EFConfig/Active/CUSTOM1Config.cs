using AuditManager.Model.EFModel.Active;

namespace AuditManager.Model.EFConfig.Active
{
    public class CUSTOM1Config : DbBaseEntityConfig<CUSTOM1>
    {
        public CUSTOM1Config()
        {
            Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            HasMany(e => e.CUSTOM2)
                .WithRequired(e => e.CUSTOM1)
                .HasForeignKey(e => e.CPARENT_ALIAS)
                .WillCascadeOnDelete(false);
        }
    }
}
