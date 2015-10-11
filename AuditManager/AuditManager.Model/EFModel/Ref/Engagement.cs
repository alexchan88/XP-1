namespace AuditManager.Model.EFModel.Ref
{
    using AuditManager.Model.EFModel;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Ref.Engagement")]
    public partial class Engagement : AmDbEntityModel
    {
        [Key]
        [StringLength(10)]
        public string EngagementNumber { get; set; }

        [StringLength(40)]
        public string EngagementDescription { get; set; }

        [StringLength(30)]
        public string EngagementStatus { get; set; }

        public DateTime? EngagementOpenDate { get; set; }

        public DateTime? EngagementClosedDate { get; set; }

        public DateTime? InsertDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(40)]
        public string InsertBy { get; set; }

        [StringLength(40)]
        public string UpdateBy { get; set; }

        [Required]
        [StringLength(10)]
        public string CustomerNumber { get; set; }
        public virtual Customer Customer { get; set; }

        [Required]
        [StringLength(11)]
        public string EMEmployeeId { get; set; }

        [Required]
        [ForeignKey("EMEmployeeId")]
        public virtual DRMSUser Manager { get; set; }

        [Required]
        [StringLength(11)]
        public string EPEmployeeId { get; set; }

        [Required]
        [ForeignKey("EPEmployeeId")]
        public virtual DRMSUser Partner { get; set; }

        [Required]
        [ForeignKey("EPEmployeeId")]
        public virtual AdminAssistant AdminAssistant { get; set; }
    }
}
