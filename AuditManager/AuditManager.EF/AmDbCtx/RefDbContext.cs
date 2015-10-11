using AuditManager.Model.EFModel.Ref;
using System.Data.Entity;

namespace AuditManager.EF.AmDbCtx
{
    public class RefDbContext : AmBaseDbContext
    {
        public RefDbContext()
            : base("ref_conStr")
        {
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<AdminAssistant> AdminAssistant { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<DRMSUser> DRMSUser { get; set; }
        public virtual DbSet<Engagement> Engagement { get; set; }
        public virtual DbSet<Function> Function { get; set; }
    }
}
