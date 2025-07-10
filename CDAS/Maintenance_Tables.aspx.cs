using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Services.Description;
using System.Collections;
using Microsoft.Owin.Security.OpenIdConnect;

namespace CDAS
{
    public partial class Maintenance_Tables : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated && !Request.Path.Contains("signin-oidc"))
            {
                Response.Redirect("default.aspx", false);
                return;

            }
            else
            {
                if (!IsPostBack)
                {
                    BindGrids();
                }
            }
        }

        private void BindListView(string query, string sort)
        {
            try
            {
                string connstring = ConfigurationManager.ConnectionStrings["DB_CDAS"].ConnectionString;
                SqlConnection conn = new SqlConnection(connstring);
                SqlCommand cmd = new SqlCommand(query + sort, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();
            }
            catch(Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        bool Check_For_Text(List<TextBox> list_textboxes)
        {
            bool valid = true;

            foreach(TextBox text_box in list_textboxes) 
            {
                string text = text_box.Text;
                if (!Regex.IsMatch(text, @"^\d+$"))
                {
                    valid = false;
                    return valid;
                }
            }
            return valid;
        }

        protected void gv_location_type_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gv_location_type.EditIndex = e.NewEditIndex;
            BindGrids();

            lbl_error_message.Text = "";
        }

        protected void gv_location_type_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string connstring = ConfigurationManager.ConnectionStrings["DB_CDAS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand cmd = null;


            //Have to use a seperate method to get values from grid view dynamically
            //GridViewRow row = gv_location_type.Rows[e.RowIndex];
            string code = gv_location_type.DataKeys[e.RowIndex].Value.ToString();
            string full_name = ((TextBox)gv_location_type.Rows[e.RowIndex].Cells[2].Controls[0]).Text.Trim();
            string abbrv_name = ((TextBox)gv_location_type.Rows[e.RowIndex].Cells[3].Controls[0]).Text.Trim();
            DropDownList ddl_status_flag = (DropDownList)gv_location_type.Rows[e.RowIndex].Cells[4].FindControl("ddl_status_flag") as DropDownList;
            string status_flag = ddl_status_flag.SelectedValue;
            //string changed_date = ((TextBox)gv_location_type.Rows[e.RowIndex].Cells[6].Controls[0]).Text.Trim();

            

            try
            {
                if(string.IsNullOrEmpty(full_name) || string.IsNullOrEmpty(abbrv_name) || string.IsNullOrEmpty(status_flag))
                {
                    throw new Exception("Values must not be empty");
                }
                if (full_name.Length > 70)
                {
                    throw new Exception("Full Name is too long");
                }
                if (abbrv_name.Length > 16)
                {
                    throw new Exception("Abbreviated Name is too long");
                }

                if (status_flag.Length != 1)
                {
                    throw new Exception("Status Flag can only be A (Active) or I (Inactive)");
                }

                if (status_flag != "A" && status_flag != "I")
                {
                    throw new Exception("Status Flag can only be A (Active) or I (Inactive)");
                }
                /*
                DateTime valid_date;
                if (DateTime.TryParse(changed_date, out valid_date))
                {
                    valid_date = DateTime.Now;
                }
                */
                using (conn = new SqlConnection(connstring))
                {
                    string query = "UPDATE [CDAS].[CDDBA].[EC_LOCATION_TYPE] SET full_name=@fullname, abbrv_name=@abbrvname, status_flag=@statusflag," +
                        " changed_by=@changedby, changed_date=@changeddate WHERE CODE = @code";

                    using (cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@code", code);
                        cmd.Parameters.AddWithValue("@fullname", full_name);
                        cmd.Parameters.AddWithValue("@abbrvname", abbrv_name);
                        cmd.Parameters.AddWithValue("@statusflag", status_flag);
                        cmd.Parameters.AddWithValue("@changedby", Session["username"].ToString());
                        cmd.Parameters.AddWithValue("@changeddate", DateTime.Now);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                gv_location_type.EditIndex = -1;
                BindGrids();
            }
            catch(Exception ex)
            {
                Response.Write(ex.Message.ToString());
                lbl_error_message.Text = ex.Message;
            }
        }

        protected void BindGrids()
        {
            //Bind each grid
            
            string connstring = ConfigurationManager.ConnectionStrings["DB_CDAS"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [CDAS].[CDDBA].[EC_LOCATION_TYPE] where status_flag = 'A' order by CODE ASC", connstring))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    gv_location_type.DataSource = dataTable;
                    gv_location_type.DataBind();
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [CDAS].[CDDBA].[EC_SCHOOL_TYPE] where status_flag = 'A'  order by CODE ASC", connstring))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    gv_school_type.DataSource = dataTable;
                    gv_school_type.DataBind();
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [CDAS].[CDDBA].[EC_PANEL] where status_flag = 'A'  order by PANEL ASC", connstring))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    gv_panel.DataSource = dataTable;
                    gv_panel.DataBind();
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [CDAS].[CDDBA].[EC_ADMIN_AREA] where status_flag = 'A'  order by CODE ASC", connstring))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    gv_admin_area.DataSource = dataTable;
                    gv_admin_area.DataBind();
                }
            }

            
            lbl_error_message.Text = "";
        }

        protected void gv_location_type_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gv_location_type.EditIndex = -1;
            BindGrids();
            lbl_error_message.Text = "";
        }

        protected void cb_location_CheckedChanged(object sender, EventArgs e)
        {
            if(gv_location_type.Visible == true)
            {
                gv_location_type.Visible = false;
                gv_location_type.EditIndex = -1;
                BindGrids();
            }
            else
            {
                gv_location_type.Visible = true;
            }
        }

        protected void gv_school_type_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gv_school_type.EditIndex = e.NewEditIndex;
            BindGrids();
        }

        protected void gv_school_type_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string connstring = ConfigurationManager.ConnectionStrings["DB_CDAS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand cmd = null;

            string code = gv_school_type.DataKeys[e.RowIndex].Value.ToString();
            string full_name = ((TextBox)gv_school_type.Rows[e.RowIndex].Cells[2].Controls[0]).Text.Trim();
            string abbrv_name = ((TextBox)gv_school_type.Rows[e.RowIndex].Cells[3].Controls[0]).Text.Trim();
            DropDownList ddl_status_flag = (DropDownList)gv_school_type.Rows[e.RowIndex].Cells[4].FindControl("ddl_status_flag") as DropDownList;
            string status_flag = ddl_status_flag.SelectedValue;

            try
            {
                if (string.IsNullOrEmpty(full_name) || string.IsNullOrEmpty(abbrv_name) || string.IsNullOrEmpty(status_flag))
                {
                    throw new Exception("Values must not be null");
                }
                if (full_name.Length > 70)
                {
                    throw new Exception("Full Name is too long");
                }
                if (abbrv_name.Length > 16)
                {
                    throw new Exception("Abbreviated Name is too long");
                }

                if (status_flag.Length != 1)
                {
                    throw new Exception("Status Flag can only be A (Active) or I (Inactive)");
                }

                if (status_flag != "A" && status_flag != "I")
                {
                    throw new Exception("Status Flag can only be A (Active) or I (Inactive)");
                }
                using (conn = new SqlConnection(connstring))
                {
                    string query = "UPDATE [CDAS].[CDDBA].[EC_SCHOOL_TYPE] SET full_name=@fullname, abbrv_name=@abbrvname, status_flag=@statusflag," +
                        " changed_by=@changedby, changed_date=@changeddate WHERE CODE = @code";

                    using (cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@code", code);
                        cmd.Parameters.AddWithValue("@fullname", full_name);
                        cmd.Parameters.AddWithValue("@abbrvname", abbrv_name);
                        cmd.Parameters.AddWithValue("@statusflag", status_flag);
                        cmd.Parameters.AddWithValue("@changedby", Session["username"].ToString());
                        cmd.Parameters.AddWithValue("@changeddate", DateTime.Now);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                gv_school_type.EditIndex = -1;
                BindGrids();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
                lbl_error_message.Text = ex.Message;
            }
        }

        protected void gv_school_type_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gv_school_type.EditIndex = -1;
            BindGrids();
            lbl_error_message.Text = "";
        }

        protected void cb_school_type_CheckedChanged(object sender, EventArgs e)
        {
            if (gv_school_type.Visible == true)
            {
                gv_school_type.Visible = false;
                gv_school_type.EditIndex = -1;
                BindGrids();
            }
            else
            {
                gv_school_type.Visible = true;
            }
        }

        protected void cb_panel_CheckedChanged(object sender, EventArgs e)
        {
            if (gv_panel.Visible == true)
            {
                gv_panel.Visible = false;
                gv_panel.EditIndex = -1;
                BindGrids();
            }
            else
            {
                gv_panel.Visible = true;
            }
        }

        protected void gv_panel_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gv_panel.EditIndex = e.NewEditIndex;
            BindGrids();
        }

        protected void gv_panel_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string connstring = ConfigurationManager.ConnectionStrings["DB_CDAS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand cmd = null;

            string panel = gv_panel.DataKeys[e.RowIndex].Value.ToString();
            string full_name = ((TextBox)gv_panel.Rows[e.RowIndex].Cells[2].Controls[0]).Text.Trim();
            string abbrv_name = ((TextBox)gv_panel.Rows[e.RowIndex].Cells[3].Controls[0]).Text.Trim();
            DropDownList ddl_status_flag = (DropDownList)gv_panel.Rows[e.RowIndex].Cells[4].FindControl("ddl_status_flag") as DropDownList;
            string status_flag = ddl_status_flag.SelectedValue;


            try
            {
                if (string.IsNullOrEmpty(full_name) || string.IsNullOrEmpty(abbrv_name) || string.IsNullOrEmpty(status_flag))
                {
                    throw new Exception("Values must not be null");
                }
                if (full_name.Length > 70)
                {
                    throw new Exception("Full Name is too long");
                }
                if (abbrv_name.Length > 16)
                {
                    throw new Exception("Abbreviated Name is too long");
                }

                if (status_flag.Length != 1)
                {
                    throw new Exception("Status Flag can only be A (Active) or I (Inactive)");
                }

                if (status_flag != "A" && status_flag != "I")
                {
                    throw new Exception("Status Flag can only be A (Active) or I (Inactive)");
                }
                using (conn = new SqlConnection(connstring))
                {
                    string query = "UPDATE [CDAS].[CDDBA].[EC_PANEL] SET full_name=@fullname, abbrv_name=@abbrvname, status_flag=@statusflag," +
                        " changed_by=@changedby, changed_date=@changeddate WHERE PANEL = @panel";

                    using (cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@panel", panel);
                        cmd.Parameters.AddWithValue("@fullname", full_name);
                        cmd.Parameters.AddWithValue("@abbrvname", abbrv_name);
                        cmd.Parameters.AddWithValue("@statusflag", status_flag);
                        cmd.Parameters.AddWithValue("@changedby", Session["username"].ToString());
                        cmd.Parameters.AddWithValue("@changeddate", DateTime.Now);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                lbl_error_message.Text = "";
                gv_panel.EditIndex = -1;
                BindGrids();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
                lbl_error_message.Text = ex.Message;
            }
        }

        protected void gv_panel_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gv_panel.EditIndex = -1;
            BindGrids();
            lbl_error_message.Text = "";
        }

        protected void cb_admin_area_CheckedChanged(object sender, EventArgs e)
        {
            if (gv_admin_area.Visible == true)
            {
                gv_admin_area.Visible = false;
                gv_admin_area.EditIndex = -1;
                BindGrids();
            }
            else
            {
                gv_admin_area.Visible = true;
            }
        }

        protected void gv_admin_area_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gv_admin_area.EditIndex = e.NewEditIndex;
            BindGrids();
        }

        protected void gv_admin_area_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string connstring = ConfigurationManager.ConnectionStrings["DB_CDAS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand cmd = null;

            string code = gv_admin_area.DataKeys[e.RowIndex].Value.ToString();
            string full_name = ((TextBox)gv_admin_area.Rows[e.RowIndex].Cells[2].Controls[0]).Text.Trim();
            string abbrv_name = ((TextBox)gv_admin_area.Rows[e.RowIndex].Cells[3].Controls[0]).Text.Trim();
            DropDownList ddl_status_flag = (DropDownList)gv_admin_area.Rows[e.RowIndex].Cells[4].FindControl("ddl_status_flag");
            string status_flag = ddl_status_flag.SelectedValue;
            string employee_id = ((TextBox)gv_admin_area.Rows[e.RowIndex].Cells[7].Controls[0]).Text.Trim();
            
            try
            {
                if (string.IsNullOrEmpty(full_name) || string.IsNullOrEmpty(abbrv_name) || string.IsNullOrEmpty(status_flag))
                {
                    throw new Exception("Values must not be null");
                }
                if (full_name.Length > 70)
                {
                    throw new Exception("Full Name is too long");
                }
                if (abbrv_name.Length > 16)
                {
                    throw new Exception("Abbreviated Name is too long");
                }

                if (status_flag.Length != 1)
                {
                    throw new Exception("Status Flag can only be A (Active) or I (Inactive)");
                }

                if (status_flag != "A" && status_flag != "I")
                {
                    throw new Exception("Status Flag can only be A (Active) or I (Inactive)");
                }

                if (employee_id.Length > 9)
                {
                    throw new Exception("Employee ID is too long");
                }



                using (conn = new SqlConnection(connstring))
                {
                    string query = "UPDATE [CDAS].[CDDBA].[EC_ADMIN_AREA] SET full_name=@fullname, abbrv_name=@abbrvname, status_flag=@statusflag," +
                        " changed_by=@changedby, changed_date=@changeddate, employee_id=@employeeid WHERE CODE = @code";

                    using (cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@code", code);
                        cmd.Parameters.AddWithValue("@fullname", full_name);
                        cmd.Parameters.AddWithValue("@abbrvname", abbrv_name);
                        cmd.Parameters.AddWithValue("@statusflag", status_flag);
                        cmd.Parameters.AddWithValue("@changedby", Session["username"].ToString());
                        cmd.Parameters.AddWithValue("@changeddate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@employeeid", employee_id);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                gv_admin_area.EditIndex = -1;
                BindGrids();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
                lbl_error_message.Text = ex.Message;
            }
        }

        protected void gv_admin_area_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gv_admin_area.EditIndex = -1;
            BindGrids();
            lbl_error_message.Text = "";
        }

        protected void ddl_maintenance_tables_SelectedIndexChanged(object sender, EventArgs e)
        {
            List <Control> insert_controls = new List<Control> {lbl_insert, lbl_insert_code, tb_insert_code, lbl_insert_full_name , tb_insert_full_name, lbl_insert_abbrv_name, tb_insert_abbrv_name, lbl_insert_status, ddl_insert_status}; 
            List<Label> admin_label = new List<Label> { lbl_insert_employee_ID};
            List<TextBox> admin_textbox = new List<TextBox> { tb_insert_employee_ID };

            //Each time the drop down is used clear the main tables. We can assume the null value is just a full clear
            GridClear();

            foreach (Label label in admin_label)
            {
                label.Visible = false;
            }
            foreach (TextBox text in admin_textbox)
            {
                text.Visible = false;
            }
            lbl_error_message.Text = "";
            tb_insert_panel.Text = "";
            tb_insert_employee_ID.Text = "";
            ddl_insert_status.SelectedIndex = 0;

            btn_insert_maint_table.Visible = true;
            btn_clear.Visible = true;
            if (ddl_maintenance_tables.SelectedValue == "LocationType")
            {
                gv_location_type.Visible = true;
                tb_insert_panel.Visible = false;
                lbl_insert_panel.Visible = false;
                foreach(Control control in insert_controls)
                {
                    control.Visible = true;
                }
                lbl_insert_code.Visible = true;
                tb_insert_code.Visible = true;
                gv_location_type.PageIndex = 0;
            }
            else if (ddl_maintenance_tables.SelectedValue == "SchoolType")
            {
                foreach (Control control in insert_controls)
                {
                    control.Visible = true;
                }
                gv_school_type.Visible = true;
                tb_insert_panel.Visible = false;
                lbl_insert_panel.Visible = false;
                lbl_insert_code.Visible = true;
                tb_insert_code.Visible = true;
                gv_school_type.PageIndex = 0;
            }
            else if (ddl_maintenance_tables.SelectedValue == "Panel")
            {
                foreach (Control control in insert_controls)
                {
                    control.Visible = true;
                }
                gv_panel.Visible = true;
                gv_panel.PageIndex = 0;
                tb_insert_panel.Visible = true;
                lbl_insert_panel.Visible = true;
                lbl_insert_code.Visible = false;
                tb_insert_code.Visible = false;
            }
            else if (ddl_maintenance_tables.SelectedValue == "AdminArea")
            {
                foreach (Control control in insert_controls)
                {
                    control.Visible = true;
                }

                gv_admin_area.Visible = true;
                foreach (Label label in admin_label)
                {
                    label.Visible = true;
                }
                foreach (TextBox text in admin_textbox)
                {
                    text.Visible = true;
                }
                gv_admin_area.PageIndex = 0;
                tb_insert_panel.Visible = false;
                lbl_insert_panel.Visible = false;
                lbl_insert_code.Visible = true;
                tb_insert_code.Visible = true;
            }
            else
            {
                foreach (Control control in insert_controls)
                {
                    control.Visible = false;
                }
                gv_admin_area.Visible = false;
                foreach (Label label in admin_label)
                {
                    label.Visible = false;
                }
                foreach (TextBox text in admin_textbox)
                {
                    text.Visible = false;
                }
                tb_insert_panel.Visible = false;
                lbl_insert_panel.Visible = false;
                btn_insert_maint_table.Visible = false;
                btn_clear.Visible = false;
            }

            lbl_error_message.Text = "";
            Insert_Clear();
            BindGrids();
        }
        
        protected void GridClear()
        {
            List<GridView> gridview_selection = new List<GridView> { gv_location_type, gv_school_type, gv_panel, gv_admin_area };

            foreach (GridView grid in gridview_selection)
            {
                grid.Visible = false;
                if (grid.EditIndex != -1)
                {
                    grid.EditIndex = -1;
                }
            }
            BindGrids();
        }

        protected void btn_insert_maint_table_Click(object sender, EventArgs e)
        {
            string connstring = ConfigurationManager.ConnectionStrings["DB_CDAS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand cmd = null;
            SqlDataReader reader;
            string query, selected_table;
            List<TextBox> admin_textbox = new List<TextBox> {tb_insert_employee_ID, tb_insert_full_name, tb_insert_abbrv_name, tb_insert_code };
            TextBox panel_textbox = tb_insert_panel;

            try
            {

                
                //Refer to dropdownlist
                if(ddl_maintenance_tables.SelectedValue == "Panel")
                {
                    if (string.IsNullOrEmpty(tb_insert_panel.Text))
                    {
                        throw new Exception("Panel value cannot be empty");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(tb_insert_code.Text))
                    {
                        throw new Exception("Code value cannot be empty");
                    }
                }

                if (ddl_maintenance_tables.SelectedValue == "AdminArea")
                {
                    string verify_code = "SELECT COUNT(*) FROM [CDAS].[CDDBA].[EC_ADMIN_AREA] WHERE CODE = @code";
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(verify_code, conn))
                    {
                        command.Parameters.AddWithValue("@code", tb_insert_code.Text.Trim());
                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            throw new Exception("Code already in use.");
                        }
                    }
                    conn.Close();

                    foreach (TextBox text in admin_textbox)
                    {
                        if (string.IsNullOrEmpty(text.Text))
                        {
                            throw new Exception("All areas must have a value");
                        }
                    }

                    if (ddl_insert_status.Text.Length != 1)
                    {
                        throw new Exception("Status Flag can only be A (Active) or I (Inactive)");
                    }

                    if (ddl_insert_status.Text != "A" && ddl_insert_status.Text != "I")
                    {
                        throw new Exception("Status Flag can only be A (Active) or I (Inactive)");
                    }

                    query = "INSERT INTO [CDAS].[CDDBA].[EC_ADMIN_AREA] (code, full_name, abbrv_name, status_flag, changed_by, changed_date, employee_id) " +
                                "VALUES (@code, @full_name, @abbrv_name, @status_flag, @changed_by, @changed_date, @employee_ID)";

                    using (cmd = new SqlCommand(query, conn))
                    {
                        #region Insert Parameters
                        cmd.Parameters.AddWithValue("@code", tb_insert_code.Text.Trim());
                        cmd.Parameters.AddWithValue("@full_name", tb_insert_full_name.Text.Trim());
                        cmd.Parameters.AddWithValue("@abbrv_name", tb_insert_abbrv_name.Text.Trim());
                        cmd.Parameters.AddWithValue("@status_flag", ddl_insert_status.Text.Trim());
                        cmd.Parameters.AddWithValue("@changed_by", Session["username"].ToString());
                        cmd.Parameters.AddWithValue("@changed_date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@employee_ID", tb_insert_employee_ID.Text.Trim());
                        #endregion

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                else
                {
                    string verify_code;
                    if (ddl_maintenance_tables.SelectedValue == "SchoolType")
                    {
                        selected_table = "[CDAS].[CDDBA].[EC_SCHOOL_TYPE]";
                        verify_code = "SELECT COUNT(*) FROM [CDAS].[CDDBA].[EC_SCHOOL_TYPE] WHERE CODE = @code";
                    }
                    else if(ddl_maintenance_tables.SelectedValue == "Panel")
                    {
                        selected_table = "[CDAS].[CDDBA].[EC_PANEL]";
                        verify_code = "SELECT COUNT(*) FROM [CDAS].[CDDBA].[EC_PANEL] WHERE PANEL = @panel";
                    }
                    else if(ddl_maintenance_tables.SelectedValue == "LocationType")
                    {
                        selected_table = "[CDAS].[CDDBA].[EC_LOCATION_TYPE]";
                        verify_code = "SELECT COUNT(*) FROM [CDAS].[CDDBA].[EC_LOCATION_TYPE] WHERE CODE = @code";
                    }
                    else
                    {
                        throw new Exception("Invalid Table Selection");
                    }

                    //string verify_code = "SELECT COUNT(*) FROM table=@table WHERE CODE = @code";
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(verify_code, conn))
                    {
                        command.Parameters.AddWithValue("@code", tb_insert_code.Text.Trim());
                        command.Parameters.AddWithValue("@panel", tb_insert_panel.Text.Trim());
                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            throw new Exception("Code already in use.");
                        }
                    }
                    conn.Close();

                    List<TextBox> textbox = new List<TextBox> {tb_insert_full_name, tb_insert_abbrv_name};
                    foreach (TextBox text in textbox)
                    {
                        if (string.IsNullOrEmpty(text.Text))
                        {
                            throw new Exception("Values must not be empty or null");
                        }
                    }

                    if (ddl_insert_status.Text.Length != 1)
                    {
                        throw new Exception("Status Flag can only be A (Active) or I (Inactive)");
                    }

                    if (ddl_insert_status.Text != "A" && ddl_insert_status.Text != "I")
                    {
                        throw new Exception("Status Flag can only be A (Active) or I (Inactive)");
                    }

                    

                    if (ddl_maintenance_tables.SelectedValue == "Panel")
                    {
                        query = "INSERT INTO [CDAS].[CDDBA].[EC_PANEL] (panel, full_name, abbrv_name, status_flag, changed_by, changed_date) " +
                                "VALUES (@panel, @full_name, @abbrv_name, @status_flag, @changed_by, @changed_date)";

                        using (cmd = new SqlCommand(query, conn))
                        {
                            #region Insert Parameters
                            cmd.Parameters.AddWithValue("@panel", tb_insert_panel.Text.Trim());
                            cmd.Parameters.AddWithValue("@full_name", tb_insert_full_name.Text.Trim());
                            cmd.Parameters.AddWithValue("@abbrv_name", tb_insert_abbrv_name.Text.Trim());
                            cmd.Parameters.AddWithValue("@status_flag", ddl_insert_status.Text.Trim());
                            cmd.Parameters.AddWithValue("@changed_by", Session["username"].ToString());
                            cmd.Parameters.AddWithValue("@changed_date", DateTime.Now);
                            #endregion

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                    else
                    {
                        //Insert into variable table dosnt work it seems. Note this.
                        if(ddl_maintenance_tables.SelectedValue == "SchoolType")
                        {
                            query = "INSERT INTO [CDAS].[CDDBA].[EC_SCHOOL_TYPE] (code, full_name, abbrv_name, status_flag, changed_by, changed_date) " +
                                "VALUES (@code, @full_name, @abbrv_name, @status_flag, @changed_by, @changed_date)";
                        }
                        else
                        {
                            query = "INSERT INTO [CDAS].[CDDBA].[EC_LOCATION_TYPE] (code, full_name, abbrv_name, status_flag, changed_by, changed_date) " +
                                "VALUES (@code, @full_name, @abbrv_name, @status_flag, @changed_by, @changed_date)";
                        }

                        using (cmd = new SqlCommand(query, conn))
                        {
                            #region Insert Parameters
                            cmd.Parameters.AddWithValue("@code", tb_insert_code.Text.Trim());
                            cmd.Parameters.AddWithValue("@full_name", tb_insert_full_name.Text.Trim());
                            cmd.Parameters.AddWithValue("@abbrv_name", tb_insert_abbrv_name.Text.Trim());
                            cmd.Parameters.AddWithValue("@status_flag", ddl_insert_status.Text.Trim());
                            cmd.Parameters.AddWithValue("@changed_by", Session["username"].ToString());
                            cmd.Parameters.AddWithValue("@changed_date", DateTime.Now);
                            #endregion

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }

                BindGrids();
                foreach(TextBox text in admin_textbox)
                {
                    text.Text = "";                    
                }
                lbl_error_message.ForeColor = System.Drawing.Color.Black;
                lbl_error_message.Text = "Create Successful!";

                //Clear all
                Insert_Clear();
            }
            catch (Exception ex) 
            {
                lbl_error_message.ForeColor = System.Drawing.Color.Red;
                lbl_error_message.Text = ex.Message;
            }
            
        }

        protected void Insert_Clear()
        {
            List<TextBox> insert_parameters = new List<TextBox> { tb_insert_code, tb_insert_panel, tb_insert_full_name, tb_insert_abbrv_name, tb_insert_employee_ID };

            foreach(TextBox text in  insert_parameters) 
            {
                text.Text = "";
            }
            ddl_insert_status.SelectedIndex = 0;
        }

        protected void gv_location_type_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_location_type.PageIndex = e.NewPageIndex;
            BindGrids();
        }

        protected void gv_school_type_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_school_type.PageIndex = e.NewPageIndex;
            BindGrids();
        }

        protected void gv_panel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_panel.PageIndex = e.NewPageIndex;
            BindGrids();
        }

        protected void gv_admin_area_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_admin_area.PageIndex = e.NewPageIndex;
            BindGrids();
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            Insert_Clear();
        }

        protected void gv_location_type_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /*
            if(e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                foreach (Control control in e.Row.Cells[0].Controls)
                {
                    if(control is LinkButton button && button.CommandName == "Update")
                    {
                        button.Attributes["Class"] = "gridview-update-btn";
                        break;
                    }
                }

                foreach(TableCell cell in e.Row.Cells)
                {
                    foreach (Control control in cell.Controls)
                    {
                        if(control is TextBox text)
                        {
                            text.Attributes["onkeydown"] = "return handleEnterKey(event);";
                        }
                    }
                }
            }
            */
        }

        protected void gv_school_type_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /*
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                foreach (Control control in e.Row.Cells[0].Controls)
                {
                    if (control is LinkButton button && button.CommandName == "Update")
                    {
                        button.Attributes["Class"] = "gridview-update-btn";
                        break;
                    }
                }

                foreach (TableCell cell in e.Row.Cells)
                {
                    foreach (Control control in cell.Controls)
                    {
                        if (control is TextBox text)
                        {
                            text.Attributes["onkeydown"] = "return handleEnterKey(event);";
                        }
                    }
                }
            }
            */
        }

        protected void gv_panel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /*
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                foreach (Control control in e.Row.Cells[0].Controls)
                {
                    if (control is LinkButton button && button.CommandName == "Update")
                    {
                        button.Attributes["Class"] = "gridview-update-btn";
                        break;
                    }
                }

                foreach (TableCell cell in e.Row.Cells)
                {
                    foreach (Control control in cell.Controls)
                    {
                        if (control is TextBox text)
                        {
                            text.Attributes["onkeydown"] = "return handleEnterKey(event);";
                        }
                    }
                }
            }
            */
        }

        protected void gv_admin_area_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /*
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                foreach (Control control in e.Row.Cells[0].Controls)
                {
                    if (control is LinkButton button && button.CommandName == "Update")
                    {
                        button.Attributes["Class"] = "gridview-update-btn";
                        break;
                    }
                }

                foreach (TableCell cell in e.Row.Cells)
                {
                    foreach (Control control in cell.Controls)
                    {
                        if (control is TextBox text)
                        {
                            text.Attributes["onkeydown"] = "return handleEnterKey(event);";
                        }
                    }
                }
            }
            */
        }
    }
}