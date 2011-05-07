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
            public string CloudFileName { get; set; } //File name (absolute path on local computer)
            public DateTime Modified { get; set; } //Last modification date
            public FileInfo FileInfo { get; set; } //More File info created by OS
            public string ContainerName { get; set; } //The holder container of this file 
            public string NameWithLink 
            { 
                get 
                {
                    return "<a href=\"" + FileUri.ToString() + "\">" + CloudFileName + "\"</a>"; 
                } 
            }
        }
}
