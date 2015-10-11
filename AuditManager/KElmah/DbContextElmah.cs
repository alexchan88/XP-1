namespace KElmah
{
    using System.Data.Entity;

    public partial class DbContextElmah : DbContext
    {
        public DbContextElmah()
            : base("name=errorLog_conStr")
        {
            //Database.Log = s => Debug.Write(s);
        }

        public virtual DbSet<ELMAH_Error> ELMAH_Error { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
