namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.LIBRARIES")]
    public partial class LIBRARy
    {
        public LIBRARy()
        {
            DOCUSERS = new HashSet<DOCUSER>();
        }

        [Key]
        [StringLength(32)]
        public string LIBRARYNAME { get; set; }

        [StringLength(254)]
        public string DESCRIPTION { get; set; }

        public virtual ICollection<DOCUSER> DOCUSERS { get; set; }
    }
}
