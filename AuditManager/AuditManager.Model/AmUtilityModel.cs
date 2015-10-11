using AuditManager.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuditManager.Model
{
    public class AmConst
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

        //spForceCheckInByDocNum
    }

    public abstract class WsLog
    {
        private DateTime _ActionWhen = DateTime.Now;

        public string EngNum { get; set; }

        public double? Id { get; set; }

        public string Name { get; set; }

        public string ActionInfo { get; set; }

        public string AdditionalInfo { get; set; }

        public WsLogActivityType WsLogActivityType { get; set; }

        public string ActionBy { get; set; }

        public string Comment { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string MailBody { get; set; }

        public WsLogUseWhat WsLogUseWhat { get; set; }

        public virtual string LogDocName
        {
            get
            {
                //return string.Format("{0}_{1}_{2}_{3}_{4}.{5}",
                return string.Format("{0}{1}_{2}{3}_{4}.{5}",
                    this.EngNum,
                    this.Id == null ? string.Empty : "_" + this.Id.ToString(),
                    this.WsLogActivityType.ToString(),
                    string.IsNullOrWhiteSpace(this.ActionInfo) ? string.Empty : "_" + this.ActionInfo,
                    _ActionWhen.ToString("yyMMdd_HHmmss"),
                    this.WsLogActivityType.ToEnumDesc<WsLogActivityType>());
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("EngNum: " + this.EngNum);

            if (this.Id != null)
                sb.AppendLine("Id: " + this.Id);

            if (!string.IsNullOrWhiteSpace(this.Name))
                sb.AppendLine("Name: " + this.Name);

            sb.AppendLine();

            sb.AppendLine("Action: " + this.WsLogActivityType.ToString());
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(this.OldValue) && !string.IsNullOrWhiteSpace(this.NewValue))
            {
                sb.AppendLine("OldValue :: -> " + this.OldValue);
                sb.AppendLine("NewValue :: -> " + this.NewValue);
                sb.AppendLine();
            }

            if (!string.IsNullOrWhiteSpace(this.AdditionalInfo))
            {
                sb.AppendLine("Info :: -> ");
                sb.AppendLine();
                sb.AppendLine(this.AdditionalInfo);
                sb.AppendLine();
            }

            sb.AppendLine("Time: " + this._ActionWhen.ToString());
            sb.AppendLine("Requestor: " + this.ActionBy);

            if (!string.IsNullOrWhiteSpace(this.Comment))
            {
                sb.AppendLine();
                sb.AppendLine("Comment:: -> " + this.Comment);
            }

            return sb.ToString();
        }
    }

    public class WsGenericLog : WsLog
    {

    }

    public class NonClsrEng
    {
        public string EngNum { get; set; }
        public List<NonClsrFldr> NonClsrFldrs { get; set; }
    }

    public class NonClsrFldr
    {
        public string Path { get; set; }
        public List<NonClsrFile> NonClsrFiles { get; set; }
    }

    public class NonClsrFile
    {
        public double Number { get; set; }
        public string Name { get; set; }
    }

    public class KUsr
    {
        public string ActiveDirectory { get { return ConfigUtility.GetActiveDirectory(); } }
        public string UserId { get; set; }
        [AltPropName("c")]
        public string Domain { get; set; }
        [AltPropName("givenName")]
        public string FName { get; set; }
        [AltPropName("sn")]
        public string LName { get; set; }
        public string FullName { get { return string.Format("{0} {1}", this.FName, this.LName); } }
        [AltPropName("mail")]
        public string EmailId { get; set; }
        [AltPropName("initials")]
        public string UserInitials { get; set; }
        [AltPropName("l")]
        public string Location { get; set; }
        [AltPropName("department")]
        public string Department { get; set; }
    }
}
