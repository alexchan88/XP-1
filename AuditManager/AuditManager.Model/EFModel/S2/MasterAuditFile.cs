using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditManager.Model.EFModel.S2
{
    [Table("S2CLR.MasterAuditFile")]
    public partial class MasterAuditFile : AmDbEntityModel
    {
        public MasterAuditFile()
        {
            Workbooks = new HashSet<Workbook>();
        }

        public int MasterAuditFileId { get; set; }

        [Required]
        [StringLength(50)]
        public string MAFGuid { get; set; }

        [StringLength(50)]
        public string EngagementNumber { get; set; }

        [StringLength(500)]
        public string EngagementName { get; set; }

        [Required]
        [StringLength(250)]
        public string FileName { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(25)]
        public string ClientNumber { get; set; }

        [StringLength(250)]
        public string ClientName { get; set; }

        [Required]
        [StringLength(100)]
        public string Partner { get; set; }

        [Required]
        [StringLength(100)]
        public string Manager { get; set; }

        public bool IsActive { get; set; }

        public DateTime? InsertDate { get; set; }

        [Required]
        [StringLength(50)]
        public string InsertBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public virtual ICollection<Workbook> Workbooks { get; set; }
    }
}
