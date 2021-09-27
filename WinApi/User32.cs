#region

using System.Runtime.InteropServices;
using System.Text;
using Light.Templates.Entities;

#endregion

namespace Light.WinApi
{
    internal static partial class Native
    {
        public delegate bool EnumWindowsProc(nint hWnd, nint lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(HandleRef hWnd, out Rect lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern nint GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(nint hWnd, out uint pId);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(nint hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowTextLength(nint hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, nint lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool IsWindowVisible(nint hWnd);
    }
}
