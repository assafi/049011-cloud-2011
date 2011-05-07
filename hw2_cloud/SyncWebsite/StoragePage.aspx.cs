using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure;
using SyncLibrary;
using Microsoft.WindowsAzure.StorageClient;

namespace SyncWebsite
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private static CloudBlobContainer blob;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool loggedIn = (GetSessionObject("LoggedIn") == null ? false : (bool)GetSessionObject("LoggedIn"));
            if (!loggedIn)
            {
                StoreInSession("goBackToURL", Request.RawUrl);
                Response.Redirect("~/Password.aspx");
                return;
            }
            var machineName = System.Environment.MachineName.ToLower();
            var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

            if (!IsPostBack)
            {
                blob = SyncFileBrowser.initBlob(account, machineName); //Need to initialize only once
            }
            /*
             * Refresh results
             */ 
            List<FileEntry> files = SyncFileBrowser.getCloudFiles(blob);

            this.filesList.DataSource = files;
            this.filesList.DataBind();
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