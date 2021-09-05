using System;
using System.Windows.Threading;

namespace Light.Services
{
    public class TimeEventArgs : EventArgs
    {
        public string CurrTime { get; set; }
        public TimeEventArgs(string currTime)
        {
            CurrTime = currTime;
        }
    }

    public class CurrentTimeService
    {
        public event EventHandler<TimeEventArgs> OnCurrTimeChanged;

        public CurrentTimeService()
        {
            DispatcherTimer timer = new();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, e) =>
            {
                Tick();
            };
            timer.Start();
        }

        public string GetCurrentTime() => DateTime.Now.ToString("HH:mm");
        private void Tick()
        {
            OnCurrTimeChanged?.Invoke(this, new TimeEventArgs(GetCurrentTime()));
        }
    }
}
