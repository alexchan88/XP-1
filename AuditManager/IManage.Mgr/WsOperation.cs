using AuditManager.Common;
using AuditManager.EF.AmDbCtx;
using AuditManager.Model;
using IManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IM.Mgr
{
    public class WsOperation
    {
        internal static void CheckInDoc(IManDocument imDoc)
        {
            //using (new TransactionScope(
            //        TransactionScopeOption.Required,
            //        new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            //{
                using (var db = new ActiveDbContext())
                {
                    var itemToRemove = db.CHECKOUT.SingleOrDefault(x => x.DOCNUM == imDoc.Number);

                    if (itemToRemove != null)
                    {
                        db.CHECKOUT.Remove(itemToRemove);
                        db.SaveChanges();
                    }

                    var original = db.DOCMASTER.SingleOrDefault(x => x.DOCNUM == imDoc.Number);
                    original.CHECKEDOUT = "N";
                    original.DOCINUSE = "N";
                    original.INUSEBY = null;
                    db.SaveChanges();

                    AddImHistoryInfo(imDoc, HistoryInfoType.CHECKIN);
                }
            //}
        }

        private static void AddImHistoryInfo(IManDocument imDoc, HistoryInfoType historyInfoType)
        {
            switch (historyInfoType)
            {
                case HistoryInfoType.CHECKIN:
                    //AddImHistoryInfo(imDoc, imHistEvent.imHistoryCheckin, AmConst.CONST_CHECKIN_COMMENT);
                    AddImHistoryInfo(imDoc, imHistEvent.imHistoryRelease, AmConst.CONST_CHECKIN_COMMENT);
                    break;
                case HistoryInfoType.DELETE:
                    AddImHistoryInfo(imDoc, imHistEvent.imHistoryDelete, AmConst.CONST_DELETE_COMMENT);
                    break;
                case HistoryInfoType.CLOSURE:
                    AddImHistoryInfo(imDoc, imHistEvent.imHistoryFrozen, AmConst.CONST_CLOSURE_COMMENT);
                    break;
                default:
                    break;
            }

        }

        private static void AddImHistoryInfo(IManDocument imDoc, imHistEvent imHistEvent, string amConstComment)
        {
            imDoc.HistoryList.Add(imHistEvent, 0, 0, AmConst.CONST_APP_NAME, amConstComment, "", "", "", null, null, null);
        }

        //public static WsFile DeleteDoc(string wsId, string wsLogFldrID, string docObjId, string comment = null)
        public static WsFile DeleteDoc(string wsId, string docObjId, string comment = null, bool toValidate = true)
        {
            IManDocument imDoc = WsUtility.GetObjectByID<IManDocument>(docObjId, isAdmin: true);

            if (!toValidate || WsValidation.ValidateDoc(imDoc, imDocumentOperation.imDeleteDocumentOp) == WsDocDelStatusType.Success)
            {
                //imDoc.Update();
                imDoc.LockContent(false);
                //imDoc.UnlockContent();
                IManProfileUpdateResult status = imDoc.UpdateWithResults();

                if (status.Succeeded)
                {
                    //Set Custom27 to true (Marked for deletion attribute) 
                    imDoc.SetAttributeByID(imProfileAttributeID.imProfileCustom27, true);

                    //Prefix document description with “DEL_”
                    if (!imDoc.Description.StartsWith(AmConst.CONST_DELETE_PREFIX))
                        imDoc.Description = AmConst.CONST_DELETE_PREFIX + imDoc.Description;

                    //Add a message to IWov history (see Autonomy’s documentation)
                    //imDoc.HistoryList.Add(imHistEvent.imHistoryDelete, 0, 0, AmConst.CONST_APP_NAME, AmConst.CONST_DELETE_COMMENT, "", "", "", null, null, null);
                    AddImHistoryInfo(imDoc, HistoryInfoType.DELETE);

                    //Store rollback information in the comments

                    //SELECT * FROM MHGROUP.GROUPS WHERE GROUPNUM IN (SELECT USER_GP_ID FROM MHGROUP.DOC_ACCESS WHERE DOCNUM = 897942)
                    //SELECT * FROM MHGROUP.GROUPS WHERE GROUPNUM IN (SELECT USER_GP_ID FROM MHGROUP.DOC_ACCESS WHERE DOCNUM = 897941)
                    //Grant right for NRTADMIN and Service Account to file.
                    //WsUtility.AddUserSecurity(imDoc, imAccessRight.imRightAll); -- Why?
                    WsUtility.AddGroupSecurity(imDoc, "NRTADMIN", imAccessRight.imRightAll);
                    
                    imDoc.Security.DefaultVisibility = imSecurityType.imPrivate;

                    //Set owner and creator to DELETE USER
                    imDoc.SetAttributeByID(imProfileAttributeID.imProfileAuthor, imDoc.Database.GetUser(AmConst.CONST_DELETE_USER));
                    imDoc.SetAttributeByID(imProfileAttributeID.imProfileOperator, imDoc.Database.GetUser(AmConst.CONST_DELETE_USER));

                    //Remove all security right and groups except for NRTADMIN group and Service Account -- Why?
                    foreach (IManUserACL userRight in imDoc.Security.UserACLs)
                    {

                        if (userRight.User.Name != "DELETEUSER")
                            //WsUtility.AddUserSecurity(imDoc, userRight.User.Name, imAccessRight.imRightNone);
                            userRight.Right = imAccessRight.imRightNone;
                    }

                    foreach (IManGroupACL groupRight in imDoc.Security.GroupACLs)
                    {
                        if (groupRight.Group.Name != "NRTADMIN")
                            //WsUtility.AddGroupSecurity(imDoc, groupRight.Group.Name, imAccessRight.imRightNone);
                            groupRight.Right = imAccessRight.imRightNone;
                    }

                    imDoc.Update();

                    imDoc.UnlockContent();

                    var wsGLog = new WsGenericLog();
                    wsGLog.ActionBy = AmUtil.GetCurrentUser;
                    wsGLog.Id = imDoc.Number;
                    wsGLog.Name = imDoc.Description;
                    wsGLog.ActionInfo = null;
                    wsGLog.AdditionalInfo = null;
                    wsGLog.Comment = comment;
                    wsGLog.OldValue = null;
                    wsGLog.NewValue = null;
                    wsGLog.WsLogActivityType = WsLogActivityType.DeleteFile;

                    WsUtility.CreateWsLog(wsId, wsGLog);
                }

                IManDocument imDocRet = WsUtility.GetObjectByID<IManDocument>(docObjId);

                return Workspace.GetWsFile(imDocRet);
            }

            return null;
        }

        public void SetFileStatus(WsModel wsModel)
        {

        }
        
        public static List<WsFile> GetAllFilesFromWs(WsModel wsModel)
        {
            List<WsFile> wsFiles = new List<WsFile>();

            WsFldr wsFldr = new WsFldr();
            wsFldr.WsFldrs = wsModel.WsFldrs;

            WsUtility.GetWsFiles(item =>
            {
                if (item.WsFiles != null && item.WsFiles.Count > 0)
                {
                    foreach (var wsFile in item.WsFiles)
                    {
                        //if (wsFile.IsIncludedInClosure)
                            wsFiles.Add(wsFile);
                    }
                }
            }, wsFldr);

            return wsFiles;
        }
        
        public static void InitiateClosure(InitiateClosureWsModel initiateClosureWsModel)
        {
            var wsModelIn = initiateClosureWsModel.WsModel;
            var largeRetFiles = initiateClosureWsModel.LargeRetFiles;
            var comment = initiateClosureWsModel.Comment;

            IManWorkspace imWs = null;

            var wsModel = Workspace.GetWs(wsModelIn.ObjectID, out imWs, WsLoadType.ALL);

            //List<WsFile> wsFiles = new List<WsFile>();

            //WsFldr wsFldr = new WsFldr();
            //wsFldr.WsFldrs = wsModelIn.WsFldrs;

            //WsUtility.GetWsFiles(item =>
            //{
            //    if (item.WsFiles != null && item.WsFiles.Count > 0)
            //    {
            //        foreach (var wsFile in item.WsFiles)
            //        {
            //            if (wsFile.IsIncludedInClosure)
            //                wsFiles.Add(wsFile);
            //        }
            //    }
            //}, wsFldr);

            List<WsFile> wsFiles = GetAllFilesFromWs(wsModelIn);
            wsFiles = wsFiles.Where(x => x.IsIncludedInClosure).ToList();

            var wsGLog = new WsGenericLog();
            wsGLog.ActionBy = AmUtil.GetCurrentUser;
            wsGLog.Id = null;
            wsGLog.Name = null;
            wsGLog.ActionInfo = null;
            wsGLog.AdditionalInfo = null;
            wsGLog.Comment = comment;
            wsGLog.OldValue = null;
            wsGLog.NewValue = null;
            wsGLog.WsLogActivityType = WsLogActivityType.Closure;

            var sbFileInfo = new StringBuilder();
            sbFileInfo.AppendLine("Event trigger Date for this closure: " + wsModelIn.WsProfile.EventTrgDate);
            sbFileInfo.AppendLine();
            sbFileInfo.AppendLine(AmUtil.GetLineSeperator);
            sbFileInfo.AppendLine();
            var sbClosureMailInfo = new StringBuilder();

            foreach (var item in wsFiles)
            {
                IManDocument imDoc = WsUtility.GetObjectByID<IManDocument>(item.ObjectID, isAdmin: true);
                if (WsValidation.ValidateDoc(imDoc, imDocumentOperation.imFreezeDocumentOp) == WsDocDelStatusType.Success)
                {
                    var wsFile = Workspace.GetWsFile(imDoc);

                    //if (wsFile.IsLocked) { }

                    //if (wsFile.IsCheckedOut)
                    //{
                    //    CheckInDoc(imDoc);
                    //}

                    //if (wsFile.IsDeleted) { }

                    //if (wsFile.IsRecord) { }

                    sbFileInfo.AppendLine("DocNum: " + wsFile.Number);
                    sbFileInfo.AppendLine("DocName: " + wsFile.Description.FileNameWithExtn(wsFile.Extn));
                    sbFileInfo.AppendLine("DocPath: " + item.FilePath);
                    sbFileInfo.AppendLine("IsLocked: " + wsFile.IsLocked);
                    sbFileInfo.AppendLine("IsCheckedOut: " + wsFile.IsCheckedOut);
                    sbFileInfo.AppendLine("IsDeleted: " + wsFile.IsDeleted);
                    sbFileInfo.AppendLine("IsRecord: " + wsFile.IsRecord);
                    sbFileInfo.AppendLine();

                    imDoc.SetAttributeByID(imProfileAttributeID.imProfileFrozen, true);

                    //imDoc.HistoryList.Add(imHistEvent.imHistoryFrozen, 0, 0, "Audit Manager", "eAudit closure", String.Empty, String.Empty, String.Empty);
                    AddImHistoryInfo(imDoc, HistoryInfoType.CLOSURE);

                    IManProfileUpdateResult res = imDoc.UpdateWithResults();
                    if (res.Succeeded)
                    {
                        sbFileInfo.AppendLine("ClosureStatus: Success");
                    }
                    else
                    {
                        sbFileInfo.AppendLine("ClosureStatus: Failure");
                    }

                    sbFileInfo.AppendLine();
                    sbFileInfo.AppendLine(AmUtil.GetLineSeperator);
                    sbFileInfo.AppendLine();

                    sbClosureMailInfo.Append("<tr><td align='center' valign='middle'>");
                    sbClosureMailInfo.Append(wsFile.Description.FileNameWithExtn(wsFile.Extn));
                    sbClosureMailInfo.Append("</td><td align='center' valign='middle'>");
                    sbClosureMailInfo.Append(item.FilePath.Remove(0, 4));
                    sbClosureMailInfo.Append("</td><td align='center' valign='middle'>");
                    if (wsFile.IsRecord)
                        sbClosureMailInfo.Append("X");
                    sbClosureMailInfo.Append("</td></tr>");
                }
            }

            if (largeRetFiles != null && largeRetFiles.Count > 0)
            {
                int idx = 0;
                sbFileInfo.AppendLine("Large Retention file included in the closure.");
                sbFileInfo.AppendLine();
                largeRetFiles.ForEach(x => sbFileInfo.AppendLine(++idx + ". " + x));

                foreach (var item in largeRetFiles)
                {
                    sbClosureMailInfo.Append("<tr align='center'><td><p>");
                    sbClosureMailInfo.Append(item);
                    sbClosureMailInfo.Append("</p></td><td><p>");
                    sbClosureMailInfo.Append("Large file retention server");
                    sbClosureMailInfo.Append("</p></td><td align='center' valign='middle'><p>");
                    sbClosureMailInfo.Append("</p></td></tr>");
                }
            }

            wsGLog.AdditionalInfo = sbFileInfo.ToString();

            WsUtility.CreateWsLog(wsModelIn.ObjectID, wsGLog);

            var recepients = WsUtility.GetEmailRecepients(wsModel, EmailRecepientType.MANAGER_N_PARTNER_N_ADMIN);

            var mailBody = AmUtil.SendMail_Closure(WsActivityType.CLOSURE_INITIATE.ToString(), wsModel.WsProfile.EngNum, wsModel.Name, recepients, sbClosureMailInfo.ToString());

            wsGLog.WsLogActivityType = WsLogActivityType.ClosureConfirmEmail;
            WsUtility.CreateEmailLog(wsModelIn.ObjectID, mailBody, wsGLog);
            
            WsUtility.SaveClosureInfo(wsModel.WsProfile.EngNum,
                string.Format("{0}|{1}", string.Join(",", wsFiles.Select(x => x.Number).ToList()),
                ((initiateClosureWsModel.LargeRetFiles == null || initiateClosureWsModel.LargeRetFiles.Count == 0) ?
                string.Empty : string.Join(",", initiateClosureWsModel.LargeRetFiles))
                ),
                string.Join(",", recepients.Select(x => x.Item2).ToList()),
                mailBody, initiateClosureWsModel.Comment,
                initiateClosureWsModel.LargeRetFiles == null ? false : (initiateClosureWsModel.LargeRetFiles.Count > 0));

            //return IM.Mgr.Workspace.GetWs(wsModelIn.ObjectID);
        }

        public static List<WsModel> UpdateWs(WsUpdateModel wsUpdateModel, UpdateProfileFrom updateProfileFrom)
        {
            var wsModelIn = wsUpdateModel.WsModel;

            IManWorkspace imWs = null;

            //--V469-583-S
            var wsModel = Workspace.GetWs(wsModelIn.ObjectID, out imWs, WsLoadType.ALL, isAdmin: true);
            //var wsModel = Workspace.GetWs_Admin(wsModelIn.ObjectID, out imWs, WsLoadType.ALL);
            //--V469-583-E

            if (wsModelIn.WsProfile.IsUnderPreservation != wsModel.WsProfile.IsUnderPreservation)
            {
                if (wsModelIn.WsProfile.IsUnderPreservation)
                {
                    if (wsModel.WsProfile.Status.StartsWith("closed", StringComparison.OrdinalIgnoreCase))
                        imWs.SetAttributeByID(imProfileAttributeID.imProfileCustom11, "CLOSED - UNDER PRESERVATION");
                    else
                        imWs.SetAttributeByID(imProfileAttributeID.imProfileCustom11, "OPEN - UNDER PRESERVATION");
                }
                else
                {
                    imWs.SetAttributeByID(imProfileAttributeID.imProfileCustom11, wsModel.WsProfile.Status.Replace(" - UNDER PRESERVATION", ""));
                }

                var wsGLog = new WsGenericLog();
                wsGLog.ActionBy = AmUtil.GetCurrentUser;
                wsGLog.Id = null;
                wsGLog.Name = null;
                wsGLog.ActionInfo = wsModelIn.WsProfile.IsUnderPreservation ? "ON" : "OFF";
                wsGLog.AdditionalInfo = null;
                wsGLog.Comment = wsUpdateModel.PreservationComment;
                wsGLog.OldValue = wsModel.WsProfile.IsUnderPreservation.ToString();
                wsGLog.NewValue = wsModelIn.WsProfile.IsUnderPreservation.ToString();
                wsGLog.WsLogActivityType = WsLogActivityType.Preservation;

                WsUtility.CreateWsLog(wsModelIn.ObjectID, wsGLog);

                //Mail
                var wsAdmin = WsUtility.GetEmailRecepients(wsModel, EmailRecepientType.ADMIN);
                var recepients = WsUtility.GetEmailRecepients(wsModel, EmailRecepientType.MEMBERS);

                var mailBody = AmUtil.SendMail_Preservation(wsModelIn.WsProfile.IsUnderPreservation ? 
                    WsActivityType.WSPROFILE_PRESERVATION_ON.ToString() : WsActivityType.WSPROFILE_PRESERVATION_OFF.ToString(),
                    wsModel.WsProfile.EngNum, wsModel.Name, wsModel.WsProfile.ClientDesc, recepients, wsAdmin,
                    wsModelIn.WsProfile.IsUnderPreservation ? "Workspace Flagged for Preservation" : "Workspace Preservation Removed");

                wsGLog.WsLogActivityType = wsModelIn.WsProfile.IsUnderPreservation ?
                     WsLogActivityType.PreservationONEmail : WsLogActivityType.PreservationOFFEmail;
                WsUtility.CreateEmailLog(wsModelIn.ObjectID, mailBody, wsGLog);

                //
            }

            if (wsModelIn.WsProfile.EventTrgDate != wsModel.WsProfile.EventTrgDate)
            {
                imWs.SetAttributeByID(imProfileAttributeID.imProfileCustom23, wsModelIn.WsProfile.EventTrgDate);

                var wsGLog = new WsGenericLog();
                wsGLog.ActionBy = AmUtil.GetCurrentUser;
                wsGLog.Id = null;
                wsGLog.Name = null;
                wsGLog.ActionInfo = null;
                wsGLog.AdditionalInfo = null;
                wsGLog.Comment = wsUpdateModel.PreservationComment;
                wsGLog.OldValue = wsModel.WsProfile.EventTrgDate.ToString();
                wsGLog.NewValue = wsModelIn.WsProfile.EventTrgDate.ToString();
                wsGLog.WsLogActivityType = WsLogActivityType.EventTrgDate;

                WsUtility.CreateWsLog(wsModelIn.ObjectID, wsGLog);
            }

            if (wsModelIn.WsProfile.IsKDrive != wsModel.WsProfile.IsKDrive)
            {
                imWs.SetAttributeByID(imProfileAttributeID.imProfileCustom26, wsModelIn.WsProfile.IsKDrive);

                var wsGLog = new WsGenericLog();
                wsGLog.ActionBy = AmUtil.GetCurrentUser;
                wsGLog.Id = null;
                wsGLog.Name = null;
                wsGLog.ActionInfo = null;
                wsGLog.AdditionalInfo = null;
                wsGLog.Comment = wsUpdateModel.PreservationComment;
                wsGLog.OldValue = wsModel.WsProfile.IsKDrive.ToString();
                wsGLog.NewValue = wsModelIn.WsProfile.IsKDrive.ToString();
                wsGLog.WsLogActivityType = WsLogActivityType.ChangeStorageLocation;

                WsUtility.CreateWsLog(wsModelIn.ObjectID, wsGLog);
            }

            if (wsModelIn.WsProfile.IsServer2 != wsModel.WsProfile.IsServer2)
            {
                imWs.SetAttributeByID(imProfileAttributeID.imProfileCustom12, wsModelIn.WsProfile.IsServer2.ToString());

                var wsGLog = new WsGenericLog();
                wsGLog.ActionBy = AmUtil.GetCurrentUser;
                wsGLog.Id = null;
                wsGLog.Name = null;
                wsGLog.ActionInfo = null;
                wsGLog.AdditionalInfo = null;
                wsGLog.Comment = wsUpdateModel.PreservationComment;
                wsGLog.OldValue = wsModel.WsProfile.IsServer2.ToString();
                wsGLog.NewValue = wsModelIn.WsProfile.IsServer2.ToString();
                wsGLog.WsLogActivityType = WsLogActivityType.Server2;

                WsUtility.CreateWsLog(wsModelIn.ObjectID, wsGLog);
            }

            if (wsModelIn.WsProfile.RetPolicy != wsModel.WsProfile.RetPolicy)
            {
                imWs.SetAttributeByID(imProfileAttributeID.imProfileCustom7, wsModelIn.WsProfile.RetPolicy.ToString());

                var wsGLog = new WsGenericLog();
                wsGLog.ActionBy = AmUtil.GetCurrentUser;
                wsGLog.Id = null;
                wsGLog.Name = null;
                wsGLog.ActionInfo = null;
                wsGLog.AdditionalInfo = null;
                wsGLog.Comment = wsUpdateModel.RetentionComment;
                wsGLog.OldValue = wsModel.WsProfile.RetPolicy.ToString();
                wsGLog.NewValue = wsModelIn.WsProfile.RetPolicy.ToString();
                wsGLog.WsLogActivityType = WsLogActivityType.RetPolicy;

                WsUtility.CreateWsLog(wsModelIn.ObjectID, wsGLog);
            }

            //wsNew.Profile.SetAttributeByID(imProfileAttributeID.imProfileCustom7, "7YEARS");

            imWs.Update();

            WsUtility.UpdateWsProfile(wsUpdateModel, updateProfileFrom);

            return IM.Mgr.Workspace.GetWs(wsModelIn.ObjectID);
        }

        public static WsFile GetWsFile(double fileNum)
        {
            IManDocument imDoc = WsUtility.GetObjectByID<IManDocument>(WsUtility.GetWsObjectTypeId(WsObjectType.File, fileNum));
            return IM.Mgr.Workspace.GetWsFile(imDoc);
        }

        public static void Move_YrEnd_Audit_RET_N_ENG_To_ElecWp(double retDocNum, double engDocNum, double engNum)
        {
            IManDocument imRetDoc = WsUtility.GetObjectByID<IManDocument>(WsUtility.GetWsObjectTypeId(WsObjectType.File, retDocNum));
            IManDocument imEngDoc = WsUtility.GetObjectByID<IManDocument>(WsUtility.GetWsObjectTypeId(WsObjectType.File, engDocNum));

            IManWorkspace imWs = WsUtility.GetObjectByID<IManWorkspace>(WsUtility.GetWsObjectTypeId(WsObjectType.Workspace, engNum));

            IManDocumentFolder imFldrPeriodEndAudit = WsUtility.GetWsFldr(imWs, WsFldrType.PeriodEndAudit);
            IManDocumentFolder imRetFldr = null;
            IManDocumentFolder imEngFldr = null;
            IManDocumentFolder imElecWpFldr = null;

            foreach(IManDocumentFolder docFldr in imFldrPeriodEndAudit.SubFolders)
            {
                if(docFldr.Name.Equals("RET Files", StringComparison.OrdinalIgnoreCase))
                {
                    imRetFldr = docFldr;
                }
                else if (docFldr.Name.Equals("ENG Files", StringComparison.OrdinalIgnoreCase))
                {
                    imEngFldr = docFldr;
                }
                else if (docFldr.Name.Equals("Electronic Workpapers (Maintained Outside of eAudIT)", StringComparison.OrdinalIgnoreCase))
                {
                    imElecWpFldr = docFldr;
                }
            }

            IManDocuments imElecWpDocs = imElecWpFldr == null ? null : (IManDocuments)imElecWpFldr.Contents;
            IManDocuments imRetDocs = imRetFldr == null ? null : (IManDocuments)imRetFldr.Contents;
            IManDocuments imEngDocs = imEngFldr == null ? null : (IManDocuments)imEngFldr.Contents;

            if(imElecWpDocs != null) 
                imElecWpDocs.AddDocumentReference(imRetDoc);

            if (imRetDocs != null)
                imRetDocs.RemoveByObject(imRetDoc);

            if (imElecWpDocs != null) 
                imElecWpDocs.AddDocumentReference(imEngDoc);

            if (imEngDocs != null)
                imEngDocs.RemoveByObject(imEngDoc);
        }
    }
}
