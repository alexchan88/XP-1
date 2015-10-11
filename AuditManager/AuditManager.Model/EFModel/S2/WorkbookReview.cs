using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditManager.Model.EFModel.S2
{
    [Table("S2CLR.WorkbookReviews")]
    public partial class WorkbookReview : AmDbEntityModel
    {
        public int WorkbookReviewId { get; set; }

        [Required]
        [StringLength(50)]
        public string WorkbookReviewGuid { get; set; }

        public int WorkbookId { get; set; }

        public int ReviewId { get; set; }

        public int StatusId { get; set; }

        [StringLength(150)]
        public string ReviewName { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public DateTime? ReviewStartDate { get; set; }

        public DateTime? ReviewEndDate { get; set; }

        public DateTime? ReviewClosedOutDate { get; set; }

        public bool? IsDeleted { get; set; }

        public int SortOrder { get; set; }

        public DateTime InsertDate { get; set; }

        [Required]
        [StringLength(50)]
        public string InsertBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        [Required]
        public virtual Status Status { get; set; }

        [Required]
        public virtual Status Status1 { get; set; }
    }
}
