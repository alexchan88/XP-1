using System;
using System.ComponentModel.DataAnnotations;

namespace AuditManager.Model
{
    public class WsSurveyModel
    {
        [AltPropName("@DRMSFileNumber")]
        public int DRMSFileNumber { get; set; }

        [AltPropName("@RequestorId")]
        [MaxLength(50)]
        public string RequestorId { get; set; }
        [AltPropName("@RequestorFirstName")]
        [MaxLength(500)]
        public string RequestorFirstName { get; set; }
        [AltPropName("@RequestorLastName")]
        [MaxLength(500)]
        public string RequestorLastName { get; set; }
        [AltPropName("@RequestorEmail")]
        [MaxLength(500)]
        public string RequestorEmail { get; set; }

        //12-
        [AltPropName("@ManagerFirstName")]
        [MaxLength(500)]
        public string ManagerFirstName { get; set; }
        [AltPropName("@ManagerLastName")]
        [MaxLength(500)]
        public string ManagerLastName { get; set; }

        [AltPropName("@ManagerEmail")]
        [MaxLength(500)]
        public string ManagerEmail { get; set; }

        [AltPropName("@PartnerFirstName")]
        [MaxLength(500)]
        public string PartnerFirstName { get; set; }
        [AltPropName("@PartnerLastName")]
        [MaxLength(500)]
        public string PartnerLastName { get; set; }

        [AltPropName("@PartnerEmail")]
        [MaxLength(500)]
        public string PartnerEmail { get; set; }
        //12-

        //1-
        [AltPropName("@ClientName")]
        [MaxLength(500)]
        public string ClientName { get; set; }
        [AltPropName("@ClientNumber")]
        [MaxLength(500)]
        public string ClientNumber { get; set; }
        [AltPropName("@EngagementNumber")]
        [MaxLength(500)]
        public string EngagementNumber { get; set; }
        [AltPropName("@EngagementName")]
        [MaxLength(500)]
        public string EngagementName { get; set; }
        //1-

        //9-
        // -- Error - File Name is blank or null.
        [AltPropName("@EngFileName")]
        [MaxLength(500)]
        public string EngFileName { get; set; }
        //9-

        //2-
        [AltPropName("@Preservation")]
        [MaxLength(1), DataType("bool")]
        public char Preservation { get; set; }
        //2-

        //10-
        [AltPropName("@NumberOfMAFs")]
        public int NumberOfMAFs { get; set; }
        //10-

        //13-
        [AltPropName("@KPMGOnly")]
        [MaxLength(1), DataType("bool")]
        public char KPMGOnly { get; set; }

        [AltPropName("@ThirdPartyAllowed")]
        [MaxLength(1)]
        public bool? ThirdPartyAllowed { get; set; }
        [AltPropName("@ContractorAllowed")]
        [MaxLength(1)]
        public int? ContractorAllowed { get; set; }
        [AltPropName("@ContractorProhibited")]
        [MaxLength(1)]
        public bool? ContractorProhibited { get; set; }

        [AltPropName("@ContractorProhibitReason")]
        [MaxLength(1000)]
        public string ContractorProhibitReason { get; set; }
        //13-

        [AltPropName("@GroupOrMulti")]
        [MaxLength(1)]
        public char GroupOrMulti { get; set; }

        [AltPropName("@GroupOrMultiInfo")]
        [MaxLength()]
        public string GroupOrMultiInfo { get; set; }

        //11-
        [AltPropName("@PrimaryWBName")]
        [MaxLength(500)]
        public string PrimaryWBName { get; set; }
        //11-

        //6-
        [AltPropName("@ENGYear")]
        [MaxLength(4)]
        public string ENGYear { get; set; }
        //6-

        //8-
        [AltPropName("@SplitMAF")]
        [MaxLength(1)]
        public char SplitMAF { get; set; }
        [AltPropName("@CombineMAF")]
        [MaxLength(1)]
        public char CombineMAF { get; set; }
        //8-

        [AltPropName("@OfficeLocation")]
        [MaxLength(500)]
        public string OfficeLocation { get; set; }

        //4-
        [AltPropName("@BusinessUnit")]
        [MaxLength(500)]
        public string BusinessUnit { get; set; }
        //4-

        //3-
        [AltPropName("@ClientYearEndDate")]
        public DateTime? ClientYearEndDate { get; set; }
        //3-

        //5-
        [AltPropName("@eAuditWorkflow")]
        [MaxLength(50)]
        public string eAuditWorkflow { get; set; }
        //5-

        //7-
        [AltPropName("@EBPEngagement")]
        [MaxLength(1)]
        public char EBPEngagement { get; set; }
        //7-

        [AltPropName("@11KEBP")]
        [MaxLength(1)]
        public char _11KEBP { get; set; }

        //@ActivityList udtTblSurveyActivityList READONLY

        //Error - @ActivityList -- Required Date cannot be blank.
        public DateTime? RequiredDate { get; set; }
        public SurveyRequestType SurveyRequestType { get; set; }
        public string WorkBooks { get; set; }

        public char IsRFInDiffWF { get; set; }
        public char IsPartilaRF { get; set; }
        public string RFModificationType { get; set; }

        public bool IsSawEng { get; set; }

        [AltPropName("@ProjectCode")]
        [MaxLength(50)]
        public string ProjectCode { get; set; }

        [AltPropName("@DcssServer")]
        [MaxLength(50)]
        public string DCSServer { get; set; }

        [AltPropName("@ListofDcssServers")]
        public string ListofDcssServers { get; set; }
    }
}
