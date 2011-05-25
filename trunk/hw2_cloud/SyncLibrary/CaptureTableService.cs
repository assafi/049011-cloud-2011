using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace SyncLibrary
{
    public class CaptureTableService : TableServiceContext
    {
        public IQueryable<CaptureEntry> Captures
        {
            get
            {
                return this.CreateQuery<CaptureEntry>("Captures");
            }
        }

        public CaptureTableService(string baseAddress, StorageCredentials credentials)
            : base(baseAddress, credentials) { }
        

        public string addCaptureEntry(string url)
        {
            string guid = Guid.NewGuid().ToString();
            this.AddObject("Captures", new CaptureEntry { id = guid, url = url });
            this.SaveChanges();
            return guid;
        }

        public string startProcessingCapture(string taskId, string wid)
        {
            CaptureEntry captureEntry = (from capture in Captures where capture.id == taskId select capture).First();
            captureEntry.StartTime = DateTime.Now;
            captureEntry.WorkerId = wid;
            this.UpdateObject(captureEntry);
            this.SaveChanges();
            return captureEntry.url;
        }

        public void finishProcessingCapture(string taskId, CloudBlob blobRef)
        {
            CaptureEntry captureEntry = (from capture in Captures where capture.id == taskId select capture).First();
            captureEntry.blobUri = blobRef.Uri.AbsolutePath;
            captureEntry.EndTime = DateTime.Now;
            this.UpdateObject(captureEntry);
            this.SaveChanges();
        }

        public IEnumerable<CaptureEntry> getAllCapturesByUrl(string url)
        {
            IEnumerable<CaptureEntry> entries = from capture in Captures where capture.url == url select capture;
            Console.Out.WriteLine("Number of entries for " + url + " is: " + entries.Count());
            return entries;
        }
    }
}
