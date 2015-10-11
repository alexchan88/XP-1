namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.USERACTIONS")]
    public partial class USERACTION
    {
        public USERACTION()
        {
            USERHISTORies = new HashSet<USERHISTORY>();
        }

        [Key]
        public int SID { get; set; }

        [Required]
        [StringLength(32)]
        public string NAME { get; set; }

        [StringLength(254)]
        public string DESCRIPTION { get; set; }

        [Required]
        [StringLength(1)]
        public string ENABLED { get; set; }

        public virtual ICollection<USERHISTORY> USERHISTORies { get; set; }
    }
}
