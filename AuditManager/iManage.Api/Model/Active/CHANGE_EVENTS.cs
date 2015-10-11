namespace iManage.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.CHANGE_EVENTS")]
    public partial class CHANGE_EVENTS
    {
        public CHANGE_EVENTS()
        {
            CHANGE_EVENT_PROPERTIES = new HashSet<CHANGE_EVENT_PROPERTIES>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SID { get; set; }

        public DateTime EVENTTIME { get; set; }

        public int ACTION_TYPE { get; set; }

        public int OBJECT_TYPE { get; set; }

        public double? OBJECT_ID { get; set; }

        public int? OBJECT_VERSION { get; set; }

        public int? TREE_ID { get; set; }

        public double? RELATED_OBJECT_ID { get; set; }

        public int? RELATED_OBJECT_VERSION { get; set; }

        [StringLength(254)]
        public string RELATED_OBJECT_DB { get; set; }

        public int? PROCID { get; set; }

        public virtual ICollection<CHANGE_EVENT_PROPERTIES> CHANGE_EVENT_PROPERTIES { get; set; }
    }
}
