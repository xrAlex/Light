using Light.Models.Entities;
using Light.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Infrastructure
{
    public class WorkTime
    {
        public bool IsWorkTime(ScreenEntity screen)
        {
            var currentTime = DateTime.Now;
            int currentMin = ConvertToMin(currentTime.Hour, currentTime.Minute);
            int startFromMin = screen.StartTime;
            int endFromMin = screen.EndTime;

            if (endFromMin < startFromMin)
            {
                if (currentMin < startFromMin && currentMin < endFromMin) return true;
                if (currentMin >= startFromMin) return true;
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
