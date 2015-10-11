namespace iManage.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.RERULEHANDLERS")]
    public partial class RERULEHANDLER
    {
        public RERULEHANDLER()
        {
            RERULES = new HashSet<RERULE>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int HANDLER_SID { get; set; }

        [Required]
        [StringLength(1)]
        public string ENABLED_F { get; set; }

        [Required]
        [StringLength(254)]
        public string COMMAND_LINE { get; set; }

        [Required]
        [StringLength(32)]
        public string NAME { get; set; }

        [StringLength(254)]
        public string DESCRIPTION { get; set; }

        public virtual ICollection<RERULE> RERULES { get; set; }
    }
}
