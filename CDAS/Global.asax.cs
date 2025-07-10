using System;
using System.Net;
using System.Security.Principal;
//using System.Collections.Generic;
//using System.Linq;
using System.Web;
//using System.Web.Helpers;
using System.Web.Security;
//using System.Web.SessionState;

namespace CDAS
{
    public class Global : System.Web.HttpApplication
    {
        //AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        protected void Application_Start(object sender, EventArgs e)
        {
            //Allows Azure AD to be used
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }
        void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            String cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (null == authCookie)
            {//There is no authentication cookie.
                return;
            }

            FormsAuthenticationTicket authTicket = null;

            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception ex)
            {
                //Write the exception to the Event Log.
                return;
            }

            if (null == authTicket)
            {//Cookie failed to decrypt.
                return;
            }

            //When the ticket was created, the UserData property was assigned a
            //pipe-delimited string of group names.
            String[] groups = authTicket.UserData.Split(new char[] { '|' });

            //Create an Identity.
            GenericIdentity id = new GenericIdentity(authTicket.Name, "LdapAuthentication");

            //This principal flows throughout the request.
            GenericPrincipal principal = new GenericPrincipal(id, groups);

            Context.User = principal;
        }
    }
}