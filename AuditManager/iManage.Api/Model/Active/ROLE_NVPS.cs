namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.ROLE_NVPS")]
    public partial class ROLE_NVPS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        [Required]
        [StringLength(32)]
        public string ALIAS { get; set; }

        [Required]
        [StringLength(64)]
        public string NAME { get; set; }

        [StringLength(254)]
        public string VALUE { get; set; }

        public virtual ROLE ROLE { get; set; }
    }
}
