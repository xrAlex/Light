using System;
using Light.Templates.Entities;

namespace Light.Infrastructure
{
    /// <summary>
    /// Contains method for check which working period needs to be set for screen
    /// </summary>
    internal sealed class WorkTime
    {
        public bool IsDayPeriod(ScreenEntity screen)
        {
            var currentTime = DateTime.Now;
            var currentMin = ConvertToMin(currentTime.Hour, currentTime.Minute);
            var startFromMin = screen.StartTime;
            var endFromMin = screen.EndTime;

            if (endFromMin < startFromMin)
            {
                if (currentMin < startFromMin && currentMin < endFromMin || currentMin >= startFromMin) return true;
            }
            else
            {
                if (currentMin >= startFromMin && currentMin < endFromMin) return true;
            }

            return false;
        }

        private int ConvertToMin(int hour, int min) => hour * 60 + min;
    }
}
