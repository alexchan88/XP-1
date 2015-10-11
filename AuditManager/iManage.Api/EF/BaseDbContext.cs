using System.Data.Entity;

namespace iManage.Api
{
    internal abstract class BaseDbContext : DbContext
    {
        public BaseDbContext(string conStr)
            : base(nameOrConnectionString: conStr)
        {
        }
    }
}
