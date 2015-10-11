namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.SID_TABLE")]
    public partial class SID_TABLE
    {
        [Key]
        [StringLength(32)]
        public string TABLE_NAME { get; set; }

        public int SID { get; set; }
    }
}
