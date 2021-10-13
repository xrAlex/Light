using Light.Templates.Entities;
using System.Runtime.InteropServices;

namespace Light.WinApi
{
    internal static partial class Native
    {
        [DllImport("shell32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern nint SHAppBarMessage(uint dwMessage, ref TaskBarData pData);
    }
}
