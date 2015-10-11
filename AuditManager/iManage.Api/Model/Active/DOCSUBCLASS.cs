namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.DOCSUBCLASSES")]
    public partial class DOCSUBCLASS
    {
        public DOCSUBCLASS()
        {
            DOCMASTERs = new HashSet<DOCMASTER>();
        }

        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string C_ALIAS { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(32)]
        public string SUBCLASS_ALIAS { get; set; }

        [StringLength(254)]
        public string CLASSDESCRIPT { get; set; }

        [StringLength(1)]
        public string INDEXABLE { get; set; }

        [StringLength(1)]
        public string SHADOW { get; set; }

        public int? RETAIN { get; set; }

        public int? FIELDREQUIRED { get; set; }

        [StringLength(8)]
        public string SECNAME { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_HIPAA { get; set; }

        public virtual DOCCLASS DOCCLASS { get; set; }

        public virtual ICollection<DOCMASTER> DOCMASTERs { get; set; }
    }
}
