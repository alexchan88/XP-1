namespace AuditManager.Model.EFModel.Ref
{
    using AuditManager.Model.EFModel;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Ref.Customer")]
    public partial class Customer : AmDbEntityModel
    {
        [Key]
        [StringLength(10)]
        public string CustomerNumber { get; set; }

        [StringLength(40)]
        public string CustomerName { get; set; }

        [StringLength(10)]
        public string NCPEmployeeId { get; set; }

        [StringLength(1)]
        public string CustomerStatus { get; set; }

        [StringLength(1)]
        public string SECRegistrationFlag { get; set; }

        [StringLength(10)]
        public string SentinelId { get; set; }

        public DateTime? CustomerCreatedDate { get; set; }

        public DateTime? InsertDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(40)]
        public string InsertBy { get; set; }

        [StringLength(40)]
        public string UpdateBy { get; set; }
    }
}
