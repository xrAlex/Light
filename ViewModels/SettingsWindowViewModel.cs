#region

using System.Windows.Input;
using Light.Commands;
using Light.Services;
using Light.ViewModels.Base;

#endregion

namespace Light.ViewModels
{
    internal sealed class SettingsWindowViewModel : ViewModelBase
    {
        #region Values

        private ViewModelBase _selectedViewModel;

        #endregion

        #region Properties

        public ViewModelBase SelectedViewModel
        {
            get => _selectedViewModel;
            private set => Set(ref _selectedViewModel, value);
        }

        #endregion

        #region Fields

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
            SelectedViewModel = new OtherSettingsPageViewModel();
        }
        private void OnToSettingsMainPageCommandExecute()
        {
            SelectedViewModel = new SettingsMainPageViewModel();
        }
        private void OnToProcessPageCommandExecute()
        {
            SelectedViewModel = new ProcessPageViewModel();
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

            var serviceLocator = ServiceLocator.Source;
            _settings = serviceLocator.Settings;
            _dialogService = serviceLocator.DialogService;
            _colorTemperatureWatcher = serviceLocator.ColorTemperatureWatcher;
            _colorTemperatureWatcher.StopWatch();

            SelectedViewModel = new SettingsMainPageViewModel();
        }

#if DEBUG
        ~SettingsWindowViewModel()
        {
            DebugConsole.Print("SettingsWindowViewModel Disposed");
        }
#endif
    }
}