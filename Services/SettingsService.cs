using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Light.Infrastructure;
using Light.Templates.Entities;
using WindowsDisplayAPI.DisplayConfig;

namespace Light.Services
{
    /// <summary>
    /// Loads, saves and storage application settings
    /// </summary>
    internal sealed partial class SettingsService
    {
        public ObservableCollection<ApplicationEntity> Applications { get; }
        public ObservableCollection<ScreenEntity> Screens { get; }
        public List<string> IgnoredApplications { get; }

        private bool _legacyMode;
        public int SelectedScreen { get; set; }
        public int SelectedLang { get; set; }
        public bool CheckFullScreenApps { get; set; }
        public SettingsService()
        {
            IgnoredApplications = new List<string>();
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
            SelectedLang = INIManager.GetValue<int>("Main", "SelectedLang", "0");
            SelectedScreen = INIManager.GetValue<int>("Main", "SelectedScreen", "0");
            CheckFullScreenApps = INIManager.GetValue<bool>("Main", "CheckFullScreenApps", "false");

            LoadScreens();
            LoadProcesses();
        }

        public void Reset()
        {
            IgnoredApplications.Clear();
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
            catch
            {
                DebugConsole.Print("Error when loading screens");
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
                        DayColorTemperature = INIManager.GetValue<int>($"{index}", "DayColorTemperature", "6600"),
                        DayBrightness = INIManager.GetValue<float>($"{index}", "DayBrightness", "1"),
                        NightColorTemperature = INIManager.GetValue<int>($"{index}", "NightColorTemperature", "4200"),
                        NightBrightness = INIManager.GetValue<float>($"{index}", "NightBrightness", "1"),
                    },
                    Height = screen.Bounds.Height,
                    Width = screen.Bounds.Width,
                    StartTime = INIManager.GetValue<int>($"{index}", "StartTime", "1380"),
                    EndTime = INIManager.GetValue<int>($"{index}", "EndTime", "420"),
                    IsActive = INIManager.GetValue<bool>($"{index}", "Active", "true"),
                    Name = INIManager.GetValue<string>($"{index}", "Name", $"Monitor {index + 1}"),
                    SysName = INIManager.GetValue<string>($"{index}", "SysName", $"{screen.DeviceName}")
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
                        DayColorTemperature = INIManager.GetValue<int>($"{displayCode}", "DayColorTemperature", "6600"),
                        DayBrightness = INIManager.GetValue<float>($"{displayCode}", "DayBrightness", "1"),
                        NightColorTemperature = INIManager.GetValue<int>($"{displayCode}", "NightColorTemperature", "4200"),
                        NightBrightness = INIManager.GetValue<float>($"{displayCode}", "NightBrightness", "1"),
                    },

                    Height = displayHeight,
                    Width = displayWidth,
                    DisplayCode = displayCode,
                    StartTime = INIManager.GetValue<int>($"{displayCode}", "StartTime", "1380"),
                    EndTime = INIManager.GetValue<int>($"{displayCode}", "EndTime", "420"),
                    IsActive = INIManager.GetValue<bool>($"{displayCode}", "Active", "true"),
                    Name = INIManager.GetValue<string>($"{displayCode}", "Name", $"{displayFriendlyName}"),
                    SysName = INIManager.GetValue<string>($"{displayCode}", "SysName", $"{displayName}")
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
                IgnoredApplications.Add($"{processName}");
            }
        }

        private void SaveProcesses()
        {
            var processStr = IgnoredApplications.Aggregate("", (current, process) => current + $"{process};");
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