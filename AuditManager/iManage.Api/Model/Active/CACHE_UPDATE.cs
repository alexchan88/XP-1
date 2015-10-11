namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.CACHE_UPDATE")]
    public partial class CACHE_UPDATE
    {
        [Key]
        [Column(Order = 0)]
        public DateTime UPDATE_WHEN { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string TABLE_NAME { get; set; }

        [StringLength(38)]
        public string C_ALIAS { get; set; }

        [StringLength(38)]
        public string P_ALIAS { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        public string OPER { get; set; }
    }
}
