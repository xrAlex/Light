using Light.Commands;
using Light.Models;
using Light.Services;
using Light.Templates.Entities;
using Light.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Hardcodet.Wpf.TaskbarNotification;
using Application = System.Windows.Application;

namespace Light.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private string _currentTime = "12:00";
        public ObservableCollection<ScreenEntity> Screens { get; }
        private readonly DialogService _dialogService;

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
            ScreenModel.SetDefaultColorTemperatureOnAllScreens();
            _dialogService.CloseDialog<MainWindowViewModel>();
            Application.Current.Shutdown();
        }

        private void OnAppToTrayCommandExecute()
        {
            _dialogService.HideDialog<MainWindowViewModel>();
        }

        private void OnOpenSettingsWindowCommandExecute()
        {
            _dialogService.CreateDialog<SettingsWindowViewModel, MainWindowViewModel>();
            _dialogService.ShowDialog<SettingsWindowViewModel>();
        }

        #endregion

        public MainWindowViewModel()
        {
            OpenSettingsWindowCommand = new LambdaCommand(p => OnOpenSettingsWindowCommandExecute());
            CloseAppCommand = new LambdaCommand(p => OnCloseAppCommandExecute());
            AppToTrayCommand = new LambdaCommand(p => OnAppToTrayCommandExecute());

            var screenModel = new ScreenModel();
            var serviceLocator = ServiceLocator.Source;
            _dialogService = serviceLocator.DialogService;
            var currentTimeService = serviceLocator.CurrentTimeService;
            var colorTemperatureWatcher = serviceLocator.PeriodWatcherService;

            Screens = screenModel.Screens;
            CurrentTime = currentTimeService.GetCurrentTime();

            currentTimeService.OnCurrTimeChanged += (_, args) => { CurrentTime = args.CurrTime; };

            screenModel.ForceColorTemperature();
            colorTemperatureWatcher.StartWatch();
        }

#if DEBUG
        ~MainWindowViewModel()
        {
            DebugConsole.Print("MainWindowViewModel Disposed");
        }
#endif
    }
}