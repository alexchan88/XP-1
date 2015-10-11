namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.SYNC_LOCATIONS")]
    public partial class SYNC_LOCATIONS
    {
        [Key]
        public int SID { get; set; }

        public int SYNC_ITEM_SID { get; set; }

        [Required]
        [StringLength(254)]
        public string LOCATION { get; set; }

        public DateTime SYNC_WHEN { get; set; }

        public virtual SYNC_ITEMS SYNC_ITEMS { get; set; }
    }
}
