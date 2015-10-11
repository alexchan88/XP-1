namespace IM.Wrapper.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.DOCMASTER")]
    internal partial class DOCMASTER : BaseModel
    {
        public DOCMASTER()
        {
            PROJECTS = new HashSet<PROJECT>();
        }

        [StringLength(254)]
        public string DOCNAME { get; set; }

        [Key]
        [Column(Order = 0)]
        public double DOCNUM { get; set; }

        public double? DOCSIZE { get; set; }

        public DateTime? EDITWHEN { get; set; }

        [StringLength(1)]
        public string DOCINUSE { get; set; }

        public DateTime? ENTRYWHEN { get; set; }

        public DateTime? EDITPROFILEWHEN { get; set; }

        [StringLength(1)]
        public string INDEXED { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VERSION { get; set; }

        [StringLength(254)]
        public string DOCLOC { get; set; }

        //[ForeignKey("DOCLOC")]
        //public virtual DOCSERVER DOCSERVER { get; set; }

        [StringLength(64)]
        public string AUTHOR { get; set; }

        [StringLength(64)]
        public string OPERATOR { get; set; }

        [StringLength(32)]
        public string ALIAS { get; set; }

        [StringLength(1)]
        public string CHECKEDOUT { get; set; }

        public string COMMENTS { get; set; }

        [StringLength(1)]
        public string COMINDEX { get; set; }

        [StringLength(1)]
        public string ARCHIVE_REQ { get; set; }

        [StringLength(32)]
        public string T_ALIAS { get; set; }

        [StringLength(32)]
        public string C_ALIAS { get; set; }

        [StringLength(32)]
        public string SUBCLASS_ALIAS { get; set; }

        [StringLength(1)]
        public string ARCHIVED { get; set; }

        [StringLength(64)]
        public string INUSEBY { get; set; }

        [StringLength(1)]
        public string INDEXABLE { get; set; }

        [StringLength(1)]
        public string ISRELATED { get; set; }

        [StringLength(32)]
        public string C1ALIAS { get; set; }

        [StringLength(32)]
        public string C2ALIAS { get; set; }

        [StringLength(32)]
        public string C3ALIAS { get; set; }

        [StringLength(32)]
        public string C4ALIAS { get; set; }

        [StringLength(32)]
        public string C5ALIAS { get; set; }

        [StringLength(32)]
        public string C6ALIAS { get; set; }

        [StringLength(32)]
        public string C7ALIAS { get; set; }

        [StringLength(32)]
        public string C8ALIAS { get; set; }

        [StringLength(32)]
        public string C9ALIAS { get; set; }

        [StringLength(32)]
        public string C10ALIAS { get; set; }

        [StringLength(32)]
        public string C11ALIAS { get; set; }

        [StringLength(32)]
        public string C12ALIAS { get; set; }

        [StringLength(96)]
        public string C13ALIAS { get; set; }

        [StringLength(96)]
        public string C14ALIAS { get; set; }

        [StringLength(96)]
        public string C15ALIAS { get; set; }

        [StringLength(96)]
        public string C16ALIAS { get; set; }

        [StringLength(32)]
        public string C29ALIAS { get; set; }

        [StringLength(32)]
        public string C30ALIAS { get; set; }

        [StringLength(32)]
        public string C31ALIAS { get; set; }

        public double? CDBL1 { get; set; }

        public double? CDBL2 { get; set; }

        public double? CDBL3 { get; set; }

        public double? CDBL4 { get; set; }

        [StringLength(1)]
        public string CBOOL1 { get; set; }

        [StringLength(1)]
        public string CBOOL2 { get; set; }

        [StringLength(1)]
        public string CBOOL3 { get; set; }

        [StringLength(1)]
        public string CBOOL4 { get; set; }

        public DateTime? CDATE1 { get; set; }

        public DateTime? CDATE2 { get; set; }

        public DateTime? CDATE3 { get; set; }

        public DateTime? CDATE4 { get; set; }

        public DateTime? DECLAREWHEN { get; set; }

        [StringLength(1)]
        public string PENDING_CLOSE { get; set; }

        public int? PENDING_IRM { get; set; }

        public int? IRM { get; set; }

        public double? ARCHV_ID { get; set; }

        [StringLength(64)]
        public string LASTUSER { get; set; }

        public int? RETAIN { get; set; }

        [StringLength(254)]
        public string LOGNODEADDR { get; set; }

        [StringLength(1)]
        public string DEFAULT_SECURITY { get; set; }

        [StringLength(1)]
        public string IS_SECURED { get; set; }

        [Required]
        [StringLength(1)]
        public string TYPE { get; set; }

        [StringLength(512)]
        public string MSG_ID { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_EXTERNAL { get; set; }

        [Required]
        [StringLength(1)]
        public string HAS_ATTACHMENT { get; set; }

        [Required]
        [StringLength(1)]
        public string EXTRNL_AS_NRML { get; set; }

        public DateTime? FILEENTRYWHEN { get; set; }

        public DateTime? FILEEDITWHEN { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_HIPAA { get; set; }

        public virtual ICollection<PROJECT> PROJECTS { get; set; }
    }
}
