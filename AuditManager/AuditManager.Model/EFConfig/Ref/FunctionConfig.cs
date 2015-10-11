using AuditManager.Model.EFModel.Ref;

namespace AuditManager.Model.EFConfig.Ref
{
    public class FunctionConfig : DbBaseEntityConfig<Function>
    {
        public FunctionConfig()
        {
            Property(e => e.FucntionID)
                .IsUnicode(false);

            Property(e => e.FunctionName)
                .IsUnicode(false);

            Property(e => e.FunctionDesc)
                .IsUnicode(false);

            Property(e => e.InsertBy)
                .IsUnicode(false);

            Property(e => e.UpdateBy)
                .IsUnicode(false);
        }
    }
}
