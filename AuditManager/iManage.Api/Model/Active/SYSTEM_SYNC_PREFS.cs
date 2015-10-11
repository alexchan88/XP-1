namespace iManage.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.SYSTEM_SYNC_PREFS")]
    public partial class SYSTEM_SYNC_PREFS
    {
        [Key]
        public int SID { get; set; }

        public int? SAVED_SEARCH_ID { get; set; }

        public int? SYNC_PREF_LIST_ID { get; set; }
    }
}
