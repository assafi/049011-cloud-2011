using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncLibrary
{
    public class WorkerEntry : Microsoft.WindowsAzure.StorageClient.TableServiceEntity
    {
        public string wid { get; set; }

        public WorkerEntry()
        {
            PartitionKey = "b";
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
        }

        override public string ToString()
        {
            return string.Format("id: {0}", wid);
        }
    }
}
