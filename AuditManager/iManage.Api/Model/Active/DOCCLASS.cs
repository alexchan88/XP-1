namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.DOCCLASSES")]
    public partial class DOCCLASS
    {
        public DOCCLASS()
        {
            DOCMASTERs = new HashSet<DOCMASTER>();
            DOCSUBCLASSES = new HashSet<DOCSUBCLASS>();
        }

        [Key]
        [StringLength(32)]
        public string C_ALIAS { get; set; }

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

        [StringLength(1)]
        public string SUBCLASS_REQD { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_HIPAA { get; set; }

        public virtual ICollection<DOCMASTER> DOCMASTERs { get; set; }

        public virtual ICollection<DOCSUBCLASS> DOCSUBCLASSES { get; set; }
    }
}
