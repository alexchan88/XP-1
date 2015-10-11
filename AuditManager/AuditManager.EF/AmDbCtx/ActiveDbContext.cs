using AuditManager.Model.EFModel.Active;
using System.Data.Entity;

namespace AuditManager.EF.AmDbCtx
{
    public class ActiveDbContext : AmBaseDbContext
    {
        public ActiveDbContext()
            : base("active_conStr")
        {
            
        }

        public DbSet<DOCMASTER> DOCMASTER { get; set; }
        public DbSet<PROJECT> PROJECT { get; set; }
        public DbSet<DOCUSER> DOCUSER { get; set; }

        public virtual DbSet<CHECKOUT> CHECKOUT { get; set; }
        public virtual DbSet<CUSTOM1> CUSTOM1 { get; set; }
        public virtual DbSet<CUSTOM2> CUSTOM2 { get; set; }
        public virtual DbSet<CUSTOM4> CUSTOM4 { get; set; }
        public virtual DbSet<CUSTOM6> CUSTOM6 { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //throw new UnintentionalCodeFirstException();

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Configurations.Add(new DOCMASTERConfig());
            //modelBuilder.Configurations.Add(new PROJECTConfig());

            //modelBuilder.Configurations.Add(new CHECKOUTConfig());
            //modelBuilder.Configurations.Add(new CUSTOM1Config());
            //modelBuilder.Configurations.Add(new CUSTOM2Config());
            //modelBuilder.Configurations.Add(new CUSTOM4Config());
            //modelBuilder.Configurations.Add(new CUSTOM6Config());
        }
    }
}
