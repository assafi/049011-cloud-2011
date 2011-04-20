using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;


namespace SyncApp
{
    class Program
    {

        private static CloudBlobClient _BlobClient = null;
        private static CloudBlobContainer _BlobContainer = null;

        private static string monitoredFolderPath;

        private static int SLEEP_TIME = 1000;

        static void Main(string[] args)
        {

            monitoredFolderPath = ConfigurationManager.AppSettings["MonitoredFolderPath"];
            if (!checkValidFolder(monitoredFolderPath))
            {
                Console.Error.WriteLine("Illegal folder path: " + monitoredFolderPath);
            }

            init();
            sync();
        }

        private static void sync()
        {
            int iter = 1;
            DirectoryInfo di = new DirectoryInfo(monitoredFolderPath);
            while (true)
            {
                Console.WriteLine("Iteration " + iter++);
                di.Refresh();
                List<FileEntry> cloudFiles = getCloudFiles();
                List<FileEntry> localFiles = getLocalFiles(di);
                syncLocalRemoteFiles(localFiles, cloudFiles);
                Thread.Sleep(SLEEP_TIME);
            }
        }

        private static Dictionary<string, FileEntry> getFilesDicFromList(List<FileEntry> fileList)
        {
            Dictionary<string, FileEntry> filesDic = new Dictionary<string, FileEntry>();
            foreach (var file in fileList)
            {
                filesDic.Add(file.CloudFileName, file);
            }

            return filesDic;
        }

        private static void syncLocalRemoteFiles(List<FileEntry> localFiles, List<FileEntry> remoteFiles)
        {
            // Create a hash from the local, to contain in O(n)
            Dictionary<string, FileEntry> localFilesDic = getFilesDicFromList(localFiles);
            Dictionary<string, FileEntry> remoteFilesDic = getFilesDicFromList(remoteFiles);

            List<FileEntry> addFilesList = new List<FileEntry>();
            List<Uri> deleteFilesList = new List<Uri>();


            // Files to add / update 
            foreach (var localFile in localFiles)
            {
                FileEntry remoteFile;
                if (remoteFilesDic.TryGetValue(localFile.CloudFileName, out remoteFile))                
                {
                    // if file had changed - update it
                    Console.WriteLine("local last modification: " + localFile.Modified.ToLongTimeString());
                    Console.WriteLine("local remote modification: " + remoteFile.Modified.ToLongTimeString());
                    //if (remoteFile.Modified.CompareTo(localFile.Modified) <= 0)
                    DateTime localDate = DateTime.Parse(localFile.Modified.ToLongTimeString());
                    if (remoteFile.Modified >= localDate)
                    {
                        Console.WriteLine("Continueing...");
                        continue;
                    }
                        
                    
                    deleteFilesList.Add(remoteFile.FileUri);
                }
                addFilesList.Add(localFile);
            }

            // Files to remove
            foreach (var remoteFile in remoteFiles)
            {                
                if (!localFilesDic.ContainsKey(remoteFile.CloudFileName))
                {
                    deleteFilesList.Add(remoteFile.FileUri);
                }
            }

            // delete the removed files
            foreach (var delFile in deleteFilesList)
                deleteRemoteFile(delFile);

            // upload the new files
            foreach (var newFile in addFilesList)
                addFile(newFile);
                
        }

        private static List<FileEntry> getLocalFiles(DirectoryInfo localDir)
        {
            Console.WriteLine("Scanning local folder [" + localDir.FullName + "]");
            var filesList = new List<FileEntry>();
            FileInfo[] localFiles = localDir.GetFiles();
            foreach (var file in localFiles)
            {
                Console.WriteLine("Processing: " + file.FullName);
                filesList.Add(new FileEntry()
                {         
                    FileUri = null,
                    CloudFileName = monitoredFolderPath + "/" + file.FullName,
                    Modified = file.LastWriteTime,
                    FileInfo = file,
                });
            }
            return filesList;

        }

        private static List<FileEntry> getCloudFiles()
        {
            Console.WriteLine("Getting cloud files...");
            // Get a list of the blobs
            var blobs = _BlobContainer.ListBlobs();
            var filesList = new List<FileEntry>();

            // For each item, create a FileEntry which will populate the grid
            foreach (var blobItem in blobs)
            {
                var cloudBlob = _BlobContainer.GetBlobReference(blobItem.Uri.ToString());
                Console.WriteLine("Blob URI: " + blobItem.Uri.ToString());
                cloudBlob.FetchAttributes();

                Console.WriteLine("Found file in remote storage: " + cloudBlob.Metadata["FileName"]);
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

        private static bool checkValidFolder(string folderPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);
            return di.Exists;
        }

        private static void init()
        {
            // Setup the connection to Windows Azure Storage
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["DataConnectionString"]);
            _BlobClient = storageAccount.CreateCloudBlobClient();

            // Get and create the container
            string machineName = System.Environment.MachineName;
            Console.WriteLine("Machine name: " + machineName);
            _BlobContainer = _BlobClient.GetContainerReference("tempcontainer3"); //machineName.ToLower());
            _BlobContainer.CreateIfNotExist();

            // Setup the permissions on the container to be public
            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            _BlobContainer.SetPermissions(permissions);
        }

        private static void deleteRemoteFile(Uri fileURI)
        {
            Console.WriteLine("Deleting file: " + fileURI);
            var blob = _BlobContainer.GetBlobReference(fileURI.ToString());
            blob.DeleteIfExists();
        }

        protected static void addFile(FileEntry entry)
        {
            Console.WriteLine("Adding file: " + entry.CloudFileName);
            // Create the Blob and upload the file
            var blob = _BlobContainer.GetBlobReference(Guid.NewGuid().ToString() + entry.FileInfo.Name);
            blob.UploadFromStream(entry.FileInfo.OpenRead());

            // Set the metadata into the blob
            blob.Metadata["FileName"] = entry.CloudFileName;
            blob.Metadata["Modified"] = entry.Modified.ToLongTimeString();
            blob.SetMetadata();
        }
    }
}
