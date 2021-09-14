using Light.Models.Entities;
using Light.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Light.Models
{
    public class ProcessModel
    {
        private readonly ServiceLocator _serviceLocator;
        private readonly SettingsService _settingsService;

        public ObservableCollection<ProcessEntity> Processes { get; private set; }
        public ObservableCollection<ProcessEntity> IgnoredProcesses { get; private set; }

        public void MoveToIngnoredProcesess()
        {
            foreach (var processEntity in Processes.ToArray())
            {
                if (processEntity.IsSelected)
                {
					IgnoredProcesses.Add(new ProcessEntity(processEntity.Instance, processEntity.Name));
                    Processes.Remove(processEntity);
                }
            }
        }

        public void MoveToProcesess()
        {
            foreach (var processEntity in IgnoredProcesses.ToArray())
            {
                if (processEntity.IsSelected)
                {
					Processes.Add(new ProcessEntity(processEntity.Instance, processEntity.Name));
                    IgnoredProcesses.Remove(processEntity);
                }
            }
        }

        private void FillProcessCollection()
        {
            var processes = Process.GetProcesses();

            foreach (var Process in processes)
            {
                if (ProcessHasWindow(Process))
                {
                    if(!IgnoredProcesses.Any(p => p.Name == Process.ProcessName))
                    {
                        Processes.Add(new ProcessEntity(Process, Process.ProcessName));
                    }
                }
            }
        }

        private bool ProcessHasWindow(Process process) => process.MainWindowHandle != IntPtr.Zero;

        public ProcessModel()
        {
            _serviceLocator = ServiceLocator.Source;
            _settingsService = _serviceLocator.Settings;
            IgnoredProcesses = _settingsService.IgnoredProcesses;
            Processes = new();
            FillProcessCollection();
        }
    }
}
