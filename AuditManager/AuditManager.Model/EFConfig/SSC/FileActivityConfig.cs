using AuditManager.Model.EFModel.SSC;

namespace AuditManager.Model.EFConfig.SSC
{
    public class FileActivityConfig : DbBaseEntityConfig<FileActivity>
    {
        public FileActivityConfig()
        {
            Property(e => e.EngagementNumber)
                .IsUnicode(false);

            Property(e => e.MachineName)
                .IsUnicode(false);

            Property(e => e.UserId)
                .IsUnicode(false);

            Property(e => e.FAComments)
                .IsUnicode(false);

            Property(e => e.Deleted)
                .IsFixedLength()
                .IsUnicode(false);

            Property(e => e.InsertBy)
                .IsUnicode(false);

            Property(e => e.LastUpdateBy)
                .IsUnicode(false);

            HasMany(e => e.DRMSPDFs)
                .WithRequired(e => e.FileActivity)
                .WillCascadeOnDelete(false);
        }
    }
}
