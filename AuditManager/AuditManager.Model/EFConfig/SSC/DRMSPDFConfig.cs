using AuditManager.Model.EFModel.SSC;

namespace AuditManager.Model.EFConfig.SSC
{
    public class DRMSPDFConfig : DbBaseEntityConfig<DRMSPDF>
    {
        public DRMSPDFConfig()
        {
            Property(e => e.DocName)
                .IsUnicode(false);

            Property(e => e.KDrivePath)
                .IsUnicode(false);
        }
    }
}
