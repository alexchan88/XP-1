namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.FORM_TYPES")]
    public partial class FORM_TYPES
    {
        public FORM_TYPES()
        {
            FORMS = new HashSet<FORM>();
            PALETTEs = new HashSet<PALETTE>();
            TEMPLATE_CONTROLS = new HashSet<TEMPLATE_CONTROLS>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FORM_TYPE_ID { get; set; }

        [StringLength(254)]
        public string DESCRIPT { get; set; }

        public virtual ICollection<FORM> FORMS { get; set; }

        public virtual ICollection<PALETTE> PALETTEs { get; set; }

        public virtual ICollection<TEMPLATE_CONTROLS> TEMPLATE_CONTROLS { get; set; }
    }
}
