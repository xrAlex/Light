using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Light.Infrastructure;
using Light.Templates.Entities;

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
            for (var i = 0; i < Screens.Count; i++)
            {
                var screen = Screens[i];
                INIManager.WriteValue($"{i}", "DayColorTemperature", screen.ColorConfiguration.DayColorTemperature.ToString());
                INIManager.WriteValue($"{i}", "DayBrightness", screen.ColorConfiguration.DayBrightness.ToString());
                INIManager.WriteValue($"{i}", "NightColorTemperature", screen.ColorConfiguration.NightColorTemperature.ToString());
                INIManager.WriteValue($"{i}", "NightBrightness", screen.ColorConfiguration.NightBrightness.ToString());
                INIManager.WriteValue($"{i}", "StartTime", screen.StartTime.ToString());
                INIManager.WriteValue($"{i}", "EndTime", screen.EndTime.ToString());
                INIManager.WriteValue($"{i}", "Active", screen.IsActive.ToString());
                INIManager.WriteValue($"{i}", "Name", screen.Name);
                INIManager.WriteValue($"{i}", "SysName", screen.SysName);
            }
        }

        private void LoadScreens()
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
                        NightColorTemperature = INIManager.GetValue<int>($"{index}", "NightColorTemperature", "4000"),
                        NightBrightness = INIManager.GetValue<float>($"{index}", "NightBrightness", "1"),
                    },
                    Bounds = new Dictionary<string, int>()
                    {
                        {"Height", screen.Bounds.Height},
                        {"Width", screen.Bounds.Width}
                    },
                    StartTime = INIManager.GetValue<int>($"{index}", "StartTime", "1380"),
                    EndTime = INIManager.GetValue<int>($"{index}", "EndTime", "420"),
                    IsActive = INIManager.GetValue<bool>($"{index}", "Active", "true"),
                    Name = INIManager.GetValue<string>($"{index}", "Name", $"#{index + 1}"),
                    SysName = INIManager.GetValue<string>($"{index}", "SysName", $"{screen.DeviceName}")
                });
                index++;
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