﻿using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Light.Infrastructure;
using Light.Services.Interfaces;
using Light.Templates.Entities;
using Light.WinApi;
using Microsoft.Extensions.DependencyInjection;

namespace Light.Models
{
    internal sealed class ScreenModel
    {
        #region Fields

        private readonly ObservableCollection<ScreenEntity> _screens;
        private readonly ISettingsService _settingsService; 

        #endregion

        #region Values

        private const int Hour = 60;

        #endregion

        #region Methods

        public void SetWorkTimeStart(int hour, int min, int screenIndex) => GetScreen(screenIndex).StartTime = hour * Hour + min;
        public void SetWorkTimeEnd(int hour, int min, int screenIndex) => GetScreen(screenIndex).EndTime = hour * Hour + min;
        public int GetStartHour(int screenIndex) => GetScreen(screenIndex).StartTime / Hour;
        public int GetEndHour(int screenIndex) => GetScreen(screenIndex).EndTime / Hour;
        public int GetStartMin(int screenIndex) => GetScreen(screenIndex).StartTime % Hour;
        public int GetEndMin(int screenIndex) => GetScreen(screenIndex).EndTime % Hour;
        public void ChangeScreenActivity(int screenIndex) => _screens[screenIndex].IsActive = !_screens[screenIndex].IsActive;
        public ScreenEntity GetScreen(int screenIndex) => _screens[screenIndex];

        public void SetDayPeriod(ScreenEntity screen)
        {
            screen.IsDayTimePeriod = true;
            ApplyColorConfiguration(screen);
        }

        public void SetNightPeriod(ScreenEntity screen)
        {
            screen.IsDayTimePeriod = false;
            ApplyColorConfiguration(screen);
        }

        /// <summary>
        /// Checks foreground Window bounds
        /// </summary>
        /// <returns> <see cref="bool"/> true, if the window is maximized to full screen </returns>
        public bool IsFullScreenWindowFounded(ScreenEntity screen)
        {
            var handle = Native.GetForegroundWindow();
            if (!SystemWindow.IsWindowValid(handle) || !SystemWindow.IsWindowOnFullScreen(screen, handle)) return false;

            var pId = SystemProcess.GetId(handle);
            using var process = SystemProcess.TryOpenProcess(pId);
            var processPath = process?.TryGetProcessPath();
            var processFileName = Path.GetFileNameWithoutExtension(processPath);

            return _settingsService.ApplicationsWhitelist.Count == 0 || _settingsService.ApplicationsWhitelist.All(p => p != processFileName);
        }

        public void ApplyColorConfiguration(ScreenEntity screen)
        {
            var configuration = screen.ColorConfiguration;
            double brightness;
            int colorTemperature;
            if (screen.IsDayTimePeriod)
            {
                colorTemperature = configuration.DayColorTemperature;
                brightness = configuration.DayBrightness;
            }
            else
            {
                colorTemperature = configuration.NightColorTemperature;
                brightness = configuration.NightBrightness;
            }
            GammaRegulator.ApplyColorConfiguration(colorTemperature, brightness, screen.SysName);
            screen.ColorConfiguration.CurrentColorTemperature = colorTemperature;
            screen.ColorConfiguration.CurrentBrightness = brightness;
        }

        public void ApplyColorConfiguration(ScreenEntity screen, int colorTemperature, double brightness)
        {
            var configuration = screen.ColorConfiguration;

            colorTemperature = colorTemperature == -1 ? configuration.CurrentColorTemperature : colorTemperature;
            brightness = brightness == -1 ? configuration.CurrentBrightness : brightness;

            GammaRegulator.ApplyColorConfiguration(colorTemperature, brightness, screen.SysName);
            screen.ColorConfiguration.CurrentColorTemperature = colorTemperature;
            screen.ColorConfiguration.CurrentBrightness = brightness;
        }

        public static void SetDefaultColorTemperatureOnAllScreens()
        {
            var screens = App.ServicesHost.Services.GetRequiredService<ISettingsService>().Screens;
#if DEBUG
            if (screens.Count < 1)
            {
                Logging.Write("Cant founded Screens");
            }
#endif
            foreach (var screen in screens)
            {
                GammaRegulator.ApplyColorConfiguration(6600, 1f, screen.SysName);
            }
        }

#endregion

        public ScreenModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _screens = _settingsService.Screens;
        }

#if DEBUG
        ~ScreenModel()
        {
            DebugConsole.Print("[Model] ScreenModel Disposed");
        }
#endif
    }
}