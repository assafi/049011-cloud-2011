<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Stats.aspx.cs" Inherits="Website.Stats" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            text-decoration: underline;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p class="style1">
        <strong>Application Statistics</strong></p>
    <p>
        Active workers:
        <asp:Label ID="WorkersCount" runat="server" onload="WorkersCount_DataBinding" 
            Text="0"></asp:Label>
    </p>
    <p>
        Captured URIs:
        <asp:Label ID="CapturesCount" runat="server" onload="CapturesCount_DataBinding" 
            Text="0"></asp:Label>
&nbsp;(Excluding pending)</p>
    <p>
        Pending URIs:
        <asp:Label ID="PendingURIs" runat="server" onload="PendingURIs_DataBinding" 
            Text="0"></asp:Label>
    </p>
    <asp:GridView ID="WorkersStats" runat="server" AutoGenerateColumns="False" 
        onload="WorkersStats_DataBinding" 
        style="margin-top: 0px; text-align: center;">
        <Columns>
            <asp:BoundField DataField="wid" HeaderText="Worker ID" />
            <asp:BoundField DataField="imgCount" HeaderText="# Images" />
            <asp:BoundField DataField="avgProcTime" 
                HeaderText="Average image creation time [Sec]" />
        </Columns>
    </asp:GridView>
</asp:Content>
