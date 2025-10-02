<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Maintenance_Tables.aspx.cs" Inherits="CDAS.Maintenance_Tables" MasterPageFile="~/Main_User.Master" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/mystyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div style="clear: both;"></div>
        <div class="row" style="min-height: 660px;">
            <div class="col-md-12">
                <asp:SqlDataSource ID="sds_location_type" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CDAS %>"
                    SelectCommand="select code, full_name, abbrv_name from [CDAS].[CDDBA].[EC_LOCATION_TYPE]"></asp:SqlDataSource>
                <asp:SqlDataSource ID="sds_panel" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CDAS %>"
                    SelectCommand="select panel from [CDAS].[CDDBA].[EC_PANEL]"></asp:SqlDataSource>
                <asp:SqlDataSource ID="sds_location" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CDAS %>"
                    SelectCommand="select full_name, abbrv_name from [CDAS].[CDDBA].[EC_LOCATION]"></asp:SqlDataSource>
                <asp:SqlDataSource ID="sds_admin_area" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CDAS %>"
                    SelectCommand="select code from [CDAS].[CDDBA].[EC_ADMIN_AREA]"></asp:SqlDataSource>
                <asp:SqlDataSource ID="sds_school_type" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CDAS %>"></asp:SqlDataSource>


                
                <div id="insertion_area">
                <asp:Label ID="lbl_insert" runat="server" Text="Create New: " Visible="false"></asp:Label>
                <asp:Table ID="tbl_insertion" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                        <asp:Label ID="lbl_maintenance_tables" runat="server" Text="Maintenance Table: " Font-Bold="true"></asp:Label>                        
                        </asp:TableCell>
                        <asp:TableCell>
                        <asp:DropDownList ID="ddl_maintenance_tables" runat="server" OnSelectedIndexChanged="ddl_maintenance_tables_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="Select..." Value="None"></asp:ListItem>
                            <asp:ListItem Text="Location Type" Value="LocationType"></asp:ListItem>
                            <asp:ListItem Text="School Type" Value="SchoolType"></asp:ListItem>
                            <asp:ListItem Text="Panel" Value="Panel"></asp:ListItem>
                            <asp:ListItem Text="FOS" Value="AdminArea"></asp:ListItem>
                        </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="lbl_insert_code" runat="server" Text="Code: " Visible="false"></asp:Label>
                             <asp:Label ID="lbl_insert_panel" runat="server" Text="Panel: " Visible="false"></asp:Label>
                         </asp:TableCell>
                         <asp:TableCell>
                             <asp:TextBox ID="tb_insert_code" runat="server" MaxLength="8" Width="100" Visible="false" onkeydown="return triggerButtonOnEnter(event, '<%= btn_insert_maint_table.ClientID %>');"></asp:TextBox>
                             <asp:TextBox ID="tb_insert_panel" runat="server" MaxLength="1" Visible="false" Width="40" Text="" onkeydown="return triggerButtonOnEnter(event, '<%= btn_insert_maint_table.ClientID %>');"></asp:TextBox>
                         </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lbl_insert_full_name" runat="server" Text="Full Name: " Visible="false"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tb_insert_full_name" runat="server" MaxLength="70" Visible="false" onkeydown="return triggerButtonOnEnter(event, '<%= btn_insert_maint_table.ClientID %>');"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lbl_insert_abbrv_name" runat="server" Text="Abbreviated Name: " Visible="false"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tb_insert_abbrv_name" runat="server" MaxLength="16" Visible="false" onkeydown="return triggerButtonOnEnter(event, '<%= btn_insert_maint_table.ClientID %>');"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lbl_insert_status" runat="server" Text="Status: " Visible="false"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddl_insert_status" runat="server" Visible="false">
                                <asp:ListItem Text="Select.." Value=""></asp:ListItem>
                                <asp:ListItem Text="A" Value="A"></asp:ListItem>
                                <asp:ListItem Text="I" Value="I"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lbl_insert_employee_ID" runat="server" Text="Employee ID: " Visible="false"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tb_insert_employee_ID" runat="server" MaxLength="9" Visible="false" Width="100" onkeydown="return triggerButtonOnEnter(event, '<%= btn_insert_maint_table.ClientID %>');"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btn_insert_maint_table" runat="server" Text="Create" OnClick="btn_insert_maint_table_Click" Visible="false" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btn_clear" runat="server" Text="Clear" OnClick="btn_clear_Click" Visible="false"/>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                    </div>
                <asp:Panel ID="pnl_tables" runat="server" DefaultButton="">
                <asp:Label ID="lbl_error_message" runat="server" ForeColor="Red" ></asp:Label><br />
                
                <asp:GridView ID="gv_location_type" runat="server" Visible="false" AutoGenerateColumns="false" AutoGenerateEditButton="true" GridLines="Both" BorderWidth="1px" BorderStyle="Solid" CssClass="grid-border aspNetGridView"
                        AllowPaging="true"
                        PageSize="10"
                        OnPageIndexChanging="gv_location_type_PageIndexChanging"
                        DataKeyNames="CODE"
                        OnRowEditing="gv_location_type_RowEditing"
                        OnRowUpdating="gv_location_type_RowUpdating"
                        OnRowCancelingEdit="gv_location_type_RowCancelingEdit"
                        OnRowDataBound="gv_location_type_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="CODE" HeaderText="Code" ReadOnly="true" />
                        <asp:BoundField DataField="FULL_NAME" HeaderText="Full Name"/>
                        <asp:BoundField DataField="ABBRV_NAME" HeaderText="Abbrv Name" />
                        <asp:TemplateField HeaderText="Status Flag">
                            <ItemTemplate>
                                <asp:Label ID="lbl_status" runat="server" Text='<%# Eval("STATUS_FLAG") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddl_status_flag" runat="server">
                                    <asp:ListItem Text="A" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="I" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CHANGED_BY" HeaderText="Changed By" ReadOnly="true"/>
                        <asp:BoundField DataField="CHANGED_DATE" HeaderText="Changed Date" DataFormatString="{0:yyyy-MM-dd}" ReadOnly="true"/>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast"
                                    FirstPageText="First"
                                    LastPageText="Last"
                                    Position="Bottom" />
                </asp:GridView>
                <asp:GridView ID="gv_school_type" runat="server" Visible="false" AutoGenerateColumns="false" AutoGenerateEditButton="true" GridLines="Both" BorderWidth="1px" BorderStyle="Solid" CssClass="grid-border aspNetGridView"
                    AllowPaging="true"
                    PageSize="10"
                    OnPageIndexChanging="gv_school_type_PageIndexChanging"
                    DataKeyNames="CODE"
                    OnRowEditing="gv_school_type_RowEditing"
                    OnRowUpdating="gv_school_type_RowUpdating"
                    OnRowCancelingEdit="gv_school_type_RowCancelingEdit"
                    OnRowDataBound="gv_school_type_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="CODE" HeaderText="Code" ReadOnly="true" />
                        <asp:BoundField DataField="FULL_NAME" HeaderText="Full Name"/>
                        <asp:BoundField DataField="ABBRV_NAME" HeaderText="Abbrv Name" />
                        <asp:TemplateField HeaderText="Status Flag">
                            <ItemTemplate>
                                <asp:Label ID="lbl_status" runat="server" Text='<%# Eval("STATUS_FLAG") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddl_status_flag" runat="server">
                                    <asp:ListItem Text="A" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="I" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CHANGED_BY" HeaderText="Changed By" ReadOnly="true"/>
                        <asp:BoundField DataField="CHANGED_DATE" HeaderText="Changed Date" DataFormatString="{0:yyyy-MM-dd}" ReadOnly="true"/>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast"
                FirstPageText="First"
                LastPageText="Last"
                Position="Bottom" />
                </asp:GridView>
                <asp:GridView ID="gv_panel" runat="server" Visible="false" AutoGenerateColumns="false" AutoGenerateEditButton="true" GridLines="Both" BorderWidth="1px" BorderStyle="Solid"  CssClass="grid-border aspNetGridView"
                    AllowPaging="true"
                    PageSize="10"
                    OnPageIndexChanging="gv_panel_PageIndexChanging"
                    DataKeyNames="PANEL"
                    OnRowEditing="gv_panel_RowEditing"
                    OnRowUpdating="gv_panel_RowUpdating"
                    OnRowCancelingEdit="gv_panel_RowCancelingEdit"
                    OnRowDataBound="gv_panel_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="PANEL" HeaderText="Panel" ReadOnly="true" />
                        <asp:BoundField DataField="FULL_NAME" HeaderText="Full Name"/>
                        <asp:BoundField DataField="ABBRV_NAME" HeaderText="Abbrv Name" />
                        <asp:TemplateField HeaderText="Status Flag">
                            <ItemTemplate>
                                <asp:Label ID="lbl_status" runat="server" Text='<%# Eval("STATUS_FLAG") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddl_status_flag" runat="server">
                                    <asp:ListItem Text="A" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="I" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CHANGED_BY" HeaderText="Changed By" ReadOnly="true"/>
                        <asp:BoundField DataField="CHANGED_DATE" HeaderText="Changed Date" DataFormatString="{0:yyyy-MM-dd}" ReadOnly="true"/>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast"
                FirstPageText="First"
                LastPageText="Last"
                Position="Bottom" />
                </asp:GridView>
                <asp:GridView ID="gv_admin_area" runat="server" Visible="false" AutoGenerateColumns="false" AutoGenerateEditButton="true" GridLines="Both" BorderWidth="1px" BorderStyle="Solid"  CssClass="grid-border aspNetGridView" EnableViewState="true"
                    AllowPaging="true"
                    PageSize="10"
                    OnPageIndexChanging="gv_admin_area_PageIndexChanging"
                    DataKeyNames="AREA_CODE"
                    OnRowEditing="gv_admin_area_RowEditing"
                    OnRowUpdating="gv_admin_area_RowUpdating"
                    OnRowCancelingEdit="gv_admin_area_RowCancelingEdit"
                    OnRowDataBound="gv_admin_area_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="AREA_CODE" HeaderText="FOS" ReadOnly="true"/>
                        <asp:BoundField DataField="FULL_NAME" HeaderText="Area Full Name" ReadOnly="true"/>
                        <asp:BoundField DataField="ABBRV_NAME" HeaderText="Abbrv Name" ReadOnly="true"/>
                        <asp:TemplateField HeaderText="Status Flag">
                            <ItemTemplate>
                                <asp:Label ID="lbl_status" runat="server" Text='<%# Eval("STATUS_FLAG") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddl_status_flag" runat="server">
                                    <asp:ListItem Text="A" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="I" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee ID">
                            <ItemTemplate>
                                <%# Eval("EMPLOYEE_ID") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_employee_id" runat="server" Text='<%# Bind("EMPLOYEE_ID") %>'></asp:TextBox>
                                <asp:HiddenField ID="hf_employee_id" runat="server" Value='<%#Eval("EMPLOYEE_ID") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Employee Name">
                            <ItemTemplate>
                                <%# Eval("COMBINED_NAME") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lbl_combined_name" runat="server" Text='<%# Bind("COMBINED_NAME") %>'></asp:Label>
                                <asp:HiddenField ID="hf_combined_name" runat="server" Value='<%#Eval("COMBINED_NAME") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ADMIN TYPE">
                            <ItemTemplate>
                                <%# Eval("ADMIN_TYPE") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddl_admin_type" runat="server" SelectedValue='<%# Bind("ADMIN_TYPE") %>'>
                                    <asp:ListItem Text="Director" Value="Director"></asp:ListItem>
                                    <asp:ListItem Text="Administrative Assistant" Value="Administrative Assistant"></asp:ListItem>
                                    <asp:ListItem Text="Associate Director" Value="Associate Director"></asp:ListItem>
                                    <asp:ListItem Text="Superintendent" Value="Superintendent"></asp:ListItem>
                                    <asp:ListItem Text="Executive Assistant to the Director" Value="Executive Assistant to the Director"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField ID="hf_admin_type" runat="server" Value='<%#Eval("ADMIN_TYPE") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CHANGED_BY" HeaderText="Changed By" ReadOnly="true"/>
                        <asp:BoundField DataField="CHANGED_DATE" HeaderText="Changed Date" DataFormatString="{0:yyyy-MM-dd}" ReadOnly="true"/>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast"
                FirstPageText="First"
                LastPageText="Last"
                Position="Bottom" />
                </asp:GridView>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function onEnterPress(event, textbox) {
            if (event.key === "Enter") {
                event.preventDefault();
                var row = textbox.closest('tr');
                var buttons = row.querySelectorAll("input[type='submit']");
                for (int i = 0; i < buttons.length; i++)
                {
                    if (buttons[i].value.toLowerCase() === "update") {
                        buttons[i].click();
                        break;
                    }
                }
                return false;
            }
            return true;
        }

        function triggerButtonOnEnter(event, buttonId) {
            if (event.key === "Enter") {
                event.preventDefault();
                var button = document.getElementById(buttonId);
                if (button) {
                    button.click();
                }
                return false;
            }
            return true;
        }
        /*
            document.addEventListener('keydown', function (e)
            {
                if (e.key === 'Enter') {
                    const active = document.activeElement;

                    //Grid Row
                    const grid = active.closest('tr');
                    const update = grid.querySelector('.gridview-update-btn');

                    if (update) {
                        e.preventDefault();
                        update.click();
                        return;
                    }

                    
                    const insert_selected = active.closest('.aspNetGridView');
                    if (insert_selected) {
                        const insert = document.getElementById('btn_insert_maint_table');
                        if (insert) {
                            e.preventDefault();
                            insert.click();
                        }
                    }
                    
                }
            }
            */
    </script>
</asp:Content>

