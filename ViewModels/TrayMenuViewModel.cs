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

        private readonly DialogService _dialogService;
        private readonly PeriodWatcherService _periodWatcherService;
        private bool _isAppPaused;
        private string _workTimeKeyText = "Приостановить";
        private double _topLocation;
        private double _leftLocation;

        #endregion

        #region Constructors

        public string WorkTimeKeyText
        {
            get => _workTimeKeyText;
            private set => Set(ref _workTimeKeyText, value);
        }
        public double TopLocation
        {
            get => _topLocation;
            private set => Set(ref _topLocation, value);
        }

        public double LeftLocation
        {
            get => _leftLocation;
            private set => Set(ref _leftLocation, value);
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
            var trayNotifier = new TrayNotifier();

            trayNotifier.OnLocationChanged += (_, args) =>
            {
                LeftLocation = args.Location.X;
                TopLocation = args.Location.Y;
            };
        }
#if DEBUG
        ~TrayMenuViewModel()
        {
            DebugConsole.Print("TrayMenuViewModel Disposed");
        }
#endif
    }
}
