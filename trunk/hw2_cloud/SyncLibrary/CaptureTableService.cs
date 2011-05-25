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
            string id = Guid.NewGuid().ToString();
            this.AddObject("Captures", new CaptureEntry {id = id, url = url });
            this.SaveChanges();
            return id;
        }

        public string startProcessingCapture(string taskId, string wid)
        {
            IEnumerable<CaptureEntry> entries = from capture in Captures where capture.id == taskId select capture;
            if (entries.Count() != 1) {
                throw new CaptureError("There are " + entries.Count() + " entries in table for task id " + taskId);
            }
            CaptureEntry captureEntry = entries.First();
            captureEntry.StartTime = DateTime.Now;
            captureEntry.WorkerId = wid;
            this.UpdateObject(captureEntry);
            this.SaveChanges();
            return captureEntry.url;
        }

        public void finishProcessingCapture(string taskId, CloudBlob blobRef)
        {
            IEnumerable<CaptureEntry> entries = from capture in Captures where capture.id == taskId select capture;
            if (entries.Count() != 1)
            {
                throw new CaptureError("There are " + entries.Count() + " entries in table for task id " + taskId);
            }
            CaptureEntry captureEntry = entries.First();
            captureEntry.blobRef = blobRef;
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
