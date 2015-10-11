using AuditManager.Web.Filters;
using System.Web.Mvc;

namespace AuditManager.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new AmMvcAuthenticationFilter());
            filters.Add(new MvcErrorHandlerFilter());
            filters.Add(new Elmah.Contrib.Mvc.ElmahHandleErrorAttribute());

            if(AuditManager.Common.ConfigUtility.IsUnderMaintenance())
                filters.Add(new AuditManager.Web.Filters.UnderMaintenanceFilter());
        }
    }
}
