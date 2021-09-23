#region

using System;
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

            var redMult = GetRedFromKelvin(colorTemperature);
            var greenMult = GetGreenFromKelvin(colorTemperature);
            var blueMult = GetBlueFromKelvin(colorTemperature);

            GammaRamp channels = new()
            {
                Red = new ushort[maxChannelValue],
                Green = new ushort[maxChannelValue],
                Blue = new ushort[maxChannelValue]
            };

            for (var i = 0; i < maxChannelValue; i++)
            {
                channels.Red[i] = (ushort) (i * channelMult * redMult * brightness);
                channels.Green[i] = (ushort) (i * channelMult * greenMult * brightness);
                channels.Blue[i] = (ushort) (i * channelMult * blueMult * brightness);
            }

            Gdi32.SetDeviceGammaRamp(dc, ref channels);
            Gdi32.DeleteDC(dc);
        }

        /// <summary>
        /// Метод преобразует кельвины в RGB формат
        /// </summary>
        /// <remarks> Алогоритм: http://tannerhelland.com/4435/convert-temperature-rgb-algorithm-code </remarks>
        /// <returns> Значение красного канала цвета </returns>
        private static double GetRedFromKelvin(int temp)
        {
            if (temp > 6600) return Math.Pow(temp * 0.01 - 60, -0.1332047592) * 329.698727446 / 255;

            return 1;
        }

        /// <summary>
        /// Метод преобразует кельвины в RGB формат
        /// </summary>
        /// <remarks> Алогоритм: http://tannerhelland.com/4435/convert-temperature-rgb-algorithm-code </remarks>
        /// <returns> Значение зеленого канала цвета </returns>
        private static double GetGreenFromKelvin(int temp)
        {
            if (temp > 6600) return Math.Pow(temp / 100 - 60, -0.0755148492) * 288.1221695283 / 255;

            return (Math.Log(temp * 0.01) * 99.4708025861 - 161.1195681661) / 255;
        }

        /// <summary>
        /// Метод преобразует кельвины в RGB формат
        /// </summary>
        /// <remarks> Алогоритм: http://tannerhelland.com/4435/convert-temperature-rgb-algorithm-code </remarks>
        /// <returns> Значение синего канала цвета </returns>
        private static double GetBlueFromKelvin(int temp)
        {
            if (temp >= 6600) return 1;
            if (temp <= 1900) return 0;

            return (Math.Log(temp * 0.01 - 10) * 138.5177312231 - 305.0447927307) / 255;
        }
    }
}