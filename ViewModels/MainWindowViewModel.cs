#region

using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using Light.Commands;
using Light.Models;
using Light.Services;
using Light.Templates.Entities;
using Light.ViewModels.Base;
using Application = System.Windows.Application;

#endregion

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

        private readonly ScreenModel _screenModel;
        private readonly DialogService _dialogService;

        #endregion

        #region Commands

        public ICommand CloseAppCommand { get; }
        public ICommand AppToTrayCommand { get; }
        public ICommand OpenSettingsWindowCommand { get; }

        private void OnCloseAppCommandExecute()
        {
            _screenModel.SetDefaultColorTemperatureOnAllScreens();
            _dialogService.CloseDialog<MainWindowViewModel>();
            Application.Current.Shutdown();
        }

        private void OnAppToTrayCommandExecute()
        {
            _dialogService.HideDialog<MainWindowViewModel>();
            _dialogService.CreateDialog<TrayMenuViewModel>();
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

            _screenModel = new ScreenModel();
            var serviceLocator = ServiceLocator.Source;
            _dialogService = serviceLocator.DialogService;
            var currentTimeService = serviceLocator.CurrentTimeService;
            var colorTemperatureWatcher = serviceLocator.PeriodWatcherService;

            Screens = _screenModel.Screens;
            CurrentTime = currentTimeService.GetCurrentTime();

            currentTimeService.OnCurrTimeChanged += (sender, args) => { CurrentTime = args.CurrTime; };

            _screenModel.ForceColorTemperature();
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