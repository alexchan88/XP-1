namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.SECURITY_TEMPLATE")]
    public partial class SECURITY_TEMPLATE
    {
        public SECURITY_TEMPLATE()
        {
            GRP_SCTY_ACC_TEMP = new HashSet<GRP_SCTY_ACC_TEMP>();
            USR_SCTY_ACC_TEMP = new HashSet<USR_SCTY_ACC_TEMP>();
            USR_SCTY_TEMP_ASSC = new HashSet<USR_SCTY_TEMP_ASSC>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(254)]
        public string NAME { get; set; }

        [StringLength(254)]
        public string DESCRIPTION { get; set; }

        [Required]
        [StringLength(1)]
        public string DEFAULT_SECURITY { get; set; }

        public virtual ICollection<GRP_SCTY_ACC_TEMP> GRP_SCTY_ACC_TEMP { get; set; }

        public virtual ICollection<USR_SCTY_ACC_TEMP> USR_SCTY_ACC_TEMP { get; set; }

        public virtual ICollection<USR_SCTY_TEMP_ASSC> USR_SCTY_TEMP_ASSC { get; set; }
    }
}
