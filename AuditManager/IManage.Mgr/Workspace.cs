using AuditManager.Model;
using IManage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace IM.Mgr
{
    public class Workspace
    {
        private static WsUserType GetWsUserType(string name)
        {
            var spName = name.Split('_');

            if (spName.Length < 3)
                return WsUserType.NONE;

            switch (spName[2].ToUpper())
            {
                case "ADMIN":
                    return WsUserType.ADMIN;
                case "MEMBERS":
                    return WsUserType.MEMBERS;
                case "READ":
                    return WsUserType.READ;
                default:
                    return WsUserType.NONE;
            }
        }

        private static WsAccessRight GetWsAccessRight(imAccessRight imAccessRight)
        {
            switch (imAccessRight)
            {
                case imAccessRight.imRightAll:
                    return WsAccessRight.ALL;
                case imAccessRight.imRightNone:
                    return WsAccessRight.NONE;
                case imAccessRight.imRightRead:
                    return WsAccessRight.READ;
                case imAccessRight.imRightReadWrite:
                    return WsAccessRight.READWRITE;
                default:
                    return WsAccessRight.NONE;
            }
        }

        private static List<WsGroup> GetSecurityGrps(IManGroupACLs imGrps)
        {
            List<WsGroup> wsGroups = new List<WsGroup>();

            foreach (IManGroupACL imGrp in imGrps)
            {
                wsGroups.Add(
                    new WsGroup
                    {
                        FullName = imGrp.Group.FullName,
                        GroupNumber = imGrp.Group.GroupNumber,
                        GrpUsers = WsUtility.GetAllGrpUser(imGrp.Group.Users),
                        Name = imGrp.Group.Name,
                        //ObjectID = imGrp.Group.ObjectID,
                        Enabled = imGrp.Group.Enabled,
                        WsAccessRight = GetWsAccessRight((imAccessRight)imGrp.Right),
                        DomainName = imGrp.Group.DomainName,
                        WsUserType = GetWsUserType(imGrp.Group.Name)
                    }
                );
            }

            return wsGroups;
        }

        private static List<WsGroup> GetSecurity(IManSecurity imSec)
        {
            return GetSecurityGrps(imSec.GroupACLs);
        }

        private static List<WsFile> GetWsFiles(IManContents imCons, string fldrPath, bool isAdmin = false)
        {
            List<WsFile> wsFiles = new List<WsFile>();

            foreach (IManContent imCon in imCons)
            {
                IManDocument imDoc = WsUtility.GetObjectByID<IManDocument>(imCon.ObjectID, isAdmin: isAdmin);
                var wsFile = GetWsFile(imDoc, fldrPath);

                if (!wsFile.IsDeleted)
                    wsFiles.Add(wsFile);
            }

            return wsFiles;
        }

        private static List<WsFldr> GetWsFldrs(IManFolders imFldrs, string fldrPath, bool isAdmin = false)
        {
            List<WsFldr> wsFldrs = new List<WsFldr>();

            foreach (IManFolder imFldr in imFldrs)
            {
                wsFldrs.Add(GetWsFldr(imFldr, fldrPath, isAdmin));
            }

            return wsFldrs.OrderBy(x => x.Name).ToList();
        }

        private static T PopulateCustomAttr<T>(T type, dynamic imObj)
        {
            foreach (PropertyInfo pI in type.GetType().GetProperties())
            {
                if (Attribute.IsDefined(pI, typeof(ImProfileAttrInfo), false))
                {
                    var cA = (ImProfileAttrInfo)pI.GetCustomAttributes(typeof(ImProfileAttrInfo), false)[0];

                    dynamic imProfAttrVal = null;

                    if ((imObj as IManProfile) != null)
                    {
                        IManProfile imp = (IManProfile)imObj;
                        imProfAttrVal = imp.GetAttributeValueByID((imProfileAttributeID)Enum.Parse(typeof(imProfileAttributeID), cA.AttrImProfileEnum));
                    }
                    else if ((imObj as IManWorkspace) != null)
                    {
                        IManWorkspace imws = (IManWorkspace)imObj;
                        imProfAttrVal = imws.GetAttributeValueByID((imProfileAttributeID)Enum.Parse(typeof(imProfileAttributeID), cA.AttrImProfileEnum));
                    }
                    else if ((imObj as IManDocument) != null)
                    {
                        IManDocument imd = (IManDocument)imObj;
                        imProfAttrVal = imd.GetAttributeValueByID((imProfileAttributeID)Enum.Parse(typeof(imProfileAttributeID), cA.AttrImProfileEnum));
                    }
                    else
                    {
                        IManProfile imp = (IManProfile)imObj;
                        imProfAttrVal = imp.GetAttributeValueByID((imProfileAttributeID)Enum.Parse(typeof(imProfileAttributeID), cA.AttrImProfileEnum));
                    }

                    if (imProfAttrVal == null)
                        continue;

                    if (cA.AttrDataType == typeof(bool))
                    {
                        bool b;
                        if (bool.TryParse(imProfAttrVal.ToString(), out b))
                            pI.SetValue(type, b);
                        else
                            pI.SetValue(type, false);
                    }
                    else if (cA.AttrDataType == typeof(DateTime))
                    {
                        if (imProfAttrVal == null || imProfAttrVal.Date == null)
                            pI.SetValue(type, null);
                        else
                        {
                            DateTime dt;
                            if (DateTime.TryParse(imProfAttrVal.Date.ToString(), out dt))
                                pI.SetValue(type, dt);
                            else
                                pI.SetValue(type, null);
                        }
                    }
                    else
                        pI.SetValue(type, imProfAttrVal.ToString());
                }
            }

            return type;
        }

        //public static WsProfile_TP GetWsProfile_TP(string engNum)
        //{
        //    using (var db = new AmDbContext())
        //    {
        //        using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
        //        {
        //            var result = db.WsFloatingField.Where(x => x.EngNum.Equals(engNum) && x.IsActive).FirstOrDefault();
        //            WsProfile_TP retResult = new WsProfile_TP();

        //            if (result == null)
        //                return retResult;


        //            var tpAns = result.TPAns.Split('|');

        //            retResult.TP_Q1 = tpAns[0].Split('=')[1];
        //            retResult.TP_Q2 = tpAns[1].Split('=')[1];
        //            retResult.TP_Q3 = tpAns[2].Split('=')[1];
        //            retResult.TP_Q3_Comment = tpAns[3].Split('=')[1];

        //            retResult.KPMGOnly = true;

        //            return retResult;
        //        }
        //    }
        //}

        public static bool GetKPMGOnlyFlag(string engNum)
        {
            return true;
        }

        internal static WsProfile GetWsProfile(IManProfile imProf, IManWorkspace imWs)
        {
            var wsProf = new WsProfile();

            wsProf = PopulateCustomAttr(wsProf, imProf);

            wsProf.IsUnderPreservation = wsProf.Status.IndexOf("UNDER PRESERVATION", StringComparison.OrdinalIgnoreCase) != -1; //wsProf.Status.Equals("UNDER PRESERVATION", StringComparison.OrdinalIgnoreCase);

            var clientCustAttr = WsUtility.GetWsCustAttr(wsProf.GetType().GetProperty("Client"), wsProf.Client);

            wsProf.ClientDesc = clientCustAttr == null ? string.Empty : clientCustAttr.Description;

            if (!string.IsNullOrWhiteSpace(wsProf.Partner))
                wsProf.PartnerDesc = WsUtility.GetWsUser(wsProf.Partner, ImDbType.Active);

            if (!string.IsNullOrWhiteSpace(wsProf.Manager))
                wsProf.ManagerDesc = WsUtility.GetWsUser(wsProf.Manager, ImDbType.Active);

            //wsProf.WsProfile_TP = GetWsProfile_TP(wsProf.EngNum);

            //wsProf.KPMGOnly = GetKPMGOnlyFlag(wsProf.EngNum);

            return wsProf;
        }

        internal static WsFldr GetWsFldr(IManFolder imFldr, string fldrPath, bool isAdmin = false)
        {
            var wsFldr = new WsFldr
            {
                Description = imFldr.Description,
                FolderID = imFldr.FolderID,
                Name = imFldr.Name,
                FolderPath = string.Format("{0}/{1}", fldrPath, imFldr.Name),
                ObjectID = imFldr.ObjectID,
                WsFldrs = GetWsFldrs(imFldr.SubFolders, string.Format("{0}/{1}", fldrPath, imFldr.Name), isAdmin),
                WsFiles = GetWsFiles(imFldr.Contents, string.Format("{0}/{1}", fldrPath, imFldr.Name), isAdmin)
            };

            return wsFldr;
        }

        public static WsFile GetWsFile(IManDocument imDoc, string fldrPath = null)
        {
            var wsFile = new WsFile
            {
                Author = WsUtility.GetWsUser(imDoc.Author),
                IsCheckedOut = imDoc.CheckedOut,
                Description = imDoc.Description,
                Extn = imDoc.Type.ApplicationExtension,

                IsLocked = imDoc.Locked,
                Name = imDoc.Name,
                FilePath = fldrPath,
                Number = imDoc.Number,
                ObjectID = imDoc.ObjectID,
                Operator = WsUtility.GetWsUser(imDoc.Operator),
                Size = imDoc.Size,
                Version = imDoc.Version,
                VersionCount = imDoc.Versions.Count,
                IsLatestVersion = imDoc.IsLatestVersion,

                CreationDate = imDoc.CreationDate.ToString("G", CultureInfo.CreateSpecificCulture("en-us")),
            };

            wsFile = PopulateCustomAttr(wsFile, imDoc);

            if (wsFile.IsRecord)
            {
                foreach (IManDocumentHistory docHistory in imDoc.HistoryList)
                {
                    if (docHistory.Operation.Equals("Declared", StringComparison.OrdinalIgnoreCase))
                    {
                        wsFile.RecordDate = docHistory.Date;
                        wsFile.RecordUser = docHistory.User;
                    }
                }
            }

            wsFile.IsUnderPreservation = wsFile.Status.Equals("UNDER PRESERVATION", StringComparison.OrdinalIgnoreCase);

            return wsFile;
        }

        internal static WsModel GetWs(string wsId, out IManWorkspace imWsOut, WsLoadType wsLoadType = WsLoadType.ALL, bool newSession = false, bool isAdmin = false)
        {
            IManWorkspace imWs = null;
            //imWs = WsUtility.GetObjectByID<IManWorkspace>(wsId, newSession);
            imWs = WsUtility.GetObjectByID_New<IManWorkspace>(wsId, true, isAdmin);

            imWsOut = imWs;

            if (imWs == null)
                return null;

            //var wsModel = (new WsModel
            //{
            //    IsLoaded = wsLoadType != WsLoadType.None,
            //    Description = imWs.Description,
            //    Name = imWs.Name,
            //    ObjectID = imWs.ObjectID,
            //    Owner = WsUtility.GetWsUser(imWs.Owner),
            //    SubType = imWs.SubType,
            //    WorkspaceID = imWs.WorkspaceID,

            //    WsProfile = WsUtility.ToLoad(wsLoadType, WsComponentType.Profile) ? GetWsProfile(imWs.Profile, imWs) : null,
            //    WsFldrs = WsUtility.ToLoad(wsLoadType, WsComponentType.Fldrs) ? GetWsFldrs(imWs.SubFolders, string.Empty, isAdmin) : null,
            //    WsGroups = WsUtility.ToLoad(wsLoadType, WsComponentType.Groups) ? GetSecurity(imWs.Security) : null
            //});

            var wsModel = (new WsModel
            {
                IsLoaded = wsLoadType != WsLoadType.None,
                Description = imWs.Description,
                Name = imWs.Name,
                ObjectID = imWs.ObjectID,
                Owner = WsUtility.GetWsUser(imWs.Owner),
                SubType = imWs.SubType,
                WorkspaceID = imWs.WorkspaceID,

                //WsProfile = WsUtility.ToLoad(wsLoadType, WsComponentType.Profile) ? GetWsProfile(imWs.Profile, imWs) : null,
                //WsFldrs = WsUtility.ToLoad(wsLoadType, WsComponentType.Fldrs) ? GetWsFldrs(imWs.SubFolders, string.Empty, isAdmin) : null,
                //WsGroups = WsUtility.ToLoad(wsLoadType, WsComponentType.Groups) ? GetSecurity(imWs.Security) : null
            });

            wsModel.WsProfile = WsUtility.ToLoad(wsLoadType, WsComponentType.Profile) ? GetWsProfile(imWs.Profile, imWs) : null;
            wsModel.WsFldrs = WsUtility.ToLoad(wsLoadType, WsComponentType.Fldrs) ? GetWsFldrs(imWs.SubFolders, string.Empty, isAdmin) : null;
            wsModel.WsGroups = WsUtility.ToLoad(wsLoadType, WsComponentType.Groups) ? GetSecurity(imWs.Security) : null;

            return wsModel;
        }

        //--V469-583-S
        internal static WsModel GetWs_Admin(string wsId, out IManWorkspace imWsOut, WsLoadType wsLoadType = WsLoadType.ALL, bool newSession = false)
        {
            IManWorkspace imWs = null;
            //imWs = WsUtility.GetObjectByID<IManWorkspace>(wsId, newSession);
            imWs = WsUtility.GetObjectByID_Admin<IManWorkspace>(wsId, newSession);

            imWsOut = imWs;

            if (imWs == null)
                return null;

            var wsModel = (new WsModel
            {
                IsLoaded = wsLoadType != WsLoadType.None,
                Description = imWs.Description,
                Name = imWs.Name,
                ObjectID = imWs.ObjectID,
                Owner = WsUtility.GetWsUser(imWs.Owner),
                SubType = imWs.SubType,
                WorkspaceID = imWs.WorkspaceID,

                WsProfile = WsUtility.ToLoad(wsLoadType, WsComponentType.Profile) ? GetWsProfile(imWs.Profile, imWs) : null,
                WsFldrs = WsUtility.ToLoad(wsLoadType, WsComponentType.Fldrs) ? GetWsFldrs(imWs.SubFolders, string.Empty) : null,
                WsGroups = WsUtility.ToLoad(wsLoadType, WsComponentType.Groups) ? GetSecurity(imWs.Security) : null
            });

            return wsModel;
        }
        //--V469-583-E

        public static List<WsModel> GetWs(string wsId = null, WsLoadType wsLoadType = WsLoadType.ALL, bool newSession = false, bool isAdmin = false)
        {
            
            IManWorkspaces imWss = null;

            List<WsModel> iLWsModel = new List<WsModel>();

            if (string.IsNullOrWhiteSpace(wsId))
            {
                IManDatabase imDb = null;

                //--V469-583-S
                //imDb = newSession ? ImSession.GetNewInstance.GetDb(ImDbType.Active) : ImSession.GetInstance.GetDb(ImDbType.Active);
                imDb = IManageSession.Db(ImDbType.Active);
                //--V469-583-E

                if (imDb == null)
                    return null;

                imWss = imDb.Workspaces;

                foreach (IManWorkspace imWs in imWss)
                {
                    IManDocumentClass imDC = imWs.SubClass;

                    if (imDC != null)
                    {
                        if (imDC.Name == "EAUDIT ENGAGEMENT" || imDC.Name == "AUDIT")
                        {
                            iLWsModel.Add(new WsModel
                            {
                                IsLoaded = false,
                                Description = imWs.Description,
                                Name = imWs.Name,
                                ObjectID = imWs.ObjectID,
                                Owner = WsUtility.GetWsUser(imWs.Owner),
                                SubType = imWs.SubType,
                                WorkspaceID = imWs.WorkspaceID,

                                WsProfile = null,
                                WsFldrs = null,
                                WsGroups = null
                            });
                        }
                    }
                }
            }
            else if (wsLoadType == WsLoadType.None)
            {
                IManWorkspace imWs = null;

                iLWsModel.Add(GetWs(wsId, out imWs, WsLoadType.None, newSession, isAdmin: isAdmin));
            }
            else
            {
                IManWorkspace imWs = null;

                iLWsModel.Add(GetWs(wsId, out imWs, WsLoadType.ALL, newSession, isAdmin: isAdmin));
            }

            return iLWsModel;
        }
    }
}
