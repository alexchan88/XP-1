namespace iManage.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.INDEX_COLLECTION")]
    public partial class INDEX_COLLECTION
    {
        public INDEX_COLLECTION()
        {
            DOC_INDEX = new HashSet<DOC_INDEX>();
        }

        [Key]
        public int SID { get; set; }

        [Required]
        [StringLength(32)]
        public string ALIAS { get; set; }

        [Required]
        [StringLength(32)]
        public string FULLNESS { get; set; }

        [Required]
        [StringLength(32)]
        public string DOCSERVER { get; set; }

        [Required]
        [StringLength(254)]
        public string LOCATION { get; set; }

        [Required]
        [StringLength(1)]
        public string STATUS { get; set; }

        [StringLength(254)]
        public string COMMENTS { get; set; }

        public DateTime? LAST_DSTRBT_TIME { get; set; }

        [StringLength(32)]
        public string IDXSERVER { get; set; }

        public virtual ICollection<DOC_INDEX> DOC_INDEX { get; set; }

        public virtual DOCSERVER DOCSERVER1 { get; set; }
    }
}
