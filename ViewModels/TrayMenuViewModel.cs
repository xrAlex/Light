using Light.Commands;
using Light.Infrastructure;
using Light.Models;
using Light.Services;
using Light.ViewModels.Base;
using System.Windows.Input;
using Hardcodet.Wpf.TaskbarNotification;
using Light.Properties;
using Application = System.Windows.Application;

namespace Light.ViewModels
{
    internal sealed class TrayMenuViewModel : ViewModelBase
    {
        #region Fields

        private readonly DialogService _dialogService;
        private readonly PeriodWatcherService _periodWatcherService;
        private bool _isAppPaused;
        private string _workTimeKeyText = "Приостановить";

        #endregion

        #region Constructors

        public string WorkTimeKeyText
        {
            get => _workTimeKeyText;
            private set => Set(ref _workTimeKeyText, value);
        }

        #endregion

        #region Commands
        public ICommand PauseCommand { get; }
        public ICommand ShutdownCommand { get; }

        private void OnPauseCommandExecute()
        {
            if (_isAppPaused)
            {
                _periodWatcherService.StartWatch();
                WorkTimeKeyText = "Приостановить";
            }
            else
            {
                _periodWatcherService.StopWatch();
                ScreenModel.SetDefaultColorTemperatureOnAllScreens();
                WorkTimeKeyText = "Продолжить";
            }
            _isAppPaused = !_isAppPaused;
            _dialogService.HideDialog<TrayMenuViewModel>();
        }

        private void OnShutdownCommandExecute()
        {
            ScreenModel.SetDefaultColorTemperatureOnAllScreens();
            Application.Current.Shutdown();
        }

        #endregion


        public TrayMenuViewModel()
        {
            PauseCommand = new LambdaCommand(p => OnPauseCommandExecute());
            ShutdownCommand = new LambdaCommand(p => OnShutdownCommandExecute());

            var serviceLocator = ServiceLocator.Source;
            _dialogService = serviceLocator.DialogService;
            _periodWatcherService = serviceLocator.PeriodWatcherService;
        }
#if DEBUG
        ~TrayMenuViewModel()
        {
            DebugConsole.Print("TrayMenuViewModel Disposed");
        }
#endif
    }
}
