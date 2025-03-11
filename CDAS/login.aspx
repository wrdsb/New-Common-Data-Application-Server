<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="CDAS.login" %>
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
                    
                    <!-- Username -->
                    <label for="tb_username">Username</label>
                    <asp:TextBox ID="tb_username" runat="server"></asp:TextBox>

                    <!-- Password -->
                    <label for="tb_password">Password</label>
                    <asp:TextBox ID="tb_password" runat="server" TextMode="Password"></asp:TextBox>
                    <p><a href="https://mypassword.wrdsb.ca/" target="_blank">Forgot your Password?</a></p>
                    <div class="alert alert-danger" role="alert" id="loginErrors" runat="server" visible="False"></div>
                    
                    <!-- Submit Form -->
                    <asp:Button ID="btn_login" runat="server" Text="Login" OnClick="btn_login_Click" />
                        
                </fieldset>
                <p>
                    <asp:Label ID="lbl_message" runat="server"></asp:Label>
                </p>
            </div>
        </div>
    </form>
</asp:Content>
