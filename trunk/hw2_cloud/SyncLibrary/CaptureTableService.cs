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
        

        public void addCaptureEntry(string url)
        {
            this.AddObject("Captures", new CaptureEntry { url = url });
            this.SaveChanges();
        }

        public void startProcessingCapture(string url, string wid)
        {
            IEnumerable<CaptureEntry> entries = from capture in Captures where capture.url == url select capture;
            if (entries.Count() != 1) {
                throw new CaptureError("There are " + entries.Count() + " entries in table for url " + url);
            }
            CaptureEntry captureEntry = entries.First();
            captureEntry.StartTime = DateTime.Now;
            captureEntry.WorkerId = wid;
            this.UpdateObject(captureEntry);
        }

        public void finishProcessingCapture(string url, string blobRef)
        {
            IEnumerable<CaptureEntry> entries = from capture in Captures where capture.url == url select capture;
            if (entries.Count() != 1)
            {
                throw new CaptureError("There are " + entries.Count() + " entries in table for url " + url);
            }
            CaptureEntry captureEntry = entries.First();
            captureEntry.blobRef = blobRef;
            captureEntry.EndTime = DateTime.Now;
            this.UpdateObject(captureEntry);
        }
    }
}
