#region

using System.Runtime.InteropServices;
using Light.Templates.Entities;

#endregion

namespace Light.Native
{
    internal static class User32
    {
        private const string Dll = "user32.dll";

        [DllImport(Dll, SetLastError = true)]
        public static extern bool GetWindowRect(HandleRef hWnd, out Rect lpRect);

        [DllImport(Dll, SetLastError = true)]
        public static extern nint GetForegroundWindow();

        [DllImport(Dll, SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(nint hWnd, ref uint pId);

        [DllImport(Dll, SetLastError = true)]
        public static extern bool IsWindowVisible(nint hWnd);
    }
}
