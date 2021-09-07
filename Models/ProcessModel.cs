using Light.Models.Entities;
using Light.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    IgnoredProcesses.Add(new ProcessEntity 
                    { 
                        Name = processEntity.Name,         
                        IsSelected = false,
                        OnFullScreen = false
                    });
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
                    Processes.Add(new ProcessEntity
                    {
                        Name = processEntity.Name,
                        IsSelected = false,
                        OnFullScreen = false
                    });
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
                        Processes.Add(new ProcessEntity
                        {
                            Name = Process.ProcessName,
                            IsSelected = false,
                            OnFullScreen = false
                        });
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
