namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.DOC_INDEX")]
    public partial class DOC_INDEX
    {
        [Key]
        public int SID { get; set; }

        public int INDX_CLCTN_SID { get; set; }

        public double DOCNUM { get; set; }

        public int VERSION { get; set; }

        public DateTime LAST_INDEX_TIME { get; set; }

        [Required]
        [StringLength(1)]
        public string STATUS { get; set; }

        public virtual INDEX_COLLECTION INDEX_COLLECTION { get; set; }
    }
}
