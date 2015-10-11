namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.ROLES")]
    public partial class ROLE
    {
        public ROLE()
        {
            ROLE_NVPS = new HashSet<ROLE_NVPS>();
            ROLE_PROFILES = new HashSet<ROLE_PROFILES>();
            DOCUSERS = new HashSet<DOCUSER>();
        }

        [Key]
        [StringLength(32)]
        public string ALIAS { get; set; }

        [StringLength(254)]
        public string DESCRIPTION { get; set; }

        public int? M1 { get; set; }

        public int? M2 { get; set; }

        public int? M3 { get; set; }

        public virtual ICollection<ROLE_NVPS> ROLE_NVPS { get; set; }

        public virtual ICollection<ROLE_PROFILES> ROLE_PROFILES { get; set; }

        public virtual ICollection<DOCUSER> DOCUSERS { get; set; }
    }
}
