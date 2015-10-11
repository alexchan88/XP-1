using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditManager.Model.EFModel.S2
{
    [Table("S2CLR.Workbooks")]
    public partial class Workbook : AmDbEntityModel
    {
        public int WorkbookId { get; set; }

        [Required]
        [StringLength(50)]
        public string WorkbookGuid { get; set; }

        [StringLength(250)]
        public string WorkbookName { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int MasterAuditFileId { get; set; }

        public int StatusId { get; set; }

        public DateTime InsertDate { get; set; }

        [Required]
        [StringLength(50)]
        public string InsertBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        [Required]
        public virtual MasterAuditFile MasterAuditFile { get; set; }
    }
}
