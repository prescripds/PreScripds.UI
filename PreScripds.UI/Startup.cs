using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PreScripds.UI.Startup))]
namespace PreScripds.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
