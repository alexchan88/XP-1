namespace AuditManager.Model.EFModel.Active
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.CUSTOM1")]
    public partial class CUSTOM1 : AmDbEntityModel
    {
        public CUSTOM1()
        {
            CUSTOM2 = new HashSet<CUSTOM2>();
        }

        [Key]
        [StringLength(32)]
        public string CUSTOM_ALIAS { get; set; }

        [StringLength(254)]
        public string C_DESCRIPT { get; set; }

        [StringLength(1)]
        public string ENABLED { get; set; }

        [StringLength(8)]
        public string SECNAME { get; set; }

        public DateTime EDITWHEN { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_HIPAA { get; set; }

        public virtual ICollection<CUSTOM2> CUSTOM2 { get; set; }
    }
}
