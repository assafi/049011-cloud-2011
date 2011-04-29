<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LogsPage.aspx.cs" Inherits="SyncWebsite.LogsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    Log Entries:
    <br />
    <br />
    <div>
        <asp:GridView ID="LogView" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False"
            BorderStyle="Solid">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="ComputerName" HeaderText="Computer Name" />
                <asp:BoundField DataField="FileName" HeaderText="File Name" />
                <asp:BoundField DataField="Operation" HeaderText="Operation Type" />
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
    </div>
</asp:Content>
