using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.WindowsAzure.StorageClient;
using System.Data.Services.Client;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using SyncLibrary;

namespace SyncWebsite
{
    public partial class LogsPage : System.Web.UI.Page
    {
        private static CaptureTableService context;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool loggedIn = (GetSessionObject("LoggedIn") == null ? false : (bool)GetSessionObject("LoggedIn"));
            if (!loggedIn)
            {
                StoreInSession("goBackToURL", Request.RawUrl); //The URL to get back to after password is verified
                Response.Redirect("~/Password.aspx");
                return;
            }

            try
            {
                if (!IsPostBack)
                {
                    /*
                     * Intialize only once
                     */ 
                    var machineName = System.Environment.MachineName.ToLower();
                    var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
                    context = new CaptureTableService(account.TableEndpoint.ToString(), account.Credentials);
                }
                
                //this.LogView.DataSource = context.Logs; //Refresh at every read
                //this.LogView.DataBind();
            }
            catch (DataServiceRequestException ex)
            {
                Console.WriteLine("Unable to connect to the table storage server. Please check that the service is running.<br>"
                                 + ex.Message);
            }
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

        private void StoreInSession(string keyName, object value)
        {
            Request.RequestContext.HttpContext.Session.Add(keyName, value);
        }
    }
}