using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Light.Infrastructure;
using Light.Models.Entities;

namespace Light.Services
{
    public sealed class SettingsService
    {
        private readonly INIManager _manager;

        public ObservableCollection<ScreenEntity> Screens { get; set; }
        public ObservableCollection<ProcessEntity> IgnoredProcesses { get; set; }

        public int SelectedScreen { get; set; }
        public bool CheckFullScreenApps { get; set; }

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
            for (var i = 0; i < Screens.Count; i++)
            {
                var monitor = Screens[i];
                _manager.WriteValue($"{i}", "UserGamma", monitor.UserGamma.ToString());
                _manager.WriteValue($"{i}", "UserBlueReduce", monitor.UserBlueReduce.ToString());
                _manager.WriteValue($"{i}", "StartTime", monitor.StartTime.ToString());
                _manager.WriteValue($"{i}", "EndTime", monitor.EndTime.ToString());
                _manager.WriteValue($"{i}", "Active", monitor.IsActive.ToString());
                _manager.WriteValue($"{i}", "Name", monitor.Name);
                _manager.WriteValue($"{i}", "SysName", monitor.SysName);
            }
        }

        private void LoadScreens()
        {
            var index = 0;
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
                    Name = _manager.GetValue<string>($"{index}", "Name", $"#{index + 1}"),
                    SysName = _manager.GetValue<string>($"{index}", "SysName", $"{screen.DeviceName}")
                });
                index++;
            }
        }

        private void SaveProcesses()
        {
            var processStr = IgnoredProcesses.Aggregate("", (current, process) => current + $"{process.Name};");
            _manager.WriteValue("Processes", "Ignored", processStr);
        }

        private void LoadProcesess()
        {
            var processStr = _manager.GetValue<string>("Processes", "Ignored", "");

            var strTable = processStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var processName in strTable)
            {
                IgnoredProcesses.Add(new ProcessEntity(null, processName));
            }
        }

        public SettingsService()
        {
            IgnoredProcesses = new ObservableCollection<ProcessEntity>();
            Screens = new ObservableCollection<ScreenEntity>();
            _manager = new INIManager();
        }
    }
}