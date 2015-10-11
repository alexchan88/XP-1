using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AuditManager.Web.Startup))]
namespace AuditManager.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
