using System;

namespace Light.Templates.EventHandlers
{
    public class TimeEventArgs : EventArgs
    { 
        public string CurrTime { get; }

        public TimeEventArgs(string currTime)
        {
            CurrTime = currTime;
        }
    }
}