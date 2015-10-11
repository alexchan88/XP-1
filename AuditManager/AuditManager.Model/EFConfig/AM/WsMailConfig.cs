using AuditManager.Model.EFModel.AM;

namespace AuditManager.Model.EFConfig.AM
{
    public class WsMailConfig : DbBaseEntityConfig<WsMail>
    {
        public WsMailConfig()
        {
            HasKey(x => x.WsActivityHistoryId);

            HasRequired(x => x.WsActivityHistory)
                .WithRequiredDependent();
            
            Property(x => x.RecepientArray).IsRequired();
            Property(x => x.MailText).IsOptional();

            HasRequired(x => x.WsMailStatus)
                .WithMany()
                .HasForeignKey(x => x.WsMailStatusTypeId)
                .WillCascadeOnDelete(false);
        }
    }
}
