using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Sparky.Infrastructure;
using Sparky.Services.Interfaces;
using Sparky.Templates.Entities;
using Sparky.WinApi;
using WindowsDisplayAPI.DisplayConfig;

namespace Sparky.Services
{
    /// <summary>
    /// Loads, saves and storage application settings
    /// </summary>
    internal sealed partial class SettingsService : ISettingsService
    {
        /// <summary>
        /// Running user apps
        /// </summary>
        public ObservableCollection<ApplicationEntity> Applications { get; }

        /// <summary>
        /// All system display devices
        /// </summary>
        public ObservableCollection<ScreenEntity> Screens { get; }

        /// <summary>
        /// User-added names of ignored apps to whitelisted apps 
        /// </summary>
        public List<string> ApplicationsWhitelist { get; }

        /// <summary>
        /// If loading screens data from API fails, then allow load Screens on Legacy mode using WinForms.Screens
        /// </summary>
        private bool _legacyMode;

        /// <summary>
        /// Current selected screen in settings menu
        /// </summary>
        public int SelectedScreen { get; set; }

        /// <summary>
        /// Allows/Prevents smooth gamma change on period switch
        /// </summary>
        public bool SmoothGammaChange { get; set; }

        /// <summary>
        /// Current selected language index
        /// </summary>
        public int SelectedLang { get; set; }

        /// <summary>
        /// Allows/Prevents to Period Watcher Service checks fullscreen application on computer
        /// </summary>
        public bool CheckFullScreenApps { get; set; }
        public SettingsService()
        {
            ApplicationsWhitelist = new List<string>();
            Screens = new ObservableCollection<ScreenEntity>();
            Applications = new ObservableCollection<ApplicationEntity>();
            Load();
        }
    }

    internal sealed partial class SettingsService
    {
        public void Save()
        {
            INIManager.WriteValue("Main", "SelectedLang", SelectedLang);
            INIManager.WriteValue("Main", "SelectedScreen", SelectedScreen);
            INIManager.WriteValue("Main", "CheckFullScreenApps", CheckFullScreenApps);
            INIManager.WriteValue("Main", "SmoothGammaChange", SmoothGammaChange);

            SaveScreens();
            SaveProcesses();
        }

        public void Load()
        {
            SelectedLang = INIManager.GetValue("Main", "SelectedLang", 0);
            SelectedScreen = INIManager.GetValue("Main", "SelectedScreen", 0);
            CheckFullScreenApps = INIManager.GetValue("Main", "CheckFullScreenApps", false);
            SmoothGammaChange = INIManager.GetValue("Main", "SmoothGammaChange", false);

            LoadScreens();
            LoadProcesses();
            if (Screens.Count < 1)
            {
                LoggingModule.Log.Fatal("Can't find any screens {0}", Screens);
            }
        }

        public void Reset()
        {
            ApplicationsWhitelist.Clear();
            Screens.Clear();
            Load();
        }
    }

    internal sealed partial class SettingsService
    {
        private void LoadScreens()
        {
            try
            {
                LoadScreensFromAPI();
            }
            catch (Exception ex)
            {
                LoggingModule.Log.Warning(ex, "Error when loading screens");
                LoadScreensLegacy();
                _legacyMode = true;
            }
        }

        private void SaveScreens()
        {
            if (!_legacyMode)
            {
                SaveScreensParameters();
            }
            else
            {
                LegacySaveScreensParameters();
            }
        }

        private void LoadScreensLegacy()
        {
            var index = 0;
            foreach (var screen in Screen.AllScreens)
            {
                var sysName = INIManager.GetValue($"{index}", "SysName", $"{screen.DeviceName}");

                Screens.Add(new ScreenEntity
                {
                    Name = INIManager.GetValue($"{index}", "Name", $"Monitor {index + 1}"),
                    SysName = INIManager.GetValue($"{index}", "SysName", $"{screen.DeviceName}"),

                    DayColorConfiguration = new ColorConfiguration
                    (
                        colorTemperature: INIManager.GetValue($"{index}", "DayColorTemperature", 6600),
                        brightness: INIManager.GetValue($"{index}", "DayBrightness", 1.0)
                    ),

                    NightColorConfiguration = new ColorConfiguration
                    (
                        colorTemperature: INIManager.GetValue($"{index}", "NightColorTemperature", 5500),
                        brightness: INIManager.GetValue($"{index}", "NightBrightness", 0.8)
                    ),

                    NightStartTime = new StartTime
                    (
                        hour: INIManager.GetValue($"{index}", "NightStartHour", 23),
                        minute: INIManager.GetValue($"{index}", "NightStartMin", 0)
                    ),

                    DayStartTime = new StartTime
                    (
                        hour: INIManager.GetValue($"{index}", "DayStartHour", 7),
                        minute: INIManager.GetValue($"{index}", "DayStartMin", 0)
                    ),

                    DeviceContext = Native.CreateDC(sysName, null, null, 0),

                    Height = screen.Bounds.Height,
                    Width = screen.Bounds.Width,

                    IsActive = INIManager.GetValue($"{index}", "Active", true),
                });
                index++;
            }
        }

        private void LoadScreensFromAPI()
        {

            // TODO: Check for display
            foreach (var display in PathDisplayTarget.GetDisplayTargets())
            {
                var displayDevice = display.ToDisplayDevice();
                var displayPreferredSettings = displayDevice.GetPreferredSetting();
                var displayCode = display.EDIDProductCode;
                var displayFriendlyName = display.FriendlyName;
                var displayName = displayDevice.DisplayName;
                var displayHeight = displayPreferredSettings.Resolution.Height;
                var displayWidth = displayPreferredSettings.Resolution.Width;

                Screens.Add(new ScreenEntity
                {
                    Name = INIManager.GetValue($"{displayCode}", "Name", $"{displayFriendlyName}"),
                    SysName = INIManager.GetValue($"{displayCode}", "SysName", $"{displayName}"),

                    DayColorConfiguration = new ColorConfiguration
                    (
                        colorTemperature: INIManager.GetValue($"{displayCode}", "DayColorTemperature", 6600),
                        brightness: INIManager.GetValue($"{displayCode}", "DayBrightness", 1.0)
                    ),

                    NightColorConfiguration = new ColorConfiguration
                    (
                        colorTemperature: INIManager.GetValue($"{displayCode}", "NightColorTemperature", 5500),
                        brightness: INIManager.GetValue($"{displayCode}", "NightBrightness", 0.8)
                    ),

                    CurrentColorConfiguration = new ColorConfiguration
                    (
                        colorTemperature: INIManager.GetValue($"{displayCode}", "DayColorTemperature", 6600),
                        brightness: INIManager.GetValue($"{displayCode}", "DayBrightness", 1.0)
                    ),

                    NightStartTime = new StartTime
                    (
                        hour: INIManager.GetValue($"{displayCode}", "NightStartHour", 23),
                        minute: INIManager.GetValue($"{displayCode}", "NightStartMin", 0)
                    ),

                    DayStartTime = new StartTime
                    (
                        hour: INIManager.GetValue($"{displayCode}", "DayStartHour", 7),
                        minute: INIManager.GetValue($"{displayCode}", "DayStartMin", 0)
                    ),

                    DeviceContext = Native.CreateDC(displayName, null, null, 0),
                    IsActive = INIManager.GetValue($"{displayCode}", "Active", true),

                    Height = displayHeight,
                    Width = displayWidth,
                    DisplayCode = displayCode,
                });
            }
        }

        private void SaveScreensParameters()
        {
            foreach (var screen in Screens)
            {
                INIManager.WriteValue($"{screen.DisplayCode}", "DayColorTemperature", screen.DayColorConfiguration.ColorTemperature);
                INIManager.WriteValue($"{screen.DisplayCode}", "DayBrightness", screen.DayColorConfiguration.Brightness);
                INIManager.WriteValue($"{screen.DisplayCode}", "NightColorTemperature", screen.NightColorConfiguration.ColorTemperature);
                INIManager.WriteValue($"{screen.DisplayCode}", "NightBrightness", screen.NightColorConfiguration.Brightness);
                INIManager.WriteValue($"{screen.DisplayCode}", "NightStartHour", screen.NightStartTime.Hour);
                INIManager.WriteValue($"{screen.DisplayCode}", "NightStartMin", screen.NightStartTime.Minute);
                INIManager.WriteValue($"{screen.DisplayCode}", "DayStartHour", screen.DayStartTime.Hour);
                INIManager.WriteValue($"{screen.DisplayCode}", "DayStartMin", screen.DayStartTime.Minute);
                INIManager.WriteValue($"{screen.DisplayCode}", "Active", screen.IsActive);
                INIManager.WriteValue($"{screen.DisplayCode}", "Name", screen.Name);
                INIManager.WriteValue($"{screen.DisplayCode}", "SysName", screen.SysName);
            }
        }

        private void LegacySaveScreensParameters()
        {
            for (var i = 0; i < Screens.Count; i++)
            {
                var screen = Screens[i];
                INIManager.WriteValue($"{i}", "DayColorTemperature", screen.DayColorConfiguration.ColorTemperature);
                INIManager.WriteValue($"{i}", "DayBrightness", screen.DayColorConfiguration.Brightness);
                INIManager.WriteValue($"{i}", "NightColorTemperature", screen.NightColorConfiguration.ColorTemperature);
                INIManager.WriteValue($"{i}", "NightBrightness", screen.NightColorConfiguration.Brightness);
                INIManager.WriteValue($"{i}", "NightStartHour", screen.NightStartTime.Hour);
                INIManager.WriteValue($"{i}", "NightStartMin", screen.NightStartTime.Minute);
                INIManager.WriteValue($"{i}", "DayStartHour", screen.DayStartTime.Hour);
                INIManager.WriteValue($"{i}", "DayStartMin", screen.DayStartTime.Minute);
                INIManager.WriteValue($"{i}", "Active", screen.IsActive);
                INIManager.WriteValue($"{i}", "Name", screen.Name);
                INIManager.WriteValue($"{i}", "SysName", screen.SysName);
            }
        }
    }

    internal sealed partial class SettingsService
    {
        private void LoadProcesses()
        {
            var processStr = INIManager.GetValue<string>("Processes", "Ignored", "");

            var strTable = processStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var processName in strTable)
            {
                ApplicationsWhitelist.Add($"{processName}");
            }
        }

        private void SaveProcesses()
        {
            var processStr = ApplicationsWhitelist.Aggregate("", (current, process) => current + $"{process};");
            INIManager.WriteValue("Processes", "Ignored", processStr);
        }

#if DEBUG
        ~SettingsService()
        {
            LoggingModule.Log.Verbose("[Service] SettingsService Disposed");
        }
#endif
    }
}