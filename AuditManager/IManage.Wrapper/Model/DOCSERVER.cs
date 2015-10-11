namespace IM.Wrapper.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.DOCSERVERS")]
    internal partial class DOCSERVER : BaseModel
    {
        [Key]
        [Column("DOCSERVER")]
        [StringLength(32)]
        public string DOCSERVER1 { get; set; }

        [Required]
        [StringLength(32)]
        public string OS { get; set; }

        [Required]
        [StringLength(254)]
        public string LOCATION { get; set; }

        [StringLength(254)]
        public string ROOTPATH { get; set; }

        [StringLength(254)]
        public string CONTEXT { get; set; }

        [StringLength(1)]
        public string READONLY { get; set; }

        [StringLength(1)]
        public string ENABLED { get; set; }

        [StringLength(32)]
        public string ACCESS_METHOD { get; set; }

        [StringLength(1)]
        public string TYPE { get; set; }

        [Required]
        [StringLength(1)]
        public string SECURE { get; set; }
    }
}

//protected override void OnModelCreating(DbModelBuilder modelBuilder)
//{
//    modelBuilder.Entity<DOCSERVER>()
//        .Property(e => e.READONLY)
//        .IsFixedLength()
//        .IsUnicode(false);

//    modelBuilder.Entity<DOCSERVER>()
//        .Property(e => e.ENABLED)
//        .IsFixedLength()
//        .IsUnicode(false);

//    modelBuilder.Entity<DOCSERVER>()
//        .Property(e => e.TYPE)
//        .IsFixedLength()
//        .IsUnicode(false);

//    modelBuilder.Entity<DOCSERVER>()
//        .Property(e => e.SECURE)
//        .IsFixedLength()
//        .IsUnicode(false);
//}
