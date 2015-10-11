namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.QUICK_RETRIEVE")]
    public partial class QUICK_RETRIEVE
    {
        [StringLength(64)]
        public string USERID { get; set; }

        public DateTime? ENTRYTIME { get; set; }

        public double? DOCNUM { get; set; }

        public int? VERSION { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TYPE { get; set; }

        public int? ITEM_ID { get; set; }
    }
}
