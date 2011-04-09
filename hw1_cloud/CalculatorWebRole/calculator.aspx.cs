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
        private static ICalculator proxy = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (proxy == null)
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                string wsdlAddress = RoleEnvironment.GetConfigurationSettingValue("WSAddress");
                EndpointAddress epAddress = new EndpointAddress(wsdlAddress);
                proxy = System.ServiceModel.ChannelFactory<ICalculator>.CreateChannel(binding, epAddress);
            }

            if (!Page.IsPostBack)
            {
                this.lblDisplay.Text = "0";
                StoreInSession("restart", false);
                //this.PageTitleLabel.Text = RoleEnvironment.GetConfigurationSettingValue("GameTitle");
            }
            this.btn0.Enabled = true;
               
        }

        protected void parseOperand(int operand)
        {

            if ((bool)GetFromSession("restart"))
            {
                lblDisplay.Text = "0";
                StoreInSession("restart",false);
            }

            string parsedNum = lblDisplay.Text + operand.ToString();
            lblDisplay.Text = double.Parse(parsedNum).ToString();

            if (GetSessionString("op").Equals("Unknown"))
                StoreInSession("loperand", double.Parse(lblDisplay.Text));
            else
                StoreInSession("roperand", double.Parse(lblDisplay.Text));
        }

        private object GetFromSession(string keyName)
        {
            if (Request.RequestContext.HttpContext.Session[keyName] != null)
            {
                return Request.RequestContext.HttpContext.Session[keyName];
            }

            return null;
        }

        private string GetSessionString(string keyName)
        {
            if (Request.RequestContext.HttpContext.Session[keyName] != null)
            {
                return Request.RequestContext.HttpContext.Session[keyName].ToString();
            }

            return "Unknown";
        }

        private double GetSessionDouble(string keyName)
        {
            if (Request.RequestContext.HttpContext.Session[keyName] != null)
            {
                return (double)Request.RequestContext.HttpContext.Session[keyName];
            }

            return 0;
        }

        private void StoreInSession(string keyName, object value)
        {
            Request.RequestContext.HttpContext.Session.Add(keyName,value);
        }


        private void NumericOperation(string op)
        {

            if (!lblDisplay.Text.Equals(string.Empty)) 
                StoreInSession("loperand", double.Parse(lblDisplay.Text));
            StoreInSession("op", op);
            lblDisplay.Text = string.Empty;
            StoreInSession("restart",false);

        }

        protected void subBtn_Click(object sender, EventArgs e)
        {
            NumericOperation("sub");
        }

        protected void addBtn_Click(object sender, EventArgs e)
        {
            NumericOperation("add");
        }

        protected void mulBtn_Click(object sender, EventArgs e)
        {
            NumericOperation("mul");
        }

        protected void divBtn_Click(object sender, EventArgs e)
        {
            NumericOperation("div");
            this.btn0.Enabled = false;        
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
            return (GetFromSession("loperand") != null &&
                    GetFromSession("roperand") != null &&
                    GetFromSession("op") != null);            
        }

        protected void eqBtn_Click(object sender, EventArgs e)
        {           
            if (allSet())
            {                
                if (GetSessionString("op").Equals("add"))
                    lblDisplay.Text = proxy.add(GetSessionDouble("loperand"), GetSessionDouble("roperand")).ToString();

                if (GetSessionString("op").Equals("sub"))
                    lblDisplay.Text = proxy.sub(GetSessionDouble("loperand"), GetSessionDouble("roperand")).ToString();

                if (GetSessionString("op").Equals("mul"))
                    lblDisplay.Text = proxy.mul(GetSessionDouble("loperand"), GetSessionDouble("roperand")).ToString();

                if (GetSessionString("op").Equals("div"))
                    lblDisplay.Text = proxy.div(GetSessionDouble("loperand"), GetSessionDouble("roperand")).ToString();

                Request.RequestContext.HttpContext.Session.Remove("op");
                Request.RequestContext.HttpContext.Session.Remove("roperand");
                StoreInSession("loperand",double.Parse(lblDisplay.Text));
                StoreInSession("restart",true);
            }
            //clear();
        }

        protected void clrBtn_Click(object sender, EventArgs e)
        {
            /*
             * Multiply by 0 to reset the calc
             */
            NumericOperation("mul");
            parseOperand(0);
            eqBtn_Click(null, null);
        }
    }
 }