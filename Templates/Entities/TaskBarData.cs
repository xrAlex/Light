using System.Runtime.InteropServices;

namespace Light.Templates.Entities
{
    [StructLayout(LayoutKind.Sequential)]
    internal class TaskBarData
    {
        public readonly int CbSize = Marshal.SizeOf(typeof(TaskBarData));
        public nint HWnd { get; set; }
        public int UCallbackMessage { get; set; }
        public int UEdge { get; set; }
        public Rect Rc { get; set; }
        public nint LParam { get; set; }
    }
}
