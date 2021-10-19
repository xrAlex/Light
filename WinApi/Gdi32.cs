using System.Runtime.InteropServices;
using Sparky.Templates.Entities;


namespace Sparky.WinApi
{
    internal static partial class Native
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern nint CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, nint lpInitData);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteDC(nint hDc);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool SetDeviceGammaRamp(nint hDc, ref GammaRamp lpRamp);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool GetDeviceGammaRamp(nint hDc, out GammaRamp lpRamp);
    }
}