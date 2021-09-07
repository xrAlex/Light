
using Light.Infrastructure;
using Light.Models;
using Light.Models.Entities;
using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Light.Services
{
    public sealed class SettingsService
    {
        private readonly INIManager _manager;
        public ObservableCollection<ScreenEntity> Screens { get; set; } = new();
        public ObservableCollection<ProcessEntity> IgnoredProcesses { get; set; } = new();

        public int SelectedScreen { get; set; }

        public SettingsService()
        {
            _manager = new();
        }
        public void Save()
        {
            _manager.WriteValue("Main", "SelectedScreen", SelectedScreen.ToString());

            SaveScreens();
            SaveProcesses();
        }

        public void Load()
        {
            SelectedScreen = _manager.GetIntValue("Main", "SelectedScreen", "0");

            LoadScreens();
            LoadProcesess();
        }

        public void Reset()
        {
            IgnoredProcesses.Clear();
            Screens.Clear();
            Load();
        }

        private void SaveScreens()
        {
            for (int i = 0; i < Screens.Count; i++)
            {
                var Monitor = Screens[i];
                _manager.WriteValue($"{i}", "UserGamma", Monitor.UserGamma.ToString());
                _manager.WriteValue($"{i}", "UserBlueReduce", Monitor.UserBlueReduce.ToString());
                _manager.WriteValue($"{i}", "StartTime", Monitor.StartTime.ToString());
                _manager.WriteValue($"{i}", "EndTime", Monitor.EndTime.ToString());
                _manager.WriteValue($"{i}", "Active", Monitor.IsActive.ToString());
                _manager.WriteValue($"{i}", "Name", Monitor.Name);
                _manager.WriteValue($"{i}", "Sys", Monitor.SysName);
            }
        }

        private void LoadScreens()
        {
            int index = 0;
            foreach (var screen in Screen.AllScreens)
            {
                Screens.Add(new ScreenEntity
                {
                    UserGamma = _manager.GetFloatValue($"{index}", "UserGamma", "100"),
                    UserBlueReduce = _manager.GetFloatValue($"{index}", "UserBlueReduce", "100"),
                    StartTime = _manager.GetIntValue($"{index}", "StartTime", "1380"),
                    EndTime = _manager.GetIntValue($"{index}", "EndTime", "420"),
                    IsActive = _manager.GetBoolValue($"{index}", "Active", "true"),
                    Name = _manager.GetStringValue($"{index}", "Name", $"#{ index + 1 }"),
                    SysName = _manager.GetStringValue($"{index}", "SysName", $"{ screen.DeviceName }"),
                });
                index++;
            }
        }

        private void SaveProcesses()
        {
            string processStr = "";
            for (int i = 0; i < IgnoredProcesses.Count; i++)
            {
                var process = IgnoredProcesses[i];
                processStr += $"{process.Name};";
            }
            _manager.WriteValue("Processes", "Ignored", processStr);
        }

        private void LoadProcesess()
        {
            string processStr = _manager.GetStringValue("Processes", "Ignored", "");

            var strTable = processStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string processName in strTable)
            {
                IgnoredProcesses.Add(new ProcessEntity
                {
                    Name = processName,
                    IsSelected = false,
                    OnFullScreen = false
                });
            }
        }
    }
}
