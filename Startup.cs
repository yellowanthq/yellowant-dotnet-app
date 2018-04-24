using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YellowAntDemo.Startup))]
namespace YellowAntDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
