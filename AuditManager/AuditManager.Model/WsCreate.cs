using System.Text;

namespace AuditManager.Model
{
    public class PartnerAssistance
    {
        public string PartnerAssistanceId { get; set; }
        public string PartnerAssistanceName { get; set; }
    }
    
    public class WsCreate
    {
        public WsCreate()
        {
            //this.WsProfile_TP = new WsProfile_TP();
        }
        public string EngagementNumber {get; set;}
        public string EngagementDescription { get; set; }

        public string ClientId { get; set; }
        public string ClientName { get; set; }

        public string ManagerId { get; set; }
        public string ManagerEmpId { get; set; }
        public string ManagerName { get; set; }
        public string ManagerFunction { get; set; }

        public string PartnerId { get; set; }
        public string PartnerEmpId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerFunction { get; set; }

        public string ParentCompany { get; set; }

        public string PartnerAssistanceId { get; set; }
        public string PartnerAssistanceEmpId { get; set; }
        public string PartnerAssistanceName { get; set; }

        //public WsProfile_TP WsProfile_TP { get; set; }

        public string GetWorkSpaceInfo
        {
            get
            {
                var sb = new StringBuilder();

                sb.AppendLine("Engagement Number: " + this.EngagementNumber);
                sb.AppendLine("Engagement Name: " + this.EngagementDescription);
                sb.AppendLine("");
                sb.AppendLine("Client Name: " + this.ClientName);
                sb.AppendLine("");
                sb.AppendLine("Manager: " + this.ManagerName + "[" + ManagerId + "]");
                sb.AppendLine("Partner: " + this.PartnerName + "[" + PartnerId + "]");
                sb.AppendLine("Partner Assistant: " + this.PartnerAssistanceName + "[" + PartnerAssistanceId + "]");
                sb.AppendLine("");

                //sb.AppendLine("Q1: " + (this.WsProfile_TP == null ? "" : this.WsProfile_TP.TP_Q1));
                //sb.AppendLine("Q2: " + (this.WsProfile_TP == null ? "" : this.WsProfile_TP.TP_Q2));
                //sb.AppendLine("Q3: " + (this.WsProfile_TP == null ? "" : this.WsProfile_TP.TP_Q3));
                //sb.AppendLine("Q3_Comment: " + (this.WsProfile_TP == null ? "" : this.WsProfile_TP.TP_Q3_Comment));

                sb.AppendLine("");
                sb.AppendLine("Created By: {0}");

                return sb.ToString();

            }
        }
    }
}
