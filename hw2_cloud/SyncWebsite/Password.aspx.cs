using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace SyncWebsite
{
    public partial class Password : System.Web.UI.Page
    {
        private static string passwd;

        protected void Page_Load(object sender, EventArgs e)
        {
            passwd = RoleEnvironment.GetConfigurationSettingValue("SuperSecretPassword");
            if (GetSessionObject("LoggedIn") != null && (bool)GetSessionObject("LoggedIn") == true) 
            {
                statusLabel.Text = "You're already logged in";
                statusLabel.ForeColor = Color.Green;
                //StoreInSession("warning", "You're already logged in");
            }
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void CheckPassword(object sender, EventArgs e)
        {
            if (PasswordText.Text == passwd)
            {
                StoreInSession("LoggedIn", true);
                string nextURL = GetSessionString("goBackToURL");
                if (nextURL != string.Empty)
                {
                    Response.Redirect(nextURL);
                    return;
                }
                refresh();
                return;
            }
            statusLabel.Text = "Incorrect password";
            statusLabel.ForeColor = Color.Red;
            //StoreInSession("warning", "Incorrect password");
            //refresh();
        }

        protected string GetSessionString(string keyName)
        {
            if (Request.RequestContext.HttpContext.Session[keyName] != null)
            {
                return Request.RequestContext.HttpContext.Session[keyName].ToString();
            }

            return string.Empty;
        }

        protected object GetSessionObject(string keyName)
        {
            return Request.RequestContext.HttpContext.Session[keyName];
        }

        private void refresh()
        {
            Response.Redirect(Request.RawUrl);
        }

        private void StoreInSession(string keyName, object value)
        {
            Request.RequestContext.HttpContext.Session.Add(keyName, value);
        }
    }
}