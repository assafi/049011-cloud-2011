<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LogsPage.aspx.cs" Inherits="SyncWebsite.LogsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        URL to search: <asp:TextBox ID="URLSearchText" runat="server"></asp:TextBox>
        <asp:Button ID="SearchSubmit" runat="server" Text="Search" 
            onclick="SearchSubmit_Click" />
        <asp:DetailsView ID="ThumbnailView" runat="server" Caption="Thumbnail data" 
            Height="250px" Width="276px" 
            AutoGenerateRows="False" BorderStyle="None" 
            EmptyDataText="No capture exists for the requested URL" GridLines="Horizontal">
            <Fields>
                <asp:ImageField HeaderText="Thumbnail:" DataImageUrlField="blobUri">
                </asp:ImageField>
                <asp:BoundField DataField="url" HeaderText="Webpage URL:" />
                <asp:BoundField DataField="WorkerId" HeaderText="Responsible server name:" />
                <asp:BoundField DataField="StartTime" HeaderText="Date taken:" />
            </Fields>
        </asp:DetailsView>
        <!-- <asp:EntityDataSource ID="dsThumbnails" runat="server" 
            EntitySetName="Captures" Where="it.url <= @URLSearchText">
            <WhereParameters>
                <asp:ControlParameter ControlID="URLSearchText" Name="url" 
                PropertyName="Text" Type="String" />
            </WhereParameters>
        </asp:EntityDataSource>
        <asp:QueryExtender ID="QueryExtender1" runat="server" 
            TargetControlID="dsThumbnails">
        </asp:QueryExtender> -->
    </div>
</asp:Content>
