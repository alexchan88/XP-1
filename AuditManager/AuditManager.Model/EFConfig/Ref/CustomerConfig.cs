using AuditManager.Model.EFModel.Ref;

namespace AuditManager.Model.EFConfig.Ref
{
    public class CustomerConfig : DbBaseEntityConfig<Customer> 
    {
        public CustomerConfig()
        {
            Property(e => e.CustomerNumber)
                .IsUnicode(false);

            Property(e => e.CustomerName)
                .IsUnicode(false);

            Property(e => e.NCPEmployeeId)
                .IsUnicode(false);

            Property(e => e.CustomerStatus)
                .IsUnicode(false);

            Property(e => e.SECRegistrationFlag)
                .IsUnicode(false);

            Property(e => e.SentinelId)
                .IsUnicode(false);

            Property(e => e.InsertBy)
                .IsUnicode(false);

            Property(e => e.UpdateBy)
                .IsUnicode(false);
        }
    }
}
