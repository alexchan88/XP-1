namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.DOCCACHE")]
    public partial class DOCCACHE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        [Required]
        [StringLength(254)]
        public string SERVER { get; set; }

        public double DOCNUM { get; set; }

        public int VERSION { get; set; }

        [Required]
        [StringLength(1)]
        public string LATESTVERSION { get; set; }

        [Required]
        [StringLength(1)]
        public string DELETED { get; set; }

        [Required]
        [StringLength(254)]
        public string DOCLOC { get; set; }

        public DateTime LASTACCESSED { get; set; }

        public DateTime EDITWHEN { get; set; }

        public DateTime CREATEDWHEN { get; set; }

        [Required]
        [StringLength(1)]
        public string DOCINUSE { get; set; }

        [StringLength(64)]
        public string INUSEBY { get; set; }

        [Required]
        [StringLength(1)]
        public string UPLOAD { get; set; }

        public int REQUESTS { get; set; }
    }
}
