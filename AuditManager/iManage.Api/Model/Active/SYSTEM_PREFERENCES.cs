namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.SYSTEM_PREFERENCES")]
    public partial class SYSTEM_PREFERENCES
    {
        public SYSTEM_PREFERENCES()
        {
            USER_PREFERENCES = new HashSet<USER_PREFERENCES>();
        }

        [Key]
        public int SID { get; set; }

        [Required]
        [StringLength(32)]
        public string ALIAS { get; set; }

        [StringLength(254)]
        public string DESCRIPTION { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_ENABLED { get; set; }

        public virtual ICollection<USER_PREFERENCES> USER_PREFERENCES { get; set; }
    }
}
