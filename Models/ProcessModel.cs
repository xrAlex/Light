#region

using Light.Infrastructure;
using Light.Services;
using Light.Templates.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

#endregion

namespace Light.Models
{
    internal sealed class ProcessModel
    {
        #region Fields

        private ObservableCollection<ProcessEntity> Processes { get; }
        private List<string> IgnoredProcesses { get; }

        #endregion

        #region Methods

        public void MoveToIgnoredProcesses()
        {
            IgnoredProcesses.Clear();

            Processes
                .Where(x => x.IsSelected)
                .ToList()
                .ForEach(x =>
                {
                    IgnoredProcesses.Add($"{x.Name}");
                });
        }

        private void FillProcessCollection()
        {
            Processes.Clear();

            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                using (process)
                {
                    if (WindowHelper.IsWindowValid(process.MainWindowHandle))
                    {
                        Processes.Add(new ProcessEntity
                        {
                            Name = process.ProcessName,
                            IsSelected = IgnoredProcesses.Any(y => y == process.ProcessName),
                            OnFullScreen = IsFullScreenProcess(process.MainWindowHandle)
                        });
                    }
                }
            }
        }

        private bool IsFullScreenProcess(nint handle)
        {
            var serviceLocator = ServiceLocator.Source;
            var settings = serviceLocator.Settings;

            return settings.Screens.Select(screen => WindowHelper.IsWindowValid(handle) && WindowHelper.IsWindowOnFullScreen(screen, handle)).FirstOrDefault();
        }

        #endregion

        public ProcessModel()
        {
            var serviceLocator = ServiceLocator.Source;
            var settingsService = serviceLocator.Settings;
            IgnoredProcesses = settingsService.IgnoredProcesses;
            Processes = settingsService.Processes;
            FillProcessCollection();
        }

#if DEBUG
        ~ProcessModel()
        {
            Debug.Print("ProcessModel Disposed");
        }
#endif
    }
}