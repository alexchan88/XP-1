namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.REEVENTLOG")]
    public partial class REEVENTLOG
    {
        [Key]
        public int EVENT_SID { get; set; }

        public DateTime CREATIONDATETIME { get; set; }

        public DateTime? PROCESSEDDATETIME { get; set; }

        [Required]
        [StringLength(64)]
        public string ACTOR { get; set; }

        public int EVENT_T { get; set; }

        public int CONTENT_T { get; set; }

        public double CONTENTID { get; set; }

        public int? DOCVERSION { get; set; }

        public string EVENT_DATA { get; set; }

        public int? CON_CONTENT_T { get; set; }

        public double? CON_CONTENTID { get; set; }

        public DateTime? ACTIVATEDATETIME { get; set; }

        public virtual REEVENTDEF REEVENTDEF { get; set; }
    }
}
