using Light.Commands;
using Light.Models;
using Light.Services;
using Light.ViewModels.Base;
using System.Windows.Input;
using Light.Services.Interfaces;
using Application = System.Windows.Application;

namespace Light.ViewModels
{
    internal sealed class TrayMenuViewModel : ViewModelBase
    {
        #region Fields

        private readonly IPeriodWatcherService _periodWatcherService;
        private readonly IDialogService _dialogService;
        private bool _isAppPaused;
        private string _workTimeButtonText = "Loc_TrayPause";
        public string CancelButtonText => "Loc_TrayClose";

        #endregion

        #region Constructors

        public string WorkTimeButtonText
        {
            get => _workTimeButtonText;
            private set => Set(ref _workTimeButtonText, value);
        }

        #endregion

        #region Commands
        public ICommand PauseCommand { get; }
        public ICommand ShutdownCommand { get; }
        public ICommand ShowMainWindowCommand { get; }

        private void OnPauseCommandExecute()
        {
            if (_isAppPaused)
            {
                _periodWatcherService.StartWatch();
                WorkTimeButtonText = "Loc_TrayPause";
            }
            else
            {
                _periodWatcherService.StopWatch();
                ScreenModel.SetDefaultColorTemperatureOnAllScreens();
                WorkTimeButtonText = "Loc_TrayUnPause";
            }
            _isAppPaused = !_isAppPaused;
        }

        private void OnShowMainWindowCommandExecute()
        {
            _dialogService.ShowDialog<MainWindowViewModel>();
        }

        private void OnShutdownCommandExecute()
        {
            ScreenModel.SetDefaultColorTemperatureOnAllScreens();
            Application.Current.Shutdown();
        }

        #endregion

        #region Methods

        private void RefreshUI()
        {
            OnPropertyChanged("WorkTimeButtonText");
            OnPropertyChanged("CancelButtonText");
        }

        #endregion


        public TrayMenuViewModel(IPeriodWatcherService periodWatcherService, IDialogService dialogService)
        {
            PauseCommand = new LambdaCommand(_ => OnPauseCommandExecute());
            ShutdownCommand = new LambdaCommand(_ => OnShutdownCommandExecute());
            ShowMainWindowCommand = new LambdaCommand(_ => OnShowMainWindowCommandExecute());

            _periodWatcherService = periodWatcherService;
            _dialogService = dialogService;

            Localization.LangDictionary.OnLocalizationChanged += (_, _) => { RefreshUI(); };
        }

        public TrayMenuViewModel() {}

#if DEBUG
        ~TrayMenuViewModel()
        {
            LoggingModule.Log.Verbose("[View Model] TrayMenuViewModel Disposed");
        }
#endif
    }
}
