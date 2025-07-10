using System.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Extensions;
using System.Security.Claims;
using System.Linq;
using System;

//[assembly: OwinStartupAttribute(typeof(CDAS.Startup))]

namespace CDAS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

    }

}