using AuditManager.Model.EFModel.Active;

namespace AuditManager.Model.EFConfig.Active
{
    public class CUSTOM2Config : DbBaseEntityConfig<CUSTOM2>
    {
        public CUSTOM2Config()
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
