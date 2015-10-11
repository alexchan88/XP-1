namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.DOC_NVPS")]
    public partial class DOC_NVPS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        public double DOCNUM { get; set; }

        public int VERSION { get; set; }

        [Required]
        [StringLength(64)]
        public string NAME { get; set; }

        [StringLength(254)]
        public string VALUE { get; set; }
    }
}
