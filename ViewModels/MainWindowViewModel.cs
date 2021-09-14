using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Light.Commands;
using Light.Models;
using Light.Models.Entities;
using Light.Services;
using Light.ViewModels.Base;

namespace Light.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {
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

        #region Fields

        public ObservableCollection<ScreenEntity> Screens { get; }

        private readonly WindowService _windowService;
        private readonly ScreenModel _screenModel;

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

        private void OnAppToTrayCommandExecute()
        {
            /* in progress */
        }

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

            _screenModel = new ScreenModel();
            var serviceLocator = ServiceLocator.Source;
            _windowService = serviceLocator.WindowService;
            var currentTimeService = serviceLocator.CurrentTimeService;
            var gammaWatcher = serviceLocator.GammaWatcher;

            Screens = _screenModel.Screens;
            CurrentTime = currentTimeService.GetCurrentTime();

            currentTimeService.OnCurrTimeChanged += (sender, args) => { CurrentTime = args.CurrTime; };

            _screenModel.ForceGamma();
            gammaWatcher.StartWatch();
        }
    }
}