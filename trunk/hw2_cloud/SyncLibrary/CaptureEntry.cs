using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace SyncLibrary
{
    public class CaptureEntry : Microsoft.WindowsAzure.StorageClient.TableServiceEntity
    {
        public string id { get; set; } //The id of the capture request
        public string url { get; set; } //File URI on the storage            
        public DateTime StartTime { get; set; } //Last modification date
        public DateTime EndTime { get; set; } //Last modification date
        public string WorkerId { get; set; } //More File info created by OS
        public string blobUri { get; set; } //More File info created by OS
        
        public CaptureEntry()
        {
            PartitionKey = "a";
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
        }

        override public string ToString()
        {
            return string.Format("id: {0}, url: {1}", id, url);
        }
    };
}
