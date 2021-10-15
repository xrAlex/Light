using Light.Infrastructure;
using Light.Templates.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Light.Services.Interfaces;

namespace Light.Models
{
    internal sealed class ApplicationModel
    {
        #region Fields

        private readonly ISettingsService _settingsService;
        private readonly ObservableCollection<ApplicationEntity> _applications;
        private readonly List<string> _ignoredApplications;

        #endregion

        #region Methods

        /// <summary>
        /// Relocates user selected applications names to whitelist
        /// </summary>
        public void RelocateToWhitelist()
        {
            _ignoredApplications.Clear();

            foreach (var applicationEntity in _applications)
            {
                if (applicationEntity.IsSelected)
                {
                    _ignoredApplications.Add($"{applicationEntity.Name}");
                }
            }
        }

        public void FillApplicationsCollection()
        {
            _applications.Clear();

            var windowsHandle = SystemWindow.GetAllWindows();

            foreach (var handle in windowsHandle)
            {
                var pId = SystemProcess.GetId(handle);
                using var process = SystemProcess.TryOpenProcess(pId);
                var processPath = process?.TryGetProcessPath();
                var processFileName = Path.GetFileNameWithoutExtension(processPath);

                if (!IsProcessValid(processPath, processFileName)) continue;

                _applications.Add(new ApplicationEntity
                {
                    ExecutableFilePath = processPath,
                    Name = processFileName,
                    IsSelected = _ignoredApplications.Any(x => x == processFileName),
                    OnFullScreen = IsFullScreenProcess(handle)
                });
            }
        }

        private bool IsProcessValid(string processPath, string processFileName)
        {
            return !string.IsNullOrWhiteSpace(processPath)
                   && !string.IsNullOrWhiteSpace(processFileName)
                   && _applications.All(y => y.ExecutableFilePath != processPath);
        }

        private bool IsFullScreenProcess(nint handle)
        {
            return _settingsService.Screens
                .Select(screen => SystemWindow.IsWindowOnFullScreen(screen, handle))
                .FirstOrDefault();
        }

        #endregion

        public ApplicationModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _ignoredApplications = settingsService.ApplicationsWhitelist;
            _applications = settingsService.Applications;
            FillApplicationsCollection();
        }

#if DEBUG
        ~ApplicationModel()
        {
            LoggingModule.Log.Verbose("[Model] ProcessModel Disposed");
        }
#endif
    }
}