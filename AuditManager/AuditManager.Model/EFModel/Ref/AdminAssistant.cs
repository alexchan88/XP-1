namespace AuditManager.Model.EFModel.Ref
{
    using AuditManager.Model.EFModel;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Ref.AdminAssistant")]
    public partial class AdminAssistant : AmDbEntityModel
    {
        //[Key]
        public int AAIdKey { get; set; }

        [StringLength(20)]
        public string AssistantEmployeeId { get; set; }

        [Key]
        [Required]
        [StringLength(20)]
        public string PartnerEmployeeID { get; set; }

        public DateTime? InsertDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(40)]
        public string InsertBy { get; set; }

        [StringLength(40)]
        public string UpdateBy { get; set; }

        [Required]
        [ForeignKey("AssistantEmployeeId")]
        public virtual DRMSUser Assistant { get; set; }
    }
}
