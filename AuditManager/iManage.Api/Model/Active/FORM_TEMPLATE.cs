namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.FORM_TEMPLATE")]
    public partial class FORM_TEMPLATE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TEMPLATE_ID { get; set; }

        public int? FORM_TYPE_ID { get; set; }

        [StringLength(254)]
        public string CAPTION { get; set; }

        [StringLength(254)]
        public string DESCRIPT { get; set; }

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
    }
}
