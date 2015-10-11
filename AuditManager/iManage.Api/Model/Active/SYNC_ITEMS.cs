namespace iManage.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.SYNC_ITEMS")]
    public partial class SYNC_ITEMS
    {
        public SYNC_ITEMS()
        {
            SYNC_LOCATIONS = new HashSet<SYNC_LOCATIONS>();
        }

        [Key]
        public int SID { get; set; }

        public double ITEMID { get; set; }

        public int? ITEMVERSION { get; set; }

        [Required]
        [StringLength(1)]
        public string ITEMTYPE { get; set; }

        public int USERNUM { get; set; }

        public int VRSN_HNDLNG { get; set; }

        public DateTime EDIT_TS { get; set; }

        public virtual ICollection<SYNC_LOCATIONS> SYNC_LOCATIONS { get; set; }
    }
}
