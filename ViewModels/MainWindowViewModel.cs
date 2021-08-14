using Light.Commands;
using Light.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Light.ViewModels
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        #region Values

        private string _currentTime = "12:00";

        #endregion

        #region Constructors

        public string CurrentTime
        {
            get => _currentTime;
            set => Set(ref _currentTime, value);
        }

        #endregion

        #region Commands

        public ICommand CloseAppCommand { get; }
        public ICommand AppToTrayCommand { get; }
        public ICommand OpenSettingsWindowCommand { get; }

        private void OnCloseAppCommandExecute() { /* in progress */ }

        private void OnAppToTrayCommandExecute() { /* in progress */ }

        private void OnOpenSettingsWindowExecute() { /* in progress */ }

        #endregion

        public MainWindowViewModel()
        {
            OpenSettingsWindowCommand = new LambdaCommand(p => OnOpenSettingsWindowExecute());
            CloseAppCommand = new LambdaCommand(p => OnCloseAppCommandExecute());
            AppToTrayCommand = new LambdaCommand(p => OnAppToTrayCommandExecute());
        }
    }
}
