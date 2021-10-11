using Light.Commands;
using Light.Models;
using Light.Services;
using Light.Templates.Entities;
using Light.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace Light.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private string _currentTime = "12:00";
        public ObservableCollection<ScreenEntity> Screens { get; }
        private readonly DialogService _dialogService;
        private readonly TrayNotifierService _trayNotifier;

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
            Application.Current.Shutdown();
        }

        private void OnAppToTrayCommandExecute()
        {
            _trayNotifier.ShowTip(Localization.LangDictionary.GetString("Loc_ToTrayNotification"));
            _dialogService.CloseDialog<MainWindowViewModel>();
        }

        private void OnOpenSettingsWindowCommandExecute()
        {
            _dialogService.ShowDialog<SettingsWindowViewModel>(typeof(MainWindowViewModel));
        }

        #endregion

        public MainWindowViewModel()
        {
            OpenSettingsWindowCommand = new LambdaCommand(p => OnOpenSettingsWindowCommandExecute());
            CloseAppCommand = new LambdaCommand(p => OnCloseAppCommandExecute());
            AppToTrayCommand = new LambdaCommand(p => OnAppToTrayCommandExecute());

            var screenModel = new ScreenModel();
            _dialogService = ServiceLocator.DialogService;
            _trayNotifier = ServiceLocator.TrayNotifier;
            var currentTimeService = ServiceLocator.CurrentTimeService;

            Screens = screenModel.Screens;
            CurrentTime = currentTimeService.GetCurrentTime();

            currentTimeService.OnCurrTimeChanged += (_, args) => { CurrentTime = args.CurrentTime; };
        }

#if DEBUG
        ~MainWindowViewModel()
        {
            DebugConsole.Print("[View Model] MainWindowViewModel Disposed");
        }
#endif
    }
}