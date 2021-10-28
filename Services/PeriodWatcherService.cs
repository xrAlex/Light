using System;
using System.Linq;
using Sparky.Infrastructure;
using Sparky.Models;
using System.Threading;
using System.Threading.Tasks;
using Sparky.Services.Interfaces;
using Sparky.Templates.Entities;

namespace Sparky.Services
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
            Task.Run(() => CycleAsync(_cts.Token), _cts.Token).ConfigureAwait(false);
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
        private async Task CycleAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                foreach (var screen in _settingsService.Screens)
                {
                    if (!screen.IsActive) continue;

                    var (period, remainingTime) = GetCurrentPeriod(screen);

                    if (period == Period.Night)
                    {
                        if (_settingsService.CheckFullScreenApps)
                        {
                            if (IsFullScreenAppFounded(screen))
                            {
                                _screenModel.ApplyColorConfiguration
                                (
                                    screen,
                                    new ColorConfiguration
                                    (
                                        screen.DayColorConfiguration.ColorTemperature,
                                        screen.DayColorConfiguration.Brightness
                                    )
                                );
                                screen.IsDayTimePeriod = true;
                            }
                            else
                            {
                                _screenModel.ApplyColorConfiguration(screen, CalculateCurrentConfiguration(period, screen, remainingTime));
                                screen.IsDayTimePeriod = false;
                            }
                        }
                        else
                        {
                            _screenModel.ApplyColorConfiguration(screen, CalculateCurrentConfiguration(period, screen, remainingTime));
                            screen.IsDayTimePeriod = false;
                        }
                    }
                    else
                    {
                        _screenModel.ApplyColorConfiguration(screen, CalculateCurrentConfiguration(period, screen, remainingTime));
                        screen.IsDayTimePeriod = true;
                    }
                }

                await Task.Delay(1000, token);
            }
        }

        private ColorConfiguration CalculateCurrentConfiguration(Period currentPeriod, ScreenEntity screen, double remainingTime)
        {
            var screenDayConfig = screen.DayColorConfiguration;
            var screenNightConfig = screen.NightColorConfiguration;

            if (!_settingsService.SmoothGammaChange)
            {
                return currentPeriod == Period.Day
                    ? new ColorConfiguration(screenDayConfig.ColorTemperature, screenDayConfig.Brightness)
                    : new ColorConfiguration(screenNightConfig.ColorTemperature, screenNightConfig.Brightness);
            }

            if (remainingTime is not (> 0 and <= 10))
            {
                return currentPeriod == Period.Day
                    ? new ColorConfiguration(screenDayConfig.ColorTemperature, screenDayConfig.Brightness)
                    : new ColorConfiguration(screenNightConfig.ColorTemperature, screenNightConfig.Brightness);
            }

            return currentPeriod == Period.Day 
                ? GetTransientColorConfiguration(screenNightConfig, screenDayConfig, remainingTime) 
                : GetTransientColorConfiguration(screenDayConfig, screenNightConfig, remainingTime);
        }

        private ColorConfiguration GetTransientColorConfiguration(ColorConfiguration targetValues, ColorConfiguration startValues, double remainingTime)
        {
            var multiplier = remainingTime * 0.1;

            return new ColorConfiguration
            (
                Extensions.Lerp
                (
                    targetValues.ColorTemperature,
                    startValues.ColorTemperature,
                    multiplier
                ), 
                Extensions.Lerp
                (
                    targetValues.Brightness,
                    startValues.Brightness,
                    multiplier
                )
            );
        }

        private (Period period, double remainingTime) GetCurrentPeriod(ScreenEntity screen)
        {
            var dayStart = screen.DayStartTime;
            var nightStart = screen.NightStartTime;

            var currentTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            var nightStartTime = new TimeSpan(nightStart.Hour, nightStart.Minute, 0);
            var dayStartTime = new TimeSpan(dayStart.Hour, dayStart.Minute, 0);
            double remainingTime;

            // in current day
            if (dayStartTime <= nightStartTime)
            {
                if (currentTime >= dayStartTime && currentTime < nightStartTime)
                {
                    remainingTime = nightStartTime.TotalMinutes - currentTime.TotalMinutes;
                    return (Period.Day, remainingTime);
                }

                remainingTime = dayStartTime.TotalMinutes - currentTime.TotalMinutes;
                return (Period.Night, remainingTime);
            }

            //in next day
            if (currentTime >= dayStartTime || currentTime < nightStartTime)
            {
                const int midNight = 1440;
                if (currentTime.TotalMinutes == 0)
                {
                    remainingTime = nightStartTime.TotalMinutes - currentTime.TotalMinutes;
                }
                else
                {
                    remainingTime = nightStartTime.TotalMinutes - (currentTime.TotalMinutes - midNight);
                }
                return (Period.Day, remainingTime);
            }

            remainingTime = dayStartTime.TotalMinutes - currentTime.TotalMinutes;
            return (Period.Night, remainingTime);
        }

        private bool IsFullScreenAppFounded(ScreenEntity screen)
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