using System;
using System.Runtime.InteropServices;

namespace Light.Native
{
    internal static class Gdi32
    {
        private const string Dll = "gdi32.dll";

        [DllImport(Dll, SetLastError = true)]
        public static extern nint CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, nint lpInitData);

        [DllImport(Dll, SetLastError = true)]
        public static extern nint DeleteDC(nint dc);

        [DllImport(Dll, SetLastError = true)]
        public static extern int SetDeviceGammaRamp(nint hDc, ref GammaRamp lpRamp);

        [DllImport(Dll, SetLastError = true)]
        public static extern bool GetDeviceGammaRamp(IntPtr hDc, out GammaRamp lpRamp);
    }
}