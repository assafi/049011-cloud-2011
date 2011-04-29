using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SyncLibrary
{
        public class FileEntry
        {
            public Uri FileUri { get; set; }
            public string CloudFileName { get; set; }
            public DateTime Modified { get; set; }
            public FileInfo FileInfo { get; set; }
        }
}
