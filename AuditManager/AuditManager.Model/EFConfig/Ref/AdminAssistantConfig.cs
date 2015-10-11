using AuditManager.Model.EFModel.Ref;

namespace AuditManager.Model.EFConfig.Ref
{
    public class AdminAssistantConfig : DbBaseEntityConfig<AdminAssistant>
    {
        public AdminAssistantConfig()
        {
            Property(e => e.AssistantEmployeeId)
                .IsUnicode(false);

            Property(e => e.PartnerEmployeeID)
                .IsUnicode(false);

            Property(e => e.InsertBy)
                .IsUnicode(false);

            Property(e => e.UpdateBy)
                .IsUnicode(false);

            HasRequired(x => x.AssistantEmployeeId)
                .WithMany()
                .HasForeignKey(x => x.Assistant)
                .WillCascadeOnDelete(false);
        }
        
    }
}
