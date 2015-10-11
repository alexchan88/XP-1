using AuditManager.Model.EFModel.S2;

namespace AuditManager.Model.EFConfig.S2
{
    public class WorkbookReviewConfig : DbBaseEntityConfig<WorkbookReview>
    {
        public WorkbookReviewConfig()
        {
            Property(e => e.WorkbookReviewGuid)
                .IsUnicode(false);

            Property(e => e.ReviewName)
                .IsUnicode(false);

            Property(e => e.Description)
                .IsUnicode(false);

            Property(e => e.InsertBy)
                .IsUnicode(false);

            Property(e => e.UpdatedBy)
                .IsUnicode(false);
        }
    }
}
