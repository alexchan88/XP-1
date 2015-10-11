namespace iManage.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.CUSTOM30")]
    public partial class CUSTOM30
    {
        public CUSTOM30()
        {
            CUSTOM31 = new HashSet<CUSTOM31>();
            DOCMASTERs = new HashSet<DOCMASTER>();
        }

        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string CUSTOM_ALIAS { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(32)]
        public string CPARENT_ALIAS { get; set; }

        [StringLength(254)]
        public string C_DESCRIPT { get; set; }

        [StringLength(1)]
        public string ENABLED { get; set; }

        public DateTime EDITWHEN { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_HIPAA { get; set; }

        public virtual CUSTOM29 CUSTOM29 { get; set; }

        public virtual ICollection<CUSTOM31> CUSTOM31 { get; set; }

        public virtual ICollection<DOCMASTER> DOCMASTERs { get; set; }
    }
}
