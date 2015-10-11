namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.GROUPS")]
    public partial class GROUP
    {
        [StringLength(254)]
        public string GROUPID { get; set; }

        [StringLength(254)]
        public string FULLNAME { get; set; }

        [StringLength(1)]
        public string ENABLED { get; set; }

        public int? GROUPNUM { get; set; }

        [StringLength(254)]
        public string GROUP_DOMAIN { get; set; }

        public int? GROUP_NOS { get; set; }

        [StringLength(254)]
        public string SYNC_ID { get; set; }

        [StringLength(254)]
        public string DIST_NAME { get; set; }

        public DateTime? LAST_SYNC_TS { get; set; }

        [Required]
        [StringLength(1)]
        public string ISEXTERNAL { get; set; }
    }
}
