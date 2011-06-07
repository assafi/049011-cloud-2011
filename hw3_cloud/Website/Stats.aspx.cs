using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.WindowsAzure.StorageClient;
using System.Data.Services.Client;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using SyncLibrary;

namespace Website
{
    public partial class Stats : System.Web.UI.Page
    {
        private static CaptureTableService capturesTableService;

        private static WorkerTableService workersTableService;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    /*
                     * Intialize only once
                     */
                    var machineName = System.Environment.MachineName.ToLower();
                    var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
                    capturesTableService = CaptureTableService.initCaptureTable(account);
                    workersTableService = WorkerTableService.initWorkersTable(account);
                }
            }
            catch (DataServiceRequestException ex)
            {
                Console.WriteLine("Unable to connect to the table storage server. Please check that the service is running.<br>"
                                 + ex.Message);
            }
        }

        protected void CapturesCount_DataBinding(object sender, EventArgs e)
        {
            this.CapturesCount.Text = capturesTableService.capturesCount().ToString();
        }

        protected void WorkersCount_DataBinding(object sender, EventArgs e)
        {
            this.WorkersCount.Text = workersTableService.workersCount().ToString();
        }

        protected void PendingURIs_DataBinding(object sender, EventArgs e)
        {
            this.PendingURIs.Text = capturesTableService.pendingCount().ToString();
        }

        protected void WorkersStats_DataBinding(object sender, EventArgs e)
        {
            this.WorkersStats.DataSource = capturesTableService.getWorkersStats();
            this.WorkersStats.DataBind();
        }

        public List<WorkerStat> statsPerWorker()
        {
            List<WorkerStat> res = new List<WorkerStat>();
            var workers = workersTableService.workers();
            foreach(var w in workers) {
               // res.Add(capturesTableService.getWorkerStat(w.wid));
            }
            return res;
        }
    }
}