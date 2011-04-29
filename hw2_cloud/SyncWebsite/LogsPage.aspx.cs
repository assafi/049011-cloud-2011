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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var machineName = System.Environment.MachineName.ToLower();
                var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
                var context = new SyncLoggerService(machineName, account.TableEndpoint.ToString(), account.Credentials);

                this.LogView.DataSource = context.Logs;
                this.LogView.DataBind();
            }
            catch (DataServiceRequestException ex)
            {
                Console.WriteLine("Unable to connect to the table storage server. Please check that the service is running.<br>"
                                 + ex.Message);
            }
        }
    }
}