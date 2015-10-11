namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.PROJARTIFACT")]
    public partial class PROJARTIFACT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        public double ORIG_PROJECT_ID { get; set; }

        [Required]
        [StringLength(32)]
        public string DEST_LIB { get; set; }

        public double DEST_PROJ_ID { get; set; }

        public DateTime CREATED_DATE { get; set; }
    }
}
