using Light.Commands;
using Light.Services;
using Light.ViewModels.Base;
using System.Windows.Input;


namespace Light.ViewModels
{
    internal sealed class SettingsWindowViewModel : ViewModelBase
    {
        #region Properties

        public ViewModelBase SelectedViewModel
        {
            get => _selectedViewModel;
            private set => Set(ref _selectedViewModel, value);
        }

        #endregion

        #region Fields
        private ViewModelBase _selectedViewModel;
        private readonly DialogService _dialogService;
        private readonly SettingsService _settings;
        private readonly PeriodWatcherService _colorTemperatureWatcher;

        #endregion

        #region Commands

        public ICommand ToOtherSettingsPageCommand { get; }
        public ICommand ToSettingsMainPageCommand { get; }
        public ICommand ToProcessPageCommand { get; }
        public ICommand ApplySettingsCommand { get; }
        public ICommand ResetSettingsCommand { get; }
        public ICommand CloseSettingsCommand { get; }

        private void OnToOtherSettingsPageCommandExecute()
        {
            SelectedViewModel = ViewModelLocator.OtherSettingsPageViewModel;
        }
        private void OnToSettingsMainPageCommandExecute()
        {
            SelectedViewModel = ViewModelLocator.SettingsMainPageViewModel;
        }
        private void OnToProcessPageCommandExecute()
        {
            SelectedViewModel = ViewModelLocator.ProcessPageViewModel;
        }

        private void OnResetSettingsCommandExecute()
        {
            _settings.Reset();
        }

        private void OnCloseSettingsCommandExecute()
        {
            _settings.Reset();
            _dialogService.CloseDialog<SettingsWindowViewModel>();
            _colorTemperatureWatcher.StartWatch();
        }

        private void OnApplySettingsCommandExecute()
        {
            _settings.Save();
            _dialogService.CloseDialog<SettingsWindowViewModel>();
            _colorTemperatureWatcher.StartWatch();
        }

        #endregion

        public SettingsWindowViewModel()
        {
            ApplySettingsCommand = new LambdaCommand(p => OnApplySettingsCommandExecute());
            ResetSettingsCommand = new LambdaCommand(p => OnResetSettingsCommandExecute());
            CloseSettingsCommand = new LambdaCommand(p => OnCloseSettingsCommandExecute());

            ToOtherSettingsPageCommand = new LambdaCommand(p => OnToOtherSettingsPageCommandExecute());
            ToSettingsMainPageCommand = new LambdaCommand(p => OnToSettingsMainPageCommandExecute());
            ToProcessPageCommand = new LambdaCommand(p => OnToProcessPageCommandExecute());

            _settings = ServiceLocator.Settings;
            _dialogService = ServiceLocator.DialogService;
            _colorTemperatureWatcher = ServiceLocator.PeriodWatcherService;
            _colorTemperatureWatcher.StopWatch();

            SelectedViewModel = new SettingsMainPageViewModel();
        }

#if DEBUG
        ~SettingsWindowViewModel()
        {
            DebugConsole.Print("[View Model] SettingsWindowViewModel Disposed");
        }
#endif
    }
}