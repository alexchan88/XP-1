
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditManager.Model.EFModel.SSC
{

    public partial class DRMSPDF : AmDbEntityModel
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FAId { get; set; }

        [Key]
        [Column(Order = 1)]
        public double DocNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string DocName { get; set; }

        [Required]
        [StringLength(500)]
        public string KDrivePath { get; set; }

        public virtual FileActivity FileActivity { get; set; }
    }
}
