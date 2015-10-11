using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuditManager.Model.EFModel.SSC
{

    public partial class FileActivity : AmDbEntityModel
    {
        public FileActivity()
        {
            DRMSPDFs = new HashSet<DRMSPDF>();
        }

        [Key]
        public int FAId { get; set; }

        public int? ParentFAId { get; set; }

        public int SurveyRowID { get; set; }

        [Required]
        [StringLength(50)]
        public string EngagementNumber { get; set; }

        public double? FileNumber { get; set; }

        public int ActivityId { get; set; }

        public int Priority { get; set; }

        [StringLength(50)]
        public string MachineName { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public DateTime? ActivityStartDate { get; set; }

        public DateTime? ActivityEndDate { get; set; }

        public int FAStatus { get; set; }

        public string FAComments { get; set; }

        [Required]
        [StringLength(1)]
        public string Deleted { get; set; }

        public DateTime? InsertDate { get; set; }

        [Required]
        [StringLength(50)]
        public string InsertBy { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        [Required]
        [StringLength(50)]
        public string LastUpdateBy { get; set; }

        public virtual ICollection<DRMSPDF> DRMSPDFs { get; set; }
    }
}
