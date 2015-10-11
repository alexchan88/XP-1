namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.DOCTYPES")]
    public partial class DOCTYPE
    {
        public DOCTYPE()
        {
            APPS = new HashSet<APP>();
            DOCMASTERs = new HashSet<DOCMASTER>();
            TYPEMAPs = new HashSet<TYPEMAP>();
        }

        [Key]
        [StringLength(32)]
        public string T_ALIAS { get; set; }

        [StringLength(254)]
        public string TYPEDESCRIPT { get; set; }

        [StringLength(1)]
        public string AUTODETECT { get; set; }

        [StringLength(8)]
        public string DMSEXTENSION { get; set; }

        [StringLength(8)]
        public string APPEXTENSION { get; set; }

        [StringLength(1)]
        public string INDEXABLE { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_HIPAA { get; set; }

        public virtual ICollection<APP> APPS { get; set; }

        public virtual ICollection<DOCMASTER> DOCMASTERs { get; set; }

        public virtual ICollection<TYPEMAP> TYPEMAPs { get; set; }
    }
}
