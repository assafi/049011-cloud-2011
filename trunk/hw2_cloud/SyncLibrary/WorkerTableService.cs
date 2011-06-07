using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace SyncLibrary
{
    public class WorkerTableService : TableServiceContext
    {
        public IQueryable<WorkerEntry> Workers
        {
            get
            {
                return this.CreateQuery<WorkerEntry>("Workers");
            }
        }

        public WorkerTableService(string baseAddress, StorageCredentials credentials)
            : base(baseAddress, credentials) { }

        public void addWorker(string wid)
        {
            string id = Guid.NewGuid().ToString();
            this.AddObject("Workers", new WorkerEntry { wid = wid });
            this.SaveChanges();
        }

        public void removeWorder(string wid)
        {
            WorkerEntry w = (from worker in Workers where worker.wid == wid select worker).First<WorkerEntry>();
            this.DeleteObject(w);
            this.SaveChanges();
        }

        public int workersCount()
        {
            IEnumerable<WorkerEntry> workers = from w in Workers select w;
            return workers.Count<WorkerEntry>();
        }

        public IEnumerable<WorkerEntry> workers()
        {
            return (from w in Workers select w);
        }

        public static WorkerTableService initWorkersTable(CloudStorageAccount account)
        {
            CloudTableClient.CreateTablesFromModel(typeof(WorkerTableService),
                                                  account.TableEndpoint.AbsoluteUri, account.Credentials);
           return new WorkerTableService(account.TableEndpoint.ToString(), account.Credentials);
        }
    }
}
