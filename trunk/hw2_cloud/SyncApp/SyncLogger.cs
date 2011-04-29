using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace SyncApp
{
    public class SyncLoggerEntry : Microsoft.WindowsAzure.StorageClient.TableServiceEntity
    {
        public string ComputerName { get; set; }
        public string FileName { get; set; }
        public string Operation { get; set; }

        public SyncLoggerEntry()
        {
            PartitionKey = "a";
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
        }

        override public string ToString()
        {
            return "Operation: " + Operation.ToString() + ", Computer Name: " + ComputerName + ", FileName: " + FileName;
        }
    };

    public enum OperationType
    {
        FILE_CREATE, FILE_DELETE
    };
}
