using AuditManager.Model.EFModel.S2;

namespace AuditManager.Model.EFConfig.S2
{
    public class StatusConfig : DbBaseEntityConfig<Status>
    {
        public StatusConfig()
        {
            Property(e => e.Status1)
            .IsUnicode(false);
            
            Property(e => e.Description)
            .IsUnicode(false);
            
            Property(e => e.InsertBy)
            .IsUnicode(false);
            
            Property(e => e.UpdatedBy)
            .IsUnicode(false);
            
            HasMany(e => e.WorkbookReviews)
            .WithRequired(e => e.Status)
            .HasForeignKey(e => e.StatusId)
            .WillCascadeOnDelete(false);
            
            HasMany(e => e.WorkbookReviews1)
            .WithRequired(e => e.Status1)
            .HasForeignKey(e => e.StatusId)
            .WillCascadeOnDelete(false);
        }
    }
}
