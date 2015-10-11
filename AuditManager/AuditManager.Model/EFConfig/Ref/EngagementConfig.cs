using AuditManager.Model.EFModel.Ref;

namespace AuditManager.Model.EFConfig.Ref
{
    public class EngagementConfig : DbBaseEntityConfig<Engagement>
    {
        public EngagementConfig()
        {
            Property(e => e.EngagementNumber)
                .IsUnicode(false);

            Property(e => e.EngagementDescription)
                .IsUnicode(false);

            Property(e => e.EngagementStatus)
                .IsUnicode(false);

            Property(e => e.InsertBy)
                .IsUnicode(false);

            Property(e => e.UpdateBy)
                .IsUnicode(false);

            //Property(e => e.CustomerNumber)
            //    .IsUnicode(false);

            HasRequired(x => x.CustomerNumber)
                .WithMany()
                .HasForeignKey(x => x.Customer)
                .WillCascadeOnDelete(false);

            //Property(e => e.EMEmployeeId)
            //    .IsUnicode(false);

            //Property(e => e.EPEmployeeId)
            //    .IsUnicode(false);

            //HasRequired(x => x.EMEmployeeId)
            //    .WithMany()
            //    .HasForeignKey(x => x.Manager)
            //    .WillCascadeOnDelete(false);

            //HasRequired(x => x.EPEmployeeId)
            //    .WithMany()
            //    .HasForeignKey(x => x.Partner)
            //    .WillCascadeOnDelete(false);

            HasRequired(x => x.EMEmployeeId)
                .WithMany()
                .HasForeignKey(x => x.Manager)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.EPEmployeeId)
                .WithMany()
                .HasForeignKey(x => x.Partner)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.EPEmployeeId)
                .WithMany()
                .HasForeignKey(x => x.AdminAssistant)
                .WillCascadeOnDelete(false);
        }
    }
}
