using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TechShop.Web.Startup))]
namespace TechShop.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
