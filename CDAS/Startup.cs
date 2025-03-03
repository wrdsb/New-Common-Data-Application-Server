using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Security_Card_System.Startup))]

namespace Security_Card_System
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}