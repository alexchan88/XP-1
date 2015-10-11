using AuditManager.Model.EFModel.Active;

namespace AuditManager.Model.EFConfig.Active
{
    public class CUSTOM4Config : DbBaseEntityConfig<CUSTOM4>
    {
        public CUSTOM4Config()
        {
            Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
