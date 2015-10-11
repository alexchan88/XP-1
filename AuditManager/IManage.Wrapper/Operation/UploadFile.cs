using IM.Wrapper.EF;
using IM.Wrapper.Model;
using IManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;

namespace IM.Wrapper.Operation
{
    internal class UploadFile
    {
        private static dynamic GetWsId(IMInstance iMInstance, List<string> engNums)
        {
            using (new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                using (var db = new IMDbContext(iMInstance))
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

        private static IManDocumentType GetIMDocType(IManWorkspace imWs, string fileName)
        {
            IManDocumentTypes imDocTypes = imWs.Database.SearchDocumentTypes(string.Empty, imSearchAttributeType.imSearchBoth, true);

            var extn = Path.GetExtension(fileName).Substring(1);

            foreach (IManDocumentType imDocType in imDocTypes)
            {
                if (imDocType.ApplicationExtension.Equals(extn, StringComparison.OrdinalIgnoreCase))
                {
                    return imDocType;
                }
            }

            return null;
        }

        private static IManFolder GetIMFolder(IManFolder imFldr, string folderPath)
        {
            if (imFldr == null)
                return null;

            foreach (IManFolder itemFldr in imFldr.SubFolders)
            {
                if (itemFldr.Name.Equals(folderPath, StringComparison.OrdinalIgnoreCase))
                {
                    return itemFldr;
                }
            }

            return null;
        }

        private static IManDocumentFolder GetIMDocFolder(IManWorkspace imWs, string folderPath)
        {
            string[] spPath = null;
            if ("1 - Final Client Reports/Deliverables".Equals(folderPath, StringComparison.OrdinalIgnoreCase))
            {
                spPath = new string[]{folderPath};
            }
            else
            {
                spPath = folderPath.Split('/');
            }
            

            var imFldr = (IManFolder)imWs;

            spPath.ToList().ForEach(x =>
            {

                imFldr = GetIMFolder(imFldr, x);

            });

            return imFldr == null ? null : (IManDocumentFolder)imFldr;
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

        public static int? Upload(IMInstance iMInstance, string engNum, string fileNameWithLocalPath, string iMFolderPath)
        {
            if (!File.Exists(fileNameWithLocalPath))
                return null;
            
            var projectIdForEngNum = GetWsId(iMInstance, new List<string> { engNum });

            if (projectIdForEngNum.Count == 0)
                return null;

            var iMSession = new IMSession(iMInstance);

            var wsId = IM.Wrapper.Utility.IMUtility.GetWsObjectID(iMInstance, iMSession, IMWSObjectType.Workspace, (double)projectIdForEngNum[0].EngId);

            IManWorkspace imWs = iMInstance.UseAdmin.GetValueOrDefault(false) ?  
                (IManWorkspace)iMSession.AdminSession().DMS.GetObjectByID(wsId) :
                (IManWorkspace)iMSession.UserSession().DMS.GetObjectByID(wsId);

            if (iMFolderPath.StartsWith("/") || iMFolderPath.StartsWith("\\"))
            {
                iMFolderPath = iMFolderPath.Substring(1);
            }

            IManDocumentFolder imFldr = GetIMDocFolder(imWs, iMFolderPath);

            if (imFldr == null)
                return null;

            IManDocument imDoc = imFldr.Database.CreateDocument();

            imDoc.SetAttributeByID(imProfileAttributeID.imProfileDescription, Path.GetFileName(fileNameWithLocalPath));

            imDoc.SetAttributeByID(imProfileAttributeID.imProfileType, GetIMDocType(imWs, Convert.ToString(imDoc.GetAttributeValueByID(imProfileAttributeID.imProfileDescription))));

            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileAuthor, imWs.Owner.Name);
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileOperator, imWs.Owner.Name);

            imDoc.SetAttributeByID(imProfileAttributeID.imProfileAuthor, iMInstance.UserId);
            imDoc.SetAttributeByID(imProfileAttributeID.imProfileOperator, iMInstance.UserId);
            //imDoc.SetAttributeByID(imProfileAttributeID.imProfileLastUser, iMInstance.UserId);

            imDoc.SetAttributeByID(imProfileAttributeID.imProfileClass, IMConst.CONST_IMPROFILECLASS);
            imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom7, IMConst.CONST_IMPROFILECUSTOM7);
            imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom1,
                imWs.Profile.GetAttributeValueByID(imProfileAttributeID.imProfileCustom1).ToString());
            imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom2,
                imWs.Profile.GetAttributeValueByID(imProfileAttributeID.imProfileCustom2).ToString());
            var engFunction = imWs.Profile.GetAttributeValueByID(imProfileAttributeID.imProfileCustom3).ToString();

            if (!string.IsNullOrWhiteSpace(engFunction))
            {
                imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom3, engFunction);
            }
            else
            {
                imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom3, IMConst.CONST_IMPROFILECUSTOM3);
            }

            foreach (IManAdditionalProperty imAddProp in imFldr.AdditionalProperties)
            {
                if (imAddProp == null)
                    continue;

                int attID = Convert.ToInt32(imAddProp.Name.Substring(imAddProp.Name.LastIndexOf('_') + 1));
                imProfileAttributeID imProfAttr = (imProfileAttributeID)attID;

                if (imProfAttr != imProfileAttributeID.imProfileAuthor && imProfAttr != imProfileAttributeID.imProfileAuthorDescription &&
                        imProfAttr != imProfileAttributeID.imProfileOperator && imProfAttr != imProfileAttributeID.imProfileOperatorDescription &&
                        imProfAttr != imProfileAttributeID.imProfileLastUser && imProfAttr != imProfileAttributeID.imProfileLastUserDescription)
                {
                    
                }
                else
                {
                    continue;
                }

                if (imAddProp.Value != IMConst.CONST_WORKSPACE_VALUE)
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

            IManCheckinResult checkInResult = imDoc.CheckInWithResults(fileNameWithLocalPath, imCheckinDisposition.imCheckinNewDocument, imCheckinOptions.imDontKeepCheckedOut);

            if (checkInResult.Succeeded)
            {
                AddSecurityToDoc(imDoc, imFldr);

                IManDocuments imDocs = (IManDocuments)imFldr.Contents;
                if (null != imDocs)
                {
                    imDocs.AddDocumentReference(checkInResult.Result);
                }

                if (iMInstance.UseAdmin.GetValueOrDefault(false))
                {
                    using (var db = new IMDbContext(iMInstance))
                    {
                        var original = db.DOCMASTER.Find(imDoc.Number, 1);
                        //original.OPERATOR = iMInstance.UserId;
                        //original.AUTHOR = iMInstance.UserId;
                        original.LASTUSER = iMInstance.UserId;
                        db.SaveChanges();
                    }
                }
                
                return imDoc.Number;
            }
            else
            {
                return null;
            }
        }
    }
}
