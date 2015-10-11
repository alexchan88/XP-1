namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.CHANGE_EVENT_PROPERTIES")]
    public partial class CHANGE_EVENT_PROPERTIES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SID { get; set; }

        public long EVENT_RSID { get; set; }

        [Required]
        [StringLength(254)]
        public string NAME { get; set; }

        public int? VALUE_INT { get; set; }

        [StringLength(254)]
        public string VALUE_CHAR { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] VALUE_DATE { get; set; }

        public double? VALUE_FLOAT { get; set; }

        public virtual CHANGE_EVENTS CHANGE_EVENTS { get; set; }
    }
}
