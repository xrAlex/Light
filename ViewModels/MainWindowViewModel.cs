using Light.Commands;
using Light.Services;
using Light.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Light.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModelBase
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

        private void OnCloseAppCommandExecute() 
        {
            Application.Current.Shutdown();
        }

        private void OnAppToTrayCommandExecute() { /* in progress */ }

        private void OnOpenSettingsWindowCommandExecute() 
        {
            var windowService = new WindowService();
            var viewModel = new SettingsWindowViewModel(windowService);
            windowService.CreateWindow(viewModel, 350, 450);
            windowService.ShowWindow();
        }

        #endregion

        public MainWindowViewModel()
        {
            OpenSettingsWindowCommand = new LambdaCommand(p => OnOpenSettingsWindowCommandExecute());
            CloseAppCommand = new LambdaCommand(p => OnCloseAppCommandExecute());
            AppToTrayCommand = new LambdaCommand(p => OnAppToTrayCommandExecute());
        }
    }
}
