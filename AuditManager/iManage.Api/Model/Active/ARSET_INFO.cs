namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.ARSET_INFO")]
    public partial class ARSET_INFO
    {
        [Key]
        [Column(Order = 0)]
        public double ARCHV_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public double DOCNUM { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VERSION { get; set; }

        [StringLength(1)]
        public string RESTORED { get; set; }

        public DateTime? RESTORE_DATE { get; set; }

        [StringLength(512)]
        public string ZDK { get; set; }

        public virtual ARCHIVETBL ARCHIVETBL { get; set; }
    }
}
