using AuditManager.Model.EFModel.AM;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace AuditManager.Model.EFConfig.AM
{
    public class WsActivityConfig : DbBaseEntityConfig<WsActivity>
    {
        public WsActivityConfig()
        {
            HasKey(x => x.WsActivityTypeId);
            Property(x => x.Description).IsRequired().HasMaxLength(64)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_WsActivity") { IsUnique = true }));
        }
    }

    public class MailStatusConfig : DbBaseEntityConfig<WsMailStatus>
    {
        public MailStatusConfig()
        {
            HasKey(x => x.WsMailStatusId);
            Property(x => x.Description).IsRequired().HasMaxLength(64)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_MailStatus") { IsUnique = true }));
        }
    }
}
