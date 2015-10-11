namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.USERHISTORY")]
    public partial class USERHISTORY
    {
        [Key]
        public int SID { get; set; }

        public int ACTIONSID { get; set; }

        [Required]
        [StringLength(64)]
        public string USERID { get; set; }

        [StringLength(64)]
        public string USER_PASSWORD { get; set; }

        public DateTime ACTIONDATETIME { get; set; }

        [StringLength(254)]
        public string COMMENTS { get; set; }

        [StringLength(254)]
        public string CUSTOM1 { get; set; }

        [StringLength(254)]
        public string CUSTOM2 { get; set; }

        [StringLength(254)]
        public string CUSTOM3 { get; set; }

        public virtual USERACTION USERACTION { get; set; }
    }
}
