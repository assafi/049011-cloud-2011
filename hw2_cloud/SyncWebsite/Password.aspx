<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Password.aspx.cs" Inherits="SyncWebsite.Password" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
    You&#39;re about to enter to a secure page.</p>
    Please enter the password:&nbsp;
        <asp:TextBox ID="PasswordText" runat="server" 
            ontextchanged="TextBox1_TextChanged" TextMode="Password"></asp:TextBox>
    &nbsp;
        <asp:Button ID="PasswordSubmit" runat="server" 
        onclientclick="CheckPassword" Text="Submit" onclick="CheckPassword"></asp:Button>
    <p>
        <asp:Label ID="statusLabel" runat="server"></asp:Label>
    </p>
</asp:Content>
