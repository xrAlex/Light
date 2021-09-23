#region

using System;
using System.Windows.Threading;
using Light.Templates.EventHandlers;

#endregion

namespace Light.Services
{
    /// <summary>
    /// Класс обновляет текущее время отображаемоена главном окне приложения
    /// </summary>
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

        public string GetCurrentTime() => DateTime.Now.ToString("HH:mm");

        private void Tick()
        {
            OnCurrTimeChanged?.Invoke(this, new TimeEventArgs(GetCurrentTime()));
        }
    }
}