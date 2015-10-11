namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.RELATED_DOCS")]
    public partial class RELATED_DOCS
    {
        [Key]
        [Column(Order = 0)]
        public double FRDOCNUM { get; set; }

        [Key]
        [Column(Order = 1)]
        public double TODOCNUM { get; set; }

        [StringLength(254)]
        public string COMMENTS { get; set; }
    }
}
