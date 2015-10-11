namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.KEYWORDS")]
    public partial class KEYWORD
    {
        public KEYWORD()
        {
            DOC_KEYWORDS = new HashSet<DOC_KEYWORDS>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        [Required]
        [StringLength(32)]
        public string ALIAS { get; set; }

        [StringLength(254)]
        public string DESCRIPTION { get; set; }

        public virtual ICollection<DOC_KEYWORDS> DOC_KEYWORDS { get; set; }
    }
}
