using Light.Commands;
using Light.Infrastructure;
using Light.Models;
using Light.Services;
using Light.ViewModels.Base;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace Light.ViewModels
{
    internal sealed class TrayMenuViewModel : ViewModelBase
    {
        #region Fields

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
        public int TopLocation { get; set; }

        public int LeftLocation { get; set; }

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

            _periodWatcherService = ServiceLocator.PeriodWatcherService;
            var tray = new TrayMenuPosition();
            var pos = tray.GetTrayMenuPos();

            LeftLocation = pos.X;
            TopLocation = pos.Y;
        }
#if DEBUG
        ~TrayMenuViewModel()
        {
            DebugConsole.Print("[View Model] TrayMenuViewModel Disposed");
        }
#endif
    }
}
