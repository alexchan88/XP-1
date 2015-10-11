namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.KMTAG")]
    public partial class KMTAG
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        public double? DOCNUM { get; set; }

        public int? VERSION { get; set; }

        public double? PRJ_ID { get; set; }

        [StringLength(64)]
        public string USERID { get; set; }

        [StringLength(1)]
        public string DEFAULT_SECURITY { get; set; }

        [Required]
        [StringLength(32)]
        public string OBJECTTYPE { get; set; }

        public DateTime CREATEWHEN { get; set; }

        public DateTime EDITWHEN { get; set; }

        public string TAGVALUE { get; set; }

        public virtual DOCMASTER DOCMASTER { get; set; }

        public virtual DOCUSER DOCUSER { get; set; }

        public virtual PROJECT PROJECT { get; set; }
    }
}
