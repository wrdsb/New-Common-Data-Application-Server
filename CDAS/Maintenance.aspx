<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Maintenance.aspx.cs" Inherits="CDAS.Maintenance" MasterPageFile="~/Main_User.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/mystyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <asp:Label ID="lbl_search" runat="server" Text="Code: "></asp:Label>
            <asp:TextBox ID="tb_search_code" runat="server" Width="40"></asp:TextBox>
            <asp:Label ID="lbl_description" runat="server" Text="Description: "></asp:Label>
            <asp:TextBox ID="tb_search_description" runat="server"></asp:TextBox>
            <asp:DropDownList ID="ddl_panel_type" runat="server" DataTextField="ABBRV_NAME" DataValueField="PANEL" Width="218" OnDataBound="ddl_panel_type_DataBound" DataSourceID="sds_school_type"></asp:DropDownList>
            <asp:LinkButton ID="lb_search" runat="server" Text="Search" OnClick="btn_search_Click" CssClass="btn btn-default"></asp:LinkButton>
            <asp:LinkButton ID="lb_clear" runat="server" Text="Clear" OnClick="btn_clear_Click" CssClass="btn btn-default"></asp:LinkButton>
            <asp:LinkButton ID="lb_insert" runat="server" Text="Create New" OnClick="lb_insert_Click" CssClass="btn btn-default"></asp:LinkButton>
        </div>
        <div style="clear: both;"></div>
        <div class="row" style="min-height: 660px;">
            <div class="col-md-12">
                <br />
                <asp:ListView ID="lv_maint" runat="server" DataKeyNames="location_code"
                    OnItemDataBound="lv_maint_ItemDataBound"
                    OnItemCommand="lv_maint_ItemCommand"
                    OnItemEditing="lv_maint_ItemEditing"
                    OnItemUpdating="lv_maint_ItemUpdating"
                    OnItemCanceling="lv_maint_ItemCanceling"
                    OnItemInserting="lv_maint_ItemInserting"
                    OnPagePropertiesChanging="lv_maint_PagePropertiesChanging">
                    <LayoutTemplate>
                        <table class="table table-list">
                            <tr>
                                <th style="text-align: left; width: 200px;">
                                    <d>Code</d></th>
                                <th style="text-align: left; width: 400px;">
                                    <d>Location Details</d></th>
                                <th style="text-align: left; width: 500px;">
                                    <d>Description</d></th>
                                <th style="text-align: left; width: 500px;">
                                    <d>Address</d></th>
                                <th style="text-align: left; width: 800px;">
                                    <d>Phone</d></th>
                                <th style="text-align: left; width: 400px;">
                                    <d>Bell Time</d></th>
                                <th style="text-align: left; width: 800px;">
                                    <d>Site Info</d></th>
                                <th></th>
                                <th></th>
                                <th></th>
                            </tr>
                            <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_location_code" runat="server" Text='<%#Eval("location_code") %>'></asp:Label>
                            </td>
                            <td>Alt Code:
                                <asp:Label ID="lbl_alternate_location_code" runat="server" Text='<%#Eval("alternate_location_code") %>'></asp:Label><br />
                                Admin Area:
                                <asp:Label ID="lbl_location_area" runat="server" Text='<%#Eval("location_area") %>'></asp:Label><br />
                                Loc. Type:
                                <asp:Label ID="lbl_location_type" runat="server" Text='<%#Eval("location_type") %>'></asp:Label><br />
                                Onsis:
                                <asp:Label ID="lbl_onsis_location_type" runat="server" Text='<%#Eval("onsis_location_type") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_description" runat="server" Text='<%#Eval("description_text") %>'></asp:Label><br />
                                <asp:Label ID="lbl_description_abbr" runat="server" Text='<%#Eval("description_abbr") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_street_1" runat="server" Text='<%#Eval("street_1") %>'></asp:Label><br />
                                <asp:Label ID="lbl_street_2" runat="server" Text='<%#Eval("street_2") %>'></asp:Label><br />
                                <asp:Label ID="lbl_city" runat="server" Text='<%#Eval("city") %>'></asp:Label><br />
                                <asp:Label ID="lbl_province" runat="server" Text='<%#Eval("province") %>'></asp:Label><br />
                                <asp:Label ID="lbl_postal_code" runat="server" Text='<%#Eval("postal_code") %>'></asp:Label><br />
                                <asp:Label ID="lbl_mident" runat="server" Text='<%#Eval("mident") %>'></asp:Label>
                            </td>
                            <td>(
                                <asp:Label ID="lbl_telephone_area" runat="server" Text='<%#Eval("telephone_area") %>'></asp:Label>
                                )
                                <asp:Label ID="lbl_telephone_no" runat="server" Text='<%#Eval("telephone_no") %>'></asp:Label><br />
                                Ext:
                                <asp:Label ID="lbl_telephone_ext" runat="server" Text='<%#Eval("telephone_ext") %>'></asp:Label><br />
                                Geo:
                                <asp:Label ID="lbl_geographic_area_code" runat="server" Text='<%#Eval("geographic_area_code") %>'></asp:Label><br />
                                Speed Dial:
                                <asp:Label ID="lbl_speed_dial_number" runat="server" Text='<%#Eval("speed_dial_number") %>'></asp:Label><br />
                                Fax:
                                <asp:Label ID="lbl_fax_area" runat="server" Text='<%#Eval("fax_area") %>' Width="30"></asp:Label>
                                -
                                <asp:Label ID="lbl_fax_no" runat="server" Text='<%#Eval("fax_no") %>' Width="80"></asp:Label>
                            </td>
                            <td>Start Time:
                                <asp:Label ID="lbl_start_time" runat="server" Text='<%#Eval("start_time","{0:MM-mm}") %>' TextMode="Time"></asp:Label><br />
                                End Time:
                                <asp:Label ID="lbl_end_time" runat="server" Text='<%#Eval("end_time","{0:MM-mm}") %>' TextMode="Time"></asp:Label>
                            </td>
                            <td>School Code:
                                <asp:Label ID="lbl_code" runat="server" Text='<%#Eval("school_code") %>'></asp:Label><br />
                                Semester:
                                <asp:Label ID="lbl_semester_code" runat="server" Text='<%#Eval("semester_code") %>'></asp:Label><br />
                                Record Status:
                                <asp:Label ID="lbl_record_status" runat="server" Text='<%#Eval("record_status") %>'></asp:Label><br />
                                Panel:
                                <asp:Label ID="lbl_panel" runat="server" Text='<%#Eval("panel") %>'></asp:Label><br />
                                FR:
                                <asp:Label ID="lbl_fr_immersion" runat="server" Text='<%#Eval("fr_immersion") %>'></asp:Label>
                            </td>
                            <td>
                            <td style="text-align: right;">
                                <asp:Button ID="btn_edit" runat="server" Text="Edit" CommandName="Edit" CssClass="btn btn-default" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_location_code" runat="server" Text='<%#Eval("location_code") %>'></asp:Label>
                            </td>
                            <td>Alt Code:
                                <asp:Label ID="lbl_alternate_location_code" runat="server" Text='<%#Eval("alternate_location_code") %>'></asp:Label><br />
                                Admin Area:
                                <asp:Label ID="lbl_location_area" runat="server" Text='<%#Eval("location_area") %>'></asp:Label><br />
                                Loc. Type:
                                <asp:Label ID="lbl_location_type" runat="server" Text='<%#Eval("location_type") %>'></asp:Label><br />
                                Onsis:
                                <asp:Label ID="lbl_onsis_location_type" runat="server" Text='<%#Eval("onsis_location_type") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_description" runat="server" Text='<%#Eval("description_text") %>'></asp:Label><br />
                                <asp:Label ID="lbl_description_abbr" runat="server" Text='<%#Eval("description_abbr") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_street_1" runat="server" Text='<%#Eval("street_1") %>'></asp:Label><br />
                                <asp:Label ID="lbl_street_2" runat="server" Text='<%#Eval("street_2") %>'></asp:Label><br />
                                <asp:Label ID="lbl_city" runat="server" Text='<%#Eval("city") %>'></asp:Label><br />
                                <asp:Label ID="lbl_province" runat="server" Text='<%#Eval("province") %>'></asp:Label><br />
                                <asp:Label ID="lbl_postal_code" runat="server" Text='<%#Eval("postal_code") %>'></asp:Label><br />
                                <asp:Label ID="lbl_mident" runat="server" Text='<%#Eval("mident") %>'></asp:Label>
                            </td>
                            <td>(
                                <asp:Label ID="lbl_telephone_area" runat="server" Text='<%#Eval("telephone_area") %>'></asp:Label>
                                )
                                <asp:Label ID="lbl_telephone_no" runat="server" Text='<%#Eval("telephone_no") %>'></asp:Label><br />
                                Ext:
                                <asp:Label ID="lbl_telephone_ext" runat="server" Text='<%#Eval("telephone_ext") %>'></asp:Label><br />
                                Geo:
                                <asp:Label ID="lbl_geographic_area_code" runat="server" Text='<%#Eval("geographic_area_code") %>'></asp:Label><br />
                                Speed Dial:
                                <asp:Label ID="lbl_speed_dial_number" runat="server" Text='<%#Eval("speed_dial_number") %>'></asp:Label><br />
                                Fax:
                                <asp:Label ID="lbl_fax_area" runat="server" Text='<%#Eval("fax_area") %>' Width="30"></asp:Label>
                                -
                                <asp:Label ID="lbl_fax_no" runat="server" Text='<%#Eval("fax_no") %>' Width="70"></asp:Label>
                            </td>
                            <td>Start Time:
                                <asp:Label ID="lbl_start_time" runat="server" Text='<%#Eval("start_time","{0:MM-mm}") %>' TextMode="Time"></asp:Label><br />
                                End Time:
                                <asp:Label ID="lbl_end_time" runat="server" Text='<%#Eval("end_time","{0:MM-mm}") %>' TextMode="Time"></asp:Label>
                            </td>
                            <td>School Code:
                                <asp:Label ID="lbl_code" runat="server" Text='<%#Eval("school_code") %>'></asp:Label><br />
                                Semester:
                                <asp:Label ID="lbl_semester_code" runat="server" Text='<%#Eval("semester_code") %>'></asp:Label><br />
                                Record Status:
                                <asp:Label ID="lbl_record_status" runat="server" Text='<%#Eval("record_status") %>'></asp:Label><br />
                                Panel:
                                <asp:Label ID="lbl_panel" runat="server" Text='<%#Eval("panel") %>'></asp:Label><br />
                                FR:
                                <asp:Label ID="lbl_fr_immersion" runat="server" Text='<%#Eval("fr_immersion") %>'></asp:Label>
                            </td>
                            <td>
                            <td style="text-align: right;">
                                <asp:Button ID="btn_edit" runat="server" Text="Edit" CommandName="Edit" CssClass="btn btn-default" />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EditItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_location_code" runat="server" Text="Location Code: *"></asp:Label>
                                <asp:TextBox ID="tb_location_code" runat="server" Text='<%#Bind("location_code") %>' Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_alternate_location_code" runat="server" Text="Alternate Location Code:"></asp:Label>
                                <asp:TextBox ID="tb_alternate_location_code" runat="server" Text='<%#Bind("alternate_location_code") %>' MaxLength="8"></asp:TextBox><br />
                                <asp:Label ID="lbl_location_area" runat="server" Text="Admin Area: *"></asp:Label>
                                <asp:DropDownList ID="ddl_location_area" runat="server" DataSourceID="sds_admin_area" DataTextField="CODE" DataValueField="code"></asp:DropDownList><br />
                                <asp:Label ID="lbl_location_type" runat="server" Text="Location Type: *"></asp:Label>
                                <asp:DropDownList ID="ddl_location_type" runat="server" DataSourceID="sds_location_type" DataTextField="CODE" DataValueField="code" AppendDataBoundItems="true">
                                </asp:DropDownList><br />
                                <asp:Label ID="lbl_onsis_location_type" runat="server" Text="ONSIS Location Type*"></asp:Label>
                                <asp:DropDownList ID="ddl_onsis_location_type" runat="server" DataSourceID="sds_panel" DataTextField="PANEL" DataValueField="Panel" AppendDataBoundItems="true">
                                </asp:DropDownList><br />
                            </td>
                            <td>
                                <asp:Label ID="lbl_description_text" runat="server" Text="Description Text: *"></asp:Label>
                                <asp:TextBox ID="tb_description_text" runat="server" Text='<%#Bind("description_text") %>' MaxLength="70"></asp:TextBox><br />
                                <asp:Label ID="lbl_description_abbr" runat="server" Text="Description Abbr: *"></asp:Label>
                                <asp:TextBox ID="tb_description_abbr" runat="server" Text='<%#Bind("description_abbr") %>' MaxLength="16"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_street_1" runat="server" Text="Street 1:*"></asp:Label>
                                <asp:TextBox ID="tb_street_1" runat="server" Text='<%#Bind("street_1") %>' MaxLength="30"></asp:TextBox><br />
                                <asp:Label ID="lbl_street_2" runat="server" Text="Street 2:*"></asp:Label>
                                <asp:TextBox ID="tb_street_2" runat="server" Text='<%#Bind("street_2") %>' MaxLength="30"></asp:TextBox><br />
                                <asp:Label ID="lbl_city" runat="server" Text="City:*"></asp:Label>
                                <asp:TextBox ID="tb_city" runat="server" Text='<%#Bind("city") %>' MaxLength="30"></asp:TextBox><br />
                                <asp:Label ID="lbl_province" runat="server" Text="Province:*"></asp:Label>
                                <asp:TextBox ID="tb_province" runat="server" Text='<%#Bind("province") %>' MaxLength="30"></asp:TextBox><br />
                                <asp:Label ID="lbl_postal_code" runat="server" Text="Postal Code:*"></asp:Label>
                                <asp:TextBox ID="tb_postal_code" runat="server" Text='<%#Bind("postal_code") %>' MaxLength="7"></asp:TextBox><br />
                                <asp:Label ID="lbl_mident_text" runat="server" Text="Mident: *"></asp:Label>
                                <asp:TextBox ID="tb_mident_text" runat="server" Text='<%#Bind("mident") %>' MaxLength="6"></asp:TextBox>
                                <asp:HiddenField ID="hf_street_1" runat="server" Value='<%#Eval("street_1") %>' />
                                <asp:HiddenField ID="hf_street_2" runat="server" Value='<%#Eval("street_2") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lbl_telephone" runat="server" Text="Phone:* ("></asp:Label>
                                <asp:TextBox ID="tb_telephone_area" runat="server" Text='<%#Bind("telephone_area") %>' Width="40" MaxLength="3"></asp:TextBox>
                                <asp:Label ID="Label1" runat="server" Text=") "></asp:Label>
                                <asp:TextBox ID="tb_telephone_number" runat="server" Text='<%#Bind("telephone_no") %>' Width="70" MaxLength="7"></asp:TextBox><br />
                                <asp:Label ID="lbl_telephone_extension" runat="server" Text="Extension:*"></asp:Label>
                                <asp:TextBox ID="tb_telephone_extension" runat="server" Text='<%#Bind("telephone_ext") %>' MaxLength="4"></asp:TextBox><br />
                                <asp:Label ID="lbl_geographic_area_code" runat="server" Text="Geographic Area Code:*"></asp:Label>
                                <asp:DropDownList ID="ddl_geographic_area_code" runat="server" SelectedValue='<%#Bind("geographic_area_code") %>'>
                                    <asp:ListItem Text="KIT" Value="KIT"></asp:ListItem>
				                    <asp:ListItem Text="WAT" Value="WAT"></asp:ListItem>
                                    <asp:ListItem Text="CAM" Value="CAM"></asp:ListItem>
                                </asp:DropDownList><br />
                                <asp:Label ID="lbl_speed_dial_number" runat="server" Text="Speed Dial Number:*"></asp:Label>
                                <asp:TextBox ID="tb_speed_dial_number" runat="server" Text='<%#Bind("speed_dial_number") %>' MaxLength="3"></asp:TextBox><br />
                                <asp:Label ID="lbl_fax_area" runat="server" Text="Fax:*("></asp:Label>
                                <asp:TextBox ID="tb_fax_area" runat="server" Text='<%#Bind("fax_area") %>' MaxLength="3" Width="40"></asp:TextBox>
                                <asp:Label ID="Label2" runat="server" Text=") "></asp:Label>
                                <asp:TextBox ID="tb_fax_number" runat="server" Text='<%#Bind("fax_no") %>' Width="70" MaxLength="7"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_start_time" runat="server" Text="Start Time:*"></asp:Label>
                                <asp:TextBox ID="tb_start_time" runat="server" Text='<%#Bind("start_time","{0:MM-mm}") %>' TextMode="Time"></asp:TextBox><br />
                                <asp:Label ID="lbl_end_time" runat="server" Text="End Time:*"></asp:Label>
                                <asp:TextBox ID="tb_end_time" runat="server" Text='<%#Bind("end_time","{0:MM-mm}") %>' TextMode="Time"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_school_code" runat="server" Text="School Code:*"></asp:Label>
                                <asp:TextBox ID="tb_school_code" runat="server" Text='<%#Bind("school_code") %>' MaxLength="8"></asp:TextBox><br />
                                <asp:Label ID="lbl_semester_code" runat="server" Text="Semester Code:*"></asp:Label>
                                <asp:DropDownList ID="ddl_semester_code" runat="server" SelectedValue='<%#Bind("semester_code") %>'>
                                    <asp:ListItem Text="Semestered" Value="Semestered"></asp:ListItem>
									<asp:ListItem Text="Not Semestered" Value="Not Semestered"></asp:ListItem>
                                </asp:DropDownList><br />
                                <asp:Label ID="lbl_record_status" runat="server" Text="Record Status:*"></asp:Label>
                                <asp:DropDownList ID="ddl_record_status" runat="server" SelectedValue='<%#Bind("record_status") %>'>
                                    <asp:ListItem Text="A" Value="A"></asp:ListItem>
				                    <asp:ListItem Text="I" Value="I"></asp:ListItem>
                                </asp:DropDownList><br />
                                <asp:Label ID="lbl_panel" runat="server" Text="Panel:*"></asp:Label>
                                <asp:DropDownList ID="ddl_panel" runat="server" DataSourceID="sds_panel" DataTextField="PANEL" DataValueField="Panel" AppendDataBoundItems="true">
                                </asp:DropDownList><br />
                                <asp:Label ID="lbl_fr_immersion" runat="server" Text="FR Immersion:*"></asp:Label>
                                <asp:TextBox ID="tb_fr_immersion" runat="server"  MaxLength="11" Text='<%#Bind("fr_immersion") %>'></asp:TextBox>
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="btn btn-success" CommandName="Modify" />
                                <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="btn btn-default" CommandName="Cancel" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8" style="text-align: center;">
                                <asp:Label ID="lbl_message" runat="server"></asp:Label></td>
                        </tr>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_location_code" runat="server" Text="Location Code: *"></asp:Label>
                                <asp:TextBox ID="tb_location_code" runat="server" MaxLength="8"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_alternate_location_code" runat="server" Text="Alternate Location Code:"></asp:Label>
                                <asp:TextBox ID="tb_alternate_location_code" runat="server" MaxLength="8"></asp:TextBox><br />
                                <asp:Label ID="lbl_location_area" runat="server" Text="Admin Area: *"></asp:Label>
                                <asp:DropDownList ID="ddl_location_area" runat="server" DataSourceID="sds_admin_area" DataTextField="CODE" DataValueField="code"></asp:DropDownList><br />
                                <asp:Label ID="lbl_location_type" runat="server" Text="Location Type: *"></asp:Label>
                                <asp:DropDownList ID="ddl_location_type" runat="server" DataSourceID="sds_location_type" DataTextField="CODE" DataValueField="code" AppendDataBoundItems="true">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList><br />
                                <asp:Label ID="lbl_onsis_location_type" runat="server" Text="ONSIS Location Type*"></asp:Label>
                                <asp:DropDownList ID="ddl_onsis_location_type" runat="server" DataSourceID="sds_panel" DataTextField="PANEL" DataValueField="Panel" AppendDataBoundItems="true">
                                </asp:DropDownList><br />
                            </td>
                            <td>
                                <asp:Label ID="lbl_description_text" runat="server" Text="Description Text: *"></asp:Label>
                                <asp:TextBox ID="tb_description_text" runat="server" MaxLength="70"></asp:TextBox><br />
                                <asp:Label ID="lbl_description_abbr" runat="server" Text="Description Abbr: *"></asp:Label>
                                <asp:TextBox ID="tb_description_abbr" runat="server" MaxLength="16"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_street_1" runat="server" Text="Street 1:* "></asp:Label>
                                <asp:TextBox ID="tb_street_1" runat="server" MaxLength="30"></asp:TextBox><br />
                                <asp:Label ID="lbl_street_2" runat="server" Text="Street 2:* "></asp:Label>
                                <asp:TextBox ID="tb_street_2" runat="server" MaxLength="30"></asp:TextBox><br />
                                <asp:Label ID="lbl_city" runat="server" Text="City:*"></asp:Label>
                                <asp:TextBox ID="tb_city" runat="server" MaxLength="30"></asp:TextBox><br />
                                <asp:Label ID="lbl_province" runat="server" Text="Province:*"></asp:Label>
                                <asp:TextBox ID="tb_province" runat="server" MaxLength="30"></asp:TextBox><br />
                                <asp:Label ID="lbl_postal_code" runat="server" Text="Postal Code:*"></asp:Label>
                                <asp:TextBox ID="tb_postal_code" runat="server" MaxLength="7"></asp:TextBox><br />
                                <asp:Label ID="lbl_mident_text" runat="server" Text="Mident:*"></asp:Label>
                                <asp:TextBox ID="tb_mident_text" runat="server"  MaxLength="6"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_telephone" runat="server" Text="Phone: *("></asp:Label>
                                <asp:TextBox ID="tb_telephone_area" runat="server" Width="40" MaxLength="3"></asp:TextBox>
                                <asp:Label ID="Label1" runat="server" Text=") "></asp:Label>
                                <asp:TextBox ID="tb_telephone_number" runat="server"  Width="70" MaxLength="7"></asp:TextBox><br />
                                <asp:Label ID="lbl_telephone_extension" runat="server" Text="Extension:*"></asp:Label>
                                <asp:TextBox ID="tb_telephone_extension" runat="server" ></asp:TextBox><br />
                                <asp:Label ID="lbl_geographic_area_code" runat="server" Text="Geographic Area Code:*"></asp:Label>
                                <asp:DropDownList ID="ddl_geographic_area_code" runat="server" SelectedValue='<%#Bind("geographic_area_code") %>'>
                                    <asp:ListItem Text="KIT" Value="KIT"></asp:ListItem>
                                    <asp:ListItem Text="WAT" Value="WAT"></asp:ListItem>
                                    <asp:ListItem Text="CAM" Value="CAM"></asp:ListItem>
                                </asp:DropDownList><br />
                                <asp:Label ID="lbl_speed_dial_number" runat="server" Text="Speed Dial Number:*"></asp:Label>
                                <asp:TextBox ID="tb_speed_dial_number" runat="server" MaxLength="3"></asp:TextBox><br />
                                <asp:Label ID="lbl_fax_area" runat="server" Text="Fax: *("></asp:Label>
                                <asp:TextBox ID="tb_fax_area" runat="server" MaxLength="3" Width="40"></asp:TextBox>
                                <asp:Label ID="Label2" runat="server" Text=")"></asp:Label>
                                <asp:TextBox ID="tb_fax_number" runat="server" Width="70" MaxLength="7"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_start_time" runat="server" Text="Start Time:*"></asp:Label>
                                <asp:TextBox ID="tb_start_time" runat="server" Text='<%#Bind("start_time","{0:MM-mm}") %>' TextMode="Time"></asp:TextBox><br />
                                <asp:Label ID="lbl_end_time" runat="server" Text="End Time:*"></asp:Label>
                                <asp:TextBox ID="tb_end_time" runat="server" Text='<%#Bind("end_time","{0:MM-mm}") %>' TextMode="Time"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_school_code" runat="server" Text="School Code:*"></asp:Label>
                                <asp:TextBox ID="tb_school_code" runat="server" MaxLength="8"></asp:TextBox><br />
                                <asp:Label ID="lbl_semester_code" runat="server" Text="Semester Code:*"></asp:Label>
                                <asp:DropDownList ID="ddl_semester_code" runat="server">
                                    <asp:ListItem Text="Semestered" Value="Semestered"></asp:ListItem>
				                    <asp:ListItem Text="Not Semestered" Value="Not Semestered"></asp:ListItem>
                                </asp:DropDownList><br />
                                <asp:Label ID="lbl_record_status" runat="server" Text="Record Status:*"></asp:Label>
                                <asp:DropDownList ID="ddl_record_status" runat="server">
                                    <asp:ListItem Text="A" Value="Semestered"></asp:ListItem>
                                    <asp:ListItem Text="I" Value="Not Semestered"></asp:ListItem>
                                </asp:DropDownList><br />
                                <asp:Label ID="lbl_panel" runat="server" Text="Panel:*"></asp:Label>

                                <asp:DropDownList ID="ddl_panel" runat="server" DataSourceID="sds_panel" DataTextField="PANEL" DataValueField="Panel" AppendDataBoundItems="true">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList><br />
                                <asp:Label ID="lbl_fr_immersion" runat="server" Text="FR Immersion:*"></asp:Label>
                                <asp:TextBox ID="tb_fr_immersion" runat="server" MaxLength="11"></asp:TextBox>
                            </td>
                            <td style="text-align: right;">
                                <asp:LinkButton ID="lb_insert_insert" runat="server" CommandName="Add" CssClass="btn btn-default" Style="vertical-align: bottom;" ToolTip="Create Record" ValidationGroup="vg_save"><d>Create</d></asp:LinkButton>
                                <asp:Button ID="btn_cancel_insert" runat="server" Text="Cancel" CssClass="btn btn-default" CommandName="Cancel" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8" style="text-align: center;">
                                <asp:Label ID="lbl_message_insert" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </InsertItemTemplate>
                </asp:ListView>
                <asp:DataPager ID="dp_pager" PagedControlID="lv_maint" PageSize="25" runat="server">
                    <Fields>
                        <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" />
                    </Fields>
                </asp:DataPager>
                <asp:SqlDataSource ID="sds_location_type" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CDAS %>"
                    SelectCommand="select code, full_name, abbrv_name from [CDAS].[CDDBA].[EC_LOCATION_TYPE]"></asp:SqlDataSource>
                <asp:SqlDataSource ID="sds_panel" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CDAS %>"
                    SelectCommand="select panel from [CDAS].[CDDBA].[EC_PANEL]"></asp:SqlDataSource>
                <asp:SqlDataSource ID="sds_location" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CDAS %>"
                    SelectCommand="select full_name, abbrv_name from [CDAS].[CDDBA].[EC_LOCATION]"></asp:SqlDataSource>
                <asp:SqlDataSource ID="sds_admin_area" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CDAS %>"
                    SelectCommand="select code from [CDAS].[CDDBA].[EC_ADMIN_AREA]"></asp:SqlDataSource>
                <asp:SqlDataSource ID="sds_school_type" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CDAS %>"></asp:SqlDataSource>

                
            </div>
        </div>
    </div>
</asp:Content>
