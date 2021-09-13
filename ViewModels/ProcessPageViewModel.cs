using Light.Commands;
using Light.Models;
using Light.Models.Entities;
using Light.Services;
using Light.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Light.ViewModels
{
    internal sealed class ProcessPageViewModel : ViewModelBase
    {
        #region Fields

        private readonly ProcessModel _processModel;
        private readonly ServiceLocator _serviceLocator;
        private readonly SettingsService _settings;
        public ObservableCollection<ProcessEntity> Processes { get; set; }
        public ObservableCollection<ProcessEntity> IgnoredProceses { get; set; }

        #endregion

        #region Properties

        public bool CheckFullScreenApps
        {
            get => _settings.CheckFullScreenApps;
            set
            {
                _settings.CheckFullScreenApps = value;
                OnPropertyChanged("SelectedScreen");
            }
        }

        #endregion

        #region Commands

        public ICommand MoveToIgnoredProcesessCommand { get; }
        public ICommand MoveToProcesessCommand { get; }

        private void OnMoveToIgnoredProcesessCommandExecute() => _processModel.MoveToIngnoredProcesess();
        private void OnMoveToProcesessCommandExecute() => _processModel.MoveToProcesess();

        #endregion

        public ProcessPageViewModel()
        {
            MoveToIgnoredProcesessCommand = new LambdaCommand(p => OnMoveToIgnoredProcesessCommandExecute());
            MoveToProcesessCommand = new LambdaCommand(p => OnMoveToProcesessCommandExecute());

            _processModel = new();
            _serviceLocator = ServiceLocator.Source;
            _settings = _serviceLocator.Settings;
            Processes = _processModel.Processes;
            IgnoredProceses = _processModel.IgnoredProcesses;
        }
    }
}
