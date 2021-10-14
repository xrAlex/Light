using Light.Infrastructure;
using Light.Models;
using System.Threading;
using System.Threading.Tasks;
using Light.Services.Interfaces;

namespace Light.Services
{
    /// <summary>
    /// Contains method for control current time period
    /// </summary>
    internal sealed class PeriodWatcherService : IPeriodWatcherService
    {
        private readonly ScreenModel _screenModel;
        private readonly WorkTime _workTime;
        private readonly ISettingsService _settingsService;
        private CancellationTokenSource _cts;

        public PeriodWatcherService(ISettingsService settingsService)
        {
            _screenModel = new ScreenModel(settingsService);
            _workTime = new WorkTime();
            _settingsService = settingsService;
        }

        /// <summary>
        /// Starts current period watcher cycle
        /// </summary>
        public void StartWatch()
        {
            _cts = new CancellationTokenSource();
            Task.Run(() => Cycle(_cts.Token), _cts.Token).ConfigureAwait(false);
#if DEBUG
            Logging.Write("PeriodWatcher started");
#endif
        }

        /// <summary>
        /// Stops current period watcher cycle
        /// </summary>
        public void StopWatch()
        {
            _cts.Cancel();
#if DEBUG
            Logging.Write("PeriodWatcher stopped");
#endif
        }

        /// <summary>
        /// Checks in cycle which color configuration should be set
        /// </summary>
        private async Task Cycle(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                foreach (var screen in _settingsService.Screens)
                {
                    if (!screen.IsActive) continue;

                    var isWorkTime = _workTime.IsDayPeriod(screen);

                    if (isWorkTime)
                    {
                        if (_settingsService.CheckFullScreenApps)
                        {
                            if (_screenModel.IsFullScreenWindowFounded(screen))
                            {
                                _screenModel.SetDayPeriod(screen);
                            }
                            else
                            {
                                _screenModel.SetNightPeriod(screen);
                            }
                        }
                        else
                        {
                            _screenModel.SetNightPeriod(screen);
                        }
                    }
                    else
                    {
                        _screenModel.SetDayPeriod(screen);
                    }
                }

                await Task.Delay(1000, token);
            }
        }
#if DEBUG
        ~PeriodWatcherService()
        {
            Logging.Write("[Service] PeriodWatcherService Disposed");
        }
#endif
    }
}