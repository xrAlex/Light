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
        private double _topLocation;
        private double _leftLocation;

        #endregion

        #region Constructors

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
        public ICommand CloseTrayMenuCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand ShutdownCommand { get; }

        private void OnPauseCommandExecute()
        {
            _periodWatcherService.StopWatch();
            ScreenModel.SetDefaultColorTemperatureOnAllScreens();
            _dialogService.CloseDialog<TrayMenuViewModel>();
        }

        private void OnShutdownCommandExecute()
        {
            ScreenModel.SetDefaultColorTemperatureOnAllScreens();
            Application.Current.Shutdown();
        }

        private void OnCloseTrayMenuCommandExecute()
        {
            _dialogService.CloseDialog<TrayMenuViewModel>();
        }

        #endregion


        public TrayMenuViewModel()
        {
            PauseCommand = new LambdaCommand(p => OnPauseCommandExecute());
            ShutdownCommand = new LambdaCommand(p => OnShutdownCommandExecute());
            CloseTrayMenuCommand = new LambdaCommand(p => OnCloseTrayMenuCommandExecute());

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
