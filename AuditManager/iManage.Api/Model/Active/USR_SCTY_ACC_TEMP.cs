namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.USR_SCTY_ACC_TEMP")]
    public partial class USR_SCTY_ACC_TEMP
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SCTY_TEMP_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int USERNUM { get; set; }

        public int ACCESS_RIGHT { get; set; }

        public virtual SECURITY_TEMPLATE SECURITY_TEMPLATE { get; set; }
    }
}
