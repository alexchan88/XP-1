using AuditManager.Common;
using AuditManager.Model;
using AuditManager.Model.EFConfig.AM;
using AuditManager.Model.EFModel.AM;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AuditManager.EF.AmDbCtx
{
    public class AmDbContext : AmBaseDbContext
    {
        public AmDbContext()
            : base("aM_conStr")
        {
            //Database.SetInitializer<AmDbContext>(new AmDbInit());
            //this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<WsActivity> WsActivity { get; set; }
        public DbSet<WsMailStatus> WsMailStatus { get; set; }

        public DbSet<WsActivityHistory> WsActivityHistory { get; set; }
        
        public DbSet<WsMail> WsMail { get; set; }
        
        public DbSet<Closure> Closure { get; set; }

        public DbSet<WsFloatingField> WsFloatingField { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new WsActivityConfig());
            modelBuilder.Configurations.Add(new MailStatusConfig());

            modelBuilder.Configurations.Add(new WsActivityHistoryConfig());

            modelBuilder.Configurations.Add(new WsMailConfig());

            modelBuilder.Configurations.Add(new ClosureFileConfig());

            modelBuilder.Configurations.Add(new WsFloatingFieldConfig());
        }
    }

    public class AmDbInit : DropCreateDatabaseIfModelChanges<AmDbContext>
    {
        protected override void Seed(AmDbContext context)
        {
            foreach(var fI in typeof(WsActivityType).GetFields())
            {
                if(!fI.IsSpecialName)
                {
                    context.WsActivity.Add(new WsActivity{ WsActivityTypeId = fI.Name.ToEnum<WsActivityType>(), Description = fI.Name});
                }
            }

            foreach (var fI in typeof(WsMailStatusType).GetFields())
            {
                if (!fI.IsSpecialName)
                {
                    context.WsMailStatus.Add(new WsMailStatus { WsMailStatusId = fI.Name.ToEnum<WsMailStatusType>(), Description = fI.Name });
                }
            }

            context.SaveChanges();
        }
    }
}
