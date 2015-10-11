namespace AuditManager.Model.EFModel.Ref
{
    using AuditManager.Model.EFModel;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Ref.Function")]
    public partial class Function : AmDbEntityModel
    {
        [Key]
        [StringLength(1)]
        public string FucntionID { get; set; }

        [StringLength(14)]
        public string FunctionName { get; set; }

        [StringLength(30)]
        public string FunctionDesc { get; set; }

        public DateTime? InsertDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(40)]
        public string InsertBy { get; set; }

        [StringLength(40)]
        public string UpdateBy { get; set; }
    }
}
