<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CalculatorWebRole._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AOCalculator - Assaf's and Oshrit's Calculator</title>
</head>
<body>
    <h2>
       AOCalculator - Assaf's and Oshrit's Calculator
    </h2>
    <asp:Label ID="teamDetailsLabel" runat="server" Text="Team members:"> </asp:Label> 
    <p>                
            <b>Assaf Israel, ID XXX</b><br />
            <b>Oshrit Feder, ID 040832990</b><br />                                             
    </p>
    <p>
    Short bio:<br />
    Assaf is about to graduate his first degree in Computer Science and start MSc, <br />
    Oshrit has a BSc in Computer Science from Tel Aviv university and currently a MBA 2012 Technion candidate<br />
    We both love to eat cookies and this is our first Azure project!
    <br /><br /> Try out our <a href="calculator.aspx">calculator</a>
    </p>
 </body>
 </html>

