#region

using System;
using Light.Templates.Entities;

#endregion

namespace Light.Infrastructure
{
    /// <summary>
    /// Класс проверяет какой рабочий период необходимо установить для экрана
    /// </summary>
    public class WorkTime
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
