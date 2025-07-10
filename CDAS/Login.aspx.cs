using Microsoft.IdentityModel;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Security.Claims;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Collections;
using Azure.Core;

namespace CDAS
{
    public partial class LoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string query = "";
            if (!Request.IsAuthenticated && !Request.Path.Contains("signin-oidc"))
            {
                Response.Redirect("Maintenance.aspx");
            }
            else
            {
                Response.Redirect("default.aspx");
            }
            
        }
        
        
    }
}