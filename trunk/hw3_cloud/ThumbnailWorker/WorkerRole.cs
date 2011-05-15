using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.IO;
using System.Data.Services.Client;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using ThumbnailWorker;
using SyncLibrary;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private CloudBlobContainer _blobContainer = null;
        private CloudQueue _queue = null;
        private CaptureTableService _table = null;
        private string _wid = RoleEnvironment.CurrentRoleInstance.Id;

        private string captureSite(string url) 
        {
            string outputPath = RoleEnvironment.GetLocalResource("LocalOutput").RootPath; // "C:\\Users\\assafi.TD-CSF\\Documents\\Visual Studio 2010\\Projects\\hw3_cloud";
            string tempFilePath = Path.Combine(outputPath, ExtractDomainNameFromURL(url) + ".png"); //  TODO: use example filename generator

            Trace.TraceInformation("Starting capture on url " + url + ", output: " + tempFilePath);
            Trace.TraceInformation("Roleroot : " + Environment.GetEnvironmentVariable("RoleRoot") + @"\approot\CutyCapt.exe");
            Trace.TraceInformation("Output dir: " + outputPath);
            
            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo(Environment.GetEnvironmentVariable("RoleRoot") + @"\approot\CutyCapt.exe",
                        string.Format(@"--url=""{0}"" --out=""{1}""",
                            url,
                            tempFilePath))
                {
                    UseShellExecute = false
                }
            };
            proc.Start();
            proc.WaitForExit();
            if (File.Exists(tempFilePath))
            {
                Trace.TraceInformation("Capture url " + url + " done.");
                return tempFilePath;
            }
            throw new CaptureError("Unable to create capture from URL: " + url);
        }

        public static string ExtractDomainNameFromURL(string Url)
        {
            if (!Url.Contains("://"))
                Url = "http://" + Url;

            return new Uri(Url).Host;
        }

        public override void Run()
        {
            /*
             * Setup
             */
            var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            _blobContainer = initStorage(storageAccount);
            _queue = initQueue(storageAccount);
            _table = initTable(storageAccount);


            /*
             * Main loop
             */ 
            _queue.AddMessage(new CloudQueueMessage(@"http://www.google.com"));
            _table.addCaptureEntry(@"http://www.google.com");
            while (true)
            {
                var msg = _queue.GetMessage(); //batch
                if (msg != null)
                {
                    if (msg.DequeueCount < 3)
                    {
                        //var split = msg.AsString.Split(new char[] { '/' }, 2);
                        //var guid = split[0];
                        var url = msg.AsString;
                        Trace.TraceInformation("Worker " + _wid + " started working on " + url);
                        _table.startProcessingCapture(url, _wid);
                        string tempFilePath = null;
                        try
                        {
                            tempFilePath = captureSite(url);
                        }
                        catch (CaptureError ce)
                        {
                            Trace.TraceWarning(ce.Message);
                            continue;
                        }

                        string blobRef = uploadCapture(tempFilePath);
                        _table.finishProcessingCapture(url, blobRef);
                    }
                    _queue.DeleteMessage(msg);
                    break; //remove this!!
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                }
            }
            Trace.TraceInformation("Worker done.");
        }

        private string uploadCapture(string tempFilePath)
        {
            FileInfo file = new FileInfo(tempFilePath);
            string blobRef = file.Name + "_" + Guid.NewGuid().ToString();
            var blob = _blobContainer.GetBlobReference(blobRef);
            blob.Properties.ContentType = "image/png";
            blob.UploadFile(tempFilePath);
            Trace.TraceInformation("Upload capture blob done.");
            File.Delete(tempFilePath);
            return blobRef;
        }

        private CloudQueue initQueue(CloudStorageAccount storageAccount)
        {
            CloudQueueClient queueStorage = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueStorage.GetQueueReference("capturequeue");

                 try
                {             
                    queue.CreateIfNotExist();                    
                }
                catch (StorageClientException e)
                {
                    if (e.ErrorCode == StorageErrorCode.TransportError)
                    {
                        Trace.TraceError(string.Format("Connect failure! The most likely reason is that the local " +
                            "Development Storage tool is not running or your storage account configuration is incorrect. " +
                            "Message: '{0}'", e.Message));
                        System.Threading.Thread.Sleep(5000);
                    }
                    else
                    {
                        throw;
                    }
                }
                 return queue;
            }

        
        

        private CloudBlobContainer initStorage(CloudStorageAccount storageAccount)
        {            

            CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobStorage.GetContainerReference("thumbnails");

            try
            {
                container.CreateIfNotExist();

                var permissions = container.GetPermissions();

                permissions.PublicAccess = BlobContainerPublicAccessType.Off;

                container.SetPermissions(permissions);                
            }
            catch (StorageClientException e)
            {
                if (e.ErrorCode == StorageErrorCode.TransportError)
                {
                    Trace.TraceError(string.Format("Connect failure! The most likely reason is that the local " +
                        "Development Storage tool is not running or your storage account configuration is incorrect. " +
                        "Message: '{0}'", e.Message));
                    System.Threading.Thread.Sleep(5000);
                }
                else
                {
                    throw;
                }
            }
            return container;

        }

        public static CaptureTableService initTable(CloudStorageAccount storageAccount)
        {
            CloudTableClient.CreateTablesFromModel(typeof(CaptureTableService),
                                                   storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
            return new CaptureTableService(storageAccount.TableEndpoint.ToString(), storageAccount.Credentials);
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // This code sets up a handler to update CloudStorageAccount instances when their corresponding
            // configuration settings change in the service configuration file.
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                // Provide the configSetter with the initial value
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));

                RoleEnvironment.Changed += (anotherSender, arg) =>
                {
                    if (arg.Changes.OfType<RoleEnvironmentConfigurationSettingChange>()
                        .Any((change) => (change.ConfigurationSettingName == configName)))
                    {
                        // The corresponding configuration setting has changed, propagate the value
                        if (!configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)))
                        {
                            // In this case, the change to the storage account credentials in the
                            // service configuration is significant enough that the role needs to be
                            // recycled in order to use the latest settings. (for example, the 
                            // endpoint has changed)
                            RoleEnvironment.RequestRecycle();
                        }
                    }
                };
            });

            return base.OnStart();
        }
    }
}
