namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.SSR_RETRY")]
    public partial class SSR_RETRY
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DOCNUM { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VERSION { get; set; }

        public int RETRY_COUNT { get; set; }

        public DateTime LAST_UPDATED { get; set; }

        public DateTime INSERT_TS { get; set; }

        [StringLength(255)]
        public string COMMENTS { get; set; }
    }
}
