using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
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
        private readonly ISettingsService _settingsService;
        private CancellationTokenSource _cts;
        private enum Period
        {
            Day,
            Night
        }

        public PeriodWatcherService(ISettingsService settingsService)
        {
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

                    var (currentPeriod, remainingTime) = GetCurrentPeriod(screen);

                    if (currentPeriod == Period.Night)
                    {
                        if (_settingsService.CheckFullScreenApps)
                        {
                            if (IsFullScreenAppFounded(screen))
                            {
                                SetColorConfiguration(screen, screen.DayColorConfiguration);
                                screen.IsDayTimePeriod = true;
                            }
                            else
                            {
                                SetColorConfiguration(screen, CalculateCurrentConfiguration(currentPeriod, screen, remainingTime));
                                screen.IsDayTimePeriod = false;
                            }
                        }
                        else
                        {
                            SetColorConfiguration(screen, CalculateCurrentConfiguration(currentPeriod, screen, remainingTime));
                            screen.IsDayTimePeriod = false;
                        }
                    }
                    else
                    {
                        SetColorConfiguration(screen, CalculateCurrentConfiguration(currentPeriod, screen, remainingTime));
                        screen.IsDayTimePeriod = true;
                    }
                    await Task.Delay(100, token);
                }
            }
        }

        // DCв скрине держать, диспозить при разрешнии класса
        private void SetColorConfiguration(ScreenEntity screen, ColorConfiguration newColorConfiguration)
        {
            var configuration = SmoothColorConfiguration(screen.CurrentColorConfiguration, newColorConfiguration);
            GammaRegulatorService.ApplyColorConfiguration(configuration, screen.SysName);
            screen.CurrentColorConfiguration = configuration;
        }

        private ColorConfiguration CalculateCurrentConfiguration(Period currentPeriod, ScreenEntity screen, double remainingTime)
        {
            var screenDayConfig = screen.DayColorConfiguration;
            var screenNightConfig = screen.NightColorConfiguration;

            if (!_settingsService.SmoothGammaChange || remainingTime is not (> 0 and <= 10))
            {
                return currentPeriod == Period.Day
                    ? new ColorConfiguration(screenDayConfig.ColorTemperature, screenDayConfig.Brightness)
                    : new ColorConfiguration(screenNightConfig.ColorTemperature, screenNightConfig.Brightness);
            }

            return currentPeriod == Period.Day
                ? GetTransientColorConfiguration(screenNightConfig, screenDayConfig, remainingTime)
                : GetTransientColorConfiguration(screenDayConfig, screenNightConfig, remainingTime);
        }

        private ColorConfiguration SmoothColorConfiguration(ColorConfiguration currentConfiguration, ColorConfiguration targetConfiguration)
        {
            var temperature = currentConfiguration.ColorTemperature;
            var brightness = currentConfiguration.Brightness;
            var targetTemperature = targetConfiguration.ColorTemperature;
            var targetBrightness = targetConfiguration.Brightness;

            if (temperature.IsCloseTo(targetTemperature) && brightness.IsCloseTo(targetBrightness))
            {
                return targetConfiguration;
            }

            const double temperatureStepSize = 60;
            const double brightnessStepSize = 0.016;

            var temperatureAbsDelta = Math.Abs(targetTemperature - temperature);
            var brightnessAbsDelta = Math.Abs(targetBrightness - brightness);

            var temperatureSteps = temperatureAbsDelta / temperatureStepSize;
            var brightnessSteps = brightnessAbsDelta / brightnessStepSize;

            var temperatureAdaptedStep = temperatureSteps >= brightnessSteps
                ? Math.Abs(temperatureStepSize)
                : Math.Abs(temperatureAbsDelta / brightnessSteps);

            var brightnessAdaptedStep = brightnessSteps >= temperatureSteps
                ? Math.Abs(brightnessStepSize)
                : Math.Abs(brightnessAbsDelta / temperatureSteps);

            temperature = targetTemperature >= temperature
                ? (temperature + temperatureAdaptedStep).ClampMax(targetTemperature)
                : (temperature - temperatureAdaptedStep).ClampMin(targetTemperature);

            brightness = targetBrightness >= brightness
                ? (brightness + brightnessAdaptedStep).ClampMax(targetBrightness)
                : (brightness - brightnessAdaptedStep).ClampMin(targetBrightness);

            return new ColorConfiguration(temperature, brightness);
        }

        private ColorConfiguration GetTransientColorConfiguration(ColorConfiguration targetValues, ColorConfiguration startValues, double remainingTime)
        {
            var multiplier = remainingTime * 0.1;

            if (startValues.ColorTemperature.IsCloseTo(targetValues.ColorTemperature) && startValues.Brightness.IsCloseTo(targetValues.Brightness))
            {
                return targetValues;
            }

            return new ColorConfiguration
            (
                startValues.ColorTemperature.Lerp(targetValues.ColorTemperature, multiplier),
                startValues.Brightness.Lerp(targetValues.Brightness, multiplier)
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
            var fullScreenWindowHandle = SystemWindow.GetFullScreenForegroundWindow(screen);
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