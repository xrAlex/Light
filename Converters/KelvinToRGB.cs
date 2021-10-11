using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Light.Templates.Entities;

namespace Light.Converters
{
    internal static class KelvinToRGB
    {
        /// <summary>
        /// Converts color temperature (Kelvin) to RGB format
        /// </summary>
        /// <remarks> <see href="http://tannerhelland.com/4435/convert-temperature-rgb-algorithm-code">Algorithm source</see> </remarks>
        /// <returns> <see cref="RGBMask"/> color mask </returns>
        public static RGBMask Convert(int kelvins)
        {
            RGBMask mask = new
            (
                kelvins > 6600
                    ? Math.Pow(kelvins * 0.01 - 60, -0.1332047592) * 329.698727446 / 255
                    : 1,
                kelvins > 6600
                    ? Math.Pow(kelvins * 0.01 - 60, -0.0755148492) * 288.1221695283 / 255
                    : (Math.Log(kelvins * 0.01) * 99.4708025861 - 161.1195681661) / 255,
                kelvins >= 6600
                    ? 1
                    : kelvins <= 1900
                        ? 0
                        : (Math.Log(kelvins * 0.01 - 10) * 138.5177312231 - 305.0447927307) / 255
            );

            return mask;
        }
    }
}
