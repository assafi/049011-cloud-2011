using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Net;

using System.Data.Services.Client;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

using SyncLibrary;

namespace WebRole1
{
    public class WebRole : RoleEntryPoint
    {
        private CloudBlobContainer _blobContainer = null;
        private CloudQueue _queue = null;
        private CaptureTableService _captureTable = null;

        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.


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

            /*
             * Setup
            */
            var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            _blobContainer = initStorage(storageAccount);
            _queue = initQueue(storageAccount);
            _captureTable = initCaptureTable(storageAccount);           

            return base.OnStart();
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

        public static CaptureTableService initCaptureTable(CloudStorageAccount storageAccount)
        {
            CloudTableClient.CreateTablesFromModel(typeof(CaptureTableService),
                                                   storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
            return new CaptureTableService(storageAccount.TableEndpoint.ToString(), storageAccount.Credentials);
        }



    }
}
