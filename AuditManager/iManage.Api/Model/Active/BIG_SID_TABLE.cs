namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.BIG_SID_TABLE")]
    public partial class BIG_SID_TABLE
    {
        [Key]
        [StringLength(32)]
        public string TABLE_NAME { get; set; }

        public long SID { get; set; }
    }
}
