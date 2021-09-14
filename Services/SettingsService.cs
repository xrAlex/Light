
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
        public ObservableCollection<ScreenEntity> Screens { get; set; }
        public ObservableCollection<ProcessEntity> IgnoredProcesses { get; set; }

        public int SelectedScreen { get; set; }
        public bool CheckFullScreenApps { get; set; }

        public SettingsService()
        {
            IgnoredProcesses = new();
            Screens = new();
            _manager = new();
        }
        public void Save()
        {
            _manager.WriteValue("Main", "SelectedScreen", SelectedScreen.ToString());
            _manager.WriteValue("Main", "CheckFullScreenApps", CheckFullScreenApps.ToString());

            SaveScreens();
            SaveProcesses();
        }

        public void Load()
        {
            SelectedScreen = _manager.GetValue<int>("Main", "SelectedScreen", "0");
            CheckFullScreenApps = _manager.GetValue<bool>("Main", "CheckFullScreenApps", "false");

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
                _manager.WriteValue($"{i}", "SysName", Monitor.SysName);
            }
        }

        private void LoadScreens()
        {
            int index = 0;
            foreach (var screen in Screen.AllScreens)
            {
                Screens.Add(new ScreenEntity
                {
                    Instance = screen,
                    UserGamma = _manager.GetValue<float>($"{index}", "UserGamma", "100"),
                    UserBlueReduce = _manager.GetValue<float>($"{index}", "UserBlueReduce", "100"),
                    StartTime = _manager.GetValue<int>($"{index}", "StartTime", "1380"),
                    EndTime = _manager.GetValue<int>($"{index}", "EndTime", "420"),
                    IsActive = _manager.GetValue<bool>($"{index}", "Active", "true"),
                    Name = _manager.GetValue<string>($"{index}", "Name", $"#{ index + 1 }"),
                    SysName = _manager.GetValue<string>($"{index}", "SysName", $"{ screen.DeviceName }"),
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
            string processStr = _manager.GetValue<string>("Processes", "Ignored", "");

            var strTable = processStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string processName in strTable)
            {
                IgnoredProcesses.Add(new ProcessEntity(null, processName));
            }
        }
    }
}
