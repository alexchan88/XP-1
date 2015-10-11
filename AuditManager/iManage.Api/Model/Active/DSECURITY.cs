namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.DSECURITY")]
    public partial class DSECURITY
    {
        [Key]
        [StringLength(8)]
        public string SECNAME { get; set; }

        [StringLength(254)]
        public string DESCRIPTION { get; set; }

        public int? IRM { get; set; }

        [StringLength(254)]
        public string ACL1 { get; set; }

        [StringLength(254)]
        public string ACL2 { get; set; }
    }
}
