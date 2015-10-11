using System;
using System.ComponentModel.DataAnnotations;

namespace IM.Wrapper.Model
{
    //[Table("MHGROUP.DOCHISTORY")]
    internal partial class DOCHISTORY : BaseModel
    {
        public double? DOCNUM { get; set; }
        public int? VERSION { get; set; }
        [StringLength(254)]
        public string ACTIVITY { get; set; }
        public int? ACTIVITY_CODE { get; set; }
        public DateTime? ACTIVITY_DATETIME { get; set; }
        public int? DURATION { get; set; }
        public int? PAGES_PRINTED { get; set; }
        public double? NUM1 { get; set; }
        public double? NUM2 { get; set; }
        public double? NUM3 { get; set; }
        [StringLength(254)]
        public string DATA1 { get; set; }
        [StringLength(254)]
        public string DATA2 { get; set; }
        [StringLength(64)]
        public string DOCUSER { get; set; }
        [StringLength(32)]
        public string APPNAME { get; set; }
        [StringLength(32)]
        public string LOCATION { get; set; }
        //[StringLength()]
        public string COMMENTS { get; set; }
    }
}
