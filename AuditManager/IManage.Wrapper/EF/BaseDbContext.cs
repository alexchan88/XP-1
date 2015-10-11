using System.Data.Entity;

namespace IM.Wrapper.EF
{
    internal class BaseDbContext : DbContext
    {
        public BaseDbContext(string conStr)
            : base(nameOrConnectionString: conStr)
            //: base(conStr)
        {
            
        }
    }
}
