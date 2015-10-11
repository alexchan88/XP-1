using AuditManager.Common;
using AuditManager.Model;
using AuditManager.Web.Binders;
using AuditManager.Web.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;

namespace AuditManager.Web.Api
{
    public class WorkspaceController : ApiController
    {
        public JArray GetEngByEngNum(string engNum, WsLoadType wsLoadType, bool isAdmin)
        {
            var result = AuditManager.Rep.Workspace.GetEngByEngNum(engNum, wsLoadType, isAdmin: isAdmin);

            if (result == null)
                return null;

            result.ForEach(x =>
                {
                    if (x.WsProfile != null)
                    {
                        x.WsProfile.KPMGOnly = ExtApi.GetKPMGOnly(x.WsProfile.EngNum);
                    }
                });

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public JArray GetEngByWsId(string wsId, WsLoadType wsLoadType, bool newSession = false)
        {
            var result = AuditManager.Rep.Workspace.GetEngByWsId(wsId, wsLoadType, newSession);

            if (result == null)
                return null;

            result.ForEach(x =>
            {
                if (x.WsProfile != null)
                {
                    x.WsProfile.KPMGOnly = ExtApi.GetKPMGOnly(x.WsProfile.EngNum);
                }
            });

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public JArray GetWs(string wsId, bool newSession = false)
        {
            var result = AuditManager.Rep.Workspace.GetWs(string.IsNullOrWhiteSpace(wsId) ? null : wsId, newSession);

            if (!string.IsNullOrWhiteSpace(wsId))
                result.ForEach(x =>
                {
                    if (x.WsProfile != null)
                    {
                        x.WsProfile.KPMGOnly = ExtApi.GetKPMGOnly(x.WsProfile.EngNum);
                    }
                });

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public JArray GetENGDocByRETDocNumber_2(double retFileNum)
        {
            //1.	FolderPath
            //2.	FileName
            //3.	DocumentNumber
            //4.	FileType

            var result = AuditManager.Rep.WsActivity.GetENGDocByRETDocNumber_2(retFileNum);
            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public JObject GetENGDocByRETDocNumber(double retFileNum)
        {
            var result = AuditManager.Rep.WsActivity.GetENGDocByRETDocNumber(retFileNum);

            if (result == null)
                return null;

            var retModel = JObject.FromObject(result);

            return retModel;
        }

        public JArray GetFileActivity(DateTime? fDate, DateTime tDate, ActivityFilterType activityFilterType, string engNum = null, string usrId = null)
        {
            var result = AuditManager.Rep.WsActivity.GetActivity(fDate, tDate, activityFilterType, engNum, usrId);

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public JArray GetGrpNUser(Model.ImDbType imDbType)
        {
            var result = AuditManager.Rep.Workspace.GetGrpNUser(imDbType);

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public JArray GetNoRecordFiles([System.Web.Http.ModelBinding.ModelBinder(typeof(CommaDelimitedListBinder))]string[] engNums)
        {
            var result = AuditManager.Rep.Workspace.GetNoRecordFiles(engNums.ToList());

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public JObject DeleteDoc(string wsId, string docObjId, string comment, bool toValidate = true)
        {
            var result = AuditManager.Rep.WsOperation.DeleteDoc(wsId, docObjId, comment, toValidate);

            var retModel = JObject.FromObject(result);

            return retModel;
        }

        public JArray GetClosureComment(string engNum)
        {
            var result = AuditManager.Rep.WsOperation.GetClosureComment(engNum);

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public JArray GetS2NonClosedWb(string engNum)
        {
            var result = AuditManager.Rep.WsOperation.GetS2NonClosedWb(engNum);

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public bool GetIsWSUnderPreservation(string engNum)
        {
            return AuditManager.Rep.WsService.IsWSUnderPreservation(engNum);
        }

        public string PostUpdateFileActivity([FromBody]FileActivity_UpdateModel activityUpdateModel)
        {
            //return "Activity-In-Progress";

            var result = AuditManager.Rep.WsActivity.UpdateFileActivity(activityUpdateModel);

            ExtApi.KWSLogKFileActivity(activityUpdateModel);

            return result ?? "Activity-In-Progress";

            //return "Acknowledged";

            //var result = AuditManager.Rep.WsActivity.UpdateFileActivity(activityUpdateModel);

            //var retModel = JArray.FromObject(result);

            //return retModel;
        }

        public JArray PostUpdateWs([FromBody]WsUpdateModel wsUpdateModel)
        {
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //var wsModel = serializer.Deserialize<WsModel>(wsModelIn);

            var result = AuditManager.Rep.WsOperation.UpdateWs(wsUpdateModel, UpdateProfileFrom.Profile);

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        [ValidateAntiForgeryToken]
        public JArray PostInitiateClosure([FromBody]InitiateClosureWsModel initiateClosureWsModel)
        {
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //var wsModel = serializer.Deserialize<WsModel>(wsModelIn);

            var result = AuditManager.Rep.WsOperation.InitiateClosure(initiateClosureWsModel);

            var retModel = JArray.FromObject(result);

            return retModel;
        }

        public JArray GetSurveyInfo(string engagementNumber, double fileNumber)
        {
            return ExtApi.GetSurveyInfo(engagementNumber, fileNumber);
        }

        [ValidateAntiForgeryToken]
        public void PostSurveyRequest(FormDataCollection formData)
        {
            var kUsr_Requestor = AuditManager.Rep.AmUtility.GetKUsr(AmUtil.GetCurrentUser);

            var partner = string.IsNullOrWhiteSpace(formData.Get("hid_PartnerDesc_Name")) ? null : formData.Get("hid_PartnerDesc_Name");
            var manager = string.IsNullOrWhiteSpace(formData.Get("hid_ManagerDesc_Name")) ? null : formData.Get("hid_ManagerDesc_Name");

            var kUsr_Manager = AuditManager.Rep.AmUtility.GetKUsr(manager ?? partner ?? AmUtil.GetCurrentUser);
            var kUsr_Partner = AuditManager.Rep.AmUtility.GetKUsr(partner ?? manager ?? AmUtil.GetCurrentUser);

            //hid_ManagerDesc_Name
            //hid_PartnerDesc_Name

            var wsSurveyModel = new WsSurveyModel
            {
                SurveyRequestType = formData.Get("surveyRequestType").ToEnum<SurveyRequestType>(),
                ClientName = formData.Get("txtClient"),
                ClientNumber = formData.Get("txtClientNbr"),
                EngagementName = formData.Get("txtEngName"),
                EngagementNumber = formData.Get("txtEngNum"),
                Preservation = formData.Get("txtPreservation").FromBool<char, string>(),

                //ClientYearEndDate = formData.Get("txtClientYearEnd").ToDate<DateTime?>(SqlDateTime.MinValue.Value),//RET - null
                ClientYearEndDate = formData.Get("txtClientYearEnd").ToDate<DateTime?>(null),//RET - null
                //BusinessUnit = formData.Get("txtBusUnit") ?? string.Empty,//RET - null
                BusinessUnit = formData.Get("txtBusUnit"),//RET - null
                //eAuditWorkflow = formData.Get("txteAudITWF") ?? string.Empty,//RET - null
                eAuditWorkflow = formData.Get("txteAudITWF"),//RET - null

                ENGYear = formData.Get("surveyRequestType").ToEnum<SurveyRequestType>() == SurveyRequestType.RET ? formData.Get("txteAudITYr_RET") : formData.Get("txteAudITYr_RF"),

                EBPEngagement = formData.Get("radRFEBP").FromBool<char, string>(),//RET - false

                _11KEBP = false.FromBool<char>(), //Always false

                SplitMAF = formData.Get("rad1MAF2Multi").FromBool<char, string>(),//RET - false
                CombineMAF = formData.Get("radMultiMAF21").FromBool<char, string>(),//RET - false
                NumberOfMAFs = formData.Get("txtNoOfMAF").ToInt(1),//RET - 1
                //PrimaryWBName = formData.Get("txtPrimWBName") ?? string.Empty,//RET - null
                PrimaryWBName = formData.Get("txtPrimWBName"),//RET - null
                GroupOrMultiInfo = formData.Get("txtAIns") ?? string.Empty,//RET - ""

                DRMSFileNumber = int.Parse(formData.Get("txtDocNum")),
                EngFileName = formData.Get("txtMAFName"),

                //RequiredDate = formData.Get("surveyRequestType").ToEnum<SurveyRequestType>() == SurveyRequestType.RF ? DateTime.Parse(formData.Get("txtRFReqDate"))
                //: default(DateTime),
                //RequiredDate = formData.Get("txtRFReqDate").ToDate<DateTime?>(null),//RET - null
                RequiredDate = formData.Get("txtRFReqDate").ToDate<DateTime?>(SqlDateTime.MaxValue.ToString().ToDate<DateTime>()),//RET - null

                //ManagerEmail = formData.Get("txtMgrEmail") ?? string.Empty,
                //PartnerEmail = formData.Get("txtParEmail") ?? string.Empty,

                //-------------07-09-2015

                //ThirdPartyAllowed = formData.Get("radAgrClient").ToBool<string>(),

                //// Something fishy
                //ContractorAllowed = formData.Get("radAgrClientCal").Equals("YES", StringComparison.OrdinalIgnoreCase) ? 0 :
                //                        formData.Get("radAgrClientCal").Equals("NO", StringComparison.OrdinalIgnoreCase) ? 1 : 2,
                //// Something fishy

                //ContractorProhibited = formData.Get("radKPMGContEnt").ToBool<string>(),
                //ContractorProhibitReason = formData.Get("txtADesc"),

                //KPMGOnly = (formData.Get("radAgrClient").ToBool<string>() &&
                //            formData.Get("radAgrClientCal").ToBool<string>() &&
                //            !formData.Get("radKPMGContEnt").ToBool<string>()).FromBool<char, bool>(),

                KPMGOnly = formData.Get("hid_spanKPMGOnly").ToBool<string>().FromBool<char, bool>(),

                //-------------07-09-2015

                RequestorEmail = kUsr_Requestor.EmailId,
                RequestorFirstName = kUsr_Requestor.FName,
                RequestorId = kUsr_Requestor.UserId,
                RequestorLastName = kUsr_Requestor.LName,

                ManagerEmail = formData.Get("hid_ManagerDesc_Email") ?? string.Empty,

                //ManagerFirstName = formData.Get("hid_ManagerDesc_FirstName") ?? string.Empty,
                //ManagerLastName = formData.Get("hid_ManagerDesc_LastName") ?? string.Empty,
                ManagerFirstName = kUsr_Manager.FName,
                ManagerLastName = kUsr_Manager.LName,

                PartnerEmail = formData.Get("hid_PartnerDesc_Email") ?? string.Empty,

                //PartnerFirstName = formData.Get("hid_PartnerDesc_FirstName") ?? string.Empty,
                //PartnerLastName = formData.Get("hid_PartnerDesc_LastName") ?? string.Empty,
                PartnerFirstName = kUsr_Partner.FName,
                PartnerLastName = kUsr_Partner.LName,

                GroupOrMulti = false.FromBool<char>(),

                OfficeLocation = kUsr_Requestor.Location,

                WorkBooks = formData.Get("workBooks"),

                IsRFInDiffWF = formData.Get("radDiffWf").FromBool<char, string>(),
                IsPartilaRF = formData.Get("radPartialRF").FromBool<char, string>(),
                RFModificationType = formData.Get("radPartialRFDesc"),

                IsSawEng = formData.Get("radSawEng").ToBool(),

                //ProjectCode = formData.Get("cmbProjectCode"),
                ProjectCode = formData.Get("hid_projectCode"),
                DCSServer = formData.Get("cmbDCSServer"),
                ListofDcssServers = formData.Get("hid_cmbDCSServer_Options") + "," + formData.Get("cmbDCSServer"),
            };

            AuditManager.Rep.WsOperation.SurveyRequest(wsSurveyModel);

            //WsProfile_TP wsProfile_TP = new WsProfile_TP();

            //wsProfile_TP.TP_Q1 = formData.Get("radAgrClient");
            //wsProfile_TP.TP_Q2 = formData.Get("radAgrClientCal");
            //wsProfile_TP.TP_Q3 = formData.Get("radKPMGContEnt");
            //wsProfile_TP.TP_Q3_Comment = formData.Get("txtADesc");

            //AuditManager.Rep.WsOperation.UpdateWsProfile(formData.Get("txtEngNum"), wsProfile_TP, wsSurveyModel.SurveyRequestType == SurveyRequestType.RET ? UpdateProfileFrom.RET : UpdateProfileFrom.RF);

            ExtApi.KWSLogKFileActivity(wsSurveyModel);
        }

        public void RequestUpdateCMS(string num, string comment)
        {
            ExtApi.RequestUpdateCMS(num, comment);
        }
    }
}
