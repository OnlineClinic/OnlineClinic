using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyOnlineClinic.Web.Startup))]
namespace MyOnlineClinic.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
