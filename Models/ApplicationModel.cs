using Light.Infrastructure;
using Light.Services;
using Light.Templates.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Light.Models
{
    internal sealed class ApplicationModel
    {
        #region Fields

        private ObservableCollection<ApplicationEntity> Applications { get; }
        private List<string> IgnoredApplications { get; }

        #endregion

        #region Methods

        public void MoveToIgnoredProcesses()
        {
            IgnoredApplications.Clear();

            foreach (var applicationEntity in Applications)
            {
                if (applicationEntity.IsSelected)
                {
                    IgnoredApplications.Add($"{applicationEntity.Name}");
                }
            }
        }

        private void FillApplicationsCollection()
        {
            Applications.Clear();

            var windowsHandle = SystemWindow.GetAllWindows();

            foreach (var handle in windowsHandle)
            {
                var pId = SystemProcess.GetId(handle);
                using var process = SystemProcess.TryOpenProcess(pId);
                var processPath = process?.TryGetProcessPath();
                var processFileName = Path.GetFileNameWithoutExtension(processPath);

                if (!IsProcessValid(processPath, processFileName)) continue;

                Applications.Add(new ApplicationEntity
                {
                    ExecutableFilePath = processPath,
                    Name = processFileName,
                    IsSelected = IgnoredApplications.Any(x => x == processFileName),
                    OnFullScreen = IsFullScreenProcess(handle)
                });
            }
        }

        private bool IsProcessValid(string processPath, string processFileName)
        {
            return !string.IsNullOrWhiteSpace(processPath) && !string.IsNullOrWhiteSpace(processFileName) 
                                                           && Applications.All(y => y.ExecutableFilePath != processPath);
        }

        private bool IsFullScreenProcess(nint handle)
        {
            var serviceLocator = ServiceLocator.Source;
            var settings = serviceLocator.Settings;

            return settings.Screens.Select(screen => SystemWindow.IsWindowOnFullScreen(screen, handle)).FirstOrDefault();
        }

        #endregion

        public ApplicationModel()
        {
            var serviceLocator = ServiceLocator.Source;
            var settingsService = serviceLocator.Settings;
            IgnoredApplications = settingsService.IgnoredApplications;
            Applications = settingsService.Application;
            FillApplicationsCollection();
        }

#if DEBUG
        ~ApplicationModel()
        {
            Debug.Print("ProcessModel Disposed");
        }
#endif
    }
}