using System;

namespace AuditManager.Model
{
    [System.AttributeUsage(AttributeTargets.Property)]
    public class ImProfileAttrInfo : System.Attribute
    {
        public string AttrImProfileEnum { get; set; }
        public Type AttrDataType { get; set; }
        public ImProfileAttrInfo(string attrImProfileEnum, Type attrDataType = null)
        {
            AttrImProfileEnum = attrImProfileEnum;
            AttrDataType = attrDataType ?? typeof(string);
        }
    }

    [System.AttributeUsage(AttributeTargets.Property)]
    public class AltPropName : System.Attribute
    {
        public string Name { get; private set; }
        public AltPropName(string name)
        {
            Name = name;
        }
    }
}
