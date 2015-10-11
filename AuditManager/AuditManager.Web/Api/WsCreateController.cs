using AuditManager.Model;
using AuditManager.Web.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;

namespace AuditManager.Web.Api
{
    public class WsCreateController : ApiController
    {
        [ValidateAntiForgeryToken]
        public void RequestAccess(string engNum, WsUserType wsUserType, string emailUsers, string comment)
        {
            AuditManager.Rep.WsCreate.RequestAccess(engNum, wsUserType, emailUsers, comment);
        }

        [ValidateAntiForgeryToken]
        public void CreateWs(FormDataCollection formData)
        {
            //Select * From Ref.Eng_WS_Metadata_V2 Where [EngagementID]  = @eID

            if (AuditManager.Rep.AmUtility.IsWSExists(formData.Get("txtEngId_CreateWs")))
            {
                throw new Exception(string.Format("Workspace => {0} already exists.", formData.Get("txtEngId_CreateWs")));
            }
            else
            {
                var wsCreate = new AuditManager.Model.WsCreate
                {
                    EngagementNumber = formData.Get("txtEngId_CreateWs"),
                    EngagementDescription = formData.Get("txtEngName_CreateWs"),

                    ClientId = formData.Get("txtClientNumber_CreateWs"),
                    ClientName = formData.Get("txtClient_CreateWs"),

                    ManagerId = formData.Get("txtEngMgrId_CreateWs"),
                    ManagerEmpId = formData.Get("txtEngMgrEmpId_CreateWs"),

                    ManagerName = formData.Get("txtEngMgr_CreateWs"),
                    ManagerFunction = formData.Get("txtManagerFunction"),

                    PartnerId = formData.Get("txtEngParId_CreateWs"),
                    PartnerEmpId = formData.Get("txtEngParEmpId_CreateWs"),

                    PartnerName = formData.Get("txtEngPar_CreateWs"),
                    PartnerFunction = formData.Get("txtPartnerFunction"),

                    ParentCompany = formData.Get("txtParentCompany"),

                    PartnerAssistanceId = formData.Get("txtPartnerAssistanceId"),
                    PartnerAssistanceEmpId = formData.Get("txtPartnerAssistanceEmpId"),

                    PartnerAssistanceName = formData.Get("txtPartnerAssistanceName"),

                    //WsProfile_TP = new WsProfile_TP
                    //{
                    //    TP_Q1 = formData.Get("radAgrClient_CreateWs"),
                    //    TP_Q2 = formData.Get("radAgrClientCal_CreateWs"),
                    //    TP_Q3 = formData.Get("radKPMGContEnt_CreateWs"),
                    //    TP_Q3_Comment = formData.Get("txtADesc_CreateWs"),
                    //}
                };

                //var engNum = formData.Get("txtEngId_CreateWs");
                //var engName = formData.Get("txtEngName_CreateWs");

                //var clientName = formData.Get("txtClient_CreateWs");
                //var clientNum = formData.Get("txtClientNumber_CreateWs");

                //var mgrName = formData.Get("txtEngMgr_CreateWs");
                //var mgrId = formData.Get("txtEngMgrId_CreateWs");
                //var mgrFun = formData.Get("txtManagerFunction");

                //var parName = formData.Get("txtEngPar_CreateWs");
                //var parId = formData.Get("txtEngParId_CreateWs");
                //var parFun = formData.Get("txtPartnerFunction");

                //var parentCompany = formData.Get("txtParentCompany");

                //WsProfile_TP wsProfile_TP = new WsProfile_TP();

                //wsProfile_TP.TP_Q1 = formData.Get("radAgrClient_CreateWs");
                //wsProfile_TP.TP_Q2 = formData.Get("radAgrClientCal_CreateWs");
                //wsProfile_TP.TP_Q3 = formData.Get("radKPMGContEnt_CreateWs");
                //wsProfile_TP.TP_Q3_Comment = formData.Get("txtADesc_CreateWs");

                //AuditManager.Rep.WsOperation.UpdateWsProfile(formData.Get("txtEngId_CreateWs"), wsProfile_TP);

                //AuditManager.Rep.WsCreate.CreateWS(wsCreate);
                ExtApi.CreateWS(wsCreate);
            }
        }

        public void RequestAccessToAuditManager(string comment)
        {
            AuditManager.Rep.WsCreate.RequestAccessToAuditManager(comment, true);
        }

        public bool GetDoesUserHaveAccessToEng(string engNum)
        {
            if (AuditManager.Rep.WsCreate.DoesUserHaveAccessToEng(engNum))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetIsWSExists(string engNum)
        {
            if (AuditManager.Rep.AmUtility.IsWSExists(engNum))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //public JObject GetWsProfile_TP(string engNum)
        //{
        //    var result = AuditManager.Rep.AmUtility.GetWsProfile_TP(engNum);

        //    var retModel = JObject.FromObject(result);

        //    return retModel;
        //}

        public JArray GetEngDetails(string engNum)
        {
            var result = AuditManager.Rep.WsCreate.GetEngDetails(engNum);

            var retModel = JArray.FromObject(result);

            return retModel;
        }
    }
}
