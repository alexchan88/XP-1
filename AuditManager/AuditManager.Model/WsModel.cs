using AuditManager.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace AuditManager.Model
{
    public class EngDocByRetModel
    {
        public string FolderPath { get; set; }
        public string FileName { get; set; }
        public int DocumentNumber { get; set; }
        public string FileType { get; set; }
    }
    
    public class FileActivity_UpdateModel
    {
        public string wsId { get; set; }
        public string docObjId { get; set; }
        public bool IsPage_Act { get; set; }

        public string EngNum { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public WsActivityType WsActivityType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public WsFileType WsFileType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public FileIn FileIn { get; set; }

        //public int FileUniqueId { get; set; }
        public string FileUniqueId { get; set; }

        public double FileNum { get; set; }
        //public bool IsS2 { get; set; }
        public bool? NonAuditFlag { get; set; }
        public string Comment { get; set; }
        public string logAs { get; set; }
    }
    
    public class DocumentStatus
    {
        public string DocumentNumber { get; set; }
        public string IsPresent { get; set; }
        public string DocumentType { get; set; }
        public string Status { get; set; }
        public string UniqueId { get; set; }
        public string ActionBy { get; set; }
        public DateTime? ActionDate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FileIn FileIn { get; set; }
    }

    public class WsUpdateModel
    {
        public WsModel WsModel { get; set; }
        public string PreservationComment { get; set; }
        public string RetentionComment { get; set; }
    }

    public class InitiateClosureWsModel
    {
        public WsModel WsModel { get; set; }
        public string Comment { get; set; }
        public List<string> LargeRetFiles { get; set; }
    }

    public class WsModel
    {
        public bool IsLoaded { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string ObjectID { get; set; }

        public WsUser Owner { get; set; }
        public string SubType { get; set; }
        public int WorkspaceID { get; set; }

        public WsProfile WsProfile { get; set; }
        public List<WsFldr> WsFldrs { get; set; }

        public List<WsGroup> WsGroups { get; set; }
    }

    //public class WsProfile_TP
    //{
    //    public string TP_Q1 { get; set; }
    //    public string TP_Q2 { get; set; }
    //    public string TP_Q3 { get; set; }
    //    public string TP_Q3_Comment { get; set; }

    //    public bool KPMGOnly { get; set; }
    //}

    public class WsProfile
    {
        [ImProfileAttrInfo("imProfileCustom7")]
        public string RetPolicy { get; set; }

        [ImProfileAttrInfo("imProfileAuthor")]
        public string Author { get; set; }
        [ImProfileAttrInfo("imProfileCustom1")]
        public string Client { get; set; }
        [ImProfileAttrInfo("imProfileCustom1Description")]
        public string ClientDesc { get; set; }
        [ImProfileAttrInfo("imProfileCustom2")]
        public string EngNum { get; set; }
        [ImProfileAttrInfo("imProfileCustom3")]
        public string EngFunction { get; set; }
        [ImProfileAttrInfo("imProfileCustom4")]
        public string Partner { get; set; }
        [ImProfileAttrInfo("imProfileCustom4Description")]
        public string Partner_Desc { get; set; }
        [ImProfileAttrInfo("imProfileCustom6")]
        public string Manager { get; set; }
        [ImProfileAttrInfo("imProfileCustom6Description")]
        public string Manager_Desc { get; set; }
        [ImProfileAttrInfo("imProfileCustom11")]
        public string Status { get; set; }

        [ImProfileAttrInfo("imProfileCustom12", typeof(bool))]
        public bool IsServer2 { get; set; }
        [ImProfileAttrInfo("imProfileCustom23", typeof(DateTime))]
        public DateTime? EventTrgDate { get; set; }
        [ImProfileAttrInfo("imProfileCustom26", typeof(bool))]
        public bool IsKDrive { get; set; }

        public bool IsUnderPreservation { get; set; }
        public WsUser PartnerDesc { get; set; }
        public WsUser ManagerDesc { get; set; }

        //public WsProfile_TP WsProfile_TP { get; set; }

        public bool KPMGOnly { get; set; }
    }

    public class WsFldr
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string FolderPath { get; set; }
        public string ObjectID { get; set; }

        public int FolderID { get; set; }

        public List<WsFldr> WsFldrs { get; set; }
        public List<WsFile> WsFiles { get; set; }
    }

    public class WsFile
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string ObjectID { get; set; }

        public WsUser Author { get; set; }
        public WsUser Operator { get; set; }

        public int Number { get; set; }
        public string Extn { get; set; }
        public int Size { get; set; }
        public int Version { get; set; }
        public bool IsLatestVersion { get; set; }
        public int VersionCount { get; set; }

        public bool IsCheckedOut { get; set; }
        public bool IsLocked { get; set; }

        public string CreationDate { get; set; }

        [ImProfileAttrInfo("imProfileFrozen", typeof(bool))]
        public bool IsRecord { get; set; }

        public DateTime? RecordDate { get; set; }
        public string RecordUser { get; set; }

        [ImProfileAttrInfo("imProfileCustom27", typeof(bool))]
        public bool IsDeleted { get; set; }

        [ImProfileAttrInfo("imProfileCustom11")]
        public string Status { get; set; }

        public bool IsUnderPreservation { get; set; }

        [ImProfileAttrInfo("imProfileCustom12", typeof(bool))]
        public bool IsServer2 { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public WsFileType WsFileType
        {
            get
            {

                var fType = Description.FileType(Extn);
                return fType.ToEnum<WsFileType>();

            }
        }

        public bool IsIncludedInClosure { get; set; }

        //[JsonConverter(typeof(StringEnumConverter))]
        //public ActivityStatusType ActivityStatusType { get; set; }

        public DocumentStatus DocumentStatus { get; set; }
    }

    public class WsUser
    {
        public string ObjectID { get; set; }
        public string DomainName { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
    }

    public class WsSecurity
    {

    }

    public class WsGroup
    {
        public string Name { get; set; }
        public string DomainName { get; set; }
        public string FullName { get; set; }
        public string ObjectID { get; set; }
        public List<WsUser> GrpUsers { get; set; }
        public int GroupNumber { get; set; }
        public bool Enabled { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public WsAccessRight WsAccessRight { get; set; }
        
        //-C
        [JsonConverter(typeof(StringEnumConverter))]
        public WsUserType WsUserType { get; set; }
    }



    public class WsCustAttr
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
    }
}
