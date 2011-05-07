using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace SyncLibrary
{
    public class SyncFileBrowser
    {
        public static IEnumerable<CloudBlobContainer> getBlobContainers(CloudStorageAccount storageAccount)
        {
            var BlobClient = storageAccount.CreateCloudBlobClient();

            //Get all containers
            IEnumerable<CloudBlobContainer> Containers = BlobClient.ListContainers();
            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            foreach (var Container in Containers)
            {
                Container.SetPermissions(permissions);
            }
            return Containers;
        }

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

                try
                {
                    filesList.Add(new FileEntry()
                    {
                        FileUri = blobItem.Uri,
                        CloudFileName = cloudBlob.Metadata["FileName"],
                        Modified = parseDateTime(cloudBlob.Metadata["Modified"]),
                        FileInfo = null,
                        ContainerName = blobContainer.Name,
                    });
                }
                catch (FormatException fe)
                {
                    Console.Error.WriteLine("Error while processing file: " + cloudBlob.Metadata["FileName"]);
                    Console.Error.WriteLine(fe.Message);
                    continue;
                }
            }

            return filesList;
        }

        private static DateTime parseDateTime(string p)
        {
            return DateTime.Parse(p, new CultureInfo("he-IL"));
        }

        public static void deleteAll(CloudStorageAccount storageAccount)
        {
            IEnumerable<CloudBlobContainer> Containers = getBlobContainers(storageAccount);
            foreach (var container in Containers)
            {
                container.Delete();
            }
        }
    }
}
