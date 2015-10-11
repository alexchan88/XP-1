using AuditManager.Model.EFModel.Active;

namespace AuditManager.Model.EFConfig.Active
{
    public class DOCMASTERConfig : DbBaseEntityConfig<DOCMASTER>
    {
        public DOCMASTERConfig()
        {
            Property(e => e.DOCINUSE)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.INDEXED)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.CHECKEDOUT)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.COMINDEX)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.ARCHIVE_REQ)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.ARCHIVED)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.INDEXABLE)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.ISRELATED)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.CBOOL1)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.CBOOL2)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.CBOOL3)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.CBOOL4)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.PENDING_CLOSE)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.DEFAULT_SECURITY)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.IS_SECURED)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.TYPE)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.IS_EXTERNAL)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.HAS_ATTACHMENT)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.EXTRNL_AS_NRML)
            .IsFixedLength()
            .IsUnicode(false);


            Property(e => e.IS_HIPAA)
            .IsFixedLength()
            .IsUnicode(false);


            HasMany(e => e.PROJECTS)
            .WithOptional(e => e.DOCMASTER)
            .HasForeignKey(e => new { e.DOCNUM, e.VERSION });
        }
    }
}
