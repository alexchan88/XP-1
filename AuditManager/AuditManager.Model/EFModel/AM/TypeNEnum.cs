
namespace AuditManager.Model.EFModel.AM
{
    public enum ResponseTypeEnum : byte
    {
        No = 0,
        Yes = 1,
        None = 2,
    }

    public class ResponseType : ShortCommonProperty
    {

    }

    public enum ProfileUpdateActionTypeEnum : byte
    {
        WSCreated = 1,
        ProfileUpdate,
        RFRequest,
        RETRequest,
        LinkGuid,
        DeLinkGuid,
    }

    public class ProfileUpdateActionType : ShortCommonProperty
    {

    }

    public enum ActivityTypeEnum : byte
    {
        CreateWorkspace = 1,
        RequestWorkspaceAccess,
        AddUser,
        DeleteUser,
        ChangeEventTriggerDate,
        PreservationOn,
        PreservationOff,
        InitiateClosure,
        LinkGuid,
        DeLinkGuid,
        FileUpload,
        FileDownload,
        Acknowledge,
        Reprocess,
        Delete
    }

    public class ActivityType : ShortCommonProperty
    {

    }

    public enum SurveyTypeEnum : byte
    {
        RF = 1,
        RET,
    }

    public class SurveyType : ShortCommonProperty
    {

    }

    public enum StatusTypeEnum : byte
    {
        Open = 1,
        Closed,
        InProgress,
        Acknowledged,
        Deleted,
        PendingAcknowledgement,
        ReprocessRequested,
    }

    public class StatusType : ShortCommonProperty
    {

    }
}
