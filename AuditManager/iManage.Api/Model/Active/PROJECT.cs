namespace iManage.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MHGROUP.PROJECTS")]
    public partial class PROJECT
    {
        public PROJECT()
        {
            EM_PROJECTS = new HashSet<EM_PROJECTS>();
            EM_REQUESTS = new HashSet<EM_REQUESTS>();
            KMTAGs = new HashSet<KMTAG>();
            PROJECT_NVPS = new HashSet<PROJECT_NVPS>();
            PROJECTS1 = new HashSet<PROJECT>();
        }

        [Key]
        public double PRJ_ID { get; set; }

        public double? PRJ_PID { get; set; }

        [Required]
        [StringLength(1)]
        public string DEFAULT_SECURITY { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_SECURED { get; set; }

        [StringLength(254)]
        public string PRJ_NAME { get; set; }

        [StringLength(64)]
        public string PRJ_OWNER { get; set; }

        [StringLength(254)]
        public string PRJ_DESCRIPT { get; set; }

        [StringLength(1)]
        public string PRJ_STATE { get; set; }

        [StringLength(1)]
        public string PRJ_PUBLIC { get; set; }

        [StringLength(254)]
        public string PRJ_LOCATION { get; set; }

        public int TYPE { get; set; }

        [StringLength(64)]
        public string SUBTYPE { get; set; }

        [Required]
        [StringLength(1)]
        public string INHERITS_SECURITY { get; set; }

        public double? DOCNUM { get; set; }

        public int? VERSION { get; set; }

        [StringLength(254)]
        public string CUSTOM1 { get; set; }

        [StringLength(254)]
        public string CUSTOM2 { get; set; }

        [StringLength(254)]
        public string CUSTOM3 { get; set; }

        public int? LEFT_VISIT { get; set; }

        public int? RIGHT_VISIT { get; set; }

        public int? TREE_ID { get; set; }

        [StringLength(254)]
        public string EMAIL { get; set; }

        public int? DOC_SAVED_SEARCH { get; set; }

        [StringLength(254)]
        public string LAYOUT_NAME { get; set; }

        public int? LAYOUT_ORDER { get; set; }

        public string LAYOUT_VIEW { get; set; }

        [StringLength(32)]
        public string REFERENCE_DATABASE { get; set; }

        public double? REFERENCE_PRJ_ID { get; set; }

        public int? REFERENCE_TYPE { get; set; }

        [StringLength(64)]
        public string REFERENCE_SUBTYPE { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_EXTERNAL { get; set; }

        [Required]
        [StringLength(1)]
        public string EXTRNL_AS_NRML { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_DOC_SVD_SRCH { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_PRJ_SVD_SRCH { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_HIDDEN { get; set; }

        public DateTime EDITWHEN { get; set; }

        public virtual DOCMASTER DOCMASTER { get; set; }

        public virtual ICollection<EM_PROJECTS> EM_PROJECTS { get; set; }

        public virtual ICollection<EM_REQUESTS> EM_REQUESTS { get; set; }

        public virtual ICollection<KMTAG> KMTAGs { get; set; }

        public virtual ICollection<PROJECT_NVPS> PROJECT_NVPS { get; set; }

        public virtual ICollection<PROJECT> PROJECTS1 { get; set; }

        public virtual PROJECT PROJECT1 { get; set; }
    }
}
