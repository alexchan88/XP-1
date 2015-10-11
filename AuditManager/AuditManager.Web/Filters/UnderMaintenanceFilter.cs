using System.Web.Mvc;

namespace AuditManager.Web.Filters
{
    public class UnderMaintenanceFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!AuditManager.Common.AmUtil.IsMaintenanceUser())
                filterContext.Result = new ViewResult { ViewName = "Maintenance" };
        }
    }
}