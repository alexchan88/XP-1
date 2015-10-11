namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.RESTORETBL")]
    public partial class RESTORETBL
    {
        [Key]
        public double DOCNUM { get; set; }

        public int? VERSION { get; set; }

        [StringLength(1)]
        public string ALLVERSIONS { get; set; }

        public DateTime? REQUEST_DATE { get; set; }

        [StringLength(64)]
        public string USERID { get; set; }

        public double? ARCHV_ID { get; set; }
    }
}
