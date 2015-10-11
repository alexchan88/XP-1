namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.SRCH_PROF_ACCESS")]
    public partial class SRCH_PROF_ACCESS
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        [StringLength(1)]
        public string OBJECT_TYPE { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int USER_GP_ID { get; set; }

        public int ACCESS_RIGHT { get; set; }
    }
}
