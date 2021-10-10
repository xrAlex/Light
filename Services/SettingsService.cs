using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Light.Infrastructure;
using Light.Templates.Entities;
using WindowsDisplayAPI.DisplayConfig;

namespace Light.Services
{
    /// <summary>
    /// Класс выполняет загрузку и сохранение настроек приложения
    /// </summary>
    public sealed partial class SettingsService
    {
        public ObservableCollection<ApplicationEntity> Applications { get; }
        public ObservableCollection<ScreenEntity> Screens { get; }
        public List<string> IgnoredApplications { get; }

        public int SelectedScreen { get; set; }
        public bool CheckFullScreenApps { get; set; }
        public bool StartMinimized { get; set; }

        public SettingsService()
        {
            IgnoredApplications = new List<string>();
            Screens = new ObservableCollection<ScreenEntity>();
            Applications = new ObservableCollection<ApplicationEntity>();
        }
    }

    public sealed partial class SettingsService
    {
        public void Save()
        {
            INIManager.WriteValue("Main", "SelectedScreen", SelectedScreen.ToString());
            INIManager.WriteValue("Main", "StartMinimized", StartMinimized.ToString());
            INIManager.WriteValue("Main", "CheckFullScreenApps", CheckFullScreenApps.ToString());

            SaveScreens();
            SaveProcesses();
        }

        public void Load()
        {
            SelectedScreen = INIManager.GetValue<int>("Main", "SelectedScreen", "0");
            StartMinimized = INIManager.GetValue<bool>("Main", "StartMinimized", "false");
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

        private void SaveScreens()
        {
            foreach (var screen in Screens)
            {
                INIManager.WriteValue($"{screen.DisplayCode}", "DayColorTemperature", screen.ColorConfiguration.DayColorTemperature.ToString());
                INIManager.WriteValue($"{screen.DisplayCode}", "DayBrightness", screen.ColorConfiguration.DayBrightness.ToString());
                INIManager.WriteValue($"{screen.DisplayCode}", "NightColorTemperature", screen.ColorConfiguration.NightColorTemperature.ToString());
                INIManager.WriteValue($"{screen.DisplayCode}", "NightBrightness", screen.ColorConfiguration.NightBrightness.ToString());
                INIManager.WriteValue($"{screen.DisplayCode}", "StartTime", screen.StartTime.ToString());
                INIManager.WriteValue($"{screen.DisplayCode}", "EndTime", screen.EndTime.ToString());
                INIManager.WriteValue($"{screen.DisplayCode}", "Active", screen.IsActive.ToString());
                INIManager.WriteValue($"{screen.DisplayCode}", "Name", screen.Name);
                INIManager.WriteValue($"{screen.DisplayCode}", "SysName", screen.SysName);
            }
        }

        private void LoadScreens()
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
                        NightColorTemperature = INIManager.GetValue<int>($"{displayCode}", "NightColorTemperature", "4000"),
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

        private void SaveProcesses()
        {
            var processStr = IgnoredApplications.Aggregate("", (current, process) => current + $"{process};");
            INIManager.WriteValue("Processes", "Ignored", processStr);
        }

        private void LoadProcesses()
        {
            var processStr = INIManager.GetValue<string>("Processes", "Ignored", "");

            var strTable = processStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var processName in strTable)
            {
                IgnoredApplications.Add($"{processName}");
            }
        }
#if DEBUG
        ~SettingsService()
        {
            DebugConsole.Print("[Service] SettingsService Disposed");
        }
#endif
    }
}