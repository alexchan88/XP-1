namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.REEVENTDEF")]
    public partial class REEVENTDEF
    {
        public REEVENTDEF()
        {
            REEVENTLOGs = new HashSet<REEVENTLOG>();
            RERULES = new HashSet<RERULE>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EVENT_T { get; set; }

        [Required]
        [StringLength(1)]
        public string ENABLE_F { get; set; }

        [Required]
        [StringLength(254)]
        public string DESCRIPTION { get; set; }

        public virtual ICollection<REEVENTLOG> REEVENTLOGs { get; set; }

        public virtual ICollection<RERULE> RERULES { get; set; }
    }
}
