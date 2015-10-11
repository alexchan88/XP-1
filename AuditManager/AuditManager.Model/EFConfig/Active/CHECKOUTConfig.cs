using AuditManager.Model.EFModel.Active;

namespace AuditManager.Model.EFConfig.Active
{
    public class CHECKOUTConfig : DbBaseEntityConfig<CHECKOUT>
    {
        public CHECKOUTConfig()
        {
            Property(e => e.PORTABLE)
            .IsFixedLength()
            .IsUnicode(false);
        }
    }
}
