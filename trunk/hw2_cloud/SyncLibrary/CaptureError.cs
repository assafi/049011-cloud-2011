using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncLibrary
{
    public class CaptureError : Exception
    {
        public CaptureError(string p) : base(p) { }
    }
}
