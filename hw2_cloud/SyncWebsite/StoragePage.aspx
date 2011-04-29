<%@ Page Title="Files" Language="C#" MasterPageFile="~/Site.master"  AutoEventWireup="true" 
CodeBehind="StoragePage.aspx.cs" Inherits="SyncWebsite.WebForm1" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
 
    List of synced files stored in the cloud: <br /><br />
    <div>
    
        <asp:ListView ID="filesList" runat="server">
         <LayoutTemplate>
                        <ul id="logs">
                            <li ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li class="even">
                            <%# Eval("CloudFileName")%> 
                        </li>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <li class="odd">
                            <%# Eval("CloudFileName") %>
                        </li>
                    </AlternatingItemTemplate>
        </asp:ListView>
    
    </div>
 
</asp:Content>