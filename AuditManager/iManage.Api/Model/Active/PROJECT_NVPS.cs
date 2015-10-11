namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.PROJECT_NVPS")]
    public partial class PROJECT_NVPS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        public double PRJ_ID { get; set; }

        [Required]
        [StringLength(64)]
        public string NAME { get; set; }

        [StringLength(254)]
        public string VALUE { get; set; }

        public virtual PROJECT PROJECT { get; set; }
    }
}
