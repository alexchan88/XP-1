using AuditManager.Common;
using System.Data.Entity;

namespace AuditManager.EF.AmDbCtx
{
    public abstract class AmBaseDbContext : DbContext
    {
        public AmBaseDbContext(string conStr)
            : base(nameOrConnectionString: conStr)
        {
            Database.Log = s => DbLog.LogDbInfo(s);
            Database.CommandTimeout = ConfigUtility.SqlCommandTimeout;
        }
    }

    public abstract class BaseDbContext : DbContext
    {
        public BaseDbContext(string conStr)
            : base(nameOrConnectionString: conStr)
        {
            Database.Log = s => DbLog.LogDbInfo(s);
        }
    }
}
