using System.Collections.ObjectModel;
using System.Windows.Input;
using Light.Commands;
using Light.Models;
using Light.Models.Entities;
using Light.Services;
using Light.ViewModels.Base;

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
        public ObservableCollection<ProcessEntity> IgnoredProceses { get; }

        #endregion

        #region Commands

        public ICommand MoveToIgnoredProcessesCommand { get; }
        public ICommand MoveToProcessesCommand { get; }

        private void OnMoveToIgnoredProcessesCommandExecute() => _processModel.MoveToIgnoredProcesses();
        private void OnMoveToProcessesCommandExecute() => _processModel.MoveToProcesses();

        #endregion

        public ProcessPageViewModel()
        {
            MoveToIgnoredProcessesCommand = new LambdaCommand(p => OnMoveToIgnoredProcessesCommandExecute());
            MoveToProcessesCommand = new LambdaCommand(p => OnMoveToProcessesCommandExecute());

            _processModel = new ProcessModel();
            var serviceLocator = ServiceLocator.Source;
            _settings = serviceLocator.Settings;
            Processes = _processModel.Processes;
            IgnoredProceses = _processModel.IgnoredProcesses;
        }
    }
}