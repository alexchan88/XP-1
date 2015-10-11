namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.TEMPLATE_CONTROLS")]
    public partial class TEMPLATE_CONTROLS
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TEMPLATE_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CONTROL_ID { get; set; }

        public int? CONTROL_TYPE { get; set; }

        [StringLength(254)]
        public string CAPTION { get; set; }

        public int? C_TOP { get; set; }

        public int? C_LEFT { get; set; }

        public int? C_BOTTOM { get; set; }

        public int? C_RIGHT { get; set; }

        public int? TAB_ORDER { get; set; }

        public int? VIEW_TYPE { get; set; }

        [StringLength(254)]
        public string DEFAULT_VALUE { get; set; }

        [StringLength(254)]
        public string HELP_STRING { get; set; }

        public virtual FORM_TYPES FORM_TYPES { get; set; }
    }
}
