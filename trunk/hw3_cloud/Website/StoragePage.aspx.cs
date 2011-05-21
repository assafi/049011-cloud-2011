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
        private static IEnumerable<CloudBlobContainer> containers;

        protected void Page_Load(object sender, EventArgs e)
        {            
            var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

            if (!IsPostBack)
            {
                containers = SyncFileBrowser.getBlobContainers(account); //Need to initialize only once
            }
            /*
             * Refresh results
             */
            List<FileEntry> files = new List<FileEntry>();
            foreach (CloudBlobContainer container in containers)
            {
                files.AddRange(SyncFileBrowser.getCloudFiles(container));
            }

            this.FilesView.DataSource = files;
            this.FilesView.DataBind();
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