namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.APPS")]
    public partial class APP
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string APPNAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(32)]
        public string T_ALIAS { get; set; }

        [StringLength(1)]
        public string PRIMARYAPP { get; set; }

        [StringLength(254)]
        public string PATH { get; set; }

        [StringLength(1)]
        public string DDE { get; set; }

        [StringLength(1)]
        public string ODMA { get; set; }

        [StringLength(32)]
        public string DDEAPPNAME { get; set; }

        [StringLength(32)]
        public string DDETOPIC { get; set; }

        [StringLength(254)]
        public string DDEOPEN { get; set; }

        [StringLength(254)]
        public string DDEOPENREAD { get; set; }

        [StringLength(254)]
        public string DDEPRINT1 { get; set; }

        [StringLength(254)]
        public string DDEPRINT2 { get; set; }

        public virtual DOCTYPE DOCTYPE { get; set; }
    }
}
