using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SOP_IAA.Startup))]
namespace SOP_IAA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
