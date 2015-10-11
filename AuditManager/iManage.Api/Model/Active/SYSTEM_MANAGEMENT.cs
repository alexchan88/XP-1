namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.SYSTEM_MANAGEMENT")]
    public partial class SYSTEM_MANAGEMENT
    {
        [Key]
        [Column(Order = 0)]
        public int SID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PWD_EXPIRED_DAYS { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PWD_EXP_WARN_DAYS { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LOGON_ATTEMPTS { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(32)]
        public string SCHEMA_VERSION { get; set; }

        [StringLength(254)]
        public string COMMENTS { get; set; }
    }
}
