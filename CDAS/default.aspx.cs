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
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string query = "";
            if (!Request.IsAuthenticated && !Request.Path.Contains("signin-oidc"))
            {
                //Not Auth relogin               
                HttpContext.Current.GetOwinContext().Authentication.Challenge(
                   new AuthenticationProperties { RedirectUri = "default.aspx" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
                return;

            }
            else
            {
                SignIn(sender, e);
            }
            
        }
        
        protected void SignIn(object sender, EventArgs e)
        {
            string email = User.Identity.Name;
            if (email != null)
            {
                try
                {
                string query = "";

                    /*test
                    if (email == "terrel_stephen@wrdsb.ca" || email == "robert_duke@wrdsb.ca" || email == "sunny_ng@wrdsb.ca")
                    {
                        query = string.Format("select employee_id, last_name, first_name, location_code, site_number, email_address, emp_group_code, ad_username from [CDAS].[CDDBA].[HD_SF_EMPLOYEE] where ad_username='{0}'", "ZHAOLL");
                    }
                    else
                    {
                        query = string.Format("select employee_id, last_name, first_name, location_code, site_number, email_address, emp_group_code, ad_username from [CDAS].[CDDBA].[HD_SF_EMPLOYEE] where email_address='{0}'", email);
                    }
                    */
                query = string.Format("select employee_id, username, surname, first_name, location_code, school_code, location_desc, email_address, emp_group_code from [wrdsb_50_test].[dbo].hd_wrdsb_employee_view where email_address= '{0}' and home_loc_ind='Y' and activity_code in ('ACTIVE','ONLEAVE')", email);
                string connString = ConfigurationManager.ConnectionStrings["DB_CDAS"].ConnectionString;
                SqlConnection con = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Session["access_type"] = "NONE";
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Session["ein"] = reader["employee_id"].ToString().Trim();
                        Session["username"] = reader["username"].ToString().Trim();
                        Session["surname"] = reader["surname"].ToString().Trim();
                        Session["firstname"] = reader["first_name"].ToString().Trim();
                        Session["location_code"] = reader["location_code"].ToString().Trim();
                        Session["email"] = reader["email_address"].ToString().ToLower().Trim();
                        Session["emp_group_code"] = reader["emp_group_code"].ToString().Trim();
                        Session["school_code"] = reader["school_code"].ToString().Trim();
                        Session["location_desc"] = reader["location_desc"].ToString().Trim();

                    }
                    Session["access_type"] = "ADMIN";
                }
                else
                {
                    throw new Exception("The entered email address: " + email + " cannot be found. Please contact administrator for assistance.");
                }
                reader.Close();
                con.Close();

                if (Session["access_type"].ToString() == "ADMIN")
                {
                    //Create the ticket, and add the groups.
                    //bool isCookiePersistent = cb_persist.Checked;
                    bool isCookiePersistent = false;
                    System.Web.Configuration.AuthenticationSection authSection = (System.Web.Configuration.AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");

                    System.Web.Configuration.FormsAuthenticationConfiguration
                        formsAuthenticationSection = authSection.Forms;

                    DateTime now = DateTime.Now;

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, User.Identity.Name,
                        now, now.Add(formsAuthenticationSection.Timeout), isCookiePersistent, "groups");

                    //Encrypt the ticket.
                    String encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    //Create a cookie, and then add the encrypted ticket to the cookie as data.
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                    if (true == isCookiePersistent)
                        authCookie.Expires = authTicket.Expiration;

                    //Add the cookie to the outgoing cookies collection.
                    Response.Cookies.Add(authCookie);

                    //You can redirect now.
                    //Session["authenticated"] = true;
                    FormsAuthentication.SetAuthCookie(User.Identity.Name, true);

                    if (Session["access_type"].ToString() == "ADMIN")
                        Response.Redirect("Maintenance.aspx");
                    else if (Session["access_type"].ToString() == "NONE")
                        throw new Exception("Insufficient Access. Please contact administrator for assistance.");
                }

                }
                catch (Exception ex)
                {
                    lbl_message.Text = ex.Message;
                    lbl_message.CssClass = "red1";
                }


            }
        }

       

        protected void btn_login_maint_Click(object sender, EventArgs e)
        {
            //Response.Redirect("login.aspx");
            //Sunny DEBUG

            //string email = tb_username.Text.Trim().ToLower();
            //string pass = tb_password.Text.Trim();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            SignIn(sender, e);
        }
    }
}