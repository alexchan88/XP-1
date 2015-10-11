using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace AuditManager.Model
{
    public enum ExtApiType : byte
    {
        //[Description(ConfigUtility.GetPrjNDcs_url_Prj)]
        Prj = 1,
        //[Description(ConfigUtility.GetPrjNDcs_url_Dcs)]
        Dcs,
        //[Description(ConfigUtility.GetPrjNDcs_url_Dcs_All)]
        DcsAll,
        LauchSite_GetFoldersRollforward,
    }

    public enum ConfigType : byte
    {
        USER = 1,

        USER_ADMIN,
        USER_SUPER,
        USER_ACTIVITY,

        EMAIL,
        
        EMAIL_CC_SUPPORT,
        EMAIL_BCC_INCLUDE,
        EMAIL_BCC_ADDRESS,

        EMAIL_SENDER_CLOSURE,
        EMAIL_SENDER_PRESERVATION,
        EMAIL_SENDER_CREATEWS,

        EMAIL_SENDER_ADDRESS_CLOSURE,
        EMAIL_SENDER_ADDRESS_PRESERVATION,
        EMAIL_SENDER_ADDRESS_CREATEWS,

        EMAIL_SUBJECT_CLOSURE,
        EMAIL_SUBJECT_PRESERVATION,
        EMAIL_SUBJECT_CREATEWS,

        SMTP,

        SMTP_HOST,
        
        SMTP_DELIVERY_NETWORK,
        SMTP_DELIVERY_SPECIFIEDPICKUPDIRECTORY,
        SMTP_DELIVERY_SPECIFIEDPICKUPDIRECTORY_FROM,

        SMTP_DEFAULTCREDENTIALS,
        SMTP_PICKUPDIRECTORYLOCATION,

        IMANAGE,

        IMANAGE_SERVER,

        IMANAGE_USER_TRUSTED,
        IMANAGE_USER_ID,
        IMANAGE_USER_PWD,

        IMANAGE_ADMIN_TRUSTED,
        IMANAGE_ADMIN_ID,
        IMANAGE_ADMIN_PWD,

        IMANAGE_WS_CREATE_TEMPLATE_ID,

        UPDOWN,

        UPDOWN_UPLOAD_URL,
        UPDOWN_DOWNLOAD_URL,

        AUDITMANAGER,

        AUDITMANAGER_ENV,
        AUDITMANAGER_HOST,
        AUDITMANAGER_DOMAIN,
        AUDITMANAGER_DOMAIN_SHORT,

        MISC,

        MISC_DEFAULT_SERACH_FROM_DATE,

        SQL,

        SQL_INITIALCATALOG_AUDITMANAGER,
        SQL_INITIALCATALOG_ACTIVE,
        SQL_INITIALCATALOG_ERROR,

        SQL_INITIALCATALOG_REFERENCE,

        SQL_INITIALCATALOG_SSC,
        SQL_INITIALCATALOG_S2,

        SQL_DATASOURCE_AUDITMANAGER,
        SQL_DATASOURCE_ACTIVE,
        SQL_DATASOURCE_ERROR,

        SQL_DATASOURCE_REFERENCE,

        SQL_DATASOURCE_SSC,
        SQL_DATASOURCE_S2,

        SQL_UID_AUDITMANAGER,
        SQL_UID_ACTIVE,
        SQL_UID_ERROR,

        SQL_UID_REFERENCE,

        SQL_UID_SSC,
        SQL_UID_S2,

        SQL_PWD_AUDITMANAGER,
        SQL_PWD_ACTIVE,
        SQL_PWD_ERROR,

        SQL_PWD_REFERENCE,

        SQL_PWD_SSC,
        SQL_PWD_S2,
    }

    public enum UpdateProfileFrom : byte
    {
        Profile = 1,
        RET,
        RF,
        CreateWs,
        S2
    }

    public enum WsLogUseWhat : byte
    {
        WsLog = 1,
        Email,
    }

    public enum EmailRecepientType : byte
    {
        ADMIN = 1,
        MEMBERS,
        ADMIN_N_MEMBERS,
        MANAGER,
        PARTNER,
        MANAGER_N_PARTNER,
        MANAGER_N_PARTNER_N_ADMIN,
        MANAGER_N_PARTNER_N_ADMIN_N_MEMBERS,
    }

    public enum HistoryInfoType : byte
    {
        DELETE = 1,
        CHECKIN,
        CLOSURE,
    }

    public enum WsActivityType : byte
    {
        CLOSURE = 1,
        CLOSURE_INITIATE,
        CLOSURE_PENDINGFILES,

        GUID,
        GUID_LINK,
        GUID_DELINK,

        WSPROFILE,
        WSPROFILE_PRESERVATION_ON,
        WSPROFILE_PRESERVATION_OFF,

        CREATE,
        WS_CREATE,
        DOC_CREATE,

        UPDATE,
        WSPROFILE_UPDATE,

        DELETE,
        DOC_DELETE,
        WS_DELETE,

        SURVEY,
        SURVEY_RF,
        SURVEY_RET,

        Activity,
        Activity_Reprocess,
        Activity_Acknowledge,
        Activity_Remove,

        Workspace,
        Workspace_RequestAccess,
    }

    public enum WsMailStatusType : byte
    {
        Success = 1,
        Failure,
        InfoSentToService,
    }

    public enum WsAccessRight : byte
    {
        NONE = 1,
        ALL,
        READ,
        READWRITE,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WsUserType : byte
    {
        NONE = 1,
        ADMIN,
        MEMBERS,
        READ,
    }

    public enum SurveyRequestType : byte
    {
        RF = 1,
        RET
    }

    public enum ImDbType : byte
    {
        Active = 1,
        CSS,
    }

    public enum WsLoadType : byte
    {
        ALL = 1,
        None,
        Profile,
        Fldrs,
        Groups,
        ProfileNFldrs,
    }

    public enum WsComponentType : byte
    {
        Profile = 1,
        Fldrs,
        Groups,
    }

    public enum WsObjectType : byte
    {
        [Description("page")]
        Workspace = 1,
        [Description("folder:ordinary,")]
        Folder,
        [Description("document")]
        File,
    }

    public enum WsDocDelStatusType : byte
    {
        Success = 1,
        MultiVersion,
        FileNotFound,
        InsufficientRights,
        WSUnderPreservation,
        IsRecord,
        IsDeleted,
        IsCheckedOut,
        IsLocked,
    }

    public enum WsLogActivityType : byte
    {
        [Description("txt")]
        DeleteFile = 1,
        [Description("txt")]
        Preservation,
        [Description("txt")]
        EventTrgDate,
        [Description("txt")]
        RetPolicy,
        [Description("txt")]
        ChangeStorageLocation,
        [Description("txt")]
        Server2,
        [Description("txt")]
        Closure,
        [Description("txt")]
        ClosureRequest,
        [Description("txt")]
        ClosureComplete,
        [Description("txt")]
        CreateWs,
        [Description("html")]
        ClosureConfirmEmail,
        [Description("html")]
        PreservationONEmail,
        [Description("html")]
        PreservationOFFEmail,
        [Description("html")]
        CreateWorkspaceEmail,
        [Description("txt")]
        ProfileUpdate,
        [Description("txt")]
        ProfileUpdate_RF,
        [Description("txt")]
        ProfileUpdate_RET,
        [Description("txt")]
        ProfileUpdate_S2,
        [Description("txt")]
        ProfileUpdate_WsCreate,
    }

    public enum WsFldrType : byte
    {
        [Description("Workspace Log")]
        WsLog = 1,
        [Description("2 - Period-end Audit")]
        PeriodEndAudit,
    }

    public enum WsFileType : byte
    {
        Other = 0,
        Eng,
        Ret,
    }

    public enum ActivityFilterType : byte
    {
        ALL = 1,
        [Description("PDF")]
        RET,
        RF,
        S2,
        S2_RET,
        S2_RF,
    }

    public enum FileIn : byte
    {
        Orphan = 0,
        SSC,
        S2,
    }

    public enum UsrSearchBy : byte
    {
        Name = 1,
        Email,
    }
}
