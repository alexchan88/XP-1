using AuditManager.Model.EFModel.S2;

namespace AuditManager.Model.EFConfig.S2
{
    public class WorkbookConfig : DbBaseEntityConfig<Workbook>
    {
        public WorkbookConfig()
        {
            Property(e => e.WorkbookGuid)
                .IsUnicode(false);

            Property(e => e.WorkbookName)
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
