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

                // this.ThumbnailView.DataSource = context.Captures; //Refresh at every read
                // this.dsThumbnails.EntitySetName = context.Capture; //Refresh at every read
                //this.ThumbnailView.DataBind();
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

        protected void SearchSubmit_Click(object sender, EventArgs e)
        {
            string searchedURL = this.URLSearchText.Text;
            if (isValid(searchedURL))
            {

                IEnumerable<CaptureEntry> entries = from capture in context.Captures where capture.url == searchedURL select capture;

                /* CaptureEntry selectedCapture =
                    (from capture in context.Captures
                     where capture.url == searchedURL
                     select capture).FirstOrDefault<CaptureEntry>(); */
                     //context.CapturesByID(searchedURL); //Refresh at every read                
               
                    this.ThumbnailView.DataSource = entries;
                    this.ThumbnailView.DataBind();
               
            }
        }

        private Boolean isValid(string url)
        {
            if (url == string.Empty)
                return false;
            else
                return true;
        }
    }
}