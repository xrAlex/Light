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
        public ObservableCollection<ProcessEntity> Processes { get; set; }
        public ObservableCollection<ProcessEntity> IgnoredProceses { get; set; }

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
            Processes = _processModel.Processes;
            IgnoredProceses = _processModel.IgnoredProcesses;
        }
    }
}
