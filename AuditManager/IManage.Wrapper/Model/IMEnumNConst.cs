using System.ComponentModel;

namespace IM.Wrapper.Model
{
    public enum Env : byte
    {
        Dev = 1,
        Stg,
        Prod,
    }

    internal enum IMDbType : byte
    {
        Active = 1,
        CSS,
    }

    internal enum IMWSObjectType : byte
    {
        [Description("page")]
        Workspace = 1,
        [Description("folder:ordinary,")]
        Folder,
        [Description("document")]
        File,
    }

    internal class IMConst
    {
        public const string CONST_WORKSPACE_VALUE = "%workspace_value%";
        public const string CONST_IMPROFILECLASS = "DOC";
        public const string CONST_IMPROFILECUSTOM7 = "7YEARS";
        public const string CONST_IMPROFILECUSTOM3 = "AUDIT";
        public const string CONST_DELETE_PREFIX = "DEL_";
        public const string CONST_DELETE_USER = "DELETEUSER";
        public const string CONST_DELETE_COMMENT = "Marked for Deletion";
        public const string CONST_APP_NAME = "AUDIT MANAGER";

        public const string CONST_CHECKIN_COMMENT = "CHECKIN";
        public const string CONST_CLOSURE_COMMENT = "eAudit closure";

        public const string CONST_WSOBJECTTYPE_WORKSPACE = "page";
    }
}
