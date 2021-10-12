using System;
using Light.Templates.Entities;
using Light.WinApi;

namespace Light.Infrastructure
{
    /// <summary>
    /// Applies a color configuration to screen source
    /// </summary>
    internal static class GammaRegulator
    {
        /// <summary>
        /// Converts color configuration to RGB colors for work with WinApi
        /// </summary>
        public static void ApplyColorConfiguration(int colorTemperature, double brightness, string screenName)
        {
            var dc = Native.CreateDC(screenName, null, null, 0);
            const int maxChannelValue = 256;
            const int channelMult = 255;

            var RGBmask = Converters.KelvinToRGB.Convert(colorTemperature);

            var channels = new GammaRamp
            {
                Red = new ushort[maxChannelValue],
                Green = new ushort[maxChannelValue],
                Blue = new ushort[maxChannelValue]
            };

            for (var i = 0; i < maxChannelValue; i++)
            {
                channels.Red[i] = (ushort) (i * channelMult * RGBmask.Red * brightness);
                channels.Green[i] = (ushort) (i * channelMult * RGBmask.Green * brightness);
                channels.Blue[i] = (ushort) (i * channelMult * RGBmask.Blue * brightness);
            }

            Native.SetDeviceGammaRamp(dc, ref channels);
            Native.DeleteDC(dc);
        }
    }
}