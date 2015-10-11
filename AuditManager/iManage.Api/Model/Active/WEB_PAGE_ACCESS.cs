namespace iManage.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.WEB_PAGE_ACCESS")]
    public partial class WEB_PAGE_ACCESS
    {
        [Key]
        [Column(Order = 0)]
        public double DOCNUM { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VERSION { get; set; }

        [Required]
        [StringLength(1)]
        public string OBJECT_TYPE { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int USER_GP_ID { get; set; }

        public int ACCESS_RIGHT { get; set; }

        public DateTime EXPIRATION_TS { get; set; }
    }
}
