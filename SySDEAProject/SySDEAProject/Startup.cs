using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SySDEAProject.Startup))]
namespace SySDEAProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
