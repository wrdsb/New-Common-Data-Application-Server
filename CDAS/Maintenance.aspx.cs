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
using System.Text;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using Azure.Core;
using Microsoft.Owin.Security.OpenIdConnect;

namespace CDAS
{
    public partial class Maintenance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["access_type"] == null)
            {
                Response.Redirect("~/default.aspx", false);
                return;
            }

            if (Session["access_type"].ToString() == "ADMIN")
            {
                if (!IsPostBack)
                {
                    sds_school_type.SelectCommand = "select panel, ABBRV_NAME from [CDAS].[CDDBA].[EC_PANEL] where status_flag = 'a' order by ABBRV_NAME";
                    ddl_panel_type.DataBind();
                }
                
            }
            else
            {
                Response.Redirect("~/default.aspx", false);
                return;
            }
            
            /*
            if(Request.IsAuthenticated)
            {
                if (!IsPostBack)
                {
                    sds_school_type.SelectCommand = "select panel, ABBRV_NAME from [CDAS].[CDDBA].[EC_PANEL] order by ABBRV_NAME";
                    ddl_panel_type.DataBind();
                }
            }
            else
            {
                HttpContext.Current.GetOwinContext().Authentication.Challenge(
                    new Microsoft.Owin.Security.AuthenticationProperties { RedirectUri = "/default.aspx" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
            */
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
            dp_pager.SetPageProperties(0, dp_pager.PageSize, false);
            lv_maint.EditIndex = -1;
            lv_maint.InsertItemPosition = InsertItemPosition.None;
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
                        query += string.Format("AND description_text LIKE '%{0}%'", search_description);
                        query += string.Format(" OR description_abbr LIKE '%{0}%'", search_description);
                    }
                }
                else
                {
                    query = string.Format("select * from [CDAS].[dbo].[hd_ec_locations] where description_text LIKE '%{0}%' ", search_description);
                    query += string.Format(" OR description_abbr LIKE '%{0}%'", search_description);
                }
                
                if (ddl_panel_type.SelectedValue != "ALL")
                {
                    query += " AND PANEL = '" + ddl_panel_type.SelectedValue + "'";
                }
            }

            ViewState["query"] = query;
            ViewState["sort_expression"] = " ORDER BY location_code";


            if (dp_pager.Visible == false)
            {
                dp_pager.Visible = true;
            }

            string connstring = ConfigurationManager.ConnectionStrings["DB_CDAS"].ConnectionString;

            StringBuilder record_string = new StringBuilder(query);
            record_string.Replace("select *", "select count(*)");
            string record_query = record_string.ToString();

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                using (SqlCommand cmd = new SqlCommand(record_query, conn))
                {
                    try
                    {
                        lbl_record_count.Visible = true;
                        conn.Open();
                        int rows_found = (int) cmd.ExecuteScalar();
                        string record_plural = "";
                        if(rows_found != 1)
                        {
                            record_plural = " records";
                        }
                        else
                        {
                            record_plural = " record";
                        }
                        lbl_record_count.Text = "Searched for ";

                        
                        if(!string.IsNullOrEmpty(search_code))
                        {
                            lbl_record_count.Text += search_code;
                            if(!string.IsNullOrEmpty(search_description))
                            {
                                lbl_record_count.Text += " and " + search_description + " and found ";
                            }
                            else
                            {
                                lbl_record_count.Text += " and found ";
                            }
                        }
                        else if(!string.IsNullOrEmpty(search_description))
                        {
                            lbl_record_count.Text += search_description + " and found ";
                        }

                        lbl_record_count.Text += rows_found +  record_plural;
                        conn.Close();
                    }
                    catch
                    {
                        lbl_record_count.Text = "No Records found";
                    }

                }
            }
            tb_search_code.Text = string.Empty;
            tb_search_description.Text = string.Empty;
            BindListView(ViewState["query"].ToString(), ViewState["sort_expression"].ToString());
            
        }

        protected void btn_edit_Click(object sender, EventArgs e)
        {

        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            /*
            dp_pager.SetPageProperties(0, dp_pager.PageSize, false);
            lv_maint.EditIndex = -1;
            lv_maint.InsertItemPosition = InsertItemPosition.None;
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
            lbl_record_count.Visible = false;
            */
            /*
            //requested that clear only clears the values in insert
            #region Item Template Variables
            TextBox tb_location_code = (TextBox)lv_maint.FindControl("tb_location_code");
            DropDownList ddl_location_area = (DropDownList)lv_maint.FindControl("ddl_location_area");
            TextBox tb_mident_text = (TextBox)lv_maint.FindControl("tb_mident_text");
            TextBox tb_description_text = (TextBox)lv_maint.FindControl("tb_description_text");
            TextBox tb_description_code = (TextBox)lv_maint.FindControl("tb_description_code"); //hide
            TextBox tb_description_abbr = (TextBox)lv_maint.FindControl("tb_description_abbr");
            TextBox tb_street_1 = (TextBox)lv_maint.FindControl("tb_street_1");
            TextBox tb_street_2 = (TextBox)lv_maint.FindControl("tb_street_2");
            TextBox tb_city = (TextBox)lv_maint.FindControl("tb_city");
            TextBox tb_province = (TextBox)lv_maint.FindControl("tb_province");
            TextBox tb_postal_code = (TextBox)lv_maint.FindControl("tb_postal_code");
            TextBox tb_telephone_area = (TextBox)lv_maint.FindControl("tb_telephone_area");
            TextBox tb_telephone_number = (TextBox)lv_maint.FindControl("tb_telephone_number");
            TextBox tb_telephone_extension = (TextBox)lv_maint.FindControl("tb_telephone_extension");
            DropDownList ddl_geographic_area_code = (DropDownList)lv_maint.FindControl("ddl_geographic_area_code");
            DropDownList ddl_onsis_location_type = (DropDownList)lv_maint.FindControl("ddl_onsis_location_type");
            DropDownList ddl_record_status = (DropDownList)lv_maint.FindControl("ddl_record_status");
            TextBox tb_alternate_location_code = (TextBox)lv_maint.FindControl("tb_alternate_location_code");
            TextBox tb_school_code = (TextBox)lv_maint.FindControl("tb_school_code");
            TextBox tb_fax_area = (TextBox)lv_maint.FindControl("tb_fax_area");
            TextBox tb_fax_number = (TextBox)lv_maint.FindControl("tb_fax_number");
            TextBox tb_speed_dial_number = (TextBox)lv_maint.FindControl("tb_speed_dial_number");
            //DropDownList ddl_onsis_location_type = (DropDownList)e.Item.FindControl("ddl_onsis_location_type");
            DropDownList ddl_panel = (DropDownList)lv_maint.FindControl("ddl_panel");
            DropDownList ddl_location_type = (DropDownList)lv_maint.FindControl("ddl_location_type");
            Label lbl_message = (Label)lv_maint.FindControl("lbl_message");
            Label lbl_message_insert = (Label)lv_maint.FindControl("lbl_message_insert");
            #endregion

            List<TextBox> insert_parameters = new List<TextBox> { tb_location_code, tb_alternate_location_code, tb_description_text, tb_description_abbr, tb_street_1, tb_street_2, tb_city, tb_province,
            tb_postal_code, tb_mident_text, tb_telephone_area, tb_telephone_number, tb_telephone_extension, tb_speed_dial_number, tb_fax_area, tb_school_code};
            List<DropDownList> drop_parameters = new List<DropDownList> { ddl_location_area , ddl_location_type , ddl_onsis_location_type , ddl_geographic_area_code , ddl_record_status };

            foreach(TextBox text in insert_parameters)
            {
                if(text != null)
                {
                    text.Text = string.Empty;
                }
            }
            
            foreach(DropDownList drop in drop_parameters)
            {
                if (drop != null)
                {
                    drop.SelectedIndex = 0;
                }
            }
            */

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
            TextBox tb_speed_dial_number = (TextBox)e.Item.FindControl("tb_speed_dial_number");
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
            List<TextBox> text_box_blank_check = new List<TextBox> { tb_description_text, tb_description_abbr, tb_street_1, tb_city, tb_province, tb_postal_code, tb_mident_text, tb_school_code };
            List<DropDownList> drop_down_blank_check = new List<DropDownList> { ddl_location_area, ddl_location_type, ddl_onsis_location_type, ddl_geographic_area_code, ddl_record_status, ddl_panel };
            //TODO :: Use EC_PANEL ABBRV_NAME to add a filter for searching
            //SC quirk check

            //Update each set of tables
            //lbl_edit_display.Text = "";
            try
            {
            if (e.CommandName == "Modify")
                {
                    string null_check;
                    List<TextBox> text_box_null_check = new List<TextBox> { tb_telephone_area, tb_telephone_extension, tb_telephone_number, tb_fax_area, tb_fax_number, tb_speed_dial_number};
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

                    //Long odd way since foreach way wont work?
                    if(String.IsNullOrWhiteSpace(tb_street_1.Text))
                    {
                        throw new Exception("Street address 1 needs to be entered");
                    }
                    if (String.IsNullOrWhiteSpace(tb_description_text.Text))
                    {
                        throw new Exception("School Name needs to be entered");
                    }
                    if (String.IsNullOrWhiteSpace(tb_description_abbr.Text))
                    {
                        throw new Exception("School needs abbreviated name");
                    }
                    if (String.IsNullOrWhiteSpace(tb_city.Text))
                    {
                        throw new Exception("City name needs to be entered");
                    }
                    if (String.IsNullOrWhiteSpace(tb_province.Text))
                    {
                        throw new Exception("Province needs to be entered");
                    }
                    if (String.IsNullOrWhiteSpace(tb_postal_code.Text))
                    {
                        throw new Exception("Postal Code needs to be entered");
                    }
                    if (String.IsNullOrWhiteSpace(tb_mident_text.Text))
                    {
                        throw new Exception("A MIDENT needs to be entered");
                    }
                    if (String.IsNullOrWhiteSpace(tb_school_code.Text))
                    {
                        throw new Exception("School Code needs to be entered");
                    }

                    //DropDowns
                    if (String.IsNullOrWhiteSpace(ddl_location_area.SelectedValue))
                    {
                        throw new Exception("Admin Area need to be select");
                    }
                    if (String.IsNullOrWhiteSpace(ddl_location_type.SelectedValue))
                    {
                        throw new Exception("Location Type need to be select");
                    }
                    if (String.IsNullOrWhiteSpace(ddl_onsis_location_type.SelectedValue))
                    {
                        throw new Exception("Onsis Location Type need to be select");
                    }
                    if (String.IsNullOrWhiteSpace(ddl_geographic_area_code.SelectedValue))
                    {
                        throw new Exception("Geographic Area Code need to be select");
                    }
                    if (String.IsNullOrWhiteSpace(ddl_record_status.SelectedValue))
                    {
                        throw new Exception("Record Status need to be select");
                    }
                    if (String.IsNullOrWhiteSpace(ddl_panel.SelectedValue))
                    {
                        throw new Exception("Panel need to be select");
                    }

                    if (ddl_location_type.SelectedValue == "SC")
                    {
                        //Everything except alt loc code
                        if (tb_telephone_area.Text.Length != tb_telephone_area.MaxLength)
                        {
                            throw new Exception("Telephone Area must only be " + tb_telephone_area.MaxLength + " digits");
                        }
                        if (tb_telephone_number.Text.Length != tb_telephone_number.MaxLength)
                        {
                            throw new Exception("Telephone Number must only be " + tb_telephone_number.MaxLength + " digits");
                        }
                        if (tb_telephone_extension.Text.Length != tb_telephone_extension.MaxLength)
                        {
                            throw new Exception("Location Code must only be " + tb_telephone_extension.MaxLength + " digits");
                        }
                        if (tb_fax_area.Text.Length != tb_fax_area.MaxLength)
                        {
                            throw new Exception("Fax Area must only be " + tb_fax_area.MaxLength + " digits");
                        }
                        if (tb_fax_number.Text.Length != tb_fax_number.MaxLength)
                        {
                            throw new Exception("Fax Number must only be " + tb_fax_number.MaxLength + " digits");
                        }
                        if (tb_speed_dial_number.Text.Length != tb_speed_dial_number.MaxLength)
                        {
                            throw new Exception("Speed dial number must only be " + tb_speed_dial_number.MaxLength + " digits");
                        }
                    }
                    else
                    {
                        //Everything except alt loc code, phone, fax, speed dials

                        /*
                        text_box_null_check = new List<TextBox> { tb_telephone_area, tb_telephone_extension, tb_telephone_number, tb_fax_area, tb_fax_number, tb_speed_dial_number,};

                        foreach (Control control in e.Item.Controls)
                        {
                            if (control is TextBox text_box)
                            {
                                if (!text_box_null_check.Contains(text_box))
                                {
                                    if (string.IsNullOrWhiteSpace(text_box.Text))
                                    {
                                        throw new Exception("Please fill out all fields marked with (*)");
                                    }
                                }
                            }
                            else if (control is DropDownList drop_down)
                            {
                                if (string.IsNullOrWhiteSpace(drop_down.SelectedValue) || drop_down.SelectedValue == "")
                                {
                                    throw new Exception("Please select all fields that marked with (*)");
                                }
                            }
                        }
                        */
                    }
                    /*   query = "UPDATE [CDAS].[dbo].[hd_ec_locations] SET alternate_location_code = @alternate_location_code, location_area = @location_area, location_type = @location_type, onsis_location_type = @onsis_location_type, description_text = @description_text, description_abbr = @description_abbr, street_1 = @street_1, street_2 = @street_2, city = @city, province = @province," +
                   " postal_code = @postal_code, mident = @mident, telephone_area = @telephone_area, telephone_no = @telephone_no, telephone_ext = @telephone_ext, geographic_area_code = @geographic_area_code, speed_dial_number = @speed_dial_number, fax_area = @fax_area, fax_no = @fax_no, school_code = @school_code," +
                   " semester_code = @semester_code, record_status = @record_status, panel = @panel WHERE location_code = @location_code"; */
                    /*
                     query = "UPDATE [CDAS].[dbo].[hd_ec_locations] SET alternate_location_code = @alternate_location_code, location_area = @location_area, location_type = @location_type, onsis_location_type = @onsis_location_type, description_text = @description_text, description_abbr = @description_abbr, street_1 = @street_1, street_2 = @street_2, city = @city, province = @province," +
                            " postal_code = @postal_code, mident = @mident, telephone_area = @telephone_area, telephone_no = @telephone_no, telephone_ext = @telephone_ext, geographic_area_code = @geographic_area_code, speed_dial_number = @speed_dial_number, fax_area = @fax_area, fax_no = @fax_no, school_code = @school_code," +
                            " semester_code = @semester_code, record_status = @record_status, panel = @panel WHERE location_code = @location_code";
                    */
                    
                

                using (SqlConnection connection = new SqlConnection(connstring))
                {
                        query = "UPDATE [CDAS].[dbo].[hd_ec_locations] SET alternate_location_code = @alternate_location_code, location_area = @location_area, location_type = @location_type, onsis_location_type = @onsis_location_type, description_text = @description_text, description_abbr = @description_abbr, street_1 = @street_1, street_2 = @street_2, city = @city, province = @province," +
                                " postal_code = @postal_code, mident = @mident, telephone_area = @telephone_area, telephone_no = @telephone_no, telephone_ext = @telephone_ext, geographic_area_code = @geographic_area_code, speed_dial_number = @speed_dial_number, fax_area = @fax_area, fax_no = @fax_no, school_code = @school_code, record_status = @record_status, panel = @panel WHERE location_code = @location_code";
                    SqlCommand command = new SqlCommand(query, connection);
                    #region Edit Parameters
                    command.Parameters.AddWithValue("@alternate_location_code", tb_alternate_location_code.Text.Trim());
                    command.Parameters.AddWithValue("@location_area", ddl_location_area.SelectedValue);
                    command.Parameters.AddWithValue("@location_type", ddl_location_type.SelectedValue);
                    command.Parameters.AddWithValue("@onsis_location_type", ddl_onsis_location_type.SelectedValue);
                    
                    command.Parameters.AddWithValue("@description_text", tb_description_text.Text.Trim());
                    command.Parameters.AddWithValue("@description_abbr", tb_description_abbr.Text.Trim());
                    command.Parameters.AddWithValue("@street_1", tb_street_1.Text.Trim());
                    command.Parameters.AddWithValue("@street_2", tb_street_2.Text.Trim());
                    command.Parameters.AddWithValue("@city", tb_city.Text.Trim());
                    command.Parameters.AddWithValue("@province", tb_province.Text.Trim());
                  
                    command.Parameters.AddWithValue("@postal_code", tb_postal_code.Text.Trim());
                    command.Parameters.AddWithValue("@mident", tb_mident_text.Text.Trim());
                    command.Parameters.AddWithValue("@telephone_area", tb_telephone_area.Text.Trim());
                    command.Parameters.AddWithValue("@telephone_no", tb_telephone_number.Text.Trim());
                    command.Parameters.AddWithValue("@telephone_ext", tb_telephone_extension.Text.Trim());
                    
                    command.Parameters.AddWithValue("@geographic_area_code", ddl_geographic_area_code.SelectedValue);
                    command.Parameters.AddWithValue("@speed_dial_number", tb_speed_dial_number.Text.Trim());
                    command.Parameters.AddWithValue("@fax_area", tb_fax_area.Text.Trim());
                    command.Parameters.AddWithValue("@fax_no", tb_fax_number.Text.Trim());
                    command.Parameters.AddWithValue("@school_code", tb_school_code.Text.Trim());

                    command.Parameters.AddWithValue("@record_status", ddl_record_status.SelectedValue);
                    
                    command.Parameters.AddWithValue("@panel", ddl_panel.SelectedValue);

                    command.Parameters.AddWithValue("@location_code", tb_location_code.Text.Trim());
                        cmd = command;
                    #endregion

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                
                    lv_maint.EditIndex = -1;
                    BindListView(ViewState["query"].ToString(), ViewState["sort_expression"].ToString());
                    lv_maint.DataBind();
                }
                else if(e.CommandName == "Add")
                {

                    #region Insert Item Template Variables
                    TextBox tb_location_code_insert = (TextBox)e.Item.FindControl("tb_location_code_insert");
                    DropDownList ddl_location_area_insert = (DropDownList)e.Item.FindControl("ddl_location_area_insert");
                    TextBox tb_mident_text_insert = (TextBox)e.Item.FindControl("tb_mident_text_insert");
                    TextBox tb_description_text_insert = (TextBox)e.Item.FindControl("tb_description_text_insert");
                    TextBox tb_description_code_insert = (TextBox)e.Item.FindControl("tb_description_code_insert"); //hide
                    TextBox tb_description_abbr_insert = (TextBox)e.Item.FindControl("tb_description_abbr_insert");
                    TextBox tb_street_1_insert = (TextBox)e.Item.FindControl("tb_street_1_insert");
                    TextBox tb_street_2_insert = (TextBox)e.Item.FindControl("tb_street_2_insert");
                    TextBox tb_city_insert = (TextBox)e.Item.FindControl("tb_city_insert");
                    TextBox tb_province_insert = (TextBox)e.Item.FindControl("tb_province_insert");
                    TextBox tb_postal_code_insert = (TextBox)e.Item.FindControl("tb_postal_code_insert");
                    TextBox tb_telephone_area_insert = (TextBox)e.Item.FindControl("tb_telephone_area_insert");
                    TextBox tb_telephone_number_insert = (TextBox)e.Item.FindControl("tb_telephone_number_insert");
                    TextBox tb_telephone_extension_insert = (TextBox)e.Item.FindControl("tb_telephone_extension_insert");
                    DropDownList ddl_geographic_area_code_insert = (DropDownList)e.Item.FindControl("ddl_geographic_area_code_insert");
                    DropDownList ddl_onsis_location_type_insert = (DropDownList)e.Item.FindControl("ddl_onsis_location_type_insert");
                    DropDownList ddl_record_status_insert = (DropDownList)e.Item.FindControl("ddl_record_status_insert");
                    TextBox tb_alternate_location_code_insert = (TextBox)e.Item.FindControl("tb_alternate_location_code_insert");
                    TextBox tb_school_code_insert = (TextBox)e.Item.FindControl("tb_school_code_insert");
                    TextBox tb_fax_area_insert = (TextBox)e.Item.FindControl("tb_fax_area_insert");
                    TextBox tb_fax_number_insert = (TextBox)e.Item.FindControl("tb_fax_number_insert");
                    TextBox tb_speed_dial_number_insert = (TextBox)e.Item.FindControl("tb_speed_dial_number_insert");
                    DropDownList ddl_panel_insert = (DropDownList)e.Item.FindControl("ddl_panel_insert");
                    DropDownList ddl_location_type_insert = (DropDownList)e.Item.FindControl("ddl_location_type_insert");
                    Label lbl_message_insert_insert = (Label)e.Item.FindControl("lbl_message_insert");
                    List<TextBox> text_box_null_check_insert = new List<TextBox> { };
                    #endregion

                    bool validInsert = false;
                    //Create new function is all "manual"
                    //Elementary and secondar school designation is in CDDBA.EC_school's panel
                    //EC_Location's Type code is the source of "SC" checking
                    //Check if user put SC before proceding.

                    //Record status is just active or inactive so maybe we can use a checkbox for that?
                    //So. Onsis Shouldnt be directly set?

                    //SC needs everything minus alt location code
                    //otherwise 
                    string verify_code = "SELECT COUNT(*) FROM [CDAS].[dbo].[hd_ec_locations] WHERE LOCATION_CODE = @location_code";
                    conn.Open();
                    using(SqlCommand command = new SqlCommand(verify_code, conn))
                    {
                        command.Parameters.AddWithValue("@location_code", tb_location_code_insert.Text.Trim());
                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            //lbl_message_insert.Text = "Location code already in use";
                            throw new Exception("Location code already in use.");
                        }
                        else
                        {
                            
                            if (string.IsNullOrEmpty(tb_location_code_insert.Text))
                            {
                                lbl_message_insert_insert.Text = "Location Code cannot be NULL";
                                throw new Exception("Location Code cannot be NULL");
                            }

                            if (string.IsNullOrEmpty(tb_description_text_insert.Text))
                            {
                                lbl_message_insert_insert.Text = "School Name cannot be NULL";
                                throw new Exception("School Name cannot be NULL");
                            }

                            if (string.IsNullOrEmpty(tb_description_abbr_insert.Text))
                            {
                                lbl_message_insert_insert.Text = "Description Abbreviation cannot be NULL";
                                throw new Exception("School Name Abbreviation cannot be NULL");
                            }

                            if (String.IsNullOrWhiteSpace(tb_street_1_insert.Text))
                            {
                                throw new Exception("Street address 1 needs to be entered");
                            }
                            if (String.IsNullOrWhiteSpace(tb_description_text_insert.Text))
                            {
                                throw new Exception("School Name needs to be entered");
                            }
                            if (String.IsNullOrWhiteSpace(tb_description_abbr_insert.Text))
                            {
                                throw new Exception("School needs abbreviated name");
                            }
                            if (String.IsNullOrWhiteSpace(tb_city_insert.Text))
                            {
                                throw new Exception("City name needs to be entered");
                            }
                            if (String.IsNullOrWhiteSpace(tb_province_insert.Text))
                            {
                                throw new Exception("Province needs to be entered");
                            }
                            if (String.IsNullOrWhiteSpace(tb_postal_code_insert.Text))
                            {
                                throw new Exception("Postal Code needs to be entered");
                            }
                            if (String.IsNullOrWhiteSpace(tb_mident_text_insert.Text))
                            {
                                throw new Exception("A MIDENT needs to be entered");
                            }
                            if (String.IsNullOrWhiteSpace(tb_school_code_insert.Text))
                            {
                                throw new Exception("School Code needs to be entered");
                            }
                            

                            //DropDowns
                            if (String.IsNullOrWhiteSpace(ddl_location_area_insert.SelectedValue))
                            {
                                throw new Exception("Admin Area need to be select");
                            }
                            if (String.IsNullOrWhiteSpace(ddl_location_type_insert.SelectedValue))
                            {
                                throw new Exception("Location Type need to be select");
                            }
                            if (String.IsNullOrWhiteSpace(ddl_onsis_location_type_insert.SelectedValue))
                            {
                                throw new Exception("Onsis Location Type need to be select");
                            }
                            if (String.IsNullOrWhiteSpace(ddl_geographic_area_code_insert.SelectedValue))
                            {
                                throw new Exception("Geographic Area Code need to be select");
                            }
                            if (String.IsNullOrWhiteSpace(ddl_record_status_insert.SelectedValue))
                            {
                                throw new Exception("Record Status need to be select");
                            }
                            if (String.IsNullOrWhiteSpace(ddl_panel_insert.SelectedValue))
                            {
                                throw new Exception("Panel need to be select");
                            }

                            if (ddl_location_type_insert.SelectedValue == "SC")
                            {
                                //Everything except alt loc code
                                if (tb_telephone_area_insert.Text.Length != tb_telephone_area_insert.MaxLength)
                                {
                                    throw new Exception("Telephone Area must only be " + tb_telephone_area_insert.MaxLength + " digits");
                                }
                                if (tb_telephone_number_insert.Text.Length != tb_telephone_number_insert.MaxLength)
                                {
                                    throw new Exception("Telephone Number must only be " + tb_telephone_number_insert.MaxLength + " digits");
                                }
                                if (tb_telephone_extension_insert.Text.Length != tb_telephone_extension_insert.MaxLength)
                                {
                                    throw new Exception("Location Code must only be " + tb_telephone_extension_insert.MaxLength + " digits");
                                }
                                if (tb_fax_area_insert.Text.Length != tb_fax_area_insert.MaxLength)
                                {
                                    throw new Exception("Fax Area must only be " + tb_fax_area_insert.MaxLength + " digits");
                                }
                                if (tb_fax_number_insert.Text.Length != tb_fax_number_insert.MaxLength)
                                {
                                    throw new Exception("Fax Number must only be " + tb_fax_number_insert.MaxLength + " digits");
                                }
                                if (tb_speed_dial_number_insert.Text.Length != tb_speed_dial_number_insert.MaxLength)
                                {
                                    throw new Exception("Speed dial number must only be " + tb_speed_dial_number_insert.MaxLength + " digits");
                                }
                            }

                            query = "INSERT INTO [CDAS].[dbo].[hd_ec_locations] (location_code, location_area, location_type, mident, description_text, description_abbr, street_1, street_2, city, province, " +
                                    "postal_code, telephone_area, telephone_no, telephone_ext, geographic_area_code, onsis_location_type, record_status, alternate_location_code, speed_dial_number, fax_area, fax_no, panel, school_code) " +
                                    "VALUES (@location_code, @location_area, @location_type, @mident, @description_text, @description_abbr, @street_1, @street_2, @city, @province, " +
                                    " @postal_code, @telephone_area, @telephone_no, @telephone_ext, @geographic_area_code, @onsis_location_type, @record_status, @alternate_location_code, @speed_dial_number, @fax_area, @fax_no, @panel, @school_code)";

                            using (cmd = new SqlCommand(query, conn))
                            {
                                #region Edit Parameters
                                cmd.Parameters.AddWithValue("@alternate_location_code", tb_alternate_location_code_insert.Text.Trim());
                                cmd.Parameters.AddWithValue("@location_area", ddl_location_area_insert.SelectedValue);
                                cmd.Parameters.AddWithValue("@location_type", ddl_location_type_insert.SelectedValue);
                                cmd.Parameters.AddWithValue("@onsis_location_type", ddl_onsis_location_type_insert.SelectedValue);
                                cmd.Parameters.AddWithValue("@description_text", tb_description_text_insert.Text.Trim());
                                cmd.Parameters.AddWithValue("@description_abbr", tb_description_abbr_insert.Text.Trim());
                                cmd.Parameters.AddWithValue("@street_1", tb_street_1_insert.Text.Trim());
                                cmd.Parameters.AddWithValue("@street_2", tb_street_2_insert.Text.Trim());
                                cmd.Parameters.AddWithValue("@city", tb_city_insert.Text.Trim());
                                cmd.Parameters.AddWithValue("@province", tb_province_insert.Text.Trim());

                                cmd.Parameters.AddWithValue("@postal_code", tb_postal_code_insert.Text.Trim());
                                cmd.Parameters.AddWithValue("@mident", tb_mident_text_insert.Text.Trim());
                                cmd.Parameters.AddWithValue("@telephone_area", tb_telephone_area_insert.Text.Trim());
                                cmd.Parameters.AddWithValue("@telephone_no", tb_telephone_number_insert.Text.Trim());
                                cmd.Parameters.AddWithValue("@telephone_ext", tb_telephone_extension_insert.Text.Trim());
                                cmd.Parameters.AddWithValue("@geographic_area_code", ddl_geographic_area_code_insert.SelectedValue);
                                cmd.Parameters.AddWithValue("@speed_dial_number", tb_speed_dial_number_insert.Text.Trim());
                                cmd.Parameters.AddWithValue("@fax_area", tb_fax_area_insert.Text.Trim());
                                cmd.Parameters.AddWithValue("@fax_no", tb_fax_number_insert.Text.Trim());
                                cmd.Parameters.AddWithValue("@school_code", tb_school_code_insert.Text.Trim());

                                cmd.Parameters.AddWithValue("@record_status", ddl_record_status_insert.SelectedValue);
                                cmd.Parameters.AddWithValue("@panel", ddl_panel_insert.SelectedValue);
                                cmd.Parameters.AddWithValue("@location_code", tb_location_code_insert.Text.Trim());
                                #endregion

                                //conn.Open();
                                cmd.ExecuteNonQuery();
                                conn.Close();
                            }

                            lbl_message_insert.Text = "Record Successfully Added";
                        }
                    }
                    conn.Close();
                }
                else if(e.CommandName == "Cancel")
                {
                    //lb_clear.Visible = false;
                    lbl_record_count.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);

                if (lbl_message != null)
                {
                    lbl_message.ForeColor = System.Drawing.Color.Red;
                    lbl_message.Text = ex.Message;
                    if (!string.IsNullOrEmpty(query) && cmd != null)
                    {
                        lbl_message.Text += cmd.CommandText;
                    }
                }

                if (lbl_message_insert != null)
                {
                    lbl_message_insert.ForeColor = System.Drawing.Color.Red;
                    lbl_message_insert.Text = ex.Message;
                    if (!string.IsNullOrEmpty(query) && cmd != null)
                    {
                        lbl_message_insert.Text += cmd.CommandText;
                    }
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
            
            TextBox tb_record_status = (TextBox)e.Item.FindControl("tb_record_status");
            TextBox tb_alternate_location_code = (TextBox)e.Item.FindControl("tb_alternate_location_code");
            TextBox tb_school_code = (TextBox)e.Item.FindControl("tb_school_code");
            TextBox tb_fax_area = (TextBox)e.Item.FindControl("tb_fax_area");
            TextBox tb_fax_number = (TextBox)e.Item.FindControl("tb_fax_number");
            TextBox tb_panel = (TextBox)e.Item.FindControl("tb_panel");
            TextBox tb_speed_dial_number = (TextBox)e.Item.FindControl("tb_speed_dial_number");
            Label lbl_location_code = (Label)e.Item.FindControl("lbl_location_code");
            Label lbl_message = (Label)e.Item.FindControl("lbl_message");
            //DropDownList ddl_onsis_location_type = (DropDownList)e.Item.FindControl("ddl_onsis_location_type");
            #endregion

            
            if (e.Item.ItemType == ListViewItemType.DataItem && (e.Item as ListViewDataItem).DisplayIndex == lv_maint.EditIndex)
            {
                DropDownList ddl_panel = (DropDownList)e.Item.FindControl("ddl_panel");
                DropDownList ddl_record_status = (DropDownList)e.Item.FindControl("ddl_record_status");
                DropDownList ddl_onsis = (DropDownList)e.Item.FindControl("ddl_onsis_location_type");
                DropDownList ddl_geographic = (DropDownList)e.Item.FindControl("ddl_geographic_area_code");
                DropDownList ddl_location_type = (DropDownList)e.Item.FindControl("ddl_location_type");
                DropDownList ddl_location_area = (DropDownList)e.Item.FindControl("ddl_location_area");

                DataRowView current_row = (DataRowView)e.Item.DataItem;

                string raw_value = current_row["panel"] == DBNull.Value ? null : current_row["panel"].ToString();

                if(string.IsNullOrEmpty(raw_value))
                {
                    //Null. Display Blank Space
                    ddl_panel.Items.Insert(0, new ListItem("", ""));
                    ddl_panel.SelectedIndex = 0;
                }
                else
                {
                    ListItem valid_value = ddl_panel.Items.FindByValue(raw_value);
                    if(valid_value != null)
                    {
                        //Not null and have value.
                        ddl_panel.SelectedValue = raw_value;
                    }
                    else
                    {
                        //Not Null. No Value. Add Value.
                        ddl_panel.Items.Insert(0, new ListItem(raw_value, raw_value));
                        ddl_panel.SelectedIndex = 0;
                    }
                }

                //ddl_record_status.DataBind();
                raw_value = current_row["record_status"] == DBNull.Value ? null : current_row["record_status"].ToString();
                if (string.IsNullOrEmpty(raw_value))
                {
                    //Null. Display Blank Space
                    ddl_record_status.Items.Insert(0, new ListItem("", ""));
                    ddl_record_status.SelectedIndex = 0;
                }
                else
                {
                    ListItem valid_value = ddl_record_status.Items.FindByValue(raw_value);
                    if (valid_value != null)
                    {
                        //Not null and have value.
                        ddl_record_status.SelectedValue = raw_value;
                    }
                    else
                    {
                        //Not Null. No Value. Add Value.
                        ddl_record_status.Items.Insert(0, new ListItem(raw_value, raw_value));
                        ddl_record_status.SelectedIndex = 0;
                    }
                }

                
                //ddl_onsis.DataBind();
                raw_value = current_row["onsis_location_type"] == DBNull.Value ? null : current_row["onsis_location_type"].ToString();
                if (string.IsNullOrEmpty(raw_value))
                {
                    //Null. Display Blank Space
                    ddl_onsis.Items.Insert(0, new ListItem("", ""));
                    ddl_onsis.SelectedIndex = 0;
                }
                else
                {
                    ListItem valid_value = ddl_onsis.Items.FindByValue(raw_value);
                    if (valid_value != null)
                    {
                        //Not null and have value.
                        ddl_onsis.SelectedValue = raw_value;
                    }
                    else
                    {
                        //Not Null. No Value. Add Value.
                        ddl_onsis.Items.Insert(0, new ListItem(raw_value, raw_value));
                        ddl_onsis.SelectedIndex = 0;
                    }
                }
                
                //ddl_geographic.DataBind();
                raw_value = current_row["geographic_area_code"] == DBNull.Value ? null : current_row["geographic_area_code"].ToString();
                if (string.IsNullOrEmpty(raw_value))
                {
                    //Null. Display Blank Space
                    ddl_geographic.Items.Insert(0, new ListItem("", ""));
                    ddl_geographic.SelectedIndex = 0;
                }
                else
                {
                    ListItem valid_value = ddl_geographic.Items.FindByValue(raw_value);
                    if (valid_value != null)
                    {
                        //Not null and have value.
                        //ddl_geographic.ClearSelection();
                        //valid_value.Selected = true;
                        ddl_geographic.SelectedValue = raw_value;
                    }
                    else
                    {
                        //Not Null. No Value. Add Value.
                        ddl_geographic.Items.Insert(0, new ListItem(raw_value, raw_value));
                        ddl_geographic.SelectedIndex = 0;
                    }
                }
                
                raw_value = current_row["location_type"] == DBNull.Value ? null : current_row["location_type"].ToString();
                if (string.IsNullOrEmpty(raw_value))
                {
                    //Null. Display Blank Space
                    ddl_location_type.Items.Insert(0, new ListItem("", ""));
                    ddl_location_type.SelectedIndex = 0;
                }
                else
                {
                    ListItem valid_value = ddl_location_type.Items.FindByValue(raw_value);
                    if (valid_value != null)
                    {
                        //Not null and have value.
                        ddl_location_type.SelectedValue = raw_value;
                    }
                    else
                    {
                        //Not Null. No Value. Add Value.
                        ddl_location_type.Items.Insert(0, new ListItem(raw_value, raw_value));
                        ddl_location_type.SelectedIndex = 0;
                    }
                }

                raw_value = current_row["location_area"] == DBNull.Value ? null : current_row["location_area"].ToString();
                if (string.IsNullOrEmpty(raw_value))
                {
                    //Null. Display Blank Space
                    ddl_location_area.Items.Insert(0, new ListItem("", ""));
                    ddl_location_area.SelectedIndex = 0;
                }
                else
                {
                    ListItem valid_value = ddl_location_area.Items.FindByValue(raw_value);
                    if (valid_value != null)
                    {
                        //Not null and have value.
                        ddl_location_area.SelectedValue = raw_value;
                    }
                    else
                    {
                        //Not Null. No Value. Add Value.
                        ddl_location_area.Items.Insert(0, new ListItem(raw_value, raw_value));
                        ddl_location_area.SelectedIndex = 0;
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
            lbl_record_count.Visible = false;
            lv_maint.DataSource = null;
            lv_maint.Items.Clear();
            lv_maint.DataBind();
            ViewState["query"] = null;
            //lb_clear.Visible = true;
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

        protected void ddl_location_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl_location_type = (DropDownList)sender;
            ListViewItem item = (ListViewItem)ddl_location_type.NamingContainer;
            //if (!Page.IsPostBack)
            //{
                if (item != null)
                {
                    Label lbl_alternate_location_code = (Label)item.FindControl("lbl_alternate_location_code");
                    Label lbl_telephone = (Label)item.FindControl("lbl_telephone");
                    Label lbl_fax_area = (Label)item.FindControl("lbl_fax_area");
                    Label lbl_speed_dial_number = (Label)item.FindControl("lbl_speed_dial_number");
                    Label lbl_telephone_extension = (Label)item.FindControl("lbl_telephone_extension");

                    if (ddl_location_type.SelectedValue == "SC")
                    {
                        lbl_alternate_location_code.Text = "Alternate Location Code:  ";
                        lbl_telephone.Text = "Phone: *(";
                        lbl_fax_area.Text = "Fax: *(";
                        lbl_speed_dial_number.Text = "Speed Dial Number:*";
                        lbl_telephone_extension.Text = "Extension:*";
                    }
                    else
                    {
                        lbl_alternate_location_code.Text = "Alternate Location Code:  ";
                        lbl_telephone.Text = "Phone:  (";
                        lbl_fax_area.Text = "Fax:  (";
                        lbl_speed_dial_number.Text = "Speed Dial Number: ";
                        lbl_telephone_extension.Text = "Extension: ";
                    }
                }
            //}
            //lv_maint.DataBind();
        }

        protected void ddl_location_type_insert_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl_location_type_insert = (DropDownList)sender;
            ListViewItem item = (ListViewItem)ddl_location_type_insert.NamingContainer;

            if (item != null) 
            {
                Label lbl_alternate_location_code = (Label)item.FindControl("lbl_alternate_location_code_insert");
                Label lbl_telephone = (Label)item.FindControl("lbl_telephone_insert");
                Label lbl_fax_area = (Label)item.FindControl("lbl_fax_area_insert");
                Label lbl_speed_dial_number = (Label)item.FindControl("lbl_speed_dial_number_insert");
                Label lbl_telephone_extension = (Label)item.FindControl("lbl_speed_dial_number_insert");

                if (ddl_location_type_insert.SelectedValue == "SC")
                {
                    lbl_alternate_location_code.Text = "Alternate Location Code:  ";
                    lbl_telephone.Text = "Phone: *(";
                    lbl_fax_area.Text = "Fax: *(";
                    lbl_speed_dial_number.Text = "Speed Dial Number:*";
                    lbl_telephone_extension.Text = "Extension:*";
                }
                else
                {
                    lbl_alternate_location_code.Text = "Alternate Location Code:  ";
                    lbl_telephone.Text = "Phone:  (";
                    lbl_fax_area.Text = "Fax:  (";
                    lbl_speed_dial_number.Text = "Speed Dial Number: ";
                    lbl_telephone_extension.Text = "Extension: ";
                }
            }
            //lv_maint.DataBind();
        }
    }
}