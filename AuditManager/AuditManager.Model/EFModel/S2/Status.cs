using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditManager.Model.EFModel.S2
{
    [Table("S2CLR.Status")]
    public partial class Status : AmDbEntityModel
    {
        public Status()
        {
            WorkbookReviews = new HashSet<WorkbookReview>();
            WorkbookReviews1 = new HashSet<WorkbookReview>();
        }

        public int StatusId { get; set; }

        [Column("Status")]
        [Required]
        [StringLength(50)]
        public string Status1 { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        public bool Enabled { get; set; }

        public DateTime? InsertDate { get; set; }

        [Required]
        [StringLength(50)]
        public string InsertBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public virtual ICollection<WorkbookReview> WorkbookReviews { get; set; }

        public virtual ICollection<WorkbookReview> WorkbookReviews1 { get; set; }
    }
}
