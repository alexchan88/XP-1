namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.USR_SCTY_TEMP_ASSC")]
    public partial class USR_SCTY_TEMP_ASSC
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SCTY_TEMP_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int USERNUM { get; set; }

        [Required]
        [StringLength(1)]
        public string DEFAULT_VALUE { get; set; }

        public virtual SECURITY_TEMPLATE SECURITY_TEMPLATE { get; set; }
    }
}
