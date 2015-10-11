namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.SYNC_PREFERENCES")]
    public partial class SYNC_PREFERENCES
    {
        [Key]
        public int SID { get; set; }

        public int SYNC_PREFS_SID { get; set; }

        [Required]
        [StringLength(64)]
        public string NAME { get; set; }

        [StringLength(254)]
        public string VALUE { get; set; }
    }
}
