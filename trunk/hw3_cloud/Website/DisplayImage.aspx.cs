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

namespace Website
{
    public partial class DisplayImage : System.Web.UI.Page
    {
        private static CloudBlobContainer container;
        private static CloudBlobClient blob;

        protected void Page_Load(object sender, EventArgs e)
        {           
            try
            {
                if (!IsPostBack)
                {
                    /*
                     * Intialize only once
                     */             
                    var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");                                        
                    blob = new CloudBlobClient(account.BlobEndpoint, account.Credentials);                    
                    container = blob.GetContainerReference("thumbnails");
                }                
            }
            catch (DataServiceRequestException ex)
            {
                Console.WriteLine("Unable to connect to the table storage server. Please check that the service is running.<br>"
                                 + ex.Message);
            }


            String blobId = Request.QueryString["id"];
            //Uri blobRef = new Uri(blobId);
            //blobId = blobRef.Segments[blobRef.Segments.Length - 1];
            //var cloudBlob2 = blob.GetBlobReference(blobId);
            var cloudBlob = container.GetBlobReference(blobId);
            //byte[] imageBuf2 = cloudBlob2.DownloadByteArray();            
            byte[] imageBuf = cloudBlob.DownloadByteArray();            
            Response.ContentType = "image/jpeg";
            Response.BinaryWrite(imageBuf);
        }
    }
}