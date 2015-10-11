namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.USER_PREFERENCES")]
    public partial class USER_PREFERENCES
    {
        [Key]
        public int SID { get; set; }

        public int USERNUM { get; set; }

        public int SYSPREFS_SID { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_ENABLED { get; set; }

        public virtual SYSTEM_PREFERENCES SYSTEM_PREFERENCES { get; set; }
    }
}
