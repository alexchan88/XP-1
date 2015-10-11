using System;
using System.Text;

namespace AuditManager.Model.EFModel.AM
{
    public class WsFloatingField : AmDbEntityModel
    {
        public int WsFloatingFieldId { get; set; }
        public string EngNum { get; set; }
        public bool? IsUnderPreservation { get; set; }
        public string Comment { get; set; }
        public DateTime? EventTrgDate { get; set; }
        public bool? IsServer2 { get; set; }
        public bool? IsKDrive { get; set; }

        //TP_Q1
        //TP_Q2
        //TP_Q3
        //TP_Q3_Comment
        public string TPAns { get; set; }

        public string EnteredBy { get; set; }
        public DateTime EnteredDate { get; set; }
        public bool IsActive { get; set; }

        public string GetWsFloatingFieldInfo
        {
            get
            {
                var sb = new StringBuilder();

                sb.AppendLine("Engagement Number: " + this.EngNum);
                sb.AppendLine("");
                sb.AppendLine("IsUnderPreservation: " + this.IsUnderPreservation);
                sb.AppendLine("EventTrgDate: " + this.EventTrgDate);
                sb.AppendLine("IsServer2: " + this.IsServer2);
                sb.AppendLine("IsKDrive: " + this.IsKDrive);
                sb.AppendLine("");
                sb.AppendLine("TPAns: " + this.TPAns);
                sb.AppendLine("");
                sb.AppendLine("UpdatedBy: " + this.EnteredBy);
                sb.AppendLine("UpdatedDate: " + this.EnteredDate);
                
                return sb.ToString();
            }
        }
    }
}
