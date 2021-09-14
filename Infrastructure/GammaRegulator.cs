using System;
using System.Runtime.InteropServices;

namespace Light.Infrastructure
{
    public class GammaRegulator
    {
        public void ApplyGamma(float gammaIntensity, float blueReduceIntensity, string screenName)
        {
            var dc = CreateDC(screenName, null, null, 0);
            const int minChannelValue = 64;
            const int maxChannelValue = 256;
            const int maxUShort = 65535;

            Ramp channels = new()
            {
                Blue = new ushort[maxChannelValue],
                Green = new ushort[maxChannelValue],
                Red = new ushort[maxChannelValue]
            };

            for (var i = 1; i < maxChannelValue; i++)
            {
                double value = i * (gammaIntensity + minChannelValue);
                value = value > maxUShort ? maxUShort : value;

                channels.Red[i] = Convert.ToUInt16(value);
                channels.Green[i] = Convert.ToUInt16(value);
                channels.Blue[i] = Convert.ToUInt16(value * blueReduceIntensity);
            }

            SetDeviceGammaRamp(dc, ref channels);
            DeleteDC(dc);
        }

        #region DllImport

        [DllImport("gdi32.dll")]
        private static extern nint CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, nint lpInitData);

        [DllImport("gdi32.dll")]
        private static extern nint DeleteDC(nint dc);

        [DllImport("gdi32.dll")]
        private static extern int SetDeviceGammaRamp(nint hDc, ref Ramp lpRamp);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct Ramp
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Red;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Green;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Blue;
        }

        #endregion
    }
}