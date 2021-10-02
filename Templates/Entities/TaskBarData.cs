using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Light.Templates.Entities
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct TaskBarData
    {
        public int CbSize { get; set; }
        public nint HWnd { get; set; }
        public int UCallbackMessage { get; set; }
        public int UEdge { get; set; }
        public Rect Rc { get; set; }
        public nint LParam { get; set; }

        public TaskBarData(int cbSize, nint hWnd, int uCallbackMessage, int uEdge, Rect rc, nint lParam)
        {
            CbSize = cbSize;
            HWnd = hWnd;
            UCallbackMessage = uCallbackMessage;
            UEdge = uEdge;
            Rc = rc;
            LParam = lParam;
        }
    }
}
