using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            //var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));

            /*
             * Table storage
             */

            var url = "http://www.google.com";
            var outputPath = "C:\\Users\\assafi.TD-CSF\\Documents\\Visual Studio 2010\\Projects\\hw3_cloud";//RoleEnvironment.GetLocalResource("LocalOutput").RootPath;
            var tempFileName = "google.png";
            Trace.TraceInformation("Roleroot : " + Environment.GetEnvironmentVariable("RoleRoot") + @"\approot\CutyCapt\CutyCapt.exe");
            Trace.TraceInformation("Output dir: " + outputPath);

            try
            {
                var proc = new Process()
                            {
                                StartInfo = new ProcessStartInfo(Environment.GetEnvironmentVariable("RoleRoot") + @"\approot\CutyCapt.exe",
                                        string.Format(@"--url=""{0}"" --out=""{1}""",
                                            url,
                                            Path.Combine(outputPath, tempFileName)))
                                {
                                    UseShellExecute = false
                                }
                            };
                proc.Start();
                proc.WaitForExit();
                if (File.Exists(outputPath))
                {
                    Trace.TraceInformation("success");
                }
            }
            catch (Exception e) { Trace.Fail(@"Failed - \approot\CutyCapt.exe"); }


           
            



            /*var container = account.CreateCloudBlobClient().GetContainerReference("output");
            container.CreateIfNotExist();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            var queue = account.CreateCloudQueueClient().GetQueueReference("queue");
            queue.CreateIfNotExist();


            var outputPath = RoleEnvironment.GetLocalResource("LocalOutput").RootPath;

            while (true)
            {
                var msg = queue.GetMessage(); //batch
                if (msg != null)
                {
                    if (msg.DequeueCount < 3)
                    {
                        var split = msg.AsString.Split(new char[] { '/' }, 2);
                        var guid = split[0];
                        var url = split[1];
                        var tempFileName = url + ".png";
                        var proc = new Process()
                        {
                            StartInfo = new ProcessStartInfo(Environment.GetEnvironmentVariable("RoleRoot") + @"\\approot\CutyCapt\CutyCapt.exe",
                                    string.Format(@"--url=""{0}"" --out=""{1}""",
                                        url,
                                        Path.Combine(outputPath, tempFileName)))
                            {
                                UseShellExecute = false
                            }
                        };
                        proc.Start();
                        proc.WaitForExit();
                        if (File.Exists(outputPath))
                        {
                            var blob = container.GetBlobReference(guid);
                            blob.Properties.ContentType = "image/png";
                            blob.UploadFile(outputPath);
                            File.Delete(outputPath);
                        }
                    }
                    queue.DeleteMessage(msg);
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }*/
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
