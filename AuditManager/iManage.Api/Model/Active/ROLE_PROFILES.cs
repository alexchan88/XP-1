namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.ROLE_PROFILES")]
    public partial class ROLE_PROFILES
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string PROFILE_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(32)]
        public string ALIAS { get; set; }

        [StringLength(254)]
        public string SEARCH_VALUE { get; set; }

        [StringLength(254)]
        public string SET_VALUE { get; set; }

        [StringLength(1)]
        public string SRCH_VALUE_ACCESS { get; set; }

        [StringLength(1)]
        public string SET_VALUE_ACCESS { get; set; }

        public virtual ROLE ROLE { get; set; }
    }
}
