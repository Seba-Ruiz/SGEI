using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(login_v6.Startup))]
namespace login_v6
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
