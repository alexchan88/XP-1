using AuditManager.Model.EFModel.S2;

namespace AuditManager.Model.EFConfig.S2
{
    public class DRMSExportedFileConfig : DbBaseEntityConfig<DRMSExportedFile>
    {
        public DRMSExportedFileConfig()
        {
            Property(e => e.DRMSFolderPath)
                .IsUnicode(false);

            Property(e => e.FileName)
                .IsUnicode(false);

            Property(e => e.InsertBy)
                .IsUnicode(false);

            Property(e => e.UpdatedBy)
                .IsUnicode(false);
        }
    }
}
