using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CDAS
{
    public partial class Main_User : System.Web.UI.MasterPage
    {
        protected void lb_logout_Click(object sender, EventArgs e)
        {
            /*
            if (Session["access_type"] != null)
            {
                Session["access_type"] = null;
            }
            Response.Redirect("login.aspx");
            */
            HttpContext.Current.GetOwinContext().Authentication.SignOut(
                OpenIdConnectAuthenticationDefaults.AuthenticationType,
                CookieAuthenticationDefaults.AuthenticationType);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["access_type"] != null)
            {
                if (!IsPostBack)
                {
                    if (Session["access_type"].ToString() == "ADMIN")
                    {
                        lbl_username.Text = Session["email"].ToString();
                        lbl_username2.Text = Session["email"].ToString();
                    }
                }
            }
            /*
            if (Page.User.Identity.IsAuthenticated)
            {
                lbl_username.Text = Context.User.Identity.Name;
                lbl_username2.Text = Context.User.Identity.Name;

                if (Session["location_desc"] != null)
                {
                    lbl_location.Text = Session["location_desc"].ToString() + " (" + Session["school_code"].ToString() + ")";
                    //lbl_groups.Text = Session["sub_job_description"].ToString();
                    //lbl_version.Text = System.Configuration.ConfigurationManager.AppSettings["version"].ToString();
                }
                else
                {
                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();

                    FormsAuthentication.SignOut();
                    FormsAuthentication.RedirectToLoginPage();
                }
            }
            else
            {
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();

                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
            }
            */
        }
            /*
    protected void Page_Load(object sender, EventArgs e)
    {
       if (Page.User.Identity.IsAuthenticated)
       {
           lbl_username.Text = Context.User.Identity.Name;
           lbl_username2.Text = Context.User.Identity.Name;

           if (Session["location_desc"] != null)
           {
               lbl_location.Text = Session["location_desc"].ToString() + " (" + Session["school_code"].ToString() + ")";
               //lbl_groups.Text = Session["sub_job_description"].ToString();
               //lbl_version.Text = System.Configuration.ConfigurationManager.AppSettings["version"].ToString();
           }
           else
           {
               Session.Clear();
               Session.Abandon();
               Session.RemoveAll();

               FormsAuthentication.SignOut();
               FormsAuthentication.RedirectToLoginPage();
           }

           SetCurrentPage();
       }
       else
       {
           Session.Clear();
           Session.Abandon();
           Session.RemoveAll();

           FormsAuthentication.SignOut();
           FormsAuthentication.RedirectToLoginPage();
       }
       */
        }
    /*
        private void SetCurrentPage()
        {
            var pageName = GetPageName();

            switch (pageName)
            {
                case "default.aspx":
                    home_link.Attributes["class"] = "active";
                    break;
                case "temporary_access.aspx":
                    temp_link.Attributes["class"] = "active";
                    break;
                case "reports.aspx":
                    report_link.Attributes["class"] = "active";
                    break;
                default:
                    break;
            }
        }
    
    }
    */
}