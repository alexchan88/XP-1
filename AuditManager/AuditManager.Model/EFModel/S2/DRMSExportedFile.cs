using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditManager.Model.EFModel.S2
{
    [Table("S2CLR.DRMSExportedFiles")]
    public partial class DRMSExportedFile : AmDbEntityModel
    {
        [Key]
        public int DRMSFileExportId { get; set; }

        public int FileMoveActivityId { get; set; }

        public int WorkbookReviewId { get; set; }

        public int TransactionDetailId { get; set; }

        [Required]
        [StringLength(1000)]
        public string DRMSFolderPath { get; set; }

        [Required]
        [StringLength(500)]
        public string FileName { get; set; }

        public int DocumentNumber { get; set; }

        public bool IsRecord { get; set; }

        public bool IsDeleted { get; set; }

        public bool? IsDRMSDeleted { get; set; }

        public DateTime? InsertDate { get; set; }

        [Required]
        [StringLength(50)]
        public string InsertBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public bool? IsNonAudit { get; set; }
    }
}
