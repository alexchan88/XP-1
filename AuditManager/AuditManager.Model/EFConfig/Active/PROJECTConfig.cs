using AuditManager.Model.EFModel.Active;

namespace AuditManager.Model.EFConfig.Active
{
    public class PROJECTConfig : DbBaseEntityConfig<PROJECT>
    {
        public PROJECTConfig()
        {
            Property(e => e.DEFAULT_SECURITY)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.IS_SECURED)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.PRJ_STATE)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.PRJ_PUBLIC)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.INHERITS_SECURITY)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.IS_EXTERNAL)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.EXTRNL_AS_NRML)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.IS_DOC_SVD_SRCH)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.IS_PRJ_SVD_SRCH)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.IS_HIDDEN)
            .IsFixedLength()
            .IsUnicode(false);


            HasMany(e => e.PROJECTS1)
            .WithOptional(e => e.PROJECT1)
            .HasForeignKey(e => e.PRJ_PID);
        }
    }
}
