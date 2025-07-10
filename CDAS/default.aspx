<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="CDAS.login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="loginform" runat="server">
        <div class="container" style="margin-top: 24px;">
            <div class="login">
                <div id="logo">
                    <img src="https://s3.amazonaws.com/wrdsb-theme/images/WRDSB_Logo.svg" />
                </div>
                <h1>Common Data Application System</h1>

                <fieldset>
                    <asp:Button ID="btn_login_maint" runat="server" Text="Login" OnClick="btn_login_maint_Click"/>
                </fieldset>
                <p>
                    <asp:Label ID="lbl_message" runat="server"></asp:Label>
                </p>
            </div>
        </div>
    </form>
</asp:Content>
