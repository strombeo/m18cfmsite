using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Madden.Startup))]
namespace Madden
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
