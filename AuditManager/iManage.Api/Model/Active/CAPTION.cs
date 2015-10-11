namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.CAPTIONS")]
    public partial class CAPTION
    {
        public int? SS_NUM { get; set; }

        public int? CAPTION_NUM { get; set; }

        [Column("CAPTION")]
        [StringLength(254)]
        public string CAPTION1 { get; set; }

        [StringLength(254)]
        public string PROFILE_FIELD { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LOCALE { get; set; }
    }
}
