using Microsoft.IdentityModel.Clients.ActiveDirectory;
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

namespace CDAS
{
    public partial class login : System.Web.UI.Page
    {
        
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            //Lets try redirecting to a new login page which then verifies the authentication and then handles the redirection there.
//            if(Request.IsAuthenticated == false)
//            {
//                //Unauthenticated. Prompt for Azure Auth
//                HttpContext.Current.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "login.aspx" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
//                return;
//            }
//            else
//            {
//                //Authenticated. Compare Azure Login Email with email from the DB
//                string email = User.Identity.Name;
                
//                if (email != null)
//                {
//                    try
//                    {
//                        string query = "";
//#if DEBUG
//                        //query = string.Format("select employee_id, surname, first_name, location_code, school_code, location_desc, email_address, emp_group_code from [wrdsb_50_test].[dbo].hd_wrdsb_employee_view where username= '{0}' and home_loc_ind='Y' and activity_code in ('ACTIVE','ONLEAVE')", tb_username.Text.ToUpper().Trim());

//                        //MOHRBEK Janet Metcalfe(725)
//                        //HARRISDL Harris Donna(371) *
//                        //LIPSKIS

//                        //test purposes
                        
//                        if (email == "terrel_stephen@wrdsb.ca" || email == "vanitha_raju@wrdsb.ca" || email == "robert_duke@wrdsb.ca" || email == "sunny_ng@wrdsb.ca")
//                        {
//                            query = string.Format("select employee_id, surname, first_name, location_code, school_code, location_desc, email_address, emp_group_code from [wrdsb_50_test].[dbo].hd_wrdsb_employee_view where username='{0}' and home_loc_ind='Y' and activity_code in ('ACTIVE','ONLEAVE')", "HARRISDL");
//                        }
//                        else
//                        {
//                            //query = string.Format("select employee_id, surname, first_name, location_code, school_code, location_desc, email_address, emp_group_code from [wrdsb_50_test].[dbo].hd_wrdsb_employee_view where email_address= '{0}' and home_loc_ind='Y' and activity_code in ('ACTIVE','ONLEAVE')", email);
//                            throw new Exception("Invalid Permission please contact administrator");
//                        }
                        
//                        //query = string.Format("select employee_id, surname, first_name, location_code, school_code, location_desc, email_address, emp_group_code from [wrdsb_50_test].[dbo].hd_wrdsb_employee_view where username='{0}' and home_loc_ind='Y' and activity_code in ('ACTIVE','ONLEAVE')", "HARRISDL");
//                        //query = string.Format("select employee_id, surname, first_name, location_code, school_code, location_desc, email_address, emp_group_code from [wrdsb_50_test].[dbo].hd_wrdsb_employee_view where email_address= '{0}' and home_loc_ind='Y' and activity_code in ('ACTIVE','ONLEAVE')", email);
//#else
///*
//                    query = string.Format("select employee_id, surname, first_name, location_code, email_address, emp_group_code " +
//                        "from [wrdsb].[dbo].hd_wrdsb_employee_view " +
//                        "where username= '{0}' and home_loc_ind='Y' and activity_code in ('ACTIVE','ONLEAVE')", tb_username.Text.ToUpper().Trim());
//*/
//                            query = string.Format("select employee_id, surname, first_name, location_code, school_code, location_desc, email_address, emp_group_code from [wrdsb_50_test].[dbo].hd_wrdsb_employee_view where email_address= '{0}' and home_loc_ind='Y' and activity_code in ('ACTIVE','ONLEAVE')", email);
//#endif

//                        string connString = ConfigurationManager.ConnectionStrings["DB_SCS"].ConnectionString;
//                        SqlConnection con = new SqlConnection(connString);
//                        SqlCommand cmd = new SqlCommand(query, con);
//                        con.Open();
//                        SqlDataReader reader = cmd.ExecuteReader();

//                        if (reader.HasRows)
//                        {
//                            while (reader.Read())
//                            {
//                                Session["ein"] = reader["employee_id"].ToString().Trim();
//                                Session["surname"] = reader["surname"].ToString().Trim();
//                                Session["firstname"] = reader["first_name"].ToString().Trim();
//                                Session["location_code"] = reader["location_code"].ToString().Trim();
//                                Session["school_code"] = reader["school_code"].ToString().Trim();
//                                Session["location_desc"] = reader["location_desc"].ToString().Trim();
//                                Session["email"] = reader["email_address"].ToString().ToLower().Trim();
//                                Session["group_code"] = reader["emp_group_code"].ToString().Trim();
//                                Session["access_type"] = "ADMIN";
//                            }
//                        }
//                        else
//                        {
//                            throw new Exception("The entered email address: " + email + " cannot be found. Please contact administrator for assistance.");
//                        }
//                        reader.Close();
//                        con.Close();

//                        //If location code = 370 or 371 then ADMIN
//                        //else USER
//                        //USER = School Administrator
//                        //Elementary: Employee Group Code: 5107A, PRINSUPE 
//                        //Secondary: Employee Group Code: 5108A, PRINSUPS

//                        //For Debug Purposes 6691 has been added to allow temp access as a User

//                        if (Session["access_type"].ToString() == "ADMIN" || Session["access_type"].ToString() == "USER")
//                        {
//                            //Create the ticket, and add the groups.
//                            //bool isCookiePersistent = cb_persist.Checked;
//                            bool isCookiePersistent = false;
//                            System.Web.Configuration.AuthenticationSection authSection = (System.Web.Configuration.AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");

//                            System.Web.Configuration.FormsAuthenticationConfiguration
//                                formsAuthenticationSection = authSection.Forms;

//                            DateTime now = DateTime.Now;

//                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, User.Identity.Name,
//                                now, now.Add(formsAuthenticationSection.Timeout), isCookiePersistent, "groups");

//                            //Encrypt the ticket.
//                            String encryptedTicket = FormsAuthentication.Encrypt(authTicket);

//                            //Create a cookie, and then add the encrypted ticket to the cookie as data.
//                            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

//                            if (true == isCookiePersistent)
//                                authCookie.Expires = authTicket.Expiration;

//                            //Add the cookie to the outgoing cookies collection.
//                            Response.Cookies.Add(authCookie);

//                            //You can redirect now.
//                            //Session["authenticated"] = true;
//                            FormsAuthentication.SetAuthCookie(User.Identity.Name, true);
//                            if (Session["access_type"].ToString() == "ADMIN")
//                                Response.Redirect("Maintenance.aspx");
//                            else if (Session["access_type"].ToString() == "NONE")
//                                throw new Exception("Insufficient Access. Please contact administrator for assistance.");
//                        }

//                    }
//                    catch(Exception ex)
//                    {
//                        lbl_message.Text = ex.Message;
//                        lbl_message.CssClass = "red1";
//                    }
//                    /*
//                    if(email == null || email == "")
//                    {
//                        lblMessage.Text = "Null value or blank";
//                    }
//                    else
//                    {
//                        lblMessage.Text = email;
//                    }
                    
//                    lblMessage.CssClass = "red1";
//                    */
//                }
//                else
//                {
//                    //Response.Redirect("sublogin.aspx");
//                    //lblMessage.Text = HttpContext.Current.User.Identity.ToString();
//                    //lblMessage.CssClass = "red1";
//                }
//            }

//        }
                

        protected void btn_login_Click(object sender, EventArgs e)
        {
            //Response.Redirect("login.aspx");
            //Sunny DEBUG
            string email = tb_username.Text.Trim().ToLower();
            string pass = tb_password.Text.Trim();
            try
            {
                if (email == "sunny_ng@wrdsb.ca" && pass == "monopoloy10!")
                {
                    Session["access_type"] = "ADMIN";                    
                }
                else
                {
                    Session["access_type"] = "N/A";
                }
                
                if (Session["access_type"].ToString() == "ADMIN")
                {
                    Response.Redirect("Maintenance.aspx");
                }
                else
                {
                    throw new Exception("Invalid User details. Please Try Again");
                }
            }
            catch (Exception ex) 
            {
                lbl_message.Text = ex.Message;
                lbl_message.ForeColor = System.Drawing.Color.Red;
            }

        }

    }
}