namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.TYPEMAP")]
    public partial class TYPEMAP
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(254)]
        public string DETECT_TYPE { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DETECT_VALUE { get; set; }

        [StringLength(254)]
        public string DESCRIPTION { get; set; }

        [StringLength(32)]
        public string TYPEALIAS { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        public string LEGACYPRIMARYDOCTYPE { get; set; }

        public virtual DOCTYPE DOCTYPE { get; set; }
    }
}
