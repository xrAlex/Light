using System.Runtime.InteropServices;

namespace Light.Templates.Entities
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct TaskBarData
    {
        public int CbSize { get; }
        public nint HWnd { get; }
        public int UCallbackMessage { get; }
        public int UEdge { get; }
        public Rect Rc { get; }
        public nint LParam { get; }

        public TaskBarData(int cbSize, nint hWnd, int uCallbackMessage, int uEdge, Rect rc, nint lParam)
        {
            CbSize = Marshal.SizeOf(typeof(TaskBarData));
            HWnd = hWnd;
            UCallbackMessage = uCallbackMessage;
            UEdge = uEdge;
            Rc = rc;
            LParam = lParam;
        }
    }
}
