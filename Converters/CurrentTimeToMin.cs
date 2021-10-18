﻿using System;

namespace Light.Converters
{
    internal sealed class CurrentTimeToMin
    {
        public static int Convert()
        {
            var currentTime = DateTime.Now;
            return currentTime.Hour * 60 + currentTime.Minute;
        }
    }
}
