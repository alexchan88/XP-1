namespace AuditManager.Model.EFModel.Active
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.CHECKOUT")]
    public partial class CHECKOUT : AmDbEntityModel
    {
        [Key]
        [Column(Order = 0)]
        public double DOCNUM { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VERSION { get; set; }

        public DateTime? CHECKOUTWHEN { get; set; }

        public DateTime? DUEWHEN { get; set; }

        [StringLength(254)]
        public string CHECKOUT_DIR { get; set; }

        [StringLength(1)]
        public string PORTABLE { get; set; }

        public string COMMENTS { get; set; }

        [StringLength(32)]
        public string APPNAME { get; set; }

        [StringLength(32)]
        public string LOCATION { get; set; }
    }
}
