using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JohnsBugTracker.Startup))]
namespace JohnsBugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
