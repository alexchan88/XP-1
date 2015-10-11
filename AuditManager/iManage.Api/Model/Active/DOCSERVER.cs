namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.DOCSERVERS")]
    public partial class DOCSERVER
    {
        public DOCSERVER()
        {
            DOCUSERS = new HashSet<DOCUSER>();
            INDEX_COLLECTION = new HashSet<INDEX_COLLECTION>();
        }

        [Key]
        [Column("DOCSERVER")]
        [StringLength(32)]
        public string DOCSERVER1 { get; set; }

        [Required]
        [StringLength(32)]
        public string OS { get; set; }

        [Required]
        [StringLength(254)]
        public string LOCATION { get; set; }

        [StringLength(254)]
        public string ROOTPATH { get; set; }

        [StringLength(254)]
        public string CONTEXT { get; set; }

        [StringLength(1)]
        public string READONLY { get; set; }

        [StringLength(1)]
        public string ENABLED { get; set; }

        [StringLength(32)]
        public string ACCESS_METHOD { get; set; }

        [StringLength(1)]
        public string TYPE { get; set; }

        [Required]
        [StringLength(1)]
        public string SECURE { get; set; }

        public virtual ICollection<DOCUSER> DOCUSERS { get; set; }

        public virtual ICollection<INDEX_COLLECTION> INDEX_COLLECTION { get; set; }
    }
}
