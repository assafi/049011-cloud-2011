<%@ Page Title="Files" Language="C#" MasterPageFile="~/Site.master"  AutoEventWireup="true" 
CodeBehind="StoragePage.aspx.cs" Inherits="SyncWebsite.WebForm1" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
 
    List of synced files stored in the cloud: <br /><br />
    <div>
    
        <asp:GridView ID="FilesView" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="ContainerName" HeaderText="Container" />
                <asp:BoundField DataField="NameWithLink" HeaderText="File Name" 
                    HtmlEncode="False" />
                <asp:BoundField DataField="Modified" HeaderText="Last Modified" />
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
    
    </div>
 
</asp:Content>