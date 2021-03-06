using System;
using System.Windows.Threading;
using Sparky.Services.Interfaces;
using Sparky.Templates.EventHandlers;

namespace Sparky.Services
{
    /// <summary>
    /// Updates current time
    /// </summary>
    internal sealed class CurrentTimeService : ICurrentTimeService
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

        /// <summary>
        /// Gets current time
        /// <remarks> 24 hrs time format 00:00 </remarks>
        /// </summary>
        /// <returns> Current time <see cref="string"/></returns>
        public string GetCurrentTime() => DateTime.Now.ToString("HH:mm");

        private void Tick()
        {
            OnCurrTimeChanged?.Invoke(this, new TimeEventArgs(GetCurrentTime()));
        }
#if DEBUG
        ~CurrentTimeService()
        {
            LoggingModule.Log.Verbose("[Service] CurrentTimeService Disposed");
        }
#endif
    }
}