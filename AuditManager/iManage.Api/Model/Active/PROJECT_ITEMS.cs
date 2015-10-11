namespace iManage.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.PROJECT_ITEMS")]
    public partial class PROJECT_ITEMS
    {
        public PROJECT_ITEMS()
        {
            PROJECT_ITEMS1 = new HashSet<PROJECT_ITEMS>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        public int? PARENT_SID { get; set; }

        public double PRJ_ID { get; set; }

        [StringLength(1)]
        public string ITEMTYPE { get; set; }

        public double? ITEM_ID { get; set; }

        [StringLength(254)]
        public string ITEM_NAME { get; set; }

        [StringLength(254)]
        public string ITEM_DB { get; set; }

        public int? LEFT_VISIT { get; set; }

        public int? RIGHT_VISIT { get; set; }

        public int? TREE_ID { get; set; }

        public int? VERSION { get; set; }

        [StringLength(1)]
        public string REFERENCE_TYPE { get; set; }

        [StringLength(32)]
        public string REFERENCE_TALIAS { get; set; }

        public DateTime? INSERT_TS { get; set; }

        public virtual ICollection<PROJECT_ITEMS> PROJECT_ITEMS1 { get; set; }

        public virtual PROJECT_ITEMS PROJECT_ITEMS2 { get; set; }
    }
}
