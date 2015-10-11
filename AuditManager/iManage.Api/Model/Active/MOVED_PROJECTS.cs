namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.MOVED_PROJECTS")]
    public partial class MOVED_PROJECTS
    {
        [Key]
        public double PRJ_ID { get; set; }

        public double? OLD_PRJ_PID { get; set; }

        public double? NEW_PRJ_PID { get; set; }

        public DateTime? EDITWHEN { get; set; }
    }
}
