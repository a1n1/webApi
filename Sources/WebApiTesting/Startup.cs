using Microsoft.Owin;
using Owin;
using WebApiTesting;

[assembly: OwinStartup(typeof(Startup))]

namespace WebApiTesting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
