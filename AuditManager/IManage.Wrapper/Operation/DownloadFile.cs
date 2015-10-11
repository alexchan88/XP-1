using IM.Wrapper.EF;
using IM.Wrapper.Model;
using IManage;
using System;
using System.Linq;
using System.Transactions;

namespace IM.Wrapper.Operation
{
    internal class DownloadFile
    {
        public static string GetDocLocation(IMInstance iMInstance, string engNum, double docNum)
        {
            using (new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                using (var db = new IMDbContext(iMInstance))
                {
                    var docLoc = db.Set<DOCMASTER>().
                        Where(x => x.C2ALIAS.Equals(engNum, StringComparison.OrdinalIgnoreCase) && x.DOCNUM == docNum && x.VERSION == 1
                        ).Select(y => y.DOCLOC).ToList().FirstOrDefault();

                    var sp_docLoc = docLoc.Split(':');

                    var p1 = sp_docLoc[0].Trim();

                    var srvLoc =
                        db.Set<DOCSERVER>().
                        Where(x => x.DOCSERVER1.Equals(p1, StringComparison.OrdinalIgnoreCase)
                        ).Select(y => y.LOCATION).ToList().FirstOrDefault();


                    var result = string.Format("{0}{1}", srvLoc, sp_docLoc[1]);
                    return result;
                }
            }
        }

        private static string GetDocLocation(IMInstance iMInstance, string docLoc)
        {
            using (new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                using (var db = new IMDbContext(iMInstance))
                {
                    var sp_docLoc = docLoc.Split(':');

                    var p1 = sp_docLoc[0].Trim();

                    var srvLoc =
                        db.Set<DOCSERVER>().
                        Where(x => x.DOCSERVER1.Equals(p1, StringComparison.OrdinalIgnoreCase)
                        ).Select(y => y.LOCATION).ToList().FirstOrDefault();


                    var result = string.Format("{0}{1}", srvLoc, sp_docLoc[1]);
                    return result;
                }
            }
        }

        private static string GetDocLocation(IMInstance iMInstance, double docNum)
        {
            var iMSession = new IMSession(iMInstance);

            var docId = IM.Wrapper.Utility.IMUtility.GetWsObjectID(iMInstance, iMSession, IMWSObjectType.File, docNum);

            IManDocument imDoc = iMInstance.UseAdmin.GetValueOrDefault(false) ?
                (IManDocument)iMSession.AdminSession().DMS.GetObjectByID(docId) :
                (IManDocument)iMSession.UserSession().DMS.GetObjectByID(docId);

            var docLoc = imDoc.GetAttributeByID(imProfileAttributeID.imProfileLocation).ToString();

            return docLoc;
        }

    //    INSERT INTO
    //    ACTIVE.MHGROUP.DOCHISTORY
    //(
    //    DOCNUM,
    //    VERSION,
    //    ACTIVITY,
    //    ACTIVITY_CODE,
    //    ACTIVITY_DATETIME,
    //    DURATION,
    //    PAGES_PRINTED,
    //    DOCUSER,
    //    APPNAME,
    //    LOCATION
    //)
	
    //    VALUES
		
    //(
	
    //    @DocNum,
    //    1,
    //    'Export',
    //    18,
    //    GETDATE(),
    //    0,
    //    0,
    //    @UserID,
    //    'KPMGFileTransfer',
    //    'USMDCKDWB2001'
    //)

        public static void PutDocDownloadHistory(IMInstance iMInstance, double docNum)
        {
            //var docHistory = new DOCHISTORY {
            //    DOCNUM = docNum,
            //    VERSION = 1,
            //    ACTIVITY = "Export",
            //    ACTIVITY_CODE = 1,
            //    ACTIVITY_DATETIME = DateTime.UtcNow,
            //    DURATION = 0,
            //    PAGES_PRINTED = 0,
            //    NUM1 = null,
            //    NUM2 = null,
            //    NUM3 = null,
            //    DATA1 =  null,
            //    DATA2 = null,
            //    DOCUSER = iMInstance.UserId,
            //    APPNAME = iMInstance.AppName_0,
            //    LOCATION = iMInstance.AppName,
            //    COMMENTS = "Downloaded from Audit Manager"
            //};

            //using (var db = new IMDbContext(iMInstance))
            //{
            //    var x = db.Database.SqlQuery<DOCHISTORY>("INSERT INTO MHGROUP.DOCHISTORY VALUES (" + docNum  + ")").ToList();
                
            //}

            var iMSession = new IMSession(iMInstance);
            var docId = IM.Wrapper.Utility.IMUtility.GetWsObjectID(iMInstance, iMSession, IMWSObjectType.File, docNum);

            IManDocument imDoc = iMInstance.UseAdmin.GetValueOrDefault(false) ?
                (IManDocument)iMSession.AdminSession().DMS.GetObjectByID(docId) :
                (IManDocument)iMSession.UserSession().DMS.GetObjectByID(docId);

            imDoc.HistoryList.Add(imHistEvent.imHistoryExport, 0, 0, iMInstance.AppName_0, "Downloaded from Audit Manager", iMInstance.AppName, "", "", null, null, null);

        }
    }
}
