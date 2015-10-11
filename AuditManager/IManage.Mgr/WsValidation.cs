using AuditManager.Model;
using IManage;

namespace IM.Mgr
{
    public class WsValidation
    {
        public static WsDocDelStatusType ValidateDoc(IManDocument imDoc, imDocumentOperation imDocumentOperation)
        {
            var wsFile = Workspace.GetWsFile(imDoc);

            if (!imDoc.IsOperationAllowed(imDocumentOperation))
                return WsDocDelStatusType.InsufficientRights;

            if (imDocumentOperation == IManage.imDocumentOperation.imDeleteDocumentOp && wsFile.IsUnderPreservation)
                return WsDocDelStatusType.WSUnderPreservation;

            if (imDocumentOperation == IManage.imDocumentOperation.imDeleteDocumentOp && wsFile.IsDeleted)
                return WsDocDelStatusType.IsDeleted;

            if (imDocumentOperation == IManage.imDocumentOperation.imDeleteDocumentOp && wsFile.IsRecord)
                return WsDocDelStatusType.IsRecord;

            if (wsFile.IsCheckedOut || wsFile.IsLocked)
            {
                //return WsDocDelStatusType.IsCheckedOut;
                WsOperation.CheckInDoc(imDoc);
                imDoc.Update();
            }

            //if (wsFile.IsCheckedOut)
            //{
            //    //return WsDocDelStatusType.IsCheckedOut;
            //    WsOperation.CheckInDoc(imDoc);
            //    imDoc.Update();
            //}
                
            //if (wsFile.IsLocked)
            //{
            //    //return WsDocDelStatusType.IsLocked;
            //    WsOperation.CheckInDoc(imDoc);
            //    imDoc.Update();
            //}

            if (imDocumentOperation == IManage.imDocumentOperation.imDeleteDocumentOp && wsFile.VersionCount > 1)
            { 
                return WsDocDelStatusType.MultiVersion; 
            }
                
            return WsDocDelStatusType.Success;
        }
    }
}
