<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadPage.aspx.cs" Inherits="Website.UploadPage" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>    
    <table>
    <tr><td>Select a file containing the urls to create captures for</td></tr>
    <tr><td><asp:FileUpload ID="FileUpload1" runat="server" Width="100%" /></td></tr>
    <tr><td><input id="Submit1" type="submit" value="Create thumbnails from file" /></td></tr>
    <tr><td><asp:Label ID="feedback" runat="server" Visible="false"></asp:Label></td></tr>        
    </table>            
     </div>    
    <script language="javascript" type="text/javascript">
// <![CDATA[

        

// ]]>
    </script>
</asp:Content>
