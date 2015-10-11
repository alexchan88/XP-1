namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.DOC_KEYWORDS")]
    public partial class DOC_KEYWORDS
    {
        [Key]
        public int SID { get; set; }

        public double DOCNUM { get; set; }

        public int VERSION { get; set; }

        public int KW_SID { get; set; }

        public virtual KEYWORD KEYWORD { get; set; }
    }
}
