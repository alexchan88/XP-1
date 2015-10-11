namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.SEARCH_PROFILES")]
    public partial class SEARCH_PROFILES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        [Required]
        [StringLength(254)]
        public string DEFAULT_SECURITY { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_SECURED { get; set; }

        [Required]
        [StringLength(254)]
        public string NAME { get; set; }

        [Required]
        [StringLength(254)]
        public string SCOPE { get; set; }

        [Required]
        [StringLength(64)]
        public string OWNER { get; set; }

        [StringLength(254)]
        public string DESCRIPTION { get; set; }
    }
}
