using Sparky.Commands;
using Sparky.Models;
using Sparky.Templates.Entities;
using Sparky.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Sparky.Services.Interfaces;

namespace Sparky.ViewModels
{
    internal sealed class ProcessPageViewModel : ViewModelBase
    {
        #region Properties

        public bool CheckFullScreenApps
        {
            get => _settingsService.CheckFullScreenApps;
            set => _settingsService.CheckFullScreenApps = value;
        }

        #endregion

        #region Fields

        private readonly ApplicationModel _applicationModel;
        private readonly ISettingsService _settingsService;
        public ObservableCollection<ApplicationEntity> Processes { get; }

        #endregion

        #region Commands
        public ICommand RefreshApplicationsList { get; }
        public ICommand MoveToIgnoredProcesses { get; }

        private void OnRefreshApplicationsListCommandExecute() => _applicationModel.RelocateToWhitelist();
        private void OnMoveToIgnoredProcessesCommandExecute() => _applicationModel.FillApplicationsCollection();

        #endregion

        public ProcessPageViewModel(ISettingsService settingsService)
        {
            RefreshApplicationsList = new LambdaCommand(_ => OnMoveToIgnoredProcessesCommandExecute());
            MoveToIgnoredProcesses = new LambdaCommand(_ => OnRefreshApplicationsListCommandExecute());

            _settingsService = settingsService;
            _applicationModel = new ApplicationModel(settingsService);
            Processes = _settingsService.Applications;
        }

        public ProcessPageViewModel() {}

#if DEBUG
        ~ProcessPageViewModel()
        {
            LoggingModule.Log.Verbose("[View Model] ProcessPageViewModel Disposed");
        }
#endif
    }
}