namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.CHANGE_EVENT_QUEUES")]
    public partial class CHANGE_EVENT_QUEUES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        [Required]
        [StringLength(254)]
        public string QUEUE_NAME { get; set; }

        public DateTime LAST_UPDATED { get; set; }

        public int? LATEST_EVENT_ID { get; set; }

        public string FILTER_OPTIONS { get; set; }
    }
}
