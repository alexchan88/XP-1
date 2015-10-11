using AuditManager.Model.EFModel.SSC;
using System.Data.Entity;

namespace AuditManager.EF.AmDbCtx
{
    public class SSCDbContext : AmBaseDbContext
    {
        public SSCDbContext()
            : base("ssc_conStr")
        {
            
        }

        public virtual DbSet<DRMSPDF> DRMSPDF { get; set; }
        public virtual DbSet<FileActivity> FileActivity { get; set; }
    }
}
