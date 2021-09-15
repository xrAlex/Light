﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Light.Infrastructure;
using Light.Models.Entities;

namespace Light.Services
{
    public sealed class SettingsService
    {
        public ObservableCollection<ScreenEntity> Screens { get; }
        public ObservableCollection<ProcessEntity> IgnoredProcesses { get; }

        public int SelectedScreen { get; set; }
        public bool CheckFullScreenApps { get; set; }

        public void Save()
        {
            INIManager.WriteValue("Main", "SelectedScreen", SelectedScreen.ToString());
            INIManager.WriteValue("Main", "CheckFullScreenApps", CheckFullScreenApps.ToString());

            SaveScreens();
            SaveProcesses();
        }

        public void Load()
        {
            SelectedScreen = INIManager.GetValue<int>("Main", "SelectedScreen", "0");
            CheckFullScreenApps = INIManager.GetValue<bool>("Main", "CheckFullScreenApps", "false");

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
                INIManager.WriteValue($"{i}", "UserGamma", monitor.UserGamma.ToString());
                INIManager.WriteValue($"{i}", "UserBlueReduce", monitor.UserBlueReduce.ToString());
                INIManager.WriteValue($"{i}", "StartTime", monitor.StartTime.ToString());
                INIManager.WriteValue($"{i}", "EndTime", monitor.EndTime.ToString());
                INIManager.WriteValue($"{i}", "Active", monitor.IsActive.ToString());
                INIManager.WriteValue($"{i}", "Name", monitor.Name);
                INIManager.WriteValue($"{i}", "SysName", monitor.SysName);
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
                    UserGamma = INIManager.GetValue<float>($"{index}", "UserGamma", "100"),
                    UserBlueReduce = INIManager.GetValue<float>($"{index}", "UserBlueReduce", "100"),
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
            var processStr = IgnoredProcesses.Aggregate("", (current, process) => current + $"{process.Name};");
            INIManager.WriteValue("Processes", "Ignored", processStr);
        }

        private void LoadProcesess()
        {
            var processStr = INIManager.GetValue<string>("Processes", "Ignored", "");

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
        }
    }
}