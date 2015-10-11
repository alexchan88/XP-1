namespace AuditManager.Model.EFModel.Active
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.CUSTOM6")]
    public partial class CUSTOM6 : AmDbEntityModel
    {
        [Key]
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
    }
}
