namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.FORMS")]
    public partial class FORM
    {
        public FORM()
        {
            FORM_CONTROLS = new HashSet<FORM_CONTROLS>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FORM_ID { get; set; }

        public int? FORM_TYPE_ID { get; set; }

        [StringLength(254)]
        public string CAPTION { get; set; }

        [StringLength(254)]
        public string DESCRIPT { get; set; }

        public int? X_POS { get; set; }

        public int? Y_POS { get; set; }

        public int? HEIGHT { get; set; }

        public int? WIDTH { get; set; }

        [StringLength(254)]
        public string ATTRIBUTES { get; set; }

        [StringLength(1)]
        public string DELETEABLE { get; set; }

        public int? BKGRND_COLOR { get; set; }

        public int? FRGRND_COLOR { get; set; }

        [StringLength(254)]
        public string ATTRIBUTE1 { get; set; }

        [StringLength(254)]
        public string ATTRIBUTE2 { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LOCALE { get; set; }

        public virtual ICollection<FORM_CONTROLS> FORM_CONTROLS { get; set; }

        public virtual FORM_TYPES FORM_TYPES { get; set; }
    }
}
