using AuditManager.Model.EFModel.Active;

namespace AuditManager.Model.EFConfig.Active
{
    public class CUSTOM6Config : DbBaseEntityConfig<CUSTOM6>
    {
        public CUSTOM6Config()
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
