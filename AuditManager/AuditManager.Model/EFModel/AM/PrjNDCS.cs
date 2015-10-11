
namespace AuditManager.Model.EFModel.AM
{
    public abstract class Pre
    {
        //IB
        //ID
        //MY
        //MD
    }

    public class EngNClient : Pre
    {
        public int Id { get; set; }
        public int DocNum { get; set; }
        //nvarchar32
        public string ClientNum { get; set; }
        //nvarchar32
        public string EngNum { get; set; }
    }

    public class DCSProject : Pre
    {
        public int Id { get; set; }
        //nvarchar32
        public string ProjectCode { get; set; }
        //nvarchar512
        public string ProjectDescription { get; set; }
    }

    public class DCSServer : Pre
    {
        public int Id { get; set; }
        public int ServerID { get; set; }
        //nvarchar32
        public string ServerName { get; set; }
        //nvarchar512
        public string ServerFullyQualifiedName { get; set; }
        //nvarchar32
        public string ServerStatus { get; set; }
        //nvarchar32
        public string ServerType { get; set; }
        //nvarchar32
        public string ServerEnvironment { get; set; }
    }

    public class DCSPrjNServer : Pre
    {
        public int Id { get; set; }
        //key
        public int DCSProjectId { get; set; }
        //key
        public int DCCServerId { get; set; }
    }

    public class EngDocPrjNDCS : Pre
    {
        public int Id { get; set; }

        public int EngNClientId { get; set; }
        public int DCSProjectId { get; set; }
        public int DCSServerId { get; set; }
    }
}
