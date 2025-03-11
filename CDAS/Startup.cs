using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CDAS.Startup))]

namespace CDAS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}