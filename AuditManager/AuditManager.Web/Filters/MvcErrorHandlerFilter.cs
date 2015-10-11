
namespace AuditManager.Web.Filters
{
    public class MvcErrorHandlerFilter : System.Web.Mvc.HandleErrorAttribute
    {
        public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }
    }
}