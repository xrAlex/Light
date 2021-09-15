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
        #region Fields
        public ObservableCollection<ProcessEntity> Processes { get; }
        public ObservableCollection<ProcessEntity> IgnoredProcesses { get; }

        #endregion

        #region Methods

        public void MoveToIngnoredProcesess()
        {
            foreach (var processEntity in Processes.ToArray())
            {
                if (!processEntity.IsSelected) continue;

                IgnoredProcesses.Add(new ProcessEntity(processEntity.Instance, processEntity.Name));
                Processes.Remove(processEntity);
            }
        }

        public void MoveToProcesess()
        {
            foreach (var processEntity in IgnoredProcesses.ToArray())
            {
                if (!processEntity.IsSelected) continue;

                Processes.Add(new ProcessEntity(processEntity.Instance, processEntity.Name));
                IgnoredProcesses.Remove(processEntity);
            }
        }

        private void FillProcessCollection()
        {
            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                nint handler = process.MainWindowHandle;
                if (handler == 0 || !Native.User32.IsWindowVisible(handler)) continue;

                if (IgnoredProcesses.All(p => p.Name != process.ProcessName))
                {
                    Processes.Add(new ProcessEntity(process, process.ProcessName));
                }
            }

        }

        #endregion

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