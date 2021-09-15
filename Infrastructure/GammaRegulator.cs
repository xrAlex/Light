using System;
using System.Runtime.InteropServices;

namespace Light.Infrastructure
{
    public class GammaRegulator
    {
        public void ApplyGamma(float gammaIntensity, float blueReduceIntensity, string screenName)
        {
            var dc = Native.Gdi32.CreateDC(screenName, null, null, 0);
            const int minChannelValue = 64;
            const int maxChannelValue = 256;
            const int maxUShort = 65535;

            Native.GammaRamp channels = new()
            {
                Blue = new ushort[maxChannelValue],
                Green = new ushort[maxChannelValue],
                Red = new ushort[maxChannelValue]
            };

            for (var i = 1; i < maxChannelValue; i++)
            {
                var value = i * (gammaIntensity + minChannelValue);
                value = value > maxUShort ? maxUShort : value;

                channels.Red[i] = Convert.ToUInt16(value);
                channels.Green[i] = Convert.ToUInt16(value);
                channels.Blue[i] = Convert.ToUInt16(value * blueReduceIntensity);
            }

            Native.Gdi32.SetDeviceGammaRamp(dc, ref channels);
            Native.Gdi32.DeleteDC(dc);
        }
    }
}