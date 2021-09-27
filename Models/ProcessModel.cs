#region

using Light.Infrastructure;
using Light.Services;
using Light.Templates.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Light.WinApi;

#endregion

namespace Light.Models
{
    internal sealed class ProcessModel
    {
        #region Fields

        private ObservableCollection<ApplicationEntity> Processes { get; }
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

            var windowsHandle = SystemWindow.GetAllWindows();

            foreach (var handle in windowsHandle)
            {
                var pId = SystemProcess.GetId(handle);
                using var process = SystemProcess.TryOpenProcess(pId);
                var processPath = process?.TryGetProcessPath();
                var processFileName = Path.GetFileNameWithoutExtension(processPath);

                if (!IsProcessValid(processPath, processFileName)) continue;

                Processes.Add(new ApplicationEntity
                {
                    ExecutableFilePath = processPath,
                    Name = processFileName,
                    IsSelected = IgnoredProcesses.Any(y => y == processFileName),
                    OnFullScreen = IsFullScreenProcess(handle)
                });
            }
        }

        private bool IsProcessValid(string processPath, string processFileName)
        {
            return !string.IsNullOrWhiteSpace(processPath) && !string.IsNullOrWhiteSpace(processFileName) 
                                                           && Processes.All(y => y.ExecutableFilePath != processPath);
        }

        private bool IsFullScreenProcess(nint handle)
        {
            var serviceLocator = ServiceLocator.Source;
            var settings = serviceLocator.Settings;

            return settings.Screens.Select(screen => SystemWindow.IsWindowOnFullScreen(screen, handle)).FirstOrDefault();
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