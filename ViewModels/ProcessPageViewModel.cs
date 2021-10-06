using Light.Commands;
using Light.Models;
using Light.Services;
using Light.Templates.Entities;
using Light.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Light.ViewModels
{
    internal sealed class ProcessPageViewModel : ViewModelBase
    {
        #region Properties

        public bool CheckFullScreenApps
        {
            get => _settings.CheckFullScreenApps;
            set => _settings.CheckFullScreenApps = value;
        }

        #endregion

        #region Fields

        private readonly ApplicationModel _processModel;
        private readonly SettingsService _settings;
        public ObservableCollection<ApplicationEntity> Processes { get; }

        #endregion

        #region Commands

        public ICommand MoveToIgnoredProcessesCommand { get; }
        private void OnMoveToIgnoredProcessesCommandExecute() => _processModel.MoveToIgnoredProcesses();

        #endregion

        public ProcessPageViewModel()
        {
            MoveToIgnoredProcessesCommand = new LambdaCommand(p => OnMoveToIgnoredProcessesCommandExecute());

            _processModel = new ApplicationModel();
            _settings = ServiceLocator.Settings;
            Processes = _settings.Application;
        }

#if DEBUG
        ~ProcessPageViewModel()
        {
            DebugConsole.Print("ProcessPageViewModel Disposed");
        }
#endif
    }
}