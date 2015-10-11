namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.EMAIL_ATTRS")]
    public partial class EMAIL_ATTRS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        public double DOCNUM { get; set; }

        public int VERSION { get; set; }

        [Required]
        [StringLength(1)]
        public string ORIGIN { get; set; }

        [Required]
        [StringLength(1)]
        public string STATUS { get; set; }

        public DateTime EDITWHEN_TS { get; set; }

        public DateTime? RECONCILED_TS { get; set; }

        public int? RETRY { get; set; }

        [StringLength(512)]
        public string STORAGEID { get; set; }

        [StringLength(512)]
        public string REASON { get; set; }

        [Required]
        [StringLength(1)]
        public string TYPE { get; set; }
    }
}
