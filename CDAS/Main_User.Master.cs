using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Security_Card_System
{
    public partial class Main_User : System.Web.UI.MasterPage
    {
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