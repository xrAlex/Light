using System;
using Light.Templates.Entities;
using Light.WinApi;

namespace Light.Infrastructure
{
    /// <summary>
    /// Applies a color configuration to screen source
    /// </summary>
    public static class GammaRegulator
    {
        /// <summary>
        /// Converts color configuration values to work with WinApi
        /// </summary>
        public static void ApplyColorConfiguration(int colorTemperature, float brightness, string screenName)
        {
            var dc = Native.CreateDC(screenName, null, null, 0);
            const int maxChannelValue = 256;
            const int channelMult = 255;

            var RGBmask = GetRGBFromKelvin(colorTemperature);

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


        /// <summary>
        /// Converts color temperature (Kelvin) to RGB format
        /// </summary>
        /// <remarks> Algorithm http://tannerhelland.com/4435/convert-temperature-rgb-algorithm-code </remarks>
        /// <returns> RGB color mask </returns>
        private static RGBMask GetRGBFromKelvin(int kelvinValue)
        {
            RGBMask mask = new
            (
                kelvinValue > 6600
                    ? Math.Pow(kelvinValue * 0.01 - 60, -0.1332047592) * 329.698727446 / 255
                    : 1, 
                kelvinValue > 6600
                    ? Math.Pow(kelvinValue * 0.01 - 60, -0.0755148492) * 288.1221695283 / 255
                    : (Math.Log(kelvinValue * 0.01) * 99.4708025861 - 161.1195681661) / 255,
               kelvinValue >= 6600
                    ? 1
                    : kelvinValue <= 1900
                        ? 0
                        : (Math.Log(kelvinValue * 0.01 - 10) * 138.5177312231 - 305.0447927307) / 255
            );

#if DEBUG
            DebugConsole.Print($"Kelvin = {kelvinValue} /nRGB Mask [R: {mask.Red} G:{mask.Green} B: {mask.Blue}]");
#endif

            return mask;
        }

    }
}