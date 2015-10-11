using AuditManager.Model;
using AuditManager.Web.Common;
using System.Web.Mvc;

namespace AuditManager.Web.Controllers
{
    public class WorkspaceController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private void ProcessKPMGOnly()
        {
            ExtApi.ProcessKPMGOnly();
        }

        private void UpdateWS()
        {
            ExtApi.UpdateWS("6666666862", false);
        }
       
        private void CreateWs()
        {
            ExtApi.CreateWS(new WsCreate
            {
                ClientId = "1000500347",
                PartnerAssistanceEmpId = "PartnerAssistanceEmpId",
                PartnerEmpId = "2181468",
                ManagerEmpId = "1893323",
                EngagementDescription = "TestEng",
                EngagementNumber = "7777777701"
            });
        }

        public ActionResult Invalid()
        {
            return View("Invalid");
        }

        public ActionResult RETnRF(SurveyRequestType surveyRequestType)
        {
            if (surveyRequestType == SurveyRequestType.RF)
                return PartialView("~\\Views\\Shared\\MyEng\\RETnRF\\_RFPartial.cshtml");
            else if (surveyRequestType == SurveyRequestType.RET)
                return PartialView("~\\Views\\Shared\\MyEng\\RETnRF\\_RETPartial.cshtml");

            return null;
        }

        public ActionResult InitiateClosure(string wsId)
        {
            return PartialView("~\\Views\\Shared\\MyEng\\_WsInitiateClosure.cshtml");
        }

        public ActionResult S2Link()
        {
            return PartialView("~\\Views\\Shared\\MyEng\\_WsS2GuidMap.cshtml");
        }

        public ActionResult RequestAccess()
        {
            return PartialView("~\\Views\\Shared\\OuterTab\\_RequestAccessPartial.cshtml");
        }
    }
}