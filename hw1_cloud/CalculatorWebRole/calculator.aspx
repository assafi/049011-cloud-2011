<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calculator.aspx.cs" Inherits="CalculatorWebRole.calculator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="Styles/Site.css" />
    <title>AOCalculator</title>
</head>
<body>
    <form id="CalculatorForm" runat="server">
    <asp:Label ID="lblDisplay" runat="server" Height="35px" Text="Label" Width="140px"></asp:Label>
    <br />
    <asp:Button ID="btn1" runat="server" CssClass="calcBtn" onclick="btn1_Click" 
        Text="1" />
    <asp:Button ID="btn2" runat="server" Text="2" CssClass="calcBtn" 
        onclick="btn2_Click" />
    <asp:Button ID="btn3" runat="server" Text="3" CssClass="calcBtn" 
        onclick="btn3_Click" />
    <asp:Button ID="addBtn" runat="server" Text="+" CssClass="calcBtn" 
        onclick="addBtn_Click" />
    <br />
    <asp:Button ID="btn4" runat="server" Text="4" CssClass="calcBtn" onclick="btn4_Click" />
    <asp:Button ID="btn5" runat="server" Text="5" CssClass="calcBtn" onclick="btn5_Click" />
    <asp:Button ID="btn6" runat="server" Text="6" CssClass="calcBtn" onclick="btn6_Click" />
    <asp:Button ID="subBtn" runat="server" Text="-" CssClass="calcBtn" 
        onclick="subBtn_Click" />
    <br />
    <asp:Button ID="btn7" runat="server" Text="7" CssClass="calcBtn" onclick="btn7_Click" />
    <asp:Button ID="btn8" runat="server" Text="8" CssClass="calcBtn" onclick="btn8_Click" />
    <asp:Button ID="btn9" runat="server" Text="9" CssClass="calcBtn" onclick="btn9_Click" />
    <asp:Button ID="multBtn" runat="server" Text="x" CssClass="calcBtn" />
    <br />
    <asp:Button ID="clrBtn" runat="server" Text="clear" BackColor="Silver" 
        Height="35px" style="margin-top: 0px" Width="70px" />
    <asp:Button ID="btn0" runat="server" Text="0" CssClass="calcBtn" onclick="btn0_Click" />
    <asp:Button ID="divBtn" runat="server" Text="/" CssClass="calcBtn" />
    <br />
    <asp:Button ID="eqBtn" runat="server" Text="=" BackColor="Silver" 
        Height="35px" Width="140px" onclick="eqBtn_Click" />
    <asp:HiddenField ID="lOperand" runat="server" />
    <asp:HiddenField ID="rOperand" runat="server" />
    <asp:HiddenField ID="op" runat="server" />
    </form>
</body>
</html>
