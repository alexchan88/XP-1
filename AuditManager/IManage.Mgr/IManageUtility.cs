using AuditManager.Common;
using AuditManager.Model;
using IManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IM.Mgr
{
    public class IManageUtility
    {
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

        private static IManFolder GetIManFolders(IManFolder imFldrs, string folderPath)
        {
            foreach (IManFolder imFldr in imFldrs.SubFolders)
            {
                if (imFldr.Name.Equals(folderPath, StringComparison.OrdinalIgnoreCase))
                {
                    return imFldr;
                }
            }

            return null;
        }

        private static IManDocumentFolder GetWsFldr(IManWorkspace imWs, string folderPath)
        {
            var spPath = folderPath.Split('/').ToList();

            var imFldr = (IManFolder)imWs;

            spPath.ForEach(x => {

                imFldr = GetIManFolders(imFldr, x);

            });

            return (IManDocumentFolder)imFldr;
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

        public static void UploadDocument(string engNum, string fileNameWithLocalPath, string iMFolderPath)
        {
            //
            var projectIdForEngNum = WsUtility.GetEngIdForEngNum(new List<string> { engNum });

            if (projectIdForEngNum.Count == 0)
                return;

            var wsId = WsUtility.GetWsObjectTypeId(WsObjectType.Workspace, projectIdForEngNum[0].EngId);
            //

            IManWorkspace imWs = WsUtility.GetObjectByID<IManWorkspace>(wsId, isAdmin: true);

            IManDocumentFolder imFldr = GetWsFldr(imWs, iMFolderPath);

            IManDocument imDoc = imFldr.Database.CreateDocument();

            WsProfile wsProfile = Workspace.GetWsProfile(imWs.Profile, imWs);

            imDoc.SetAttributeByID(imProfileAttributeID.imProfileDescription, Path.GetFileName(fileNameWithLocalPath));

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

            //string tempFile = System.IO.Path.GetTempFileName();
            //System.IO.File.WriteAllText(tempFile, wsLog.WsLogUseWhat == WsLogUseWhat.Email ? wsLog.MailBody : wsLog.ToString());

            IManCheckinResult checkInResult = imDoc.CheckInWithResults(fileNameWithLocalPath, imCheckinDisposition.imCheckinNewDocument, imCheckinOptions.imDontKeepCheckedOut);

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

        public static void UploadDocument2(string wsId, string fileNameWithLocalPath, string iMFolderPath)
        {
            IManWorkspace imWs = WsUtility.GetObjectByID<IManWorkspace>(wsId, isAdmin: true);

            IManDocumentFolder imFldr = GetWsFldr(imWs, iMFolderPath);

            IManDocument imDoc = imFldr.Database.CreateDocument();

            WsProfile wsProfile = Workspace.GetWsProfile(imWs.Profile, imWs);

            imDoc.SetAttributeByID(imProfileAttributeID.imProfileDescription, Path.GetFileName(fileNameWithLocalPath));

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

            //string tempFile = System.IO.Path.GetTempFileName();
            //System.IO.File.WriteAllText(tempFile, wsLog.WsLogUseWhat == WsLogUseWhat.Email ? wsLog.MailBody : wsLog.ToString());

            IManCheckinResult checkInResult = imDoc.CheckInWithResults(fileNameWithLocalPath, imCheckinDisposition.imCheckinNewDocument, imCheckinOptions.imDontKeepCheckedOut);

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
    }
}
