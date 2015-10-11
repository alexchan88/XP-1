namespace AuditManager.Model.EFModel.Active
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.CUSTOM2")]
    public partial class CUSTOM2 : AmDbEntityModel
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string CPARENT_ALIAS { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(32)]
        public string CUSTOM_ALIAS { get; set; }

        [StringLength(254)]
        public string C_DESCRIPT { get; set; }

        [StringLength(1)]
        public string ENABLED { get; set; }

        public DateTime EDITWHEN { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_HIPAA { get; set; }

        public virtual CUSTOM1 CUSTOM1 { get; set; }
    }
}
