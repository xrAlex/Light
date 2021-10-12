using Light.Commands;
using Light.Models;
using Light.Services;
using Light.Templates.Entities;
using Light.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Light.Services.Interfaces;

namespace Light.ViewModels
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

        private readonly ApplicationModel _processModel;
        private readonly ISettingsService _settingsService;
        public ObservableCollection<ApplicationEntity> Processes { get; }

        #endregion

        #region Commands

        public ICommand MoveToIgnoredProcessesCommand { get; }
        private void OnMoveToIgnoredProcessesCommandExecute() => _processModel.MoveToIgnoredProcesses();

        #endregion

        public ProcessPageViewModel(ISettingsService settingsService)
        {
            MoveToIgnoredProcessesCommand = new LambdaCommand(p => OnMoveToIgnoredProcessesCommandExecute());
            _settingsService = settingsService;
            _processModel = new ApplicationModel(settingsService);
            Processes = _settingsService.Applications;
        }

#if DEBUG
        ~ProcessPageViewModel()
        {
            DebugConsole.Print("[View Model] ProcessPageViewModel Disposed");
        }
#endif
    }
}