#region

using System.Collections.ObjectModel;
using System.Windows.Input;
using Light.Commands;
using Light.Models;
using Light.Services;
using Light.Templates.Entities;
using Light.ViewModels.Base;

#endregion

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

        private readonly ProcessModel _processModel;
        private readonly SettingsService _settings;
        public ObservableCollection<ProcessEntity> Processes { get; }

        #endregion

        #region Commands

        public ICommand MoveToIgnoredProcessesCommand { get; }
        private void OnMoveToIgnoredProcessesCommandExecute() => _processModel.MoveToIgnoredProcesses();

        #endregion

        public ProcessPageViewModel()
        {
            MoveToIgnoredProcessesCommand = new LambdaCommand(p => OnMoveToIgnoredProcessesCommandExecute());

            _processModel = new ProcessModel();
            var serviceLocator = ServiceLocator.Source;
            _settings = serviceLocator.Settings;
            Processes = _settings.Processes;
        }

#if DEBUG
        ~ProcessPageViewModel()
        {
            DebugConsole.Print("ProcessPageViewModel Disposed");
        }
#endif
    }
}