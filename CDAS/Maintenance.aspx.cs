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

namespace CDAS
{
    public partial class Maintenance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["access_type"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (Session["access_type"].ToString() == "ADMIN")
            {
                if (!IsPostBack)
                {
                    sds_school_type.SelectCommand = "select panel, ABBRV_NAME from [CDAS].[CDDBA].[EC_PANEL] order by ABBRV_NAME";
                    ddl_panel_type.DataBind();
                }
            }
            else
            {
                Response.Redirect("login.aspx");
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

                lv_maint.DataSource = dt;
                lv_maint.DataBind();
                if (dp_pager.Visible == false)
                {
                    dp_pager.Visible = true;
                }
            }
            catch(Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            string query = "";
            string search_code = tb_search_code.Text.Trim();
            string search_description = tb_search_description.Text.Trim();
            lv_maint.DataSource = null;
            lv_maint.Items.Clear();
            lv_maint.DataBind();
            ViewState["query"] = null;

            search_code = search_code.Replace('*', '%');
            search_code = search_code.Replace('?', '_');
            
            search_description = search_description.Replace('*', '%');
            search_description = search_description.Replace('?', '_');
            if (string.IsNullOrEmpty(search_code) && string.IsNullOrEmpty(search_description))
            {
                //Both Empty display everything
                query = string.Format("select * from [CDAS].[dbo].[hd_ec_locations]");

                if (ddl_panel_type.SelectedValue != "ALL")
                {
                    query += " where PANEL = '" + ddl_panel_type.SelectedValue + "'";
                }
            }
            else
            {
                //Theres values in atleast 1 of the 2 textboxes
                if (!string.IsNullOrEmpty(search_code))
                {
                    query = string.Format("select * from [CDAS].[dbo].[hd_ec_locations] where location_code LIKE '%{0}%' ", search_code);
                    if (!string.IsNullOrEmpty(search_description))
                    {
                        query += string.Format("AND where description_text LIKE '%{0}%' ", search_description);
                    }
                }
                else
                {
                    query = string.Format("select * from [CDAS].[dbo].[hd_ec_locations] where description_text LIKE '%{0}%' ", search_description);
                }
                
                if (ddl_panel_type.SelectedValue != "ALL")
                {
                    query += " where PANEL = '" + ddl_panel_type.SelectedValue + "'";
                }
            }

            ViewState["query"] = query;
            ViewState["sort_expression"] = " ORDER BY location_code";

            if (dp_pager.Visible == false)
            {
                dp_pager.Visible = true;
            }

            BindListView(ViewState["query"].ToString(), ViewState["sort_expression"].ToString());
        }

        protected void btn_edit_Click(object sender, EventArgs e)
        {

        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            tb_search_code.Text = "";
            tb_search_description.Text = "";
            lv_maint.DataSource = null;
            lv_maint.Items.Clear();
            lv_maint.DataBind();
            ViewState["query"] = null;

            if (dp_pager.Visible == true)
            {
                dp_pager.Visible = false;
            }

        }

        protected void lv_maint_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            
            #region Item Template Variables
            TextBox tb_location_code = (TextBox)e.Item.FindControl("tb_location_code");
            DropDownList ddl_location_area = (DropDownList)e.Item.FindControl("ddl_location_area");            
            TextBox tb_mident_text = (TextBox)e.Item.FindControl("tb_mident_text");
            TextBox tb_description_text = (TextBox)e.Item.FindControl("tb_description_text");
            TextBox tb_description_code = (TextBox)e.Item.FindControl("tb_description_code"); //hide
            TextBox tb_description_abbr = (TextBox)e.Item.FindControl("tb_description_abbr");
            TextBox tb_street_1 = (TextBox)e.Item.FindControl("tb_street_1");
            TextBox tb_street_2 = (TextBox)e.Item.FindControl("tb_street_2");
            TextBox tb_city = (TextBox)e.Item.FindControl("tb_city");
            TextBox tb_province = (TextBox)e.Item.FindControl("tb_province");
            TextBox tb_postal_code = (TextBox)e.Item.FindControl("tb_postal_code");
            TextBox tb_telephone_area = (TextBox)e.Item.FindControl("tb_telephone_area");
            TextBox tb_telephone_number = (TextBox)e.Item.FindControl("tb_telephone_number");
            TextBox tb_telephone_extension = (TextBox)e.Item.FindControl("tb_telephone_extension");
            DropDownList ddl_geographic_area_code = (DropDownList)e.Item.FindControl("ddl_geographic_area_code");
            DropDownList ddl_onsis_location_type = (DropDownList)e.Item.FindControl("ddl_onsis_location_type");
            DropDownList ddl_record_status = (DropDownList)e.Item.FindControl("ddl_record_status");
            TextBox tb_alternate_location_code = (TextBox)e.Item.FindControl("tb_alternate_location_code");
            TextBox tb_school_code = (TextBox)e.Item.FindControl("tb_school_code");
            TextBox tb_fax_area = (TextBox)e.Item.FindControl("tb_fax_area");
            TextBox tb_fax_number = (TextBox)e.Item.FindControl("tb_fax_number");
            DropDownList ddl_semester_code = (DropDownList)e.Item.FindControl("ddl_semester_code");
            TextBox tb_speed_dial_number = (TextBox)e.Item.FindControl("tb_speed_dial_number");
            TextBox tb_fr_immersion = (TextBox)e.Item.FindControl("tb_fr_immersion");
            TextBox tb_start_time = (TextBox)e.Item.FindControl("tb_start_time");
            TextBox tb_end_time = (TextBox)e.Item.FindControl("tb_end_time");
            //DropDownList ddl_onsis_location_type = (DropDownList)e.Item.FindControl("ddl_onsis_location_type");
            DropDownList ddl_panel = (DropDownList)e.Item.FindControl("ddl_panel");
            DropDownList ddl_location_type = (DropDownList)e.Item.FindControl("ddl_location_type");
            Label lbl_message = (Label)e.Item.FindControl("lbl_message");
            Label lbl_message_insert = (Label)e.Item.FindControl("lbl_message_insert");
            string connstring = ConfigurationManager.ConnectionStrings["DB_CDAS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand cmd = null;
            SqlDataReader reader;
            HiddenField hf_street_1 = (HiddenField)e.Item.FindControl("hf_street_1");
            HiddenField hf_street_2 = (HiddenField)e.Item.FindControl("hf_street_2");
            string query = "";
            #endregion

            //TODO :: Use EC_PANEL ABBRV_NAME to add a filter for searching
            //SC quirk check

            //Update each set of tables
            //lbl_edit_display.Text = "";
            try
            {
                if(e.CommandName == "Modify")
                {
                    string null_check;
                    //tb_telephone_area.Text = 

                    List<TextBox> text_box_null_check = new List<TextBox> { tb_telephone_area, tb_telephone_extension, tb_telephone_number, tb_speed_dial_number, tb_fax_area, tb_fax_number };
                    List<TextBox> non_null_text_box = new List<TextBox> { };
                    foreach (TextBox textbox in text_box_null_check)
                    {
                        null_check = textbox.Text;
                        if(!string.IsNullOrEmpty(null_check))
                        {
                            non_null_text_box.Add(textbox);
                        }
                    }

                    if(Check_For_Text(non_null_text_box) == false)
                    {
                        throw new Exception("Phone Number value is NOT numerical");
                    }

                    foreach(Control control in lv_maint.EditItem.Controls)
                    {
                        if(control is TextBox text_box)
                        {
                            if(text_box.MaxLength > 0 && text_box.Text.Length != text_box.MaxLength)
                            {
                                #region Length Checks
                                if (control == tb_telephone_area)
                                {
                                    throw new Exception("Telephone Area must only be " + text_box.MaxLength + " digits");
                                }

                                if (control ==  tb_telephone_number)
                                {
                                    throw new Exception("Telephone Number must only be " + text_box.MaxLength + " digits");
                                }

                                if (control == tb_location_code)
                                {
                                    throw new Exception("Location Code must only be " + text_box.MaxLength + " digits");
                                }

                                if (control == tb_fax_area)
                                {
                                    throw new Exception("Fax Area must only be " + text_box.MaxLength + " digits");
                                }

                                if (control == tb_fax_number)
                                {
                                    throw new Exception("Fax Number must only be " + text_box.MaxLength + " digits");
                                }

                                if (control == tb_speed_dial_number)
                                {
                                    throw new Exception("Speed dial number must only be " + text_box.MaxLength + " digits");
                                }
                                #endregion
                            }
                        }
                    }



                    /*   query = "UPDATE [CDAS].[dbo].[hd_ec_locations] SET alternate_location_code = @alternate_location_code, location_area = @location_area, location_type = @location_type, onsis_location_type = @onsis_location_type, description_text = @description_text, description_abbr = @description_abbr, street_1 = @street_1, street_2 = @street_2, city = @city, province = @province," +
                   " postal_code = @postal_code, mident = @mident, telephone_area = @telephone_area, telephone_no = @telephone_no, telephone_ext = @telephone_ext, geographic_area_code = @geographic_area_code, speed_dial_number = @speed_dial_number, fax_area = @fax_area, fax_no = @fax_no, school_code = @school_code," +
                   " semester_code = @semester_code, record_status = @record_status, panel = @panel, fr_immersion = @fr_immersion, start_time = @start_time, end_time = @end_time WHERE location_code = @location_code"; */

                    query = "UPDATE [CDAS].[dbo].[hd_ec_locations] SET alternate_location_code = @alternate_location_code, location_area = @location_area, location_type = @location_type, onsis_location_type = @onsis_location_type, description_text = @description_text, description_abbr = @description_abbr, street_1 = @street_1, street_2 = @street_2, city = @city, province = @province," +
                        " postal_code = @postal_code, mident = @mident, telephone_area = @telephone_area, telephone_no = @telephone_no, telephone_ext = @telephone_ext, geographic_area_code = @geographic_area_code, speed_dial_number = @speed_dial_number, fax_area = @fax_area, fax_no = @fax_no, school_code = @school_code," +
                        " start_time = @start_time, end_time = @end_time, fr_immersion = @fr_immersion, semester_code = @semester_code, record_status = @record_status, panel = @panel WHERE location_code = @location_code";
                conn.Open();
                using (cmd = new SqlCommand(query, conn))
                {
                    #region Edit Parameters
                    cmd.Parameters.AddWithValue("@alternate_location_code", tb_alternate_location_code.Text.Trim());
                    cmd.Parameters.AddWithValue("@location_area", ddl_location_area.SelectedValue);
                    cmd.Parameters.AddWithValue("@location_type", ddl_location_type.SelectedValue);
                    cmd.Parameters.AddWithValue("@onsis_location_type", ddl_onsis_location_type.SelectedValue);
                    
                    cmd.Parameters.AddWithValue("@description_text", tb_description_text.Text.Trim());
                    cmd.Parameters.AddWithValue("@description_abbr", tb_description_abbr.Text.Trim());
                    cmd.Parameters.AddWithValue("@street_1", tb_street_1.Text.Trim());
                    cmd.Parameters.AddWithValue("@street_2", tb_street_2.Text.Trim());
                    cmd.Parameters.AddWithValue("@city", tb_city.Text.Trim());
                    cmd.Parameters.AddWithValue("@province", tb_province.Text.Trim());
                    
                    cmd.Parameters.AddWithValue("@postal_code", tb_postal_code.Text.Trim());
                    cmd.Parameters.AddWithValue("@mident", tb_mident_text.Text.Trim());
                    cmd.Parameters.AddWithValue("@telephone_area", tb_telephone_area.Text.Trim());
                    cmd.Parameters.AddWithValue("@telephone_no", tb_telephone_number.Text.Trim());
                    cmd.Parameters.AddWithValue("@telephone_ext", tb_telephone_extension.Text.Trim());
                    
                    cmd.Parameters.AddWithValue("@geographic_area_code", ddl_geographic_area_code.SelectedValue);
                    cmd.Parameters.AddWithValue("@speed_dial_number", tb_speed_dial_number.Text.Trim());
                    cmd.Parameters.AddWithValue("@fax_area", tb_fax_area.Text.Trim());
                    cmd.Parameters.AddWithValue("@fax_no", tb_fax_number.Text.Trim());
                    cmd.Parameters.AddWithValue("@school_code", tb_school_code.Text.Trim());
                    
                    cmd.Parameters.AddWithValue("@semester_code", ddl_semester_code.SelectedValue);
                    
                    cmd.Parameters.AddWithValue("@record_status", ddl_record_status.SelectedValue);
                    
                    cmd.Parameters.AddWithValue("@panel", ddl_panel.SelectedValue);
                    
                    cmd.Parameters.AddWithValue("@fr_immersion", tb_fr_immersion.Text.Trim());
                    
                    cmd.Parameters.AddWithValue("@start_time", tb_start_time.Text.Trim());
                    cmd.Parameters.AddWithValue("@end_time", tb_end_time.Text.Trim());
                    
                    cmd.Parameters.AddWithValue("@location_code", tb_location_code.Text.Trim());
                    
                    #endregion

                    //conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                
                    lv_maint.EditIndex = -1;
                    BindListView(ViewState["query"].ToString(), ViewState["sort_expression"].ToString());
                    lv_maint.DataBind();
                }
                else if(e.CommandName == "Add")
                {

                
                    bool validInsert = false;
                    //Create new function is all "manual"
                    //Elementary and secondar school designation is in CDDBA.EC_school's panel
                    //EC_Location's Type code is the source of "SC" checking
                    //Check if user put SC before proceding.

                    //Record status is just active or inactive so maybe we can use a checkbox for that?
                    //So. Onsis Shouldnt be directly set?

                    //Null checks insert only?
                    string verify_code = "SELECT COUNT(*) FROM [CDAS].[dbo].[hd_ec_locations] WHERE LOCATION_CODE = @location_code";
                    conn.Open();
                    using(SqlCommand command = new SqlCommand(verify_code, conn))
                    {
                        command.Parameters.AddWithValue("@location_code", tb_location_code.Text.Trim());
                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            //lbl_message_insert.Text = "Location code already in use";
                            throw new Exception("Location code already in use.");
                        }
                        else
                        {
                            
                            if (string.IsNullOrEmpty(tb_location_code.Text))
                            {
                                lbl_message_insert.Text = "Location Code cannot be NULL";
                                throw new Exception("Location Code cannot be NULL");
                            }

                            if (string.IsNullOrEmpty(tb_description_text.Text))
                            {
                                lbl_message_insert.Text = "Description Text cannot be NULL";
                                throw new Exception("Description Text cannot be NULL");
                            }

                            if (string.IsNullOrEmpty(tb_description_abbr.Text))
                            {
                                lbl_message_insert.Text = "Description Abbreviation cannot be NULL";
                                throw new Exception("Description Abbreviation cannot be NULL");
                            }

                            if (string.IsNullOrEmpty(tb_telephone_number.Text))
                            {
                                lbl_message_insert.Text = "Telephone Number cannot be NULL";
                                throw new Exception("Telephone Number cannot be NULL");
                            }

                            if (string.IsNullOrEmpty(tb_fax_number.Text))
                            {
                                lbl_message_insert.Text = "Fax Number cannot be NULL";
                                throw new Exception("Fax Number cannot be NULL");
                            }

                            if (string.IsNullOrEmpty(tb_fr_immersion.Text))
                            {
                                lbl_message_insert.Text = "French Immersion cannot be NULL";
                                throw new Exception("French Immersion cannot be NULL");
                            }
                            

                            //Fully valid for insertion
                            if (ddl_location_type.SelectedValue == "SC")
                            {
                                foreach (Control c in lv_maint.InsertItem.Controls)
                                {
                                    if (c is TextBox textbox && textbox != tb_alternate_location_code && textbox != tb_start_time && textbox != tb_end_time)
                                    {
                                        if (!string.IsNullOrEmpty(textbox.Text))
                                        {
                                            validInsert = true;
                                        }
                                        else
                                        {
                                            validInsert = false;
                                            lbl_message_insert.Text = "Location Code cannot be NULL";
                                            throw new Exception("Location Code cannot be NULL");
                                            
                                        }
                                    }
                                    
                                }

                                if (validInsert == true)
                                {
                                    query = "INSERT INTO [CDAS].[dbo].[hd_ec_locations] (location_code, alternate_location_code, location_area, location_type, onsis_location_type, description_text, description_abbr, street_1, street_2, city, province," +
                                    "postal_code, mident, telephone_area, telephone_no, telephone_ext, geographic_area_code, speed_dial_number, fax_area, fax_no, school_code, semester_code, record_status, panel, fr_immersion,start_time, end_time) " +
                                    "VALUES (@location_code, @alternate_location_code, @location_area, @location_type, @onsis_location_type, @description_text, @description_abbr, @street_1, @street_2, @city, @province, " +
                                    " @postal_code, @mident, @telephone_area, @telephone_no, @telephone_ext, @geographic_area_code, @speed_dial_number, @fax_area, @fax_no, @school_code, @semester_code, @record_status, @panel, @fr_immersion, @start_time, @end_time)";

                                    using (cmd = new SqlCommand(query, conn))
                                    {
                                        #region Edit Parameters
                                        cmd.Parameters.AddWithValue("@alternate_location_code", tb_alternate_location_code.Text);
                                        cmd.Parameters.AddWithValue("@location_area", ddl_location_area.SelectedValue);
                                        cmd.Parameters.AddWithValue("@location_type", ddl_location_type.SelectedValue);
                                        cmd.Parameters.AddWithValue("@onsis_location_type", ddl_onsis_location_type.SelectedValue);
                                        cmd.Parameters.AddWithValue("@description_text", tb_description_text.Text);
                                        cmd.Parameters.AddWithValue("@description_abbr", tb_description_abbr.Text);
                                        cmd.Parameters.AddWithValue("@street_1", tb_street_1.Text);
                                        cmd.Parameters.AddWithValue("@street_2", tb_street_2.Text);
                                        cmd.Parameters.AddWithValue("@city", tb_city.Text);
                                        cmd.Parameters.AddWithValue("@province", tb_province.Text);

                                        cmd.Parameters.AddWithValue("@postal_code", tb_postal_code.Text);
                                        cmd.Parameters.AddWithValue("@mident", tb_mident_text.Text);
                                        cmd.Parameters.AddWithValue("@telephone_area", tb_telephone_area.Text);
                                        cmd.Parameters.AddWithValue("@telephone_no", tb_telephone_number.Text);
                                        cmd.Parameters.AddWithValue("@telephone_ext", tb_telephone_extension.Text);
                                        cmd.Parameters.AddWithValue("@geographic_area_code", ddl_geographic_area_code.SelectedValue);
                                        cmd.Parameters.AddWithValue("@speed_dial_number", tb_speed_dial_number.Text);
                                        cmd.Parameters.AddWithValue("@fax_area", tb_fax_area.Text);
                                        cmd.Parameters.AddWithValue("@fax_no", tb_fax_number.Text);
                                        cmd.Parameters.AddWithValue("@school_code", tb_school_code.Text);

                                        cmd.Parameters.AddWithValue("@semester_code", ddl_semester_code.SelectedValue);
                                        cmd.Parameters.AddWithValue("@record_status", ddl_record_status.SelectedValue);
                                        cmd.Parameters.AddWithValue("@panel", ddl_panel.SelectedValue);
                                        cmd.Parameters.AddWithValue("@fr_immersion", tb_fr_immersion.Text);
                                        cmd.Parameters.AddWithValue("@start_time", tb_start_time.Text);
                                        cmd.Parameters.AddWithValue("@end_time", tb_end_time.Text);
                                        cmd.Parameters.AddWithValue("@location_code", tb_location_code.Text);
                                        #endregion

                                        conn.Open();
                                        cmd.ExecuteNonQuery();
                                        conn.Close();
                                    }

                                    lbl_message_insert.Text = "Record Successfully Added";
                                }
                            }
                            else
                            {
                                //NON SC
                                foreach (Control c in lv_maint.InsertItem.Controls)
                                {
                                    if (c is TextBox textbox && textbox != tb_alternate_location_code && textbox != tb_telephone_area
                                        && textbox != tb_telephone_number && textbox != tb_telephone_extension && textbox != tb_fax_area
                                        && textbox != tb_fax_number && textbox != tb_speed_dial_number && textbox != tb_start_time && textbox != tb_end_time)
                                    {
                                        if (!string.IsNullOrEmpty(textbox.Text))
                                        {
                                            validInsert = true;
                                        }
                                        else
                                        {
                                            validInsert = false;
                                            lbl_message_insert.ForeColor = System.Drawing.Color.Red;
                                            lbl_message_insert.Text = textbox.ID + " must be given a value";
                                            break;
                                        }
                                    }
                                }

                                if (validInsert == true)
                                {
                                    query = "INSERT INTO [CDAS].[dbo].[hd_ec_locations] (location_code, alternate_location_code, location_area, location_type, onsis_location_type, description_text, description_abbr, street_1, street_2, city, province," +
                                        "postal_code, mident, telephone_area, telephone_no, telephone_ext, geographic_area_code, speed_dial_number, fax_area, fax_no, school_code, semester_code, record_status, panel, fr_immersion, start_time, end_time) " +
                                        "VALUES (@location_code, @alternate_location_code, @location_area, @location_type, @onsis_location_type, @description_text, @description_abbr, @street_1, @street_2, @city, @province, " +
                                        " @postal_code, @mident, @telephone_area, @telephone_no, @telephone_ext, @geographic_area_code, @speed_dial_number, @fax_area, @fax_no, @school_code, @semester_code, @record_status, @panel, @fr_immersion, @start_time, @end_time)";

                                    using (cmd = new SqlCommand(query, conn))
                                    {
                                        
                                        #region Insert Parameters
                                        cmd.Parameters.AddWithValue("@alternate_location_code", tb_alternate_location_code.Text.Trim());
                                        cmd.Parameters.AddWithValue("@location_area", ddl_location_area.SelectedValue);
                                        cmd.Parameters.AddWithValue("@location_type", ddl_location_type.SelectedValue);
                                        cmd.Parameters.AddWithValue("@onsis_location_type", ddl_onsis_location_type.SelectedValue);
                                        cmd.Parameters.AddWithValue("@description_text", tb_description_text.Text.Trim());
                                        cmd.Parameters.AddWithValue("@description_abbr", tb_description_abbr.Text.Trim());
                                        cmd.Parameters.AddWithValue("@street_1", tb_street_1.Text.Trim());
                                        cmd.Parameters.AddWithValue("@street_2", tb_street_2.Text.Trim());
                                        cmd.Parameters.AddWithValue("@city", tb_city.Text.Trim());
                                        cmd.Parameters.AddWithValue("@province", tb_province.Text.Trim());

                                        cmd.Parameters.AddWithValue("@postal_code", tb_postal_code.Text.Trim());
                                        cmd.Parameters.AddWithValue("@mident", tb_mident_text.Text.Trim());
                                        cmd.Parameters.AddWithValue("@telephone_area", tb_telephone_area.Text.Trim());
                                        cmd.Parameters.AddWithValue("@telephone_no", tb_telephone_number.Text.Trim());
                                        cmd.Parameters.AddWithValue("@telephone_ext", tb_telephone_extension.Text.Trim());
                                        cmd.Parameters.AddWithValue("@geographic_area_code", ddl_geographic_area_code.SelectedValue);
                                        cmd.Parameters.AddWithValue("@speed_dial_number", tb_speed_dial_number.Text.Trim());
                                        cmd.Parameters.AddWithValue("@fax_area", tb_fax_area.Text.Trim());
                                        cmd.Parameters.AddWithValue("@fax_no", tb_fax_number.Text.Trim());
                                        cmd.Parameters.AddWithValue("@school_code", tb_school_code.Text.Trim());

                                        cmd.Parameters.AddWithValue("@semester_code", ddl_semester_code.SelectedValue);
                                        cmd.Parameters.AddWithValue("@record_status", ddl_record_status.SelectedValue);
                                        cmd.Parameters.AddWithValue("@panel", ddl_panel.SelectedValue);
                                        cmd.Parameters.AddWithValue("@fr_immersion", tb_fr_immersion.Text.Trim());
                                        cmd.Parameters.AddWithValue("@start_time", tb_start_time.Text.Trim());
                                        cmd.Parameters.AddWithValue("@end_time", tb_end_time.Text.Trim());
                                        cmd.Parameters.AddWithValue("@location_code", tb_location_code.Text.Trim());
                                        #endregion

                                        //conn.Open();
                                        cmd.ExecuteNonQuery();
                                        conn.Close();
                                        lbl_message_insert.ForeColor = System.Drawing.Color.Black;
                                        lbl_message_insert.Text = "Record Successfully Added";
                                    }
                                }
                            }
                        }
                    }
                    conn.Close();
                }
                else if(e.CommandName == "CancelEdit")
                {

                }
            
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);

                if (lbl_message != null)
                {
                    lbl_message.ForeColor = System.Drawing.Color.Red;
                    lbl_message.Text = ex.Message;
                }

                if(lbl_message_insert != null)
                {
                    lbl_message_insert.ForeColor = System.Drawing.Color.Red;
                    lbl_message_insert.Text = ex.Message;
                }

                if(!string.IsNullOrEmpty(query))
                {
                    lbl_message.Text += cmd.CommandText;
                }
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

        protected void lv_maint_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            #region Template Variables
            TextBox tb_location_type = (TextBox)e.Item.FindControl("tb_location_type");
            TextBox tb_mident_text = (TextBox)e.Item.FindControl("tb_mident_text");
            TextBox tb_description_abbr = (TextBox)e.Item.FindControl("tb_description_abbr");
            TextBox tb_street_1 = (TextBox)e.Item.FindControl("tb_street_1");
            TextBox tb_street_2 = (TextBox)e.Item.FindControl("tb_street_2");
            TextBox tb_city = (TextBox)e.Item.FindControl("tb_city");
            TextBox tb_province = (TextBox)e.Item.FindControl("tb_province");
            TextBox tb_postal_code = (TextBox)e.Item.FindControl("tb_postal_code");
            TextBox tb_telephone_area = (TextBox)e.Item.FindControl("tb_telephone_area");
            TextBox tb_telephone_number = (TextBox)e.Item.FindControl("tb_telephone_number");
            TextBox tb_telephone_extension = (TextBox)e.Item.FindControl("tb_telephone_extension");
            DropDownList ddl_geographic_area_code = (DropDownList)e.Item.FindControl("ddl_geographic_area_code");
            TextBox tb_start_time = (TextBox)e.Item.FindControl("tb_start_time");
            TextBox tb_end_time = (TextBox)e.Item.FindControl("tb_end_time");
            TextBox tb_record_status = (TextBox)e.Item.FindControl("tb_record_status");
            TextBox tb_alternate_location_code = (TextBox)e.Item.FindControl("tb_alternate_location_code");
            TextBox tb_school_code = (TextBox)e.Item.FindControl("tb_school_code");
            TextBox tb_fax_area = (TextBox)e.Item.FindControl("tb_fax_area");
            TextBox tb_fax_number = (TextBox)e.Item.FindControl("tb_fax_number");
            TextBox tb_panel = (TextBox)e.Item.FindControl("tb_panel");
            TextBox tb_speed_dial_number = (TextBox)e.Item.FindControl("tb_speed_dial_number");
            TextBox tb_fr_immersion = (TextBox)e.Item.FindControl("tb_fr_immersion");
            TextBox tb_semester_code = (TextBox)e.Item.FindControl("tb_semester_code");
            Label lbl_location_code = (Label)e.Item.FindControl("lbl_location_code");
            Label lbl_message = (Label)e.Item.FindControl("lbl_message");
            //DropDownList ddl_onsis_location_type = (DropDownList)e.Item.FindControl("ddl_onsis_location_type");
            #endregion


            if (lv_maint.EditIndex >= 0)
            {
                ListViewDataItem data_item = (ListViewDataItem)e.Item;
                if (data_item.DisplayIndex == lv_maint.EditIndex)
                {
                    DropDownList ddl_panel = (DropDownList)e.Item.FindControl("ddl_panel");
                    string null_check = DataBinder.Eval(data_item.DataItem, "PANEL")?.ToString();
                    //This should set Panel respectively
                    if (!string.IsNullOrEmpty(null_check))
                    {
                        ddl_panel.SelectedValue = null_check;
                    }
                    else
                    {
                        ddl_panel.SelectedIndex = 0;
                    }

                    DropDownList ddl_location_type = (DropDownList)e.Item.FindControl("ddl_location_type");
                    null_check = DataBinder.Eval(data_item.DataItem, "LOCATION_TYPE")?.ToString();

                    if (!string.IsNullOrEmpty(null_check))
                    {
                        ddl_location_type.SelectedValue = null_check;
                    }
                    else
                    {
                        ddl_location_type.SelectedIndex = 0;
                    }

                    DropDownList ddl_location_area = (DropDownList)e.Item.FindControl("ddl_location_area");
                    null_check = DataBinder.Eval(data_item.DataItem, "LOCATION_AREA")?.ToString();

                    if (!string.IsNullOrEmpty(null_check))
                    {
                        ddl_location_area.SelectedValue = null_check;
                    }
                    else
                    {
                        ddl_location_area.SelectedIndex = 0;
                    }

                    DropDownList ddl_semester_code = (DropDownList)e.Item.FindControl("ddl_semester_code");
                    ddl_semester_code.DataSource = new List<string> { "Semestered", "Not Semestered" };
                    ddl_semester_code.DataBind();

                    null_check = DataBinder.Eval(data_item.DataItem, "SEMESTER_CODE")?.ToString();

                    if (!string.IsNullOrEmpty(null_check))
                    {
                        ddl_semester_code.SelectedValue = null_check;
                    }
                    else
                    {
                        ddl_semester_code.SelectedIndex = 0;
                    }

                    DropDownList ddl_record_status = (DropDownList)e.Item.FindControl("ddl_record_status");
                    ddl_record_status.DataSource = new List<string> { "A", "I" };
                    ddl_record_status.DataBind();

                    null_check = DataBinder.Eval(data_item.DataItem, "RECORD_STATUS")?.ToString();

                    if (!string.IsNullOrEmpty(null_check))
                    {
                        ddl_record_status.SelectedValue = null_check;
                    }
                    else
                    {
                        ddl_record_status.SelectedIndex = 0;
                    }

                    DropDownList ddl_onsis_location_type = (DropDownList)e.Item.FindControl("ddl_onsis_location_type");
                    null_check = DataBinder.Eval(data_item.DataItem, "ONSIS_LOCATION_TYPE")?.ToString();
                    //This should set Panel respectively
                    if (!string.IsNullOrEmpty(null_check))
                    {
                        ddl_onsis_location_type.SelectedValue = null_check;
                    }
                    else
                    {
                        ddl_onsis_location_type.SelectedIndex = 0;
                    }


                    ddl_geographic_area_code.DataSource = new List<string> { "KIT", "WAT", "CAM" };
                    ddl_geographic_area_code.DataBind();

                    null_check = DataBinder.Eval(data_item.DataItem, "geographic_area_code")?.ToString();

                    if (!string.IsNullOrEmpty(null_check))
                    {
                        ddl_geographic_area_code.SelectedValue = null_check;
                    }
                    else
                    {
                        ddl_geographic_area_code.SelectedIndex = 0;
                    }
                }

            }

            BindListView(ViewState["query"].ToString(), ViewState["sort_expression"].ToString());
        }

        protected void lv_maint_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            try
            {
                //Response.Write("Edit CHeck: Edit INdex: " + lv_maint.EditIndex + " [E]New Edit Index: " + e.NewEditIndex);
                lv_maint.EditIndex = e.NewEditIndex;
                BindListView(ViewState["query"].ToString(), ViewState["sort_expression"].ToString());

                ListViewItem item = lv_maint.Items[e.NewEditIndex];
                //TextBox semester_code = (TextBox)item.FindControl("tb_semester_code");
                Label error_message = (Label)item.FindControl("lbl_message");

                
                string connstring = ConfigurationManager.ConnectionStrings["DB_CDAS"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    /*
                    conn.Open();
                    //string query = "SELECT COUNT(SEMESTER_CODE) FROM CDDBA.EC_SCHOOL WHERE LOCATION_CODE = @location_code";
                    string query = @"SELECT ECSCHOOL.SEMESTER_CODE
                                        FROM CDDBA.HD_LOCATIONS HDLOCATIONS
                                        INNER JOIN CDDBA.EC_SCHOOL ECSCHOOL ON HDLOCATIONS.LOCATION_CODE = ECSCHOOL.LOCATION_CODE
                                        WHERE ECSCHOOL.LOCATION_CODE = @loc_code";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@loc_code", location_code.Text);

                    //SqlDataReader reader = cmd.ExecuteReader();
                    object verify_code_object = cmd.ExecuteScalar();
                    
                    if (verify_code_object == null)
                    {
                        //error_message.Text = "No SEMESTER CODE FOUND???????????????";
                        return;
                    }
                    else
                    {
                        string address_code = verify_code_object.ToString();
                        //error_message.Text = "OKAY SO THERE IT IS!???? IT FOUND " + address_code + " CODES.";
                        semester_code.Text = address_code;
                    }
                    */
                    /*
                    int count = (int)cmd.ExecuteScalar();
                    if(count == 0)
                    {
                        error_message.Text = "No SEMESTER CODE FOUND???????????????";
                        return;
                    }
                    else
                    {
                        error_message.Text = "OKAY SO THERE IT IS!???? IT FOUND " + count + " CODES.";
                    }
                    */
                    /*
                    if(reader.Read())
                    {
                        semester_code.Text = reader["SEMESTER_CODE"].ToString();
                    }
                    */
                }
                /*
                string connstring = ConfigurationManager.ConnectionStrings["DB_CDAS"].ConnectionString;

                string location_code = lv_maint.DataKeys[e.NewEditIndex].Value.ToString();
                string semester_code;
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT SEMESTER_CODE FROM CDDBA.EC_SCHOOL WHERE LOCATION_CODE = @Location_Code", conn);
                    cmd.Parameters.AddWithValue("@location_code", location_code);
                    semester_code = cmd.ExecuteScalar().ToString();
                }
                //Check and see if this gets and sets the control properly. Fixed?
                TextBox tb_semester_code = (TextBox)lv_maint.Items[e.NewEditIndex].FindControl("tb_semester_code");
                if(tb_semester_code != null)
                {
                    tb_semester_code.Text = semester_code;
                }
                */
            }
            catch(Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
        protected void ddl_locations_DataBound(object sender, EventArgs e)
        {
            //ddl_locations.Items.Insert(0, new ListItem("ALL", "ALL"));
        }

        protected void lv_maint_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }

        protected void lv_maint_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            
            lv_maint.EditIndex = -1;
            lv_maint.InsertItemPosition = InsertItemPosition.None;
            lv_maint.DataBind();
            if(dp_pager.Visible == true)
            {
                dp_pager.Visible = false;
            }
            
            
            //TODO: Cancel FIX
            /*
            lv_maint.EditIndex = -1;
            //lv_staff_user.DataBind();
            if (lv_maint.InsertItemPosition != InsertItemPosition.None)
            {
                lv_maint.InsertItem.Visible = false;
                lv_maint.InsertItemPosition = InsertItemPosition.None;
                lv_maint.DataBind();
            }
            else
            {
                if (ViewState["query"] != null)
                {
                    BindListView(ViewState["query"].ToString(), ViewState["sort_expression"].ToString());
                }
            }
            */
        }

        protected void lv_maint_ItemInserting(object sender, ListViewInsertEventArgs e)
        {

        }

        protected void lb_insert_Click(object sender, EventArgs e)
        {
            lv_maint.InsertItemPosition = InsertItemPosition.FirstItem;

            lv_maint.DataSource = null;
            lv_maint.Items.Clear();
            lv_maint.DataBind();
            ViewState["query"] = null;

            //Insert Item template will display at the top
            lv_maint.InsertItemPosition = InsertItemPosition.FirstItem;

            //Clear the board to make the insert template. Throws error on Find Control if table_header wasnt made yet.
            if (lv_maint.FindControl("table_header") != null)
            {
                lv_maint.FindControl("table_header").Visible = false;
            }
        }

        protected void lv_maint_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            dp_pager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            BindListView(ViewState["query"].ToString(), ViewState["sort_expression"].ToString());
        }

        protected void ddl_panel_type_DataBound(object sender, EventArgs e)
        {
            ddl_panel_type.Items.Insert(0, new ListItem("ALL", "ALL"));
        }

        protected void btn_cancel_insert_Click(object sender, EventArgs e)
        {
            lv_maint.InsertItemPosition = InsertItemPosition.None;
            lv_maint.InsertItem.Visible = false;
            lv_maint.InsertItemTemplate = null;
        }

    }
}