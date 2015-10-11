using IM.Wrapper.Model;
using IM.Wrapper.Utility;
using System.Data.Entity;

namespace IM.Wrapper.EF
{
    internal class IMDbContext : BaseDbContext
    {
        public IMDbContext(IMInstance iMInstance)
            : base(SqlConStr.GetSqlConStr(iMInstance))
            //: base("active_conStr")
        {
            
        }

        public DbSet<DOCMASTER> DOCMASTER { get; set; }
        public DbSet<PROJECT> PROJECT { get; set; }
        public DbSet<DOCSERVER> DOCSERVERS { get; set; }

        //public DbSet<DOCHISTORY> DOCHISTORY { get; set; }
    }
}
