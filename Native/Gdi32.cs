#region

using System.Runtime.InteropServices;
using Light.Templates.Entities;

#endregion

namespace Light.Native
{
    internal static class Gdi32
    {
        private const string Dll = "gdi32.dll";

        [DllImport(Dll, SetLastError = true)]
        public static extern nint CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, nint lpInitData);

        [DllImport(Dll, SetLastError = true)]
        public static extern bool DeleteDC(nint hDc);

        [DllImport(Dll, SetLastError = true)]
        public static extern bool SetDeviceGammaRamp(nint hDc, ref GammaRamp lpRamp);

        [DllImport(Dll, SetLastError = true)]
        public static extern bool GetDeviceGammaRamp(nint hDc, out GammaRamp lpRamp);
    }
}