using AuditManager.Model.EFModel.S2;
using System.Data.Entity;

namespace AuditManager.EF.AmDbCtx
{
    public class S2DbContext : AmBaseDbContext
    {
        public S2DbContext()
            : base("s2_conStr")
        { 
        
        }

        public virtual DbSet<Closure> Closure { get; set; }
        public virtual DbSet<MasterAuditFile> MasterAuditFile { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<WorkbookReview> WorkbookReview { get; set; }
        public virtual DbSet<Workbook> Workbook { get; set; }
        public virtual DbSet<DRMSExportedFile> DRMSExportedFile { get; set; }
    }
}
