using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Light.Infrastructure
{
    public class GammaRegulator
    {
        #region DllImport

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr DeleteDC(IntPtr DC);

        [DllImport("gdi32.dll")]
        public static extern int SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

        #endregion

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct RAMP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Blue;
        }

        public void ApplyGamma(float GammaIntensity, float BlueReduceIntensity, string screenName)
        {
            var DC = CreateDC(screenName, null, null, IntPtr.Zero);
            const int minChannelValue = 128;
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
