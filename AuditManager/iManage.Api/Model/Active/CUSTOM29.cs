namespace iManage.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.CUSTOM29")]
    public partial class CUSTOM29
    {
        public CUSTOM29()
        {
            CUSTOM30 = new HashSet<CUSTOM30>();
            DOCMASTERs = new HashSet<DOCMASTER>();
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

        public virtual ICollection<CUSTOM30> CUSTOM30 { get; set; }

        public virtual ICollection<DOCMASTER> DOCMASTERs { get; set; }
    }
}
