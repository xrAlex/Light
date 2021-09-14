using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Light.Models.Entities;
using Light.Services;

namespace Light.Models
{
    public class ProcessModel
    {
        public ObservableCollection<ProcessEntity> Processes { get; }
        public ObservableCollection<ProcessEntity> IgnoredProcesses { get; }

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

            foreach (var process in processes)
            {
                if (ProcessHasWindow(process))
                {
                    if (IgnoredProcesses.All(p => p.Name != process.ProcessName))
                    {
                        Processes.Add(new ProcessEntity(process, process.ProcessName));
                    }
                }
            }

        }

        private bool ProcessHasWindow(Process process)
        {
            return process.MainWindowHandle != IntPtr.Zero;
        }

        public ProcessModel()
        {
            var serviceLocator = ServiceLocator.Source;
            var settingsService = serviceLocator.Settings;
            IgnoredProcesses = settingsService.IgnoredProcesses;
            Processes = new ObservableCollection<ProcessEntity>();
            FillProcessCollection();
        }

    }
}