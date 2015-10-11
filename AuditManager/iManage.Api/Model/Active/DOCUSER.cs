namespace iManage.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.DOCUSERS")]
    public partial class DOCUSER
    {
        public DOCUSER()
        {
            EM_PROJECTS = new HashSet<EM_PROJECTS>();
            EM_REQUESTS = new HashSet<EM_REQUESTS>();
            KMTAGs = new HashSet<KMTAG>();
            ROLES = new HashSet<ROLE>();
        }

        [Key]
        [StringLength(64)]
        public string USERID { get; set; }

        [Required]
        [StringLength(64)]
        public string USERIDEX { get; set; }

        [StringLength(254)]
        public string FULLNAME { get; set; }

        [StringLength(254)]
        public string USERLOC { get; set; }

        [StringLength(32)]
        public string PHONE { get; set; }

        [StringLength(32)]
        public string EXTENSION { get; set; }

        [StringLength(1)]
        public string LOGIN { get; set; }

        [StringLength(32)]
        public string FAX { get; set; }

        [StringLength(254)]
        public string GENERAL { get; set; }

        [StringLength(254)]
        public string EMAIL { get; set; }

        [StringLength(254)]
        public string EMAIL2 { get; set; }

        [StringLength(254)]
        public string EMAIL3 { get; set; }

        [StringLength(254)]
        public string EMAIL4 { get; set; }

        [StringLength(254)]
        public string EMAIL5 { get; set; }

        public int? PERM1 { get; set; }

        public int? PERM2 { get; set; }

        public int? USERNUM { get; set; }

        [StringLength(32)]
        public string DOCSERVER { get; set; }

        [StringLength(32)]
        public string LIBRARYNAME { get; set; }

        public int? PRIMARY_GROUP { get; set; }

        [StringLength(254)]
        public string USER_DOMAIN { get; set; }

        [StringLength(64)]
        public string USER_PASSWD { get; set; }

        public int? USER_NOS { get; set; }

        [StringLength(254)]
        public string CUSTOM1 { get; set; }

        [StringLength(254)]
        public string CUSTOM2 { get; set; }

        [StringLength(254)]
        public string CUSTOM3 { get; set; }

        public DateTime? PWD_CHANGED_TS { get; set; }

        [Required]
        [StringLength(1)]
        public string PWD_NEVER_EXPIRE_F { get; set; }

        public DateTime? LOCKOUT_TS { get; set; }

        public int FAILED_LOGINS { get; set; }

        [StringLength(254)]
        public string SYNC_ID { get; set; }

        [StringLength(254)]
        public string DIST_NAME { get; set; }

        public DateTime? LAST_SYNC_TS { get; set; }

        [Required]
        [StringLength(1)]
        public string ISEXTERNAL { get; set; }

        public DateTime EDITWHEN { get; set; }

        public string DOCPROFILE { get; set; }

        [StringLength(32)]
        public string SECUREDOCSERVER { get; set; }

        [StringLength(1024)]
        public string EXCH_AUTO_DISCOVER { get; set; }

        public virtual DOCSERVER DOCSERVER1 { get; set; }

        public virtual LIBRARy LIBRARy { get; set; }

        public virtual ICollection<EM_PROJECTS> EM_PROJECTS { get; set; }

        public virtual ICollection<EM_REQUESTS> EM_REQUESTS { get; set; }

        public virtual ICollection<KMTAG> KMTAGs { get; set; }

        public virtual ICollection<ROLE> ROLES { get; set; }
    }
}
