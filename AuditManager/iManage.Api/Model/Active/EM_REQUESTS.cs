namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.EM_REQUESTS")]
    public partial class EM_REQUESTS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        public int USERNUM { get; set; }

        public double PRJ_ID { get; set; }

        [Required]
        public string MAILBOX_ID { get; set; }

        [StringLength(900)]
        public string FOLDER_ID { get; set; }

        [StringLength(512)]
        public string MSG_ID { get; set; }

        public int RETRY_COUNT { get; set; }

        public int REQUEST_TYPE { get; set; }

        public int CLIENT_TYPE { get; set; }

        public int FILING_OPTIONS { get; set; }

        [Required]
        public string FILTER { get; set; }

        [Required]
        public string PROPERTIES { get; set; }

        public string DOCPROFILE { get; set; }

        public DateTime SUBMITWHEN { get; set; }

        [Required]
        [StringLength(4)]
        public string LOCALE { get; set; }

        public int STATUS { get; set; }

        [StringLength(256)]
        public string STATUS_DESC { get; set; }

        [StringLength(64)]
        public string OPERATOR { get; set; }

        public int? SHARE_ACCESS { get; set; }

        [StringLength(256)]
        public string EMAIL_GUID { get; set; }

        public string FOLDER_PATH { get; set; }

        public virtual DOCUSER DOCUSER { get; set; }

        public virtual PROJECT PROJECT { get; set; }
    }
}
