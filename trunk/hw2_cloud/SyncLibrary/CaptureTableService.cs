using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using System.Data.Services.Client;

namespace SyncLibrary
{
    public class CaptureTableService : TableServiceContext
    {
        
        public IQueryable<CaptureEntry> Captures {
            get {
                return CreateQuery<CaptureEntry>("captures");
            }
        }

        public CaptureTableService(string baseAddress, StorageCredentials credentials)
            : base(baseAddress, credentials) {
                base.IgnoreResourceNotFoundException = true;

            /*
                foreach (var c in Captures)
                {
                    DeleteObject(c);
                }
                SaveChanges();
             */
        }
        
        public string addCaptureEntry(string url)
        {
            string guid = Guid.NewGuid().ToString();
            CaptureEntry e = new CaptureEntry { id = guid, url = url, done = false };
            e.WorkerId = string.Empty;
            e.StartTime = DateTime.Now;
            e.EndTime = DateTime.Now;
            e.blobUri = string.Empty;
            this.AddObject("captures", e);
            this.SaveChanges();
            return guid;
        }

        public string startProcessingCapture(string taskId, string wid)
        {
            CaptureEntry captureEntry = (from capture in Captures where capture.id == taskId select capture).First();
            captureEntry.StartTime = DateTime.Now;
            captureEntry.EndTime = DateTime.Now;
            captureEntry.done = false;
            captureEntry.WorkerId = wid;
            captureEntry.blobUri = string.Empty;
            this.UpdateObject(captureEntry);
            this.SaveChanges();
            return captureEntry.url;
        }

       public void finishProcessingCapture(string taskId, CloudBlob blobRef)
       {
           CaptureEntry captureEntry = (from capture in Captures
                where capture.id == taskId select capture).First();

           // Remark: need only the relative uri when internally downloading from blob,
           // didnt find a way to retrieve it. If it makes problems when in the Cloud, might need to change here
           captureEntry.blobUri = blobRef.Uri.Segments[blobRef.Uri.Segments.Length - 1];
           captureEntry.EndTime = DateTime.Now;
           captureEntry.done = true;
           this.UpdateObject(captureEntry);
           this.SaveChanges();
       }

       public IEnumerable<CaptureEntry> getAllCapturesByUrl(string url)
       {
           IEnumerable<CaptureEntry> entries = from capture in
                                                   Captures
                                               where capture.url == url &&
                                                   !capture.blobUri.Equals(string.Empty)
                                               select capture;

           return entries;
       }

       public int capturesCount()
       {
           try
           {
               IEnumerable<CaptureEntry> captures = (from capture in Captures where capture.done == true select capture);
               return captures.Count();
           }
           catch (DataServiceQueryException)
           {
               return 0;
           }
       }

       public int pendingCount()
       {
           try
           {
               IEnumerable<CaptureEntry> captures = (from capture in Captures where capture.done == false select capture);
               return captures.Count();
           }
           catch (DataServiceQueryException)
           {
               return 0;
           }
       }

       public IEnumerable<WorkerStat> getWorkersStats()
       {
           List<CaptureEntry> list = (from c in Captures select c).ToList();
           var qry = from c in list
                     where c.done == true
                     group c by c.WorkerId into wgrp
                     select new WorkerStat
                     {
                         wid = wgrp.Key,
                         imgCount = wgrp.Count(),
                         avgProcTime = new TimeSpan((long)wgrp.Average(z => z.EndTime.Ticks - z.StartTime.Ticks)).TotalSeconds
                     };

           return qry;
       }

       public static CaptureTableService initCaptureTable(CloudStorageAccount account)
       {
           CloudTableClient.CreateTablesFromModel(typeof(CaptureTableService),
                                                  account.TableEndpoint.AbsoluteUri, account.Credentials);
           return new CaptureTableService(account.TableEndpoint.ToString(), account.Credentials);
       }
    }
}
