<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calculator.aspx.cs" Inherits="CalculatorWebRole.calculator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="Styles/Site.css" />
    <title>AOCalculator</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="lblDisplay" 
        
        style="border-style: dotted; border-width: medium; width: 131px; height: 35px;">
    
    </div>
    <asp:Button ID="btn1" runat="server" CssClass="calcBtn" onclick="btn1_Click" 
        Text="1" />
    <asp:Button ID="btn2" runat="server" Text="2" CssClass="calcBtn" />
    <asp:Button ID="btn3" runat="server" Text="3" CssClass="calcBtn" />
    <asp:Button ID="Button4" runat="server" Text="+" CssClass="calcBtn" />
    <br />
    <asp:Button ID="btn4" runat="server" Text="4" CssClass="calcBtn" />
    <asp:Button ID="btn5" runat="server" Text="5" CssClass="calcBtn" />
    <asp:Button ID="btn6" runat="server" Text="6" CssClass="calcBtn" />
    <asp:Button ID="Button8" runat="server" Text="-" CssClass="calcBtn" />
    <br />
    <asp:Button ID="btn7" runat="server" Text="7" CssClass="calcBtn" />
    <asp:Button ID="btn8" runat="server" Text="8" CssClass="calcBtn" />
    <asp:Button ID="btn9" runat="server" Text="9" CssClass="calcBtn" />
    <asp:Button ID="Button12" runat="server" Text="x" CssClass="calcBtn" />
    <br />
    <asp:Button ID="Button13" runat="server" Text="clear" BackColor="Silver" 
        Height="35px" style="margin-top: 0px" Width="70px" />
    <asp:Button ID="Button14" runat="server" Text="0" CssClass="calcBtn" />
    <asp:Button ID="Button15" runat="server" Text="/" CssClass="calcBtn" />
    <br />
    <asp:Button ID="Button16" runat="server" Text="=" BackColor="Silver" 
        Height="35px" Width="141px" />
    </form>
</body>
</html>
