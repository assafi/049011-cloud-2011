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
        protected void Page_Load(object sender, EventArgs e)
        {
            var machineName = System.Environment.MachineName.ToLower();
            var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

            CloudBlobContainer blob = SyncFileBrowser.initBlob(account, machineName);
           List<FileEntry> files = SyncFileBrowser.getCloudFiles(blob);

           this.filesList.DataSource = files;
           this.filesList.DataBind();
        }
    }
}