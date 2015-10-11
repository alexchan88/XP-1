namespace iManage.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.ARCHIVETBL")]
    public partial class ARCHIVETBL
    {
        public ARCHIVETBL()
        {
            ARSET_INFO = new HashSet<ARSET_INFO>();
            DOCMASTERs = new HashSet<DOCMASTER>();
        }

        [Key]
        public double ARCHV_ID { get; set; }

        public DateTime? ARCHIVE_DATE { get; set; }

        [StringLength(254)]
        public string ARCHIVE_PATH { get; set; }

        [StringLength(254)]
        public string ARCHIVE_COMMENT { get; set; }

        [StringLength(64)]
        public string OPERATOR { get; set; }

        public int? NUM_OF_DOCS { get; set; }

        public int? REST_COUNT { get; set; }

        [StringLength(32)]
        public string MEDIA_ID { get; set; }

        [StringLength(254)]
        public string MEDIA_LOCATION { get; set; }

        public virtual ICollection<ARSET_INFO> ARSET_INFO { get; set; }

        public virtual ICollection<DOCMASTER> DOCMASTERs { get; set; }
    }
}
