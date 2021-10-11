using System;

namespace Light.Templates.EventHandlers
{
    internal sealed class TimeEventArgs : EventArgs
    { 
        public string CurrentTime { get; }

        public TimeEventArgs(string currentTime)
        {
            CurrentTime = currentTime;
        }
    }
}