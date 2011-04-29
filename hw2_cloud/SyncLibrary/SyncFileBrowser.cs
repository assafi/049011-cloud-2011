using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace SyncLibrary
{
    public class SyncFileBrowser
    {
        public static CloudBlobContainer initBlob(CloudStorageAccount storageAccount, string name)
        {
            var BlobClient = storageAccount.CreateCloudBlobClient();

            // Get and create the container
            var BlobContainer = BlobClient.GetContainerReference(name);
            BlobContainer.CreateIfNotExist();

            // Setup the permissions on the container to be public
            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            BlobContainer.SetPermissions(permissions);

            return BlobContainer;
        }

        public static SyncLoggerService initTable(CloudStorageAccount storageAccount, string name)
        {
            CloudTableClient.CreateTablesFromModel(typeof(SyncLoggerService),
                                                   storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
            return (new SyncLoggerService(name, storageAccount.TableEndpoint.ToString(), storageAccount.Credentials));
        }

        public static List<FileEntry> getCloudFiles(CloudBlobContainer blobContainer)
        {
            // Get a list of the blobs
            var blobs = blobContainer.ListBlobs();
            var filesList = new List<FileEntry>();

            // For each item, create a FileEntry which will populate the grid
            foreach (var blobItem in blobs)
            {
                var cloudBlob = blobContainer.GetBlobReference(blobItem.Uri.ToString());
                cloudBlob.FetchAttributes();

                filesList.Add(new FileEntry()
                {
                    FileUri = blobItem.Uri,
                    CloudFileName = cloudBlob.Metadata["FileName"],
                    Modified = DateTime.Parse(cloudBlob.Metadata["Modified"]),
                    FileInfo = null,
                });
            }

            return filesList;
        }
    }
}
