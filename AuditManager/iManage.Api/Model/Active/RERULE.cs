namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.RERULES")]
    public partial class RERULE
    {
        public RERULE()
        {
            REEVENTDEFs = new HashSet<REEVENTDEF>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RULE_SID { get; set; }

        [Required]
        [StringLength(64)]
        public string OWNER { get; set; }

        public int CONTENT_T { get; set; }

        public double CONTENTID { get; set; }

        public int? DOCVERSION { get; set; }

        public string RULEACTIONDATA { get; set; }

        public int HANDLER_SID { get; set; }

        [Required]
        [StringLength(1)]
        public string ENABLED_F { get; set; }

        [StringLength(254)]
        public string DESCRIPTION { get; set; }

        [StringLength(254)]
        public string LOCATOR { get; set; }

        [StringLength(254)]
        public string EMAIL_SUBJECT { get; set; }

        public virtual RERULEHANDLER RERULEHANDLER { get; set; }

        public virtual ICollection<REEVENTDEF> REEVENTDEFs { get; set; }
    }
}
