using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Threading;
using System.Data.Services.Client;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using SyncLibrary;


namespace SyncApp
{
    class Program
    {

        private static CloudBlobContainer _BlobContainer = null;
        private static SyncLoggerService _LogService = null;

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
            DirectoryInfo di = new DirectoryInfo(monitoredFolderPath);
            while (true)
            {
                di.Refresh();
                List<FileEntry> cloudFiles = SyncFileBrowser.getCloudFiles(_BlobContainer);
                List<FileEntry> localFiles = getLocalFiles(di);
                syncLocalRemoteFiles(localFiles, cloudFiles);
                Thread.Sleep(SLEEP_TIME);
            }
        }

/*        private static void printLog()
        {
            Console.WriteLine("Log:");
            foreach (var entry in _LogService.Logs) {
                Console.WriteLine(entry);
            }
        }
 */ 

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
            List<FileEntry> deleteFilesList = new List<FileEntry>();


            // Files to add / update 
            foreach (var localFile in localFiles)
            {
                FileEntry remoteFile;
                if (remoteFilesDic.TryGetValue(localFile.CloudFileName, out remoteFile))                
                {
                    // if file had changed - update it
                    DateTime localTime = DateTime.Parse(localFile.Modified.ToString());
                    if (remoteFile.Modified >= localTime)
                    {
                        continue;
                    }
                        
                    
                    deleteFilesList.Add(remoteFile);
                }
                addFilesList.Add(localFile);
            }

            // Files to remove
            foreach (var remoteFile in remoteFiles)
            {                
                if (!localFilesDic.ContainsKey(remoteFile.CloudFileName))
                {
                    deleteFilesList.Add(remoteFile);
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
            var filesList = new List<FileEntry>();
            FileInfo[] localFiles = localDir.GetFiles();
            foreach (var file in localFiles)
            {
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


        private static bool checkValidFolder(string folderPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);
            return di.Exists;
        }

        private static void init()
        {
            string machineName = System.Environment.MachineName.ToLower();

            // Setup the connection to Windows Azure Storage
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["DataConnectionString"]);
            _BlobContainer = SyncFileBrowser.initBlob(storageAccount, machineName);
            _LogService = SyncFileBrowser.initTable(storageAccount, machineName);
        }

        

        private static void deleteRemoteFile(FileEntry entry)
        {
            /*
             * Delete Blob
             */
            Console.WriteLine("Deleting file: " + entry.CloudFileName);
            var blob = _BlobContainer.GetBlobReference(entry.FileUri.ToString());
            blob.DeleteIfExists();

            /*
             * Log delete operation
             */ 
            try
            {
                _LogService.LogEntry(entry.CloudFileName, OperationType.FILE_DELETE);
            }
            catch (DataServiceRequestException ex)
            {
                Console.WriteLine("Unable to connect to the table storage server. Please check that the service is running.\n"
                                 + ex.Message);
            }
        }

        protected static void addFile(FileEntry entry)
        {
            Console.WriteLine("Adding file: " + entry.CloudFileName);
            // Create the Blob and upload the file
            var blob = _BlobContainer.GetBlobReference(Guid.NewGuid().ToString() + entry.FileInfo.Name);
            blob.UploadFromStream(entry.FileInfo.OpenRead());

            // Set the metadata into the blob
            blob.Metadata["FileName"] = entry.CloudFileName;
            blob.Metadata["Modified"] = entry.Modified.ToString();
            blob.SetMetadata();

            /*
            * Log add operation
            */
            try
            {
                _LogService.LogEntry(entry.CloudFileName, OperationType.FILE_CREATE);
            }
            catch (DataServiceRequestException ex)
            {
                Console.WriteLine("Unable to connect to the table storage server. Please check that the service is running.\n"
                                 + ex.Message);
            }
        }
    }
}
