using System;

namespace Light.Services
{
    public class TimeEventArgs : EventArgs
    {
        public TimeEventArgs(string currTime)
        {
            CurrTime = currTime;
        }

        public string CurrTime { get; set; }
    }
}