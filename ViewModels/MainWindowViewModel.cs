using Light.Commands;
using Light.Models;
using Light.Models.Entities;
using Light.Services;
using Light.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Light.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        public ObservableCollection<ScreenEntity> Screens { get; }

        private readonly WindowService _windowService;
        private readonly ServiceLocator _serviceLocator;
        private readonly CurrentTimeService _currentTimeService;
        private readonly GammaWatcherService _gammaWatcher;
        private readonly ScreenModel _screenModel;

        #endregion

        #region Values

        private string _currentTime = "12:00";

        #endregion

        #region Properties

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
            _screenModel.SetDefaultGammaOnAllScreens();
            Application.Current.Shutdown();
        }

        private void OnAppToTrayCommandExecute() { /* in progress */ }

        private void OnOpenSettingsWindowCommandExecute()
        {
            var settingsVM = new SettingsWindowViewModel();
            _windowService.CreateWindow(settingsVM, 350, 450);
            _windowService.ShowWindow();
        }

        #endregion

        public MainWindowViewModel()
        {
            OpenSettingsWindowCommand = new LambdaCommand(p => OnOpenSettingsWindowCommandExecute());
            CloseAppCommand = new LambdaCommand(p => OnCloseAppCommandExecute());
            AppToTrayCommand = new LambdaCommand(p => OnAppToTrayCommandExecute());

            _screenModel = new();
            _serviceLocator = ServiceLocator.Source;
            _windowService = _serviceLocator.WindowService;
            _currentTimeService = _serviceLocator.CurrentTimeService;
            _gammaWatcher = _serviceLocator.GammaWatcher;

            Screens = _screenModel.Screens;
            CurrentTime = _currentTimeService.GetCurrentTime();

            _currentTimeService.OnCurrTimeChanged += (sender, args) =>
            {
                CurrentTime = args.CurrTime;
            };

            _screenModel.ForceGamma();
            _gammaWatcher.StartWatch();
        }
    }
}
