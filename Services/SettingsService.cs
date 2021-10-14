using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Light.Infrastructure;
using Light.Services.Interfaces;
using Light.Templates.Entities;
using WindowsDisplayAPI.DisplayConfig;

namespace Light.Services
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
        /// If loading data from API fails then allow load Screens on Legacy mode using WinForms.Screens
        /// </summary>
        private bool _legacyMode;

        /// <summary>
        /// Current selected screen in settings menu
        /// </summary>
        public int SelectedScreen { get; set; }

        /// <summary>
        /// Current selected language
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
        }
    }

    internal sealed partial class SettingsService
    {
        public void Save()
        {
            INIManager.WriteValue("Main", "SelectedLang", SelectedLang);
            INIManager.WriteValue("Main", "SelectedScreen", SelectedScreen);
            INIManager.WriteValue("Main", "CheckFullScreenApps", CheckFullScreenApps);

            SaveScreens();
            SaveProcesses();
        }

        public void Load()
        {
            SelectedLang = INIManager.GetValue("Main", "SelectedLang", 0);
            SelectedScreen = INIManager.GetValue("Main", "SelectedScreen", 0);
            CheckFullScreenApps = INIManager.GetValue("Main", "CheckFullScreenApps", false);

            LoadScreens();
            LoadProcesses();
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
            catch(Exception ex)
            {
                Logging.Write("Error when loading screens", ex);
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
                Screens.Add(new ScreenEntity
                {
                    ColorConfiguration =
                    {
                        DayColorTemperature = INIManager.GetValue($"{index}", "DayColorTemperature", 6600),
                        DayBrightness = INIManager.GetValue($"{index}", "DayBrightness", 1.0),
                        NightColorTemperature = INIManager.GetValue($"{index}", "NightColorTemperature", 5500),
                        NightBrightness = INIManager.GetValue($"{index}", "NightBrightness", 0.8),
                    },
                    Height = screen.Bounds.Height,
                    Width = screen.Bounds.Width,
                    StartTime = INIManager.GetValue($"{index}", "StartTime", 1380),
                    EndTime = INIManager.GetValue($"{index}", "EndTime", 420),
                    IsActive = INIManager.GetValue($"{index}", "Active", true),
                    Name = INIManager.GetValue($"{index}", "Name", $"Monitor {index + 1}"),
                    SysName = INIManager.GetValue($"{index}", "SysName", $"{screen.DeviceName}")
                });
                index++;
            }
        }

        private void LoadScreensFromAPI()
        {
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
                    ColorConfiguration =
                    {
                        DayColorTemperature = INIManager.GetValue($"{displayCode}", "DayColorTemperature", 6600),
                        DayBrightness = INIManager.GetValue($"{displayCode}", "DayBrightness", 1.0),
                        NightColorTemperature = INIManager.GetValue($"{displayCode}", "NightColorTemperature", 5500),
                        NightBrightness = INIManager.GetValue($"{displayCode}", "NightBrightness", 0.8),
                    },

                    Height = displayHeight,
                    Width = displayWidth,
                    DisplayCode = displayCode,
                    StartTime = INIManager.GetValue($"{displayCode}", "StartTime", 1380),
                    EndTime = INIManager.GetValue($"{displayCode}", "EndTime", 420),
                    IsActive = INIManager.GetValue($"{displayCode}", "Active", true),
                    Name = INIManager.GetValue($"{displayCode}", "Name", $"{displayFriendlyName}"),
                    SysName = INIManager.GetValue($"{displayCode}", "SysName", $"{displayName}")
                });
            }
        }

        private void SaveScreensParameters()
        {
            foreach (var screen in Screens)
            {
                INIManager.WriteValue($"{screen.DisplayCode}", "DayColorTemperature", screen.ColorConfiguration.DayColorTemperature);
                INIManager.WriteValue($"{screen.DisplayCode}", "DayBrightness", screen.ColorConfiguration.DayBrightness);
                INIManager.WriteValue($"{screen.DisplayCode}", "NightColorTemperature", screen.ColorConfiguration.NightColorTemperature);
                INIManager.WriteValue($"{screen.DisplayCode}", "NightBrightness", screen.ColorConfiguration.NightBrightness);
                INIManager.WriteValue($"{screen.DisplayCode}", "StartTime", screen.StartTime);
                INIManager.WriteValue($"{screen.DisplayCode}", "EndTime", screen.EndTime);
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
                INIManager.WriteValue($"{i}", "DayColorTemperature", screen.ColorConfiguration.DayColorTemperature);
                INIManager.WriteValue($"{i}", "DayBrightness", screen.ColorConfiguration.DayBrightness);
                INIManager.WriteValue($"{i}", "NightColorTemperature", screen.ColorConfiguration.NightColorTemperature);
                INIManager.WriteValue($"{i}", "NightBrightness", screen.ColorConfiguration.NightBrightness);
                INIManager.WriteValue($"{i}", "StartTime", screen.StartTime);
                INIManager.WriteValue($"{i}", "EndTime", screen.EndTime);
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
            DebugConsole.Print("[Service] SettingsService Disposed");
        }
#endif
    }
}