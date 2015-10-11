using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditManager.Model.EFModel.S2
{
    [Table("S2CLR.Closure")]
    public partial class Closure : AmDbEntityModel
    {
        public int ClosureId { get; set; }

        [Required]
        [StringLength(32)]
        public string EngNum { get; set; }

        public DateTime? ClosureDate { get; set; }

        public bool IsClosed { get; set; }

        public DateTime? LastMailSentDate { get; set; }
    }
}


