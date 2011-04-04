using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using Microsoft.WindowsAzure.ServiceRuntime;
using CalculatorWS;


namespace CalculatorWebRole
{
    public partial class calculator : System.Web.UI.Page
    {
        /*
         *  Calculator supported operations: number, operator, number, equal.
         *  Punching the buttons in any other order wont work for now.
         */

        private ICalculator Proxy
        {
            get
            {
                //For simplicity, we always return new instance of proxy
                return System.ServiceModel.ChannelFactory<ICalculator>.CreateChannel(new BasicHttpBinding(BasicHttpSecurityMode.None),
                                                                                new EndpointAddress(RoleEnvironment.GetConfigurationSettingValue("WSAddress")));
            }
        }

        private const int NOT_SET = -1;
        
        // state : 0 - loperand, 1 - op, 2 - roperand
        private int calculatorState = 0;

        protected void Page_Load(object sender, EventArgs e)
        {                       
            if (!Page.IsPostBack)
            {
                // Update result of pervious calculation
                //_lOperand = this.GetCookieInt("result");
                //this.lblDisplay.Text = _lOperand.ToString();
                this.lblDisplay.Text = string.Empty;    
            
                //this.PageTitleLabel.Text = RoleEnvironment.GetConfigurationSettingValue("GameTitle");
                //this.AgeLabel.Text = this.GetCookieString("Age");
                //this.PostNewQuestion(this.GetCookieInt("CurrentQuestion"));
            }                
            
               
        }

        protected void parseOperand(int operand)
        {
            lblDisplay.Text += operand.ToString();

            //int parsedOperand = int.Parse(lblDisplay.Text);
            if (GetCookieString("op").Equals("Unknown"))
                Response.Cookies["loperand"].Value = lblDisplay.Text;
            else
                Response.Cookies["roperand"].Value = lblDisplay.Text;
        }

        private string GetCookieString(string keyName)
        {
            if (Request.Cookies[keyName] != null)
            {
                return Request.Cookies[keyName].Value;
            }

            return "Unknown";
        }

        private int GetCookieInt(string keyName)
        {
            if (Request.Cookies[keyName] != null)
            {
                return int.Parse(Request.Cookies[keyName].Value);
            }

            return 0;
        }

        private void SetCookie(string keyName, string value)
        {
            Response.Cookies[keyName].Value = value;
        }

        protected void subBtn_Click(object sender, EventArgs e)
        {
            if (lblDisplay.Text != string.Empty)
            {
                Response.Cookies["loperand"].Value = lblDisplay.Text;
                Response.Cookies["op"].Value = "sub";
                lblDisplay.Text = string.Empty;
            }             
        }

        protected void addBtn_Click(object sender, EventArgs e)
        {
            if (lblDisplay.Text != string.Empty)
            {
                Response.Cookies["loperand"].Value = lblDisplay.Text;
                Response.Cookies["op"].Value = "add";
                lblDisplay.Text = string.Empty;
            }
        }

        protected void btn2_Click(object sender, EventArgs e)
        {
            parseOperand(int.Parse((sender as Button).Text));  
        }

        protected void btn3_Click(object sender, EventArgs e)
        {
            parseOperand(int.Parse((sender as Button).Text));
        }

        protected void btn4_Click(object sender, EventArgs e)
        {
            parseOperand(int.Parse((sender as Button).Text));
        }

        protected void btn5_Click(object sender, EventArgs e)
        {
            parseOperand(int.Parse((sender as Button).Text));
        }

        protected void btn6_Click(object sender, EventArgs e)
        {
            parseOperand(int.Parse((sender as Button).Text));
        }

        protected void btn7_Click(object sender, EventArgs e)
        {
            parseOperand(int.Parse((sender as Button).Text));
        }

        protected void btn8_Click(object sender, EventArgs e)
        {
            parseOperand(int.Parse((sender as Button).Text));
        }

        protected void btn9_Click(object sender, EventArgs e)
        {
            parseOperand(int.Parse((sender as Button).Text));
        }

        protected void btn0_Click(object sender, EventArgs e)
        {
            parseOperand(int.Parse((sender as Button).Text));
        }

        protected void btn1_Click(object sender, EventArgs e)
        {
            parseOperand(int.Parse((sender as Button).Text));            
        }

        protected bool allSet()
        {
            return (!GetCookieString("loperand").Equals("Unknown") &&
                    !GetCookieString("roperand").Equals("Unknown") &&
                    !GetCookieString("op").Equals("Unknown"));            
        }

        protected void eqBtn_Click(object sender, EventArgs e)
        {           
            if (allSet())
            {                
                if (GetCookieString("op").Equals("add"))
                    lblDisplay.Text = Proxy.add(GetCookieInt("loperand"), GetCookieInt("roperand")).ToString();
                Response.Cookies.Remove("op");
                Response.Cookies.Remove("roperand");
                Response.Cookies["loperand"].Value = lblDisplay.Text;
            }
            //clear();
        }

    }
 }