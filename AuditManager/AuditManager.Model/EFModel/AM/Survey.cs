using System;

namespace AuditManager.Model.EFModel.AM
{
    public class BusinessUnit : CommonProperty
    {
        
    }

    public class EAuditWorkflow : CommonProperty
    {
        
    }

    public class Survey : PostFix
    {
        public int Id { get; set; }
        public SurveyTypeEnum SurveyTypeId { get; set; }
        public virtual SurveyType SurveyType { get; set; }
        public int EngagementProfileId { get; set; }
        public virtual EngagementProfile EngagementProfile { get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
        public DateTime ClientYearEnd { get; set; }
        public int BusinessUnitId { get; set; }
        public virtual BusinessUnit BusinessUnit { get; set; }
        public int EAuditWorkflowId { get; set; }
        public virtual EAuditWorkflow EAuditWorkflow { get; set; }
        public int YearOfEAudit { get; set; }
        public bool IsEBP { get; set; }
        public bool IsToSplitMAF { get; set; }
        public bool IsToJoinMAF { get; set; }
        public bool IsRFToDifferentWorkflow { get; set; }
        public bool IsPartialRF { get; set; }
        public string PartialRFMethodology { get; set; }
        public bool IsSAW { get; set; }
        public int NumberOfMAF { get; set; }
        public string PrimaryWorkbookName { get; set; }
        public string RFProcessInstruction { get; set; }
    }

    public class SurveyWorkbook : BaseEntity
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public virtual Survey Survey { get; set; }
        public string Workbook { get; set; }
    }
}
