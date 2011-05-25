using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure;
using SyncLibrary;
using Microsoft.WindowsAzure.StorageClient;

namespace Website
{
    public partial class UploadPage : System.Web.UI.Page
    {
        static CloudStorageAccount account = null;
        static CaptureTableService context = null;
        static CloudQueue queue = null;

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
                CloudQueueClient queueStorage = account.CreateCloudQueueClient();
                queue = queueStorage.GetQueueReference("capturequeue");
                context = new CaptureTableService(account.TableEndpoint.ToString(), account.Credentials);
            }
            else
            {
                // Submitted

                // Parse file and put capture tasks on queue
                List<string> urlsToUpload = parseURLFile(this.FileUpload1.FileContent);
                generateTasks(urlsToUpload);

                // Message back the number of captures placed
                this.feedback.Text = "Number of sites to be captured : " + urlsToUpload.Count;
                this.feedback.Visible = true;
            }
        }
        

        private void generateTasks(List<string> urlsToUpload)
        {            
            foreach (string url in urlsToUpload)
            {
                string requestId = context.addCaptureEntry(url);
                queue.AddMessage(new CloudQueueMessage(requestId));                
            }                                 
        }

        private List<string> parseURLFile(System.IO.Stream stream)
        {
            List<string> urls = new List<string>();

            StreamReader reader = new StreamReader(stream);
            
            string currentURL = null;
            while (!reader.EndOfStream)
            {
                currentURL = reader.ReadLine();
                urls.Add(currentURL);                
            }
            return urls;
        }
    }
}