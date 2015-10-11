using AuditManager.Common;
using AuditManager.EF.AmDbCtx;
using AuditManager.Model;
using AuditManager.Model.EFModel.Active;
using AuditManager.Model.EFModel.AM;
using Elmah;
using IManage;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace IM.Mgr
{
    public class WsUtility
    {
        //private const string TempDocLocation = "\\tempDoc";

        public static void TestWs()
        {
            var ms = new ManStrings();
            ms.Add("ACTIVE");
            var s1 = IManageSession.AdminSession().DMS.CreateProfileSearchParameters();
            s1.Add(imProfileAttributeID.imProfileCustom2, "12357296");
            var s2 = IManageSession.AdminSession().DMS.CreateWorkspaceSearchParameters();
            s2.Add(imFolderAttributeID.imFolderSubtype, "work");

            IManFolders f = (IManFolders)IManageSession.AdminSession().SearchWorkspaces(ms, s1, s2);

            //var retFolders = _IManDatabase.Session.SearchWorkspaces(colDBs, wsProfileParams, wsSearchParams);
            if (f != null)
            {
                List<IManWorkspace> retWSs = new List<IManWorkspace>();

                foreach (IManFolder retWs in f)
                {
                    try
                    {
                        IManWorkspace manWS = (IManWorkspace)retWs;
                        //if (manWS.SubTypeEnum == imWorkspaceSubtype.imWorkspaceWorkSubtype)
                        {
                            retWSs.Add(manWS);
                        }
                    }
                    catch { } //Ignore any folder to Workspace conversion errors
                }
                //return retWSs;
            }
            else
            {
                //return null;
            } // Workspaces found check



        }

        private static void HandleTreeItems(Action<WsFldr> item, WsFldr parent)
        {
            if (parent.WsFldrs.Count > 0)
            {
                foreach (WsFldr child in parent.WsFldrs)
                {
                    child.Name = parent.Name + "/" + child.Name;
                    HandleTreeItems(item, child);
                }

            }

            item(parent);
        }

        private static IManDocumentType GetWsDocType(IManWorkspace imWs, string fileName)
        {
            IManDocumentTypes imDocTypes = imWs.Database.SearchDocumentTypes(string.Empty, imSearchAttributeType.imSearchBoth, true);

            foreach (IManDocumentType imDocType in imDocTypes)
            {
                if (imDocType.ApplicationExtension.Equals(fileName.FileExtn(), StringComparison.OrdinalIgnoreCase))
                {
                    return imDocType;
                }
            }

            return null;
        }

        internal static IManDocumentFolder GetWsFldr(IManWorkspace imWs, WsFldrType wsFldrType)
        {
            foreach (IManDocumentFolder imFldr in imWs.SubFolders)
            {
                if (imFldr.Name.Equals(wsFldrType.ToEnumDesc<WsFldrType>(), StringComparison.OrdinalIgnoreCase))
                {
                    return imFldr;
                }
            }

            return null;
        }

        internal static T GetObjectByID_New<T>(string objId, bool newSession = false, bool isAdmin = false)
        {
            //--V469-583-S
            //return (T)ImSession.Get_New_Instance.GetSession().DMS.GetObjectByID(objId);
            return isAdmin ? (T)IManageSession.AdminSession().DMS.GetObjectByID(objId) : (T)IManageSession.UsrSession(newSession).DMS.GetObjectByID(objId);
            //--V469-583-E
        }

        //--V469-583-S
        internal static T GetObjectByID_Admin<T>(string objId, bool newSession = false)
        {
            return (T)IManageSession.AdminSession().DMS.GetObjectByID(objId);
        }
        //--V469-583-E

        internal static T GetObjectByID<T>(string objId, bool newSession = false, bool isAdmin = false)
        {
            //--V469-583-S
            //return newSession ? (T)ImSession.GetNewInstance.GetSession().DMS.GetObjectByID(objId) : (T)ImSession.GetInstance.GetSession().DMS.GetObjectByID(objId);
            return isAdmin ? (T)IManageSession.AdminSession().DMS.GetObjectByID(objId) : (T)IManageSession.UsrSession().DMS.GetObjectByID(objId);
            //--V469-583-E
        }

        internal static WsCustAttr GetWsCustAttr<T>(T prop, string propVal)
            where T : PropertyInfo
        {
            var cA = (ImProfileAttrInfo)prop.GetCustomAttribute(typeof(ImProfileAttrInfo), false);

            if (cA == null)
                return null;

            //--V469-583-S
            //IManCustomAttributes custAttrs = ImSession.GetInstance.GetDb(ImDbType.Active).SearchCustomAttributes(propVal,
            //                (imProfileAttributeID)Enum.Parse(typeof(imProfileAttributeID), cA.AttrImProfileEnum),
            //                imSearchAttributeType.imSearchID,
            //                imSearchEnabledDisabled.imSearchEnabledOrDisabled, true);
            IManCustomAttributes custAttrs = IManageSession.Db(ImDbType.Active).SearchCustomAttributes(propVal,
                            (imProfileAttributeID)Enum.Parse(typeof(imProfileAttributeID), cA.AttrImProfileEnum),
                            imSearchAttributeType.imSearchID,
                            imSearchEnabledDisabled.imSearchEnabledOrDisabled, true);
            //--V469-583-E

            foreach (IManCustomAttribute custAttr in custAttrs)
            {
                return new WsCustAttr
                {
                    Description = custAttr.Description,
                    Enabled = custAttr.Enabled,
                    Name = custAttr.Name,
                };
            }

            return null;
        }

        internal static List<WsUser> GetAllGrpUser(IManUsers imUsrs)
        {
            List<WsUser> wsUsers = new List<WsUser>();

            foreach (IManUser imUsr in imUsrs)
            {
                wsUsers.Add(GetWsUser(imUsr));
            }

            return wsUsers;
        }

        internal static WsUser GetWsUser(IManUser imUsr)
        {
            return new WsUser
            {
                DomainName = imUsr.DomainName,
                Email = imUsr.Email,
                FullName = imUsr.FullName,
                Name = imUsr.Name,
                ObjectID = imUsr.ObjectID,
                Location = imUsr.Location,
            };
        }

        internal static bool UserInCustom(imProfileAttributeID imProfileAttributeID)
        {
            //--V469-583-S
            //IManDatabase imDb = ImSession.GetInstance.GetDb(ImDbType.Active);
            IManDatabase imDb = IManageSession.Db(ImDbType.Active);
            //--V469-583-E
            if (imDb == null)
                return false;

            imDb.Session.MaxRowsForSearch = int.MaxValue;

            IManCustomAttributes imCustAttrs = imDb.SearchCustomAttributes(AmUtil.GetCurrentUser, imProfileAttributeID,
                imSearchAttributeType.imSearchBoth, imSearchEnabledDisabled.imSearchEnabledOrDisabled, true);

            return imCustAttrs.ItemByName(AmUtil.GetCurrentUser) != null;
        }

        internal static bool ToLoad(WsLoadType wsLoadType, WsComponentType wsComponentType)
        {
            switch (wsLoadType)
            {
                case WsLoadType.ALL:
                    return true;
                case WsLoadType.Fldrs:
                    return wsComponentType == WsComponentType.Fldrs;
                case WsLoadType.Groups:
                    return wsComponentType == WsComponentType.Groups;
                case WsLoadType.Profile:
                    return wsComponentType == WsComponentType.Profile;
                case WsLoadType.ProfileNFldrs:
                    return (wsComponentType == WsComponentType.Profile || wsComponentType == WsComponentType.Fldrs);
                case WsLoadType.None:
                    return false;
                default:
                    return false;
            }
        }

        internal static void AddUserSecurity(IManDocument imDoc, imAccessRight accessRight)
        {
            if (imDoc.Security.UserACLs.Contains(AmUtil.GetCurrentUser))
                imDoc.Security.UserACLs.ItemByName(AmUtil.GetCurrentUser).Right = accessRight;
            else
                imDoc.Security.UserACLs.Add(AmUtil.GetCurrentUser, accessRight);
        }

        internal static void AddUserSecurity(IManDocument imDoc, string userId, imAccessRight accessRight)
        {
            imDoc.Security.UserACLs.Add(userId, accessRight);
        }

        internal static void AddGroupSecurity(IManDocument imDoc, string grpId, imAccessRight accessRight)
        {
            if (imDoc.Security.GroupACLs.Contains(grpId))
                imDoc.Security.GroupACLs.ItemByName(grpId).Right = accessRight;
            else
                imDoc.Security.GroupACLs.Add(grpId, accessRight);
        }

        public static void GetWsFiles(Action<WsFldr> item, WsFldr parent)
        {
            if (parent.WsFldrs.Count > 0)
            {
                foreach (WsFldr child in parent.WsFldrs)
                {
                    GetWsFiles(item, child);
                }
            }

            item(parent);
        }

        public static Dictionary<string, List<WsFile>> GetFileWithPath(string wsId)
        {
            var wsModel = Workspace.GetWs(wsId, WsLoadType.Fldrs).FirstOrDefault();

            if (wsModel == null || wsModel.WsFldrs == null || wsModel.WsFldrs.Count == 0)
                return null;

            var wsFldr = new WsFldr();
            wsFldr.Name = string.Empty;
            wsFldr.WsFldrs = new List<WsFldr>();
            wsModel.WsFldrs.ForEach(x => wsFldr.WsFldrs.Add(x));

            var dicReturn = new Dictionary<string, List<WsFile>>();

            HandleTreeItems(item =>
            {
                var files = new List<WsFile>();

                if (item.WsFiles != null && item.WsFiles.Count > 0)
                {
                    foreach (var wsFile in item.WsFiles)
                    {
                        files.Add(wsFile);
                    }
                }

                dicReturn.Add(item.Name, files);

            }, wsFldr);

            return dicReturn;
        }

        public static List<WsGroup> GetGrpNUser(ImDbType imDbType)
        {
            //--V469-583-S
            //IManUser imUsr = ImSession.GetInstance.GetDb(imDbType).GetUser(AmUtil.GetCurrentUser);
            IManUser imUsr = IManageSession.Db(imDbType).GetUser(AmUtil.GetCurrentUser);
            //--V469-583-E

            List<WsGroup> wsGroups = new List<WsGroup>();

            foreach (IManGroup imGrp in imUsr.Groups)
            {
                wsGroups.Add(new WsGroup
                {
                    DomainName = imGrp.DomainName,
                    Enabled = imGrp.Enabled,
                    FullName = imGrp.FullName,
                    GroupNumber = imGrp.GroupNumber,
                    GrpUsers = GetAllGrpUser(imGrp.Users),
                    Name = imGrp.Name,
                    ObjectID = imGrp.ObjectID,

                }
                    );
            }

            return wsGroups;
        }

        public static List<WsGroup> GetGrpNUser_Nrt(ImDbType imDbType)
        {
            //--V469-583-S
            //IMANADMIN.INRTUser inrtUsr = NrtSession.GetInstance.GetDb(imDbType).GetUser(AmUtil.GetCurrentUser);
            //IMANADMIN.INRTGroup inrtGrp = NrtSession.GetInstance.GetDb(imDbType).GetGroup("");
            IMANADMIN.INRTUser inrtUsr = NrtSession2.Db(imDbType).GetUser(AmUtil.GetCurrentUser);
            IMANADMIN.INRTGroup inrtGrp = NrtSession2.Db(imDbType).GetGroup("");
            //--V469-583-E

            //--V469-583-S
            //IManUser imUsr = ImSession.GetInstance.GetDb(imDbType).GetUser(AmUtil.GetCurrentUser);
            IManUser imUsr = IManageSession.Db(imDbType).GetUser(AmUtil.GetCurrentUser);
            //--V469-583-E

            List<WsGroup> wsGroups = new List<WsGroup>();

            foreach (IManGroup imGrp in imUsr.Groups)
            {
                wsGroups.Add(new WsGroup
                {
                    DomainName = imGrp.DomainName,
                    Enabled = imGrp.Enabled,
                    FullName = imGrp.FullName,
                    GroupNumber = imGrp.GroupNumber,
                    GrpUsers = GetAllGrpUser(imGrp.Users),
                    Name = imGrp.Name,
                    ObjectID = imGrp.ObjectID,

                }
                    );
            }

            return wsGroups;
        }

        //public static WsUser GetWsUser(string usrId, ImDbType imDbType)
        //{
        //    //--V469-583-S
        //    //IManUser imUsr = ImSession.GetInstance.GetDb(imDbType).GetUser(usrId);
        //    IManUser imUsr = IManageSession.Db(imDbType).GetUser(usrId);
        //    //--V469-583-E

        //    return GetWsUser(imUsr);
        //}

        public static string GetUserIdFromCustomAlias(string customAlias)
        {
            using (var db = new ActiveDbContext())
            {
                var result = db.DOCUSER.Where(x => x.CUSTOM1.Equals(customAlias, StringComparison.OrdinalIgnoreCase)).Select(x => x.USERID).FirstOrDefault();
                return result ?? customAlias;
            }
        }

        public static WsUser GetWsUser(string usrId, ImDbType imDbType)
        {
            try
            {
                //IManUser imUsr = IManageSession.Db(imDbType).GetUser(usrId);
                IManUser imUsr = IManageSession.AdminDb(imDbType).GetUser(GetUserIdFromCustomAlias(usrId));
                return GetWsUser(imUsr);
            }
            catch (Exception ex)
            //catch (COMException ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return null;
        }

        private static void AddSecurityToDoc(IManDocument imDoc, IManDocumentFolder imFldr)
        {
            foreach (IManGroupACL fldrGroup in imFldr.Security.GroupACLs)
            {
                imDoc.Security.GroupACLs.Add(fldrGroup.Group.Name, fldrGroup.Right);
            }

            foreach (IManUserACL fldrUser in imFldr.Security.UserACLs)
            {
                bool addUser = false;
                try
                {
                    if (imDoc.Security.UserACLs.ItemByName(fldrUser.User.Name) == null)
                    {
                        addUser = false;
                    }
                }
                catch { addUser = true; }

                if (addUser)
                {
                    imDoc.Security.UserACLs.Add(fldrUser.User.Name, fldrUser.Right);
                }
            }

            imDoc.Update();
        }

        public static string GetWsObjectTypeId(WsObjectType wsObjectType, double objNum)
        {
            switch (wsObjectType)
            {
                case WsObjectType.File:
                    //--V469-583-S
                    //return string.Format("{0}!{1}:{2},1:", ImSession.GetInstance.GetDb(ImDbType.Active).ObjectID, wsObjectType.ToEnumDesc<WsObjectType>(), objNum);
                    return string.Format("{0}!{1}:{2},1:", IManageSession.Db(ImDbType.Active).ObjectID, wsObjectType.ToEnumDesc<WsObjectType>(), objNum);
                //--V469-583-E
                case WsObjectType.Folder:
                    //--V469-583-S
                    //return string.Format("{0}!{1}:{2}:", ImSession.GetInstance.GetDb(ImDbType.Active).ObjectID, wsObjectType.ToEnumDesc<WsObjectType>(), objNum);
                    return string.Format("{0}!{1}:{2}:", IManageSession.Db(ImDbType.Active).ObjectID, wsObjectType.ToEnumDesc<WsObjectType>(), objNum);
                //--V469-583-E
                case WsObjectType.Workspace:
                    //--V469-583-S
                    //return string.Format("{0}!{1}:{2}:", ImSession.GetInstance.GetDb(ImDbType.Active).ObjectID, wsObjectType.ToEnumDesc<WsObjectType>(), objNum);
                    return string.Format("{0}!{1}:{2}:", IManageSession.Db(ImDbType.Active).ObjectID, wsObjectType.ToEnumDesc<WsObjectType>(), objNum);
                //--V469-583-E
                default:
                    return null;
            }
        }

        public static List<Tuple<string, string>> GetEmailRecepients(WsModel wsModel, EmailRecepientType emailRecepientType, bool includeRequester = true)
        {
            var recepients = new List<Tuple<string, string>>();

            if (emailRecepientType == EmailRecepientType.ADMIN || emailRecepientType == EmailRecepientType.ADMIN_N_MEMBERS
                   || emailRecepientType == EmailRecepientType.MANAGER_N_PARTNER_N_ADMIN || emailRecepientType == EmailRecepientType.MANAGER_N_PARTNER_N_ADMIN_N_MEMBERS)
            {
                wsModel.WsGroups.Where(x => x.WsUserType == WsUserType.ADMIN).FirstOrDefault().GrpUsers.ForEach(y => recepients.Add(new Tuple<string, string>(y.FullName, y.Name)));
            }

            if (emailRecepientType == EmailRecepientType.MEMBERS || emailRecepientType == EmailRecepientType.ADMIN_N_MEMBERS
                || emailRecepientType == EmailRecepientType.MANAGER_N_PARTNER_N_ADMIN_N_MEMBERS)
            {
                wsModel.WsGroups.Where(x => x.WsUserType == WsUserType.MEMBERS).FirstOrDefault().GrpUsers.ForEach(y => recepients.Add(new Tuple<string, string>(y.FullName, y.Name)));
            }

            if (emailRecepientType == EmailRecepientType.MANAGER || emailRecepientType == EmailRecepientType.MANAGER_N_PARTNER
                || emailRecepientType == EmailRecepientType.MANAGER_N_PARTNER_N_ADMIN || emailRecepientType == EmailRecepientType.MANAGER_N_PARTNER_N_ADMIN_N_MEMBERS)
            {
                recepients.Add(new Tuple<string, string>(wsModel.WsProfile.ManagerDesc.FullName, wsModel.WsProfile.ManagerDesc.Name));
            }

            if (emailRecepientType == EmailRecepientType.PARTNER || emailRecepientType == EmailRecepientType.MANAGER_N_PARTNER
                || emailRecepientType == EmailRecepientType.MANAGER_N_PARTNER_N_ADMIN || emailRecepientType == EmailRecepientType.MANAGER_N_PARTNER_N_ADMIN_N_MEMBERS)
            {
                recepients.Add(new Tuple<string, string>(wsModel.WsProfile.PartnerDesc.FullName, wsModel.WsProfile.PartnerDesc.Name));
            }

            if (includeRequester)
            {
                var requestor = WsUsrMgmt.SearchUsr(AmUtil.GetCurrentUser, UsrSearchBy.Email, true).FirstOrDefault();

                recepients.Add(new Tuple<string, string>(requestor.FullName, requestor.Name));
            }

            return recepients.Distinct().ToList();
        }

        private static List<string> GetAllWsForUser(string usrId = null)
        {
            using (new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                using (var db = new ActiveDbContext())
                {
                    SqlParameter p1 = new SqlParameter("@user", string.IsNullOrWhiteSpace(usrId) ? AmUtil.GetCurrentUser : usrId);

                    var result = db.Database.SqlQuery<string>(
                                "SELECT GROUPID " +
                                "FROM [ACTIVE].[MHGROUP].[GROUPS] WHERE " +
                                "GROUPNUM IN " +
                                "( " +
                                "SELECT GROUPNUM  " +
                                "FROM [ACTIVE].[MHGROUP].[GROUPMEMS] WHERE  " +
                                "USERNUM = (SELECT USERNUM  " +
                                "FROM [ACTIVE].[MHGROUP].[DOCUSERS] WHERE  " +
                                "USERID = @user " +
                                "))",
                                p1
                                ).ToList();

                    return result;
                }
            }
        }

        private static KeyValuePair<string, WsUserType> GetEngWithWsUserType(string engWithWsUserType)
        {
            return new KeyValuePair<string, WsUserType>
                (engWithWsUserType.SplitNGet('_', 0),
                (engWithWsUserType.SplitNGet('_', 2)).ToEnum<WsUserType>());
        }

        public static Dictionary<string, List<WsUserType>> GetAllWsForUserWithWsUserType(string usrId = null)
        {
            Dictionary<string, List<WsUserType>> wsForUserWithWsUserType = new Dictionary<string, List<WsUserType>>();

            GetAllWsForUser(usrId).ForEach(x =>
                        {
                            var engWithWsUserType = GetEngWithWsUserType(x);

                            if (wsForUserWithWsUserType.ContainsKey(engWithWsUserType.Key))
                            {
                                var item = wsForUserWithWsUserType.First(y => y.Key == engWithWsUserType.Key);
                                item.Value.Add(engWithWsUserType.Value);

                            }
                            else
                            {
                                var newItem = new List<WsUserType>();
                                newItem.Add(engWithWsUserType.Value);
                                wsForUserWithWsUserType.Add(engWithWsUserType.Key, newItem);
                            }
                        }

                );

            //6666666696_E_MEMBERS
            //6666666667_E_ADMIN
            //6666666894_E_READ_ONLY

            return wsForUserWithWsUserType;
        }

        //var WorkspaceGroupPrefix = "E_";
        //var ReGroupClientID = EngNum; 

        public static void CreateWS(AuditManager.Model.WsCreate wsCreate)
        {
            var wsSource = Ws_Group(wsCreate.EngagementNumber);

            //--V469-583-S
            //var wsNew = ImSession.GetNewInstance.GetDb(ImDbType.Active).CreateWorkspace();
            var wsNew = IManageSession.AdminDb(ImDbType.Active).CreateWorkspace();
            //--V469-583-E

            wsNew.SubType = "work"; //Set project type ("work" for workspaces) 
            wsNew.Name = wsCreate.EngagementDescription + " - " + wsCreate.EngagementNumber;  //set workspace name

            //set workspace description
            if (wsNew.Name != null)
            {
                wsNew.Description = wsNew.Name;
            }
            else
            {
                //If template description is blank, use workspace category(subClass) description
                if (string.IsNullOrEmpty(wsSource.Description))
                {
                    wsNew.Description = wsSource.GetAttributeValueByID(imProfileAttributeID.imProfileSubClassDescription).ToString();
                }
                else
                {
                    wsNew.Description = wsSource.Description;
                }
            } //wsDesc null check

            //Set Security
            //--V469-583-S
            //IManUser usrOwner = ImSession.GetNewInstance.GetDb(ImDbType.Active).GetUser(AuditManager.Common.ConfigUtility.GetImAdminLoginInfo().Item1);  //Get owner user object
            IManUser usrOwner = IManageSession.AdminDb(ImDbType.Active).GetUser(AuditManager.Common.ConfigUtility.GetImAdminLoginInfo().Item1);  //Get owner user object
            //--V469-583-E
            //If the owner user account is found then assign him/her as the owner of WS'
            //If the Owner is not found then Admin user is the owner
            if (usrOwner != null)
            {
                wsNew.Owner = usrOwner;
            }

            //Copy Default security and assign groups
            //Utility.copySecurity(wsSource.Security, wsNew.Security, reGroups);

            var fromSecurity = wsSource.Security;
            var toSecurity = wsNew.Security;

            //Build Regroups
            var reGroups = new NameValueCollection();
            foreach (IManGroupACL tempACL in wsSource.Security.GroupACLs)
            {
                if (tempACL.Group.Name.Substring(0, 2).ToLower() == "E_".Trim().ToLower())
                {
                    reGroups.Add(tempACL.Group.Name, wsCreate.EngagementNumber.Trim() + "_" + tempACL.Group.Name);
                }
            }//Group ACLs for loop

            //
            //Set Security
            toSecurity.DefaultVisibility = fromSecurity.DefaultVisibility;
            toSecurity.Inherited = fromSecurity.Inherited;

            if (!fromSecurity.Inherited)
            {
                //Set user ACLs
                foreach (IManUserACL tempUserACL in fromSecurity.UserACLs)
                {
                    toSecurity.UserACLs.Add(tempUserACL.User.Name, tempUserACL.Right);
                }

                //Set group ACLs
                foreach (IManGroupACL tempGroupACL in fromSecurity.GroupACLs)
                {
                    if (reGroups != null)
                    {
                        if (reGroups.Get(tempGroupACL.Group.Name) == null)
                        {
                            //If the group is NOT in the rename groups collectin then add to security
                            toSecurity.GroupACLs.Add(tempGroupACL.Group.Name, tempGroupACL.Right);
                        }
                        else
                        {
                            //If the group IS in the rename groups collection then 
                            //Add new groups to workspace
                            toSecurity.GroupACLs.Add(reGroups.Get(tempGroupACL.Group.Name), tempGroupACL.Right);
                        }
                    }
                    else
                    {
                        //If Regroups is null then add existing groups to the new workspace
                        toSecurity.GroupACLs.Add(tempGroupACL.Group.Name, tempGroupACL.Right);
                    }
                } //foreach group ACL
            } //Inherited check
            //

            //Set Workspace default security to PRIVATE
            wsNew.Security.DefaultVisibility = imSecurityType.imPrivate;

            //Set default workspace profile from the template
            wsNew.Profile.SetAttributeByID(imProfileAttributeID.imProfileSubClass, wsSource.GetAttributeValueByID(imProfileAttributeID.imProfileSubClass));
            for (int i = (int)imProfileAttributeID.imProfileCustom1; i <= (int)imProfileAttributeID.imProfileCustom30; i++)
            {
                wsNew.Profile.SetAttributeByID((imProfileAttributeID)i, wsSource.GetAttributeValueByID((imProfileAttributeID)i));
            }

            //Set requested Profile
            //if (wsProfile != null)
            //{
            //    for (int i = 0; i <= wsProfile.Keys.Count - 1; i++)
            //    {
            //        wsNew.Profile.SetAttributeByID(wsProfile.Keys.ElementAt(i), wsProfile.Values.ElementAt(i));
            //    }
            //}

            wsNew.Profile.SetAttributeByID(imProfileAttributeID.imProfileCustom1, wsCreate.ClientId);
            wsNew.Profile.SetAttributeByID(imProfileAttributeID.imProfileCustom2, wsCreate.EngagementNumber);
            //wsNew.Profile.SetAttributeByID(imProfileAttributeID.imProfileCustom3, wsCreate.ManagerFunction);
            wsNew.Profile.SetAttributeByID(imProfileAttributeID.imProfileCustom3, "AUDIT");
            //wsNew.Profile.SetAttributeByID(imProfileAttributeID.imProfileCustom4, wsCreate.PartnerId.ToUserIdFromDnsName());
            wsNew.Profile.SetAttributeByID(imProfileAttributeID.imProfileCustom4, wsCreate.PartnerEmpId);
            //IBS Changes
            //profileItemList.Add(imProfileAttributeID.imProfileCustom5, this.EngLOBSector);
            //wsNew.Profile.SetAttributeByID(imProfileAttributeID.imProfileCustom6, wsCreate.ManagerId.ToUserIdFromDnsName());
            wsNew.Profile.SetAttributeByID(imProfileAttributeID.imProfileCustom6, wsCreate.ManagerEmpId);
            wsNew.Profile.SetAttributeByID(imProfileAttributeID.imProfileCustom7, "7YEARS");

            //wsNew.SetAttributeByID(imProfileAttributeID.imProfileCustom11, "OPEN - UNDER PRESERVATION");
            wsNew.SetAttributeByID(imProfileAttributeID.imProfileCustom11, "OPEN");

            //Commit workspace to the database.
            //Workspace has to be committed before adding other objects
            string filePath = Path.GetTempFileName();
            IManProfileUpdateResult updateResult = wsNew.UpdateAllWithResults(filePath);

            //Add Workspace tab (home page) Document folders 
            copyDocFolders(wsSource.DocumentFolders, wsNew.DocumentFolders, usrOwner, reGroups);

            //Copy Tabs and Tab objects and security
            copyWSTabs(wsSource.Tabs, wsNew.Tabs, usrOwner, reGroups);

            if (!String.IsNullOrEmpty(wsCreate.PartnerId))
            {
                IM.Mgr.WsUsrMgmt.AddUsrToGrp(null, wsCreate.EngagementNumber + "_E_" + "ADMIN", wsCreate.PartnerId.ToUserIdFromDnsName());
                IM.Mgr.WsUsrMgmt.AddUsrToGrp(null, wsCreate.EngagementNumber + "_E_" + "MEMBERS", wsCreate.PartnerId.ToUserIdFromDnsName());
            }
            if (!String.IsNullOrEmpty(wsCreate.ManagerId))
            {
                IM.Mgr.WsUsrMgmt.AddUsrToGrp(null, wsCreate.EngagementNumber + "_E_" + "ADMIN", wsCreate.ManagerId.ToUserIdFromDnsName());
                IM.Mgr.WsUsrMgmt.AddUsrToGrp(null, wsCreate.EngagementNumber + "_E_" + "MEMBERS", wsCreate.ManagerId.ToUserIdFromDnsName());
            }
            if (!String.IsNullOrEmpty(wsCreate.PartnerAssistanceId))
            {
                IM.Mgr.WsUsrMgmt.AddUsrToGrp(null, wsCreate.EngagementNumber + "_E_" + "ADMIN", wsCreate.PartnerAssistanceId.ToUserIdFromDnsName());
                IM.Mgr.WsUsrMgmt.AddUsrToGrp(null, wsCreate.EngagementNumber + "_E_" + "MEMBERS", wsCreate.PartnerAssistanceId.ToUserIdFromDnsName());
            }
            //if (!String.IsNullOrEmpty(AuditManager.Common.ConfigUtility.GetImAdminLoginInfo().Item1))
            //{
            //    IM.Mgr.WsUsrMgmt.AddUsrToGrp(null, wsCreate.EngagementNumber + "_E_" + "ADMIN", AuditManager.Common.ConfigUtility.GetImAdminLoginInfo().Item1);
            //    IM.Mgr.WsUsrMgmt.AddUsrToGrp(null, wsCreate.EngagementNumber + "_E_" + "MEMBERS", AuditManager.Common.ConfigUtility.GetImAdminLoginInfo().Item1);
            //}

            //IM.Mgr.WsUsrMgmt.AddUsrToGrp(null, wsCreate.EngagementNumber + "_E_" + "ADMIN", "viveksingh1");
            //IM.Mgr.WsUsrMgmt.AddUsrToGrp(null, wsCreate.EngagementNumber + "_E_" + "MEMBERS", "viveksingh1");

            var wsGLog = new WsGenericLog();
            wsGLog.ActionBy = AmUtil.GetCurrentUser;
            wsGLog.Id = null;
            wsGLog.Name = wsNew.Name;
            wsGLog.ActionInfo = null;
            //wsGLog.AdditionalInfo = "Workspace Created By -> " + AmUtil.GetCurrentUser + wsCreate.GetWorkSpaceInfo;
            wsGLog.AdditionalInfo = string.Format(wsCreate.GetWorkSpaceInfo, AmUtil.GetCurrentUser);
            wsGLog.Comment = null;
            wsGLog.OldValue = null;
            wsGLog.NewValue = null;
            wsGLog.WsLogActivityType = WsLogActivityType.CreateWs;

            IM.Mgr.WsUtility.CreateWsLog(wsNew.ObjectID, wsGLog);

            var recepients = new List<Tuple<string, string, string>>();
            recepients.Add(new Tuple<string, string, string>("Manager", wsCreate.ManagerName, wsCreate.ManagerId.ToUserIdFromDnsName()));
            recepients.Add(new Tuple<string, string, string>("Partner", wsCreate.PartnerName, wsCreate.PartnerId.ToUserIdFromDnsName()));
            if (!String.IsNullOrEmpty(wsCreate.PartnerAssistanceId))
            {
                recepients.Add(new Tuple<string, string, string>("PartnerAssistance", wsCreate.PartnerAssistanceName, wsCreate.PartnerAssistanceId.ToUserIdFromDnsName()));
            }
            else
            {
                recepients.Add(new Tuple<string, string, string>("PartnerAssistance", "", ""));
            }
            recepients.Add(new Tuple<string, string, string>("Creator", AmUtil.GetCurrentUser, AmUtil.GetCurrentUser));

            var mailBody = AmUtil.SendMail_WsCreate(WsActivityType.WS_CREATE.ToString(), wsCreate.EngagementNumber, wsNew.Name,
                recepients);

            wsGLog.WsLogActivityType = WsLogActivityType.CreateWorkspaceEmail;
            WsUtility.CreateEmailLog(wsNew.ObjectID, mailBody, wsGLog);

            WsUtility.SaveWsCreationInfo(wsCreate.EngagementNumber,
                string.Join(",", recepients.Select(x => x.Item3).ToList()),
                mailBody);
        }

        private static void copyDocFolders(IManDocumentFolders fromFolders, IManDocumentFolders toFolders,
          IManUser fldrOwner, NameValueCollection reGroups)
        {
            for (int i = fromFolders.Count; i >= 1; i--)
            {
                IManDocumentFolder newFldr;

                IManDocumentFolder tempFldr = (IManDocumentFolder)fromFolders.ItemByIndex(i);

                if (tempFldr.Security.Inherited)
                {
                    newFldr = toFolders.AddNewDocumentFolderInheriting(tempFldr.Name, tempFldr.Description);
                }
                else
                {
                    newFldr = toFolders.AddNewDocumentFolder(tempFldr.Name, tempFldr.Description);
                }

                //Set Location
                newFldr.Location.Cell = tempFldr.Location.Cell;
                newFldr.Location.Order = tempFldr.Location.Order;

                //Set View
                newFldr.View = tempFldr.View;
                newFldr.Hidden = tempFldr.Hidden;

                //Set Security
                if (fldrOwner != null)
                {
                    newFldr.Owner = fldrOwner;
                }

                copySecurity(tempFldr.Security, newFldr.Security, reGroups);

                //Set Default Document Profile
                foreach (IManAdditionalProperty addProp in tempFldr.AdditionalProperties)
                {
                    if (addProp.Value != "%workspace_value%")
                    {
                        newFldr.AdditionalProperties.Add(addProp.Name, addProp.Value);
                    }
                    else
                    {
                        imProfileAttributeID tempAttID = getAttributeID(addProp);
                        object attribute = newFldr.Workspace.Profile.GetAttributeValueByID(tempAttID);
                        if (attribute != null)
                        {
                            newFldr.AdditionalProperties.Add(addProp.Name, attribute.ToString());
                        }
                    }
                }

                newFldr.Update();

                //Copy documents from template folder
                IManDocuments fldrDocs = (IManDocuments)tempFldr.Contents;
                if (fldrDocs != null && fldrDocs.Count > 0)
                {
                    copyDocs(fldrDocs, (IManDocuments)newFldr.Contents, reGroups);
                }

                //Check for Subfolders
                if (tempFldr.SubFolders.Count > 0)
                {
                    copyDocFolders((IManDocumentFolders)tempFldr.SubFolders, (IManDocumentFolders)newFldr.SubFolders, fldrOwner, reGroups);
                }
            } //foreach fromFolders

        } //copyDocFolders

        private static imProfileAttributeID getAttributeID(IManAdditionalProperty addProp)
        {
            if (addProp != null)
            {
                int attID = Convert.ToInt32(addProp.Name.Substring(addProp.Name.LastIndexOf('_') + 1));
                return (imProfileAttributeID)attID;
            }
            else
            {
                return 0;
            }
        } //getAttributeID

        private static void copyDocs(IManDocuments fromDocs, IManDocuments toDocs, NameValueCollection reGroups)
        {
            for (int i = fromDocs.Count; i >= 1; i--)
            {
                //string tempDocPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\tempDoc";
                //string tempDocPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TempDocLocation;
                string tempDocPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + ConfigUtility.GetTempDocLocation;

                try
                {
                    IManDocument newDoc = null;
                    IManDocument tempDoc = (IManDocument)fromDocs.ItemByIndex(i);

                    //Copy the document using tempDoc
                    tempDoc.GetCopy(tempDocPath, imGetCopyOptions.imNativeFormat);

                    newDoc = tempDoc.Database.CreateDocument(); //Create new document

                    //Set document profile
                    newDoc.SetAttributeByID(imProfileAttributeID.imProfileType, tempDoc.Type.Name);
                    newDoc.SetAttributeByID(imProfileAttributeID.imProfileDescription, tempDoc.Description);
                    newDoc.SetAttributeByID(imProfileAttributeID.imProfileClass, tempDoc.Class.Name);
                    if (tempDoc.SubClass != null)
                        newDoc.SetAttributeByID(imProfileAttributeID.imProfileSubClass, tempDoc.SubClass.Name);
                    newDoc.SetAttributeByID(imProfileAttributeID.imProfileAuthor, tempDoc.Author.Name);
                    newDoc.SetAttributeByID(imProfileAttributeID.imProfileOperator, tempDoc.Operator.Name);
                    for (int j = (int)imProfileAttributeID.imProfileCustom1; j <= (int)imProfileAttributeID.imProfileCustom30; j++)
                    {
                        newDoc.SetAttributeByID((imProfileAttributeID)j, tempDoc.GetAttributeValueByID((imProfileAttributeID)j));
                    }

                    //Profile Set --> Check-in the doc 
                    IManCheckinResult result = newDoc.CheckInWithResults(tempDocPath, imCheckinDisposition.imCheckinNewDocument,
                                                                           imCheckinOptions.imDontKeepCheckedOut);
                    if (!result.Succeeded)  //Check-in failure
                    {
                        throw new Exception("Error while creating document: " + tempDoc.Name + ": " + result.ErrorMessage);
                    }
                    //Document successfully checked-in
                    else
                    {
                        //Set document security
                        copySecurity(tempDoc.Security, newDoc.Security, reGroups);
                    }

                    newDoc.Update();  //update security and other changes

                    //Add the new document to folder contents
                    toDocs.AddDocumentReference(newDoc);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    //cleanup
                    File.Delete(tempDocPath);
                }
            } //foreach fromDocs

        } //copyDocs

        private static void copyWSTabs(IManTabs fromTabs, IManTabs toTabs,
          IManUser tabOwner, NameValueCollection reGroups)
        {
            for (int i = fromTabs.Count; i >= 1; i--)
            {
                IManTab newTab;

                IManTab tempTab = (IManTab)fromTabs.ItemByIndex(i);

                if (tempTab.Security.Inherited)
                {
                    newTab = toTabs.AddNewTabInheriting(tempTab.Name, tempTab.Description);
                }
                else
                {
                    newTab = toTabs.AddNewTab(tempTab.Name, tempTab.Description);
                }

                //Set Location
                newTab.Location.Cell = tempTab.Location.Cell;
                newTab.Location.Order = tempTab.Location.Order;

                //Set View
                newTab.View = tempTab.View;
                newTab.Hidden = tempTab.Hidden;

                //Set Security
                if (tabOwner != null)
                {
                    newTab.Owner = tabOwner;
                }

                copySecurity(tempTab.Security, newTab.Security, reGroups);

                //Check for Document Folders
                if (tempTab.DocumentFolders.Count > 0)
                {
                    copyDocFolders(tempTab.DocumentFolders, newTab.DocumentFolders, tabOwner, reGroups);
                }

                newTab.Update();
            } //foreach fromTabs
        } //copyWSTabs

        private static void copySecurity(IManSecurity fromSecurity, IManSecurity toSecurity, NameValueCollection reGroups)
        {
            //Set Security
            toSecurity.DefaultVisibility = fromSecurity.DefaultVisibility;
            toSecurity.Inherited = fromSecurity.Inherited;

            if (!fromSecurity.Inherited)
            {
                //Set user ACLs
                foreach (IManUserACL tempUserACL in fromSecurity.UserACLs)
                {
                    toSecurity.UserACLs.Add(tempUserACL.User.Name, tempUserACL.Right);
                }

                //Set group ACLs
                foreach (IManGroupACL tempGroupACL in fromSecurity.GroupACLs)
                {
                    if (reGroups != null)
                    {
                        if (reGroups.Get(tempGroupACL.Group.Name) == null)
                        {
                            //If the group is NOT in the rename groups collectin then add to security
                            toSecurity.GroupACLs.Add(tempGroupACL.Group.Name, tempGroupACL.Right);
                        }
                        else
                        {
                            //If the group IS in the rename groups collection then 
                            //Add new groups to workspace
                            toSecurity.GroupACLs.Add(reGroups.Get(tempGroupACL.Group.Name), tempGroupACL.Right);
                        }
                    }
                    else
                    {
                        //If Regroups is null then add existing groups to the new workspace
                        toSecurity.GroupACLs.Add(tempGroupACL.Group.Name, tempGroupACL.Right);
                    }
                } //foreach group ACL
            } //Inherited check
        } //copySecurity

        public static IMANADMIN.INRTGroup GetGrp(string grpName, ImDbType imDbType = ImDbType.Active)
        {
            //IMANADMIN.NRTGroup inrtGrp = (IMANADMIN.NRTGroup)NrtSession.GetInstance.GetDb(imDbType).GetGroup(grpName);
            IMANADMIN.INRTGroup inrtGrp = null;
            try
            {
                //--V469-583-S
                //inrtGrp = NrtSession.GetInstance.GetDb(imDbType).GetGroup(grpName);
                //inrtGrp = NrtSession2.Db(imDbType).GetGroup(grpName);
                inrtGrp = NrtSession2.AdminDb(imDbType).GetGroup(grpName);
                //--V469-583-E
            }
            catch (Exception ex)
            //catch (COMException ex)
            {
                return null;
            }

            return inrtGrp;
        }

        public static IMANADMIN.NRTGroup CreateGrp(string grpName, ImDbType imDbType = ImDbType.Active)
        {
            //--V469-583-S
            //IMANADMIN.NRTGroup inrtGrp = (IMANADMIN.NRTGroup)NrtSession.GetInstance.GetDb(imDbType).CreateGroup(grpName);
            IMANADMIN.NRTGroup inrtGrp = (IMANADMIN.NRTGroup)NrtSession2.AdminDb(imDbType).CreateGroup(grpName);
            //--V469-583-E

            return inrtGrp;
        }

        public static IManFolder GetWsTemplateFldr_0()
        {
            //--V469-583-S
            //return ImSession.GetNewInstance.GetDb(ImDbType.Active).GetFolder(AuditManager.Common.ConfigUtility.WsCreateTemplateFldrId);
            return IManageSession.AdminDb(ImDbType.Active).GetFolder(AuditManager.Common.ConfigUtility.WsCreateTemplateFldrId);
            //--V469-583-E
        }

        public static IManWorkspace GetWsTemplateFldr_1()
        {
            return (IManWorkspace)GetWsTemplateFldr_0();
        }

        public static IManWorkspace Ws_Group(string engNum)
        {
            var wsTemp = GetWsTemplateFldr_1();

            foreach (IManGroupACL tempACL in wsTemp.Security.GroupACLs)
            {
                if (tempACL.Group.Name.Substring(0, 2).ToLower() == "E_".ToLower())
                {
                    var newGrp = engNum + "_" + tempACL.Group.Name;

                    var grpAlreadyExists = GetGrp(newGrp);

                    if (grpAlreadyExists == null)
                    {
                        var newCreatedGrp = CreateGrp(newGrp);

                        var grpSrc = GetGrp(tempACL.Group.Name);

                        if (newCreatedGrp != null)
                        {
                            newCreatedGrp.FullName = grpSrc.FullName;
                            newCreatedGrp.DomainName = grpSrc.DomainName;
                            newCreatedGrp.Enabled = true;
                            newCreatedGrp.NOS = grpSrc.NOS;
                            newCreatedGrp.External = false;
                            newCreatedGrp.Update();

                            foreach (IMANADMIN.NRTUser groupUser in grpSrc.Users)
                            {
                                newCreatedGrp.AddUser(groupUser);
                            }
                            newCreatedGrp.Update();
                        }
                    }

                    //ReGroups.Add(tempACL.Group.Name, ReGroupClientID.Trim() + "_" + tempACL.Group.Name);
                }
            }//Group ACLs for loop

            return wsTemp;
        }

        public static void Validate_GetWsTemplateFldr_1()
        {
            var wsTemp = GetWsTemplateFldr_1();

            if (wsTemp.ConnectorFolders.Count > 0)
            {
                //throw new TemporaryException("Template " + TemplateName + " not suported - contains ConnectorFolders");
            }
            else if (wsTemp.EventFolders.Count > 0)
            {
                //throw new TemporaryException("Template " + TemplateName + " not suported - contains EventFolders");
            }
            else if (wsTemp.LinkListFolders.Count > 0)
            {
                //throw new TemporaryException("Template " + TemplateName + " not suported - contains LinkListFolders");
            }
            else if (wsTemp.MessageFolders.Count > 0)
            {
                //throw new TemporaryException("Template " + TemplateName + " not suported - contains MessageFolders");
            }
            else if (wsTemp.NoteFolders.Count > 0)
            {
                //throw new TemporaryException("Template " + TemplateName + " not suported - contains NoteFolders");
            }
            else if (wsTemp.TaskFolders.Count > 0)
            {
                //throw new TemporaryException("Template " + TemplateName + " not suported - contains TaskFolders");
            }
            else
            {
                //Check Tabs also for non-Document folders
                foreach (IManTab tabTemp in wsTemp.Tabs)
                {
                    if (tabTemp.ConnectorFolders.Count > 0)
                    {
                        //throw new TemporaryException("Template " + TemplateName + " not suported - Tab " + tabTemp.Name + " contains ConnectorFolders");
                    }
                    else if (tabTemp.EventFolders.Count > 0)
                    {
                        //throw new TemporaryException("Template " + TemplateName + " not suported - Tab " + tabTemp.Name + " contains EventFolders");
                    }
                    else if (tabTemp.LinkListFolders.Count > 0)
                    {
                        //throw new TemporaryException("Template " + TemplateName + " not suported - Tab " + tabTemp.Name + " contains LinkListFolders");
                    }
                    else if (tabTemp.MessageFolders.Count > 0)
                    {
                        //throw new TemporaryException("Template " + TemplateName + " not suported - Tab " + tabTemp.Name + " contains MessageFolders");
                    }
                    else if (tabTemp.NoteFolders.Count > 0)
                    {
                        //throw new TemporaryException("Template " + TemplateName + " not suported - Tab " + tabTemp.Name + " contains NoteFolders");
                    }
                    else if (tabTemp.TaskFolders.Count > 0)
                    {
                        //throw new TemporaryException("Template " + TemplateName + " not suported - Tab " + tabTemp.Name + " contains TaskFolders");
                    }
                } //Tabs loop
            } //End Check for non-Document folders
        }

        #region WsLog

        public static dynamic GetEngIdForEngNum(List<string> engNums)
        {
            using (new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                using (var db = new ActiveDbContext())
                {
                    var result = (
                            from dm in db.Set<DOCMASTER>()
                            join prj in db.Set<PROJECT>() on
                            dm.DOCNUM equals prj.DOCNUM
                            where
                            engNums.Contains(dm.C2ALIAS) &&
                            dm.C_ALIAS.Equals("WEBDOC", StringComparison.OrdinalIgnoreCase) &&
                            dm.DOCNAME.Equals("", StringComparison.OrdinalIgnoreCase)

                            select new { EngNum = dm.C2ALIAS, EngId = prj.PRJ_ID }).ToList();

                    return result;
                }
            }
        }

        public static void CreateWsLog_ProfileUpdate(WsFloatingField wsFloatingField, UpdateProfileFrom updateProfileFrom)
        {
            var engIdForEngNum = GetEngIdForEngNum(new List<string> { wsFloatingField.EngNum });

            var wsGLog = new WsGenericLog();
            wsGLog.EngNum = wsFloatingField.EngNum;
            wsGLog.ActionBy = AmUtil.GetCurrentUser;
            wsGLog.Id = null;
            wsGLog.Name = null;
            wsGLog.ActionInfo = null;
            wsGLog.AdditionalInfo = wsFloatingField.GetWsFloatingFieldInfo;
            wsGLog.Comment = null;
            wsGLog.OldValue = null;
            wsGLog.NewValue = null;
            wsGLog.WsLogActivityType = updateProfileFrom == UpdateProfileFrom.CreateWs ? WsLogActivityType.ProfileUpdate_WsCreate
                : (updateProfileFrom == UpdateProfileFrom.Profile ? WsLogActivityType.ProfileUpdate
                : (updateProfileFrom == UpdateProfileFrom.RET ? WsLogActivityType.ProfileUpdate_RET
                : (updateProfileFrom == UpdateProfileFrom.RF ? WsLogActivityType.ProfileUpdate_RF
                : (updateProfileFrom == UpdateProfileFrom.S2 ? WsLogActivityType.ProfileUpdate_S2 : WsLogActivityType.ProfileUpdate
                ))));

            wsGLog.WsLogUseWhat = WsLogUseWhat.WsLog;

            var wsId = WsUtility.GetWsObjectTypeId(WsObjectType.Workspace, engIdForEngNum[0].EngId);

            WsLog_Thru_Delegate(wsId, wsGLog);
        }

        private static WsGenericLog GetLogCopy(WsLog wsLog)
        {
            var log = new WsGenericLog();
            log.ActionBy = wsLog.ActionBy;
            log.ActionInfo = wsLog.ActionInfo;
            log.AdditionalInfo = wsLog.AdditionalInfo;
            log.Comment = wsLog.Comment;
            log.EngNum = wsLog.EngNum;
            log.Id = wsLog.Id;
            log.Name = wsLog.Name;
            log.NewValue = wsLog.NewValue;
            log.OldValue = wsLog.OldValue;
            log.WsLogActivityType = wsLog.WsLogActivityType;

            return log;
        }

        public static void CreateEmailLog(string wsId, string mailBody, WsLog wsLog)
        {
            var log = GetLogCopy(wsLog);

            log.MailBody = mailBody;
            log.WsLogUseWhat = WsLogUseWhat.Email;


            WsLog_Thru_Delegate(wsId, log);

            ////--V469-583-S
            //IManWorkspace imWs = WsUtility.GetObjectByID<IManWorkspace>(wsId, isAdmin: true);
            ////IManWorkspace imWs = WsUtility.GetObjectByID_Admin<IManWorkspace>(wsId);
            ////--V469-583-E

            //IManDocumentFolder imFldr = GetWsFldr(imWs, WsFldrType.WsLog);

            //IManDocument imDoc = imFldr.Database.CreateDocument();

            //WsProfile wsProfile = Workspace.GetWsProfile(imWs.Profile, imWs);

            ////wsLog.EngNum = wsProfile.EngNum;

            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileDescription, wsLog.LogDocName);
            ////imDoc.SetAttributeByID(imProfileAttributeID.imProfileDescription, wsLogActivityType.ToString());
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileType, GetWsDocType(imWs, Convert.ToString(imDoc.GetAttributeValueByID(imProfileAttributeID.imProfileDescription))));

            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileAuthor, imWs.Owner.Name);
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileOperator, imWs.Owner.Name);
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileClass, AmConst.CONST_IMPROFILECLASS);
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom7, AmConst.CONST_IMPROFILECUSTOM7);
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom1, wsProfile.Client);
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom2, wsProfile.EngNum);

            //if (!string.IsNullOrWhiteSpace(wsProfile.EngFunction))
            //{
            //    imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom3, wsProfile.EngFunction);
            //}
            //else
            //{
            //    imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom3, AmConst.CONST_IMPROFILECUSTOM3);
            //}

            //foreach (IManAdditionalProperty imAddProp in imFldr.AdditionalProperties)
            //{
            //    if (imAddProp == null)
            //        continue;

            //    int attID = Convert.ToInt32(imAddProp.Name.Substring(imAddProp.Name.LastIndexOf('_') + 1));
            //    imProfileAttributeID imProfAttr = (imProfileAttributeID)attID;

            //    if (imAddProp.Value != AmConst.CONST_WORKSPACE_VALUE)
            //    {
            //        //Date
            //        if (imProfAttr == imProfileAttributeID.imProfileCustom21 ||
            //                    imProfAttr == imProfileAttributeID.imProfileCustom22 ||
            //                    imProfAttr == imProfileAttributeID.imProfileCustom23 ||
            //                    imProfAttr == imProfileAttributeID.imProfileCustom24)
            //        {
            //            DateTime parsedTime;
            //            if (DateTime.TryParse(imAddProp.Value, out parsedTime))
            //            {
            //                imDoc.SetAttributeByID(imProfAttr, parsedTime);
            //            }
            //            else
            //            {
            //                imDoc.SetAttributeByID(imProfAttr, null);
            //            }
            //        }
            //        //Boolean
            //        else if (imProfAttr == imProfileAttributeID.imProfileCustom25 ||
            //                    imProfAttr == imProfileAttributeID.imProfileCustom26 ||
            //                    imProfAttr == imProfileAttributeID.imProfileCustom27 ||
            //                    imProfAttr == imProfileAttributeID.imProfileCustom28)
            //        {
            //            if (imAddProp.Value != null && imAddProp.Value.ToLower() == "true")
            //            {
            //                imDoc.SetAttributeByID(imProfAttr, true);
            //            }
            //            else
            //            {
            //                imDoc.SetAttributeByID(imProfAttr, false);
            //            }
            //        }
            //        else
            //        {
            //            imDoc.SetAttributeByID(imProfAttr, imAddProp.Value);
            //        }
            //    }
            //    else
            //    {
            //        imDoc.SetAttributeByID(imProfAttr, imFldr.Workspace.Profile.GetAttributeValueByID(imProfAttr));
            //    }
            //}

            //string tempFile = System.IO.Path.GetTempFileName();
            //System.IO.File.WriteAllText(tempFile, mailBody);

            //IManCheckinResult checkInResult = imDoc.CheckInWithResults(tempFile, imCheckinDisposition.imCheckinNewDocument, imCheckinOptions.imDontKeepCheckedOut);

            //if (checkInResult.Succeeded)
            //{
            //    AddSecurityToDoc(imDoc, imFldr);

            //    IManDocuments imDocs = (IManDocuments)imFldr.Contents;
            //    if (null != imDocs)
            //    {
            //        imDocs.AddDocumentReference(checkInResult.Result);
            //    }
            //}
            //else
            //{
            //    //Error
            //}
        }

        public static void CreateWsLog(string wsId, WsLog wsLog)
        {
            var log = GetLogCopy(wsLog);

            wsLog.WsLogUseWhat = WsLogUseWhat.WsLog;


            WsLog_Thru_Delegate(wsId, log);

            ////--V469-583-S
            //IManWorkspace imWs = WsUtility.GetObjectByID<IManWorkspace>(wsId, isAdmin: true);
            ////IManWorkspace imWs = WsUtility.GetObjectByID_Admin<IManWorkspace>(wsId);
            ////--V469-583-E

            //IManDocumentFolder imFldr = GetWsFldr(imWs, WsFldrType.WsLog);

            //IManDocument imDoc = imFldr.Database.CreateDocument();

            //WsProfile wsProfile = Workspace.GetWsProfile(imWs.Profile, imWs);

            //wsLog.EngNum = wsProfile.EngNum;

            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileDescription, wsLog.LogDocName);
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileType, GetWsDocType(imWs, Convert.ToString(imDoc.GetAttributeValueByID(imProfileAttributeID.imProfileDescription))));

            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileAuthor, imWs.Owner.Name);
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileOperator, imWs.Owner.Name);
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileClass, AmConst.CONST_IMPROFILECLASS);
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom7, AmConst.CONST_IMPROFILECUSTOM7);
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom1, wsProfile.Client);
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom2, wsProfile.EngNum);

            //if (!string.IsNullOrWhiteSpace(wsProfile.EngFunction))
            //{
            //    imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom3, wsProfile.EngFunction);
            //}
            //else
            //{
            //    imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom3, AmConst.CONST_IMPROFILECUSTOM3);
            //}

            //foreach (IManAdditionalProperty imAddProp in imFldr.AdditionalProperties)
            //{
            //    if (imAddProp == null)
            //        continue;

            //    int attID = Convert.ToInt32(imAddProp.Name.Substring(imAddProp.Name.LastIndexOf('_') + 1));
            //    imProfileAttributeID imProfAttr = (imProfileAttributeID)attID;

            //    if (imAddProp.Value != AmConst.CONST_WORKSPACE_VALUE)
            //    {
            //        //Date
            //        if (imProfAttr == imProfileAttributeID.imProfileCustom21 ||
            //                    imProfAttr == imProfileAttributeID.imProfileCustom22 ||
            //                    imProfAttr == imProfileAttributeID.imProfileCustom23 ||
            //                    imProfAttr == imProfileAttributeID.imProfileCustom24)
            //        {
            //            DateTime parsedTime;
            //            if (DateTime.TryParse(imAddProp.Value, out parsedTime))
            //            {
            //                imDoc.SetAttributeByID(imProfAttr, parsedTime);
            //            }
            //            else
            //            {
            //                imDoc.SetAttributeByID(imProfAttr, null);
            //            }
            //        }
            //        //Boolean
            //        else if (imProfAttr == imProfileAttributeID.imProfileCustom25 ||
            //                    imProfAttr == imProfileAttributeID.imProfileCustom26 ||
            //                    imProfAttr == imProfileAttributeID.imProfileCustom27 ||
            //                    imProfAttr == imProfileAttributeID.imProfileCustom28)
            //        {
            //            if (imAddProp.Value != null && imAddProp.Value.ToLower() == "true")
            //            {
            //                imDoc.SetAttributeByID(imProfAttr, true);
            //            }
            //            else
            //            {
            //                imDoc.SetAttributeByID(imProfAttr, false);
            //            }
            //        }
            //        else
            //        {
            //            imDoc.SetAttributeByID(imProfAttr, imAddProp.Value);
            //        }
            //    }
            //    else
            //    {
            //        imDoc.SetAttributeByID(imProfAttr, imFldr.Workspace.Profile.GetAttributeValueByID(imProfAttr));
            //    }
            //}

            //string tempFile = System.IO.Path.GetTempFileName();
            //System.IO.File.WriteAllText(tempFile, wsLog.ToString());

            //IManCheckinResult checkInResult = imDoc.CheckInWithResults(tempFile, imCheckinDisposition.imCheckinNewDocument, imCheckinOptions.imDontKeepCheckedOut);

            //if (checkInResult.Succeeded)
            //{
            //    AddSecurityToDoc(imDoc, imFldr);

            //    IManDocuments imDocs = (IManDocuments)imFldr.Contents;
            //    if (null != imDocs)
            //    {
            //        imDocs.AddDocumentReference(checkInResult.Result);
            //    }
            //}
            //else
            //{
            //    //Error
            //}
        }

        public static void TestMe() { }
        public static void TestMeAlways(string wsId, WsLog wsLog) { }

        public static void Create_Ws_Log(string wsId, WsLog wsLog, HttpContext httpContext)
        {
            //--V469-583-S
            IManWorkspace imWs = WsUtility.GetObjectByID<IManWorkspace>(wsId, isAdmin: true);
            //IManWorkspace imWs = WsUtility.GetObjectByID_Admin<IManWorkspace>(wsId);
            //--V469-583-E

            IManDocumentFolder imFldr = GetWsFldr(imWs, WsFldrType.WsLog);

            IManDocument imDoc = imFldr.Database.CreateDocument();

            WsProfile wsProfile = Workspace.GetWsProfile(imWs.Profile, imWs);

            wsLog.EngNum = wsProfile.EngNum;

            imDoc.SetAttributeByID(imProfileAttributeID.imProfileDescription, wsLog.LogDocName);
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileDescription, wsLogActivityType.ToString());
            imDoc.SetAttributeByID(imProfileAttributeID.imProfileType, GetWsDocType(imWs, Convert.ToString(imDoc.GetAttributeValueByID(imProfileAttributeID.imProfileDescription))));

            imDoc.SetAttributeByID(imProfileAttributeID.imProfileAuthor, imWs.Owner.Name);
            imDoc.SetAttributeByID(imProfileAttributeID.imProfileOperator, imWs.Owner.Name);
            imDoc.SetAttributeByID(imProfileAttributeID.imProfileClass, AmConst.CONST_IMPROFILECLASS);
            imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom7, AmConst.CONST_IMPROFILECUSTOM7);
            imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom1, wsProfile.Client);
            imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom2, wsProfile.EngNum);

            if (!string.IsNullOrWhiteSpace(wsProfile.EngFunction))
            {
                imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom3, wsProfile.EngFunction);
            }
            else
            {
                imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom3, AmConst.CONST_IMPROFILECUSTOM3);
            }

            foreach (IManAdditionalProperty imAddProp in imFldr.AdditionalProperties)
            {
                if (imAddProp == null)
                    continue;

                int attID = Convert.ToInt32(imAddProp.Name.Substring(imAddProp.Name.LastIndexOf('_') + 1));
                imProfileAttributeID imProfAttr = (imProfileAttributeID)attID;

                if (imAddProp.Value != AmConst.CONST_WORKSPACE_VALUE)
                {
                    //Date
                    if (imProfAttr == imProfileAttributeID.imProfileCustom21 ||
                                imProfAttr == imProfileAttributeID.imProfileCustom22 ||
                                imProfAttr == imProfileAttributeID.imProfileCustom23 ||
                                imProfAttr == imProfileAttributeID.imProfileCustom24)
                    {
                        DateTime parsedTime;
                        if (DateTime.TryParse(imAddProp.Value, out parsedTime))
                        {
                            imDoc.SetAttributeByID(imProfAttr, parsedTime);
                        }
                        else
                        {
                            imDoc.SetAttributeByID(imProfAttr, null);
                        }
                    }
                    //Boolean
                    else if (imProfAttr == imProfileAttributeID.imProfileCustom25 ||
                                imProfAttr == imProfileAttributeID.imProfileCustom26 ||
                                imProfAttr == imProfileAttributeID.imProfileCustom27 ||
                                imProfAttr == imProfileAttributeID.imProfileCustom28)
                    {
                        if (imAddProp.Value != null && imAddProp.Value.ToLower() == "true")
                        {
                            imDoc.SetAttributeByID(imProfAttr, true);
                        }
                        else
                        {
                            imDoc.SetAttributeByID(imProfAttr, false);
                        }
                    }
                    else
                    {
                        imDoc.SetAttributeByID(imProfAttr, imAddProp.Value);
                    }
                }
                else
                {
                    imDoc.SetAttributeByID(imProfAttr, imFldr.Workspace.Profile.GetAttributeValueByID(imProfAttr));
                }
            }

            string tempFile = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllText(tempFile, wsLog.WsLogUseWhat == WsLogUseWhat.Email ? wsLog.MailBody : wsLog.ToString());

            IManCheckinResult checkInResult = imDoc.CheckInWithResults(tempFile, imCheckinDisposition.imCheckinNewDocument, imCheckinOptions.imDontKeepCheckedOut);

            if (checkInResult.Succeeded)
            {
                AddSecurityToDoc(imDoc, imFldr);

                IManDocuments imDocs = (IManDocuments)imFldr.Contents;
                if (null != imDocs)
                {
                    imDocs.AddDocumentReference(checkInResult.Result);
                }
            }
            else
            {
                //throw new LogOnlyException(string.Format("Error creating IManage log file => {0}", checkInResult.ErrorMessage));
                //AuditManager.Common.DbLog.LogToElmah(string.Format("Error creating IManage log file => {0}", checkInResult.ErrorMessage), httpContext);
            }

            //AuditManager.Common.DbLog.LogToElmah(string.Format("Error creating IManage log file => {0}", "checkInResult.ErrorMessage"), httpContext);
            //throw new LogOnlyException(string.Format("Error creating IManage log file => {0}", "checkInResult.ErrorMessage"));
        }

        public delegate void WsLog_Delegate(string wsId, WsLog wsLog);

        public static void WsLog_Thru_Delegate(string wsId, WsLog wsLog)
        {
            var currentContext = HttpContext.Current;

            Task task = new Task(() => WsUtility.Create_Ws_Log(wsId, wsLog, currentContext));
            task.Start();

            //try
            //{
            //    task.Wait();
            //}
            //catch (Exception ex)
            //{
            //    //AuditManager.Common.DbLog.LogToElmah(string.Format("Error creating IManage log file => {0}", ex.Message));
            //    //throw ex;
            //}

            //WsLog_Delegate del = new WsLog_Delegate(Create_Ws_Log);
            //del.BeginInvoke(wsId, wsLog, null, null);
        }

        #endregion

        #region AuditManager Table
        //private static string BuildTp(WsProfile_TP wsProfile_TP)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.Append("TP_Q1=");
        //    sb.Append(wsProfile_TP.TP_Q1);
        //    sb.Append("|");
        //    sb.Append("TP_Q2=");
        //    sb.Append(wsProfile_TP.TP_Q2);
        //    sb.Append("|");
        //    sb.Append("TP_Q3=");
        //    sb.Append(wsProfile_TP.TP_Q3);
        //    sb.Append("|");
        //    sb.Append("TP_Q3_Comment=");
        //    sb.Append(wsProfile_TP.TP_Q3_Comment);

        //    return sb.ToString();
        //}

        //public static void UpdateWsProfile(string engNum, WsProfile_TP wsProfile_TP, UpdateProfileFrom updateProfileFrom)
        public static void UpdateWsProfile(string engNum, UpdateProfileFrom updateProfileFrom)
        {
            using (var db = new AmDbContext())
            {
                List<WsFloatingField> wsFloatingField = db.WsFloatingField.Where(x => x.EngNum.Equals(engNum) && x.IsActive).ToList();

                foreach (WsFloatingField item in wsFloatingField)
                {
                    item.IsActive = false;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                if (wsFloatingField != null && wsFloatingField.Count > 0)
                {
                    var wsFloatingFieldNew = db.WsFloatingField.Add(
                        new WsFloatingField
                        {
                            EngNum = engNum,
                            Comment = wsFloatingField.FirstOrDefault().Comment,
                            EnteredBy = AmUtil.GetCurrentUser,
                            EnteredDate = DateTime.Now,
                            EventTrgDate = wsFloatingField.FirstOrDefault().EventTrgDate,
                            IsActive = true,
                            IsKDrive = wsFloatingField.FirstOrDefault().IsKDrive,
                            IsServer2 = wsFloatingField.FirstOrDefault().IsServer2,
                            IsUnderPreservation = wsFloatingField.FirstOrDefault().IsUnderPreservation,
                            //TPAns = BuildTp(wsProfile_TP),
                            TPAns = string.Empty,
                        }
                        );

                    CreateWsLog_ProfileUpdate(wsFloatingFieldNew, updateProfileFrom);

                    db.SaveChanges();
                }
                else
                {
                    var wsFloatingFieldNew = db.WsFloatingField.Add(
                        new WsFloatingField
                        {
                            EngNum = engNum,
                            Comment = null,
                            EnteredBy = AmUtil.GetCurrentUser,
                            EnteredDate = DateTime.Now,
                            EventTrgDate = null,
                            IsActive = true,
                            IsKDrive = null,
                            IsServer2 = null,
                            IsUnderPreservation = null,
                            //TPAns = BuildTp(wsProfile_TP),
                            TPAns = string.Empty,
                        }
                        );

                    CreateWsLog_ProfileUpdate(wsFloatingFieldNew, updateProfileFrom);

                    db.SaveChanges();
                }
            }
        }

        //When Profile is Updated
        public static void UpdateWsProfile(WsUpdateModel wsUpdateModel, UpdateProfileFrom updateProfileFrom)
        {
            var wsProfile = wsUpdateModel.WsModel.WsProfile;
            using (var db = new AmDbContext())
            {
                List<WsFloatingField> wsFloatingField = db.WsFloatingField.Where(x => x.EngNum.Equals(wsProfile.EngNum) && x.IsActive).ToList();

                foreach (WsFloatingField item in wsFloatingField)
                {
                    item.IsActive = false;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                var wsFloatingFieldNew = db.WsFloatingField.Add(
                    new WsFloatingField
                    {
                        EngNum = wsProfile.EngNum,
                        Comment = wsUpdateModel.PreservationComment,
                        EnteredBy = AmUtil.GetCurrentUser,
                        EnteredDate = DateTime.Now,
                        EventTrgDate = wsProfile.EventTrgDate,
                        IsActive = true,
                        IsKDrive = wsProfile.IsKDrive,
                        IsServer2 = wsProfile.IsServer2,
                        IsUnderPreservation = wsProfile.IsUnderPreservation,
                        //TPAns = BuildTp(wsProfile.WsProfile_TP),
                        TPAns = string.Empty,
                    }
                    );

                db.SaveChanges();

                CreateWsLog_ProfileUpdate(wsFloatingFieldNew, updateProfileFrom);

                //ProfileWsLog
            }
        }

        public static void SaveClosureInfo(string engNum,
            string fileArray, string recepientArray,
            string mailText = null,
            string comment = null,
            bool hasLargeRetFile = false)
        {
            var dtNow = DateTime.Now;

            using (var db = new AmDbContext())
            {
                var wsActivityHistory = db.WsActivityHistory.Add(
                    new WsActivityHistory
                    {
                        EngNum = engNum,
                        UserId = AmUtil.GetCurrentUser,
                        WsActivityTypeId = WsActivityType.CLOSURE,
                        WsSubActivityTypeId = WsActivityType.CLOSURE_INITIATE,
                        Comment = comment,
                        WsActivityInfo = (new { fileArray = fileArray, hasLargeRetFile = hasLargeRetFile }).ToJString(),
                        DateCreated = dtNow
                    }
                    );

                db.SaveChanges();

                var wsMail = db.WsMail.Add(new WsMail
                {
                    WsActivityHistoryId = wsActivityHistory.WsActivityHistoryId,
                    WsMailStatusTypeId = WsMailStatusType.Success,
                    MailText = mailText,
                    RecepientArray = recepientArray
                });

                var clsr = db.Closure.Add(new Closure
                {
                    EngNum = engNum,
                    ClosureDate = dtNow,
                    IsClosed = false,
                    LastMailSentDate = dtNow
                });

                db.SaveChanges();
            }

            using (var db = new S2DbContext())
            {
                var clsr = db.Closure.Add(new AuditManager.Model.EFModel.S2.Closure
                {
                    EngNum = engNum,
                    ClosureDate = dtNow,
                    IsClosed = false,
                    LastMailSentDate = dtNow
                });

                db.SaveChanges();
            }
        }

        public static void SaveWsCreationInfo(string engNum,
            string recepientArray,
            string mailText = null
            )
        {
            var dtNow = DateTime.Now;

            using (var db = new AmDbContext())
            {
                var wsActivityHistory = db.WsActivityHistory.Add(
                    new WsActivityHistory
                    {
                        EngNum = engNum,
                        UserId = AmUtil.GetCurrentUser,
                        WsActivityTypeId = WsActivityType.CREATE,
                        WsSubActivityTypeId = WsActivityType.WS_CREATE,
                        Comment = null,
                        WsActivityInfo = null,
                        DateCreated = dtNow
                    }
                    );

                db.SaveChanges();

                var wsMail = db.WsMail.Add(new WsMail
                {
                    WsActivityHistoryId = wsActivityHistory.WsActivityHistoryId,
                    WsMailStatusTypeId = WsMailStatusType.Success,
                    MailText = mailText,
                    RecepientArray = recepientArray
                });

                db.SaveChanges();
            }

        }

        public static void SaveGuidInfo_Link(Guid mafGuid, string engNum)
        {
            var dtNow = DateTime.Now;

            using (var db = new AmDbContext())
            {
                var wsActivityHistory = db.WsActivityHistory.Add(
                    new WsActivityHistory
                    {
                        EngNum = engNum,
                        UserId = AmUtil.GetCurrentUser,
                        WsActivityTypeId = WsActivityType.GUID,
                        WsSubActivityTypeId = WsActivityType.GUID_LINK,
                        Comment = null,
                        WsActivityInfo = mafGuid.ToString(),
                        DateCreated = dtNow
                    }
                    );

                db.SaveChanges();
            }
        }

        public static void SaveGuidInfo_DeLink(Guid mafGuid, string engNum, string comment, int masterAuditFileId)
        {
            var dtNow = DateTime.Now;

            using (var db = new AmDbContext())
            {
                var wsActivityHistory = db.WsActivityHistory.Add(
                    new WsActivityHistory
                    {
                        EngNum = engNum,
                        UserId = AmUtil.GetCurrentUser,
                        WsActivityTypeId = WsActivityType.GUID,
                        WsSubActivityTypeId = WsActivityType.GUID_DELINK,
                        Comment = comment,
                        WsActivityInfo = string.Format("MafGuid = {0}, MasterAuditFileId = {1}", mafGuid.ToString(), masterAuditFileId),
                        DateCreated = dtNow
                    }
                    );

                db.SaveChanges();
            }
        }

        public static void SaveActivityInfo(FileActivity_UpdateModel activityUpdateModel, string returnStatus)
        {
            var dtNow = DateTime.Now;

            using (var db = new AmDbContext())
            {
                var wsActivityHistory = db.WsActivityHistory.Add(
                    new WsActivityHistory
                    {
                        EngNum = activityUpdateModel.EngNum,
                        UserId = AmUtil.GetCurrentUser,
                        WsActivityTypeId = WsActivityType.Activity,
                        WsSubActivityTypeId = activityUpdateModel.WsActivityType,
                        Comment = activityUpdateModel.Comment,
                        WsActivityInfo = string.Format("FileIn = {0}, WorkbookReviewId = {1}, FAId = {2}, NonAuditFlag = {3}, DocNum = {4}, ReturnStatus = {5}",
                        activityUpdateModel.FileIn.ToString(),
                        activityUpdateModel.FileIn == FileIn.S2 ? activityUpdateModel.FileUniqueId.ToString() : null,
                        activityUpdateModel.FileIn == FileIn.SSC ? activityUpdateModel.FileUniqueId.ToString() : null,
                        activityUpdateModel.NonAuditFlag,
                        activityUpdateModel.FileNum,
                        returnStatus
                        ),
                        DateCreated = dtNow
                    }
                    );

                db.SaveChanges();
            }
        }
        #endregion
    }
}
