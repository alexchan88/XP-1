namespace iManage.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.CUSTOM31")]
    public partial class CUSTOM31
    {
        public CUSTOM31()
        {
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

        [Key]
        [Column(Order = 2)]
        [StringLength(32)]
        public string CGRANDPARENT_ALIAS { get; set; }

        [StringLength(254)]
        public string C_DESCRIPT { get; set; }

        [StringLength(1)]
        public string ENABLED { get; set; }

        public DateTime EDITWHEN { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_HIPAA { get; set; }

        public virtual CUSTOM30 CUSTOM30 { get; set; }

        public virtual ICollection<DOCMASTER> DOCMASTERs { get; set; }
    }
}
