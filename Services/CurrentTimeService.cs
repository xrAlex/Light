using System;
using System.Windows.Threading;

namespace Light.Services
{
    public class CurrentTimeService
    {
        public CurrentTimeService()
        {
            DispatcherTimer timer = new()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, e) => { Tick(); };
            timer.Start();
        }

        public event EventHandler<TimeEventArgs> OnCurrTimeChanged;

        public string GetCurrentTime()
        {
            return DateTime.Now.ToString("HH:mm");
        }

        private void Tick()
        {
            OnCurrTimeChanged?.Invoke(this, new TimeEventArgs(GetCurrentTime()));
        }
    }
}