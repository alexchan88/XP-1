namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.DOCARTIFACT")]
    public partial class DOCARTIFACT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        public double ORIG_DOCNUM { get; set; }

        public int ORIG_VERSION { get; set; }

        [Required]
        [StringLength(32)]
        public string DEST_LIB { get; set; }

        public double DEST_DOCNUM { get; set; }

        public int DEST_VERSION { get; set; }

        public DateTime CREATED_DATE { get; set; }
    }
}
