using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ERacingWebApp.Startup))]
namespace ERacingWebApp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
