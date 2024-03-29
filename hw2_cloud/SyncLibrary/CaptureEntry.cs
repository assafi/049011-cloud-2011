﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace SyncLibrary
{
    [Serializable]
    public class CaptureEntry : Microsoft.WindowsAzure.StorageClient.TableServiceEntity
    {
        public string id { get; set; } //The id of the capture request
        public string url { get; set; } //File URI on the storage            
        public DateTime StartTime { get; set; } //Start proc time by the last worker who picked this task
        public DateTime EndTime { get; set; } //End proc time
        public string WorkerId { get; set; } //Responsible worker id
        public string blobUri { get; set; } //More File info created by OS
        public Boolean done { get; set; }  //done proc?
        
        public CaptureEntry()
        {
            PartitionKey = "c";
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
        }

        override public string ToString()
        {
            return string.Format("id: {0}, url: {1}", id, url);
        }
    };
}
