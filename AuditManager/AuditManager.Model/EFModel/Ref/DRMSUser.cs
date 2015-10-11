namespace AuditManager.Model.EFModel.Ref
{
    using AuditManager.Model.EFModel;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Ref.DRMSUsers")]
    public partial class DRMSUser : AmDbEntityModel
    {
        //[Key]
        [StringLength(12)]
        public string DSID { get; set; }

        //[Required]
        [Key]
        [StringLength(11)]
        public string EmplID { get; set; }

        [StringLength(256)]
        public string FirstName { get; set; }

        [StringLength(256)]
        public string LastName { get; set; }

        [Required]
        [StringLength(256)]
        public string SamAccountName { get; set; }

        [StringLength(256)]
        public string DisplayName { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(1)]
        public string GoFunction { get; set; }

        [ForeignKey("GoFunction")]
        public virtual Function Function { get; set; }

        [StringLength(6)]
        public string JobCode { get; set; }

        [StringLength(5)]
        public string TransactionType { get; set; }

        public DateTime? InsertDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(40)]
        public string InsertBy { get; set; }

        [StringLength(40)]
        public string UpdateBy { get; set; }
    }
}
