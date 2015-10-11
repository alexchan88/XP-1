namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.NODELOC")]
    public partial class NODELOC
    {
        [Key]
        [StringLength(32)]
        public string LOGNODEADDR { get; set; }

        [StringLength(254)]
        public string LOCATION { get; set; }
    }
}
