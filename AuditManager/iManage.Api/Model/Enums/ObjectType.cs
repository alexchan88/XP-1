using System.ComponentModel;

namespace iManage.Api
{
    public enum ObjectType : byte
    {
        [Description("document")]
        File = 1,
        [Description("folder:ordinary,")]
        Folder,
        [Description("page")]
        Workspace
    }
}
