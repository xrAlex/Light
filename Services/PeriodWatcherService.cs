using System.Linq;
using Light.Infrastructure;
using Light.Models;
using System.Threading;
using System.Threading.Tasks;
using Light.Converters;
using Light.Services.Interfaces;
using Light.Templates.Entities;

namespace Light.Services
{
    /// <summary>
    /// Contains method for control current time period
    /// </summary>
    internal sealed class PeriodWatcherService : IPeriodWatcherService
    {
        private readonly ScreenModel _screenModel;
        private readonly ISettingsService _settingsService;
        private CancellationTokenSource _cts;
        private enum Period
        {
            Day,
            Night
        }

        public PeriodWatcherService(ISettingsService settingsService)
        {
            _screenModel = new ScreenModel(settingsService);
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
            LoggingModule.Log.Information("PeriodWatcher started");
#endif
        }

        /// <summary>
        /// Stops current period watcher cycle
        /// </summary>
        public void StopWatch()
        {
            _cts.Cancel();
#if DEBUG
            LoggingModule.Log.Information("PeriodWatcher stopped");
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

                    if (GetCurrentPeriod(screen) == Period.Night)
                    {
                        if (_settingsService.CheckFullScreenApps)
                        {
                            if (IsFullscreenAppFounded(screen))
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

        private Period GetCurrentPeriod(ScreenEntity screen)
        {
            var currentMin = CurrentTimeToMin.Convert();
            var startFromMin = screen.StartTime;
            var endFromMin = screen.EndTime;

            if (endFromMin < startFromMin)
            {
                if (currentMin >= startFromMin || currentMin < startFromMin && currentMin < endFromMin) return Period.Day;
            }
            else
            {
                if (currentMin >= startFromMin && currentMin < endFromMin) return Period.Day;
            }

            return Period.Night;
        }

        private bool IsFullscreenAppFounded(ScreenEntity screen)
        {
            var fullScreenWindowHandle = SystemWindow.GetFullscreenForegroundWindow(screen);
            if (fullScreenWindowHandle == 0) return false;

            var processFileName = SystemProcess.TryGetProcessFileName(fullScreenWindowHandle);
            return processFileName != null && IsAppProcessNameNotInIgnored(processFileName);
        }

        private bool IsAppProcessNameNotInIgnored(string processFileName)
        {
            return _settingsService.ApplicationsWhitelist.Count != 0 && _settingsService.ApplicationsWhitelist.All(p => p != processFileName);
        }

#if DEBUG
        ~PeriodWatcherService()
        {
            LoggingModule.Log.Verbose("[Service] PeriodWatcherService Disposed");
        }
#endif
    }
}