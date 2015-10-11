namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.EM_PROJECTS")]
    public partial class EM_PROJECTS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        public int? USERNUM { get; set; }

        public double PRJ_ID { get; set; }

        [Required]
        public string MAILBOX_ID { get; set; }

        [Required]
        [StringLength(900)]
        public string FOLDER_ID { get; set; }

        [Required]
        [StringLength(1)]
        public string ENABLED { get; set; }

        public int REQUEST_TYPE { get; set; }

        public int CLIENT_TYPE { get; set; }

        public string SYNC_COOKIE { get; set; }

        public int FILING_OPTIONS { get; set; }

        public string FILTER { get; set; }

        public string PROPERTIES { get; set; }

        public DateTime SYNCWHEN { get; set; }

        public DateTime CHANGEWHEN { get; set; }

        [Required]
        [StringLength(4)]
        public string LOCALE { get; set; }

        public int STATUS { get; set; }

        [StringLength(256)]
        public string STATUS_DESC { get; set; }

        [StringLength(64)]
        public string OPERATOR { get; set; }

        public int? SHARE_ACCESS { get; set; }

        public int? FREQUENCY { get; set; }

        public string FOLDER_PATH { get; set; }

        [StringLength(256)]
        public string FOLDER_GUID { get; set; }

        public virtual DOCUSER DOCUSER { get; set; }

        public virtual PROJECT PROJECT { get; set; }
    }
}
