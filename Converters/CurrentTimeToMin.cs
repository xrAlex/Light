using System;

namespace Sparky.Converters
{
    internal struct CurrentTimeToMin
    {
        public static int Convert()
        {
            var currentTime = DateTime.Now;
            return currentTime.Hour * 60 + currentTime.Minute;
        }
    }
}
