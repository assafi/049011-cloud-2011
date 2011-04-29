using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace SyncApp
{
    public class SyncLoggerService : TableServiceContext
    {
        private string _serviceName = null;

        public IQueryable<SyncLoggerEntry> Logs
        {
            get
            {
                return this.CreateQuery<SyncLoggerEntry>("Logs");
            }
        }

        public SyncLoggerService(string serviceName, string baseAddress, StorageCredentials credentials)
            : base(baseAddress, credentials)
        {
            _serviceName = serviceName;
        }

        public void LogEntry(string fileName, OperationType operation)
        {
            this.AddObject("Logs", new SyncLoggerEntry { ComputerName = _serviceName, FileName = fileName, Operation = operation.ToString() });
            this.SaveChanges();
        }
    }
}
