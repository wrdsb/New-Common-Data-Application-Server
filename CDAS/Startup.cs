using System.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(CDAS.Startup))]

namespace CDAS
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            /*
            app.UseCookieAuthentication(new CookieAuthenticationOptions { AuthenticationType = CookieAuthenticationDefaults.AuthenticationType });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://login.microsoftonline.com/cd25c694-bfb8-48f4-9d0d-b9af282c4ab4",
                ClientId = "b68f29b1-3f11-4dd0-a2a9-0f998f6189ca",
                RedirectUri = "https://commondataapplicationservertest.wrdsb.ca/",
                ResponseType = "id_token",
                UseTokenLifetime = false,
                SignInAsAuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = context =>
                    {
                        ClaimsIdentity claim_id = context.AuthenticationTicket.Identity;
                        claim_id.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "User"));
                        return Task.CompletedTask;
                    },
                    AuthenticationFailed = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/");
                        return Task.CompletedTask;
                    }
                }
            });
            //ConfigureAuth(app);
            */

            /*recent
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["ida:ClientId"],
                Authority = $"https://login.microsoftonline.com/{ConfigurationManager.AppSettings["ida:TenantId"]}/v2.0",
                RedirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"],
                PostLogoutRedirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"],
                ResponseType = OpenIdConnectResponseType.IdToken,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true
                },
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/Error.aspx?message=" + context.Exception.Message);
                        return Task.FromResult(0);
                    }
                }

            }); ;
            */
        }
            
    }
}