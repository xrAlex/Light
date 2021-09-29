using System;

namespace Light.Templates.EventHandlers
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