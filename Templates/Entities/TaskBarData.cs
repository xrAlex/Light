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
        private nint HWnd { get; set; }
        private int UCallbackMessage { get; set; }
        private int UEdge { get; set; }
        public Rect Rc { get; set; }
        private nint LParam { get; set; }

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
