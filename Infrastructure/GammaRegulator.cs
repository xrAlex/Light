#region

using System;
using System.Collections.Generic;
using System.Windows.Documents;
using Light.Native;
using Light.Templates.Entities;

#endregion

namespace Light.Infrastructure
{
    /// <summary>
    /// Класс применяет цветовую конфигурацию к источнику отображения
    /// </summary>
    public static class GammaRegulator
    {
        /// <summary>
        /// Метод преобразует значения цветовой конфигурации для работы с WinApi
        /// </summary>
        public static void ApplyColorConfiguration(int colorTemperature, float brightness, string screenName)
        {
            // http://jonls.dk/2010/09/windows-gamma-adjustments/
            var dc = Gdi32.CreateDC(screenName, null, null, 0);
            const int maxChannelValue = 256;
            const int channelMult = 255;

            var RGBmask = GetRGBFromKelvin(colorTemperature);

            GammaRamp channels = new()
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

            Gdi32.SetDeviceGammaRamp(dc, ref channels);
            Gdi32.DeleteDC(dc);
        }


        /// <summary>
        /// Метод преобразует кельвины в RGB формат
        /// </summary>
        /// <remarks> Алогоритм: http://tannerhelland.com/4435/convert-temperature-rgb-algorithm-code </remarks>
        /// <returns> Маску RGB цветов </returns>
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


            return mask;
        }

    }
}