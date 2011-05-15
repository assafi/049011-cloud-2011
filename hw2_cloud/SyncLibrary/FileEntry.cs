using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SyncLibrary
{
        public class FileEntry
        {
            public Uri FileUri { get; set; } //File URI on the storage            
            public DateTime StartTime { get; set; } //Last modification date
            public DateTime EndTime { get; set; } //Last modification date
            public int WorkerId { get; set; } //More File info created by OS
            public string blobRef { get; set; } //More File info created by OS
        }
}
