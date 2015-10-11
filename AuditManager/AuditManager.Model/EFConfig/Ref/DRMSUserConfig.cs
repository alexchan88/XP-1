using AuditManager.Model.EFModel.Ref;

namespace AuditManager.Model.EFConfig.Ref
{
    public class DRMSUserConfig : DbBaseEntityConfig<DRMSUser>
    {
        public DRMSUserConfig()
        {
            Property(e => e.DSID)
                .IsUnicode(false);

            Property(e => e.EmplID)
                .IsUnicode(false);

            Property(e => e.FirstName)
                .IsUnicode(false);

            Property(e => e.LastName)
                .IsUnicode(false);

            Property(e => e.SamAccountName)
                .IsUnicode(false);

            Property(e => e.DisplayName)
                .IsUnicode(false);

            Property(e => e.Email)
                .IsUnicode(false);

            //Property(e => e.GoFunction)
            //    .IsUnicode(false);

            HasRequired(x => x.GoFunction)
                .WithMany()
                .HasForeignKey(x => x.Function)
                .WillCascadeOnDelete(false);

            Property(e => e.JobCode)
                .IsUnicode(false);

            Property(e => e.TransactionType)
                .IsUnicode(false);

            Property(e => e.InsertBy)
                .IsUnicode(false);

            Property(e => e.UpdateBy)
                .IsUnicode(false);

        }
    }
}
