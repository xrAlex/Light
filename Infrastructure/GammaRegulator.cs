using System;
using System.Runtime.InteropServices;

namespace Light.Infrastructure
{
    public class GammaRegulator
    {
        #region DllImport

        [DllImport("gdi32.dll")]
        private static extern nint CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, nint lpInitData);

        [DllImport("gdi32.dll")]
        private static extern nint DeleteDC(nint DC);

        [DllImport("gdi32.dll")]
        private static extern int SetDeviceGammaRamp(nint hDC, ref RAMP lpRamp);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct RAMP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Blue;
        }

        #endregion

        public void ApplyGamma(float GammaIntensity, float BlueReduceIntensity, string screenName)
        {
            var DC = CreateDC(screenName, null, null, 0);
            const int minChannelValue = 64;
            const int maxChannelValue = 256;
            const int maxUShort = 65535;
           
            RAMP channels = new()
            {
                Blue = new ushort[maxChannelValue],
                Green = new ushort[maxChannelValue],
                Red = new ushort[maxChannelValue]
            };

            for (int i = 1; i < maxChannelValue; i++)
            {
                double value = i * (GammaIntensity + minChannelValue);
                value = value > maxUShort ? maxUShort : value;
   
                channels.Red[i] = Convert.ToUInt16(value);
                channels.Green[i] = Convert.ToUInt16(value);
                channels.Blue[i] = Convert.ToUInt16(value * BlueReduceIntensity);
            }

            SetDeviceGammaRamp(DC, ref channels);
            DeleteDC(DC);
        }
    }
}
