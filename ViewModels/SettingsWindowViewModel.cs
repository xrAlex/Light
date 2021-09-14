using System.Windows.Input;
using Light.Commands;
using Light.Services;
using Light.ViewModels.Base;

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

        private readonly WindowService _windowService;
        private readonly SettingsService _settings;
        private readonly GammaWatcherService _gammaWatcher;
        public OtherSettingsPageViewModel OtherSettingsPage { get; }
        public SettingsMainPageViewModel SettingsMainPage { get; }
        public ProcessPageViewModel ProcessPage { get; }

        #endregion

        #region Commands

        public ICommand ChangePageCommand { get; }
        public ICommand ApplySettingsCommand { get; }
        public ICommand ResetSettingsCommand { get; }
        public ICommand CloseSettingsCommand { get; }

        private void OnChangePageCommandExecute(object viewModel)
        {
            SelectedViewModel = (ViewModelBase)viewModel;
        }

        private void OnResetSettingsCommandExecute()
        {
            _settings.Reset();
        }

        private void OnCloseSettingsCommandExecute()
        {
            _settings.Reset();
            _windowService.CloseWindow();
            _gammaWatcher.StartWatch();
        }

        private void OnApplySettingsCommandExecute()
        {
            _settings.Save();
            _windowService.CloseWindow();
            _gammaWatcher.StartWatch();
        }

        #endregion

        public SettingsWindowViewModel()
        {
            ApplySettingsCommand = new LambdaCommand(p => OnApplySettingsCommandExecute());
            ResetSettingsCommand = new LambdaCommand(p => OnResetSettingsCommandExecute());
            CloseSettingsCommand = new LambdaCommand(p => OnCloseSettingsCommandExecute());
            ChangePageCommand = new LambdaCommand(OnChangePageCommandExecute);

            var serviceLocator = ServiceLocator.Source;
            _settings = serviceLocator.Settings;
            _windowService = serviceLocator.WindowService;
            _gammaWatcher = serviceLocator.GammaWatcher;

            OtherSettingsPage = new OtherSettingsPageViewModel();
            SettingsMainPage = new SettingsMainPageViewModel();
            ProcessPage = new ProcessPageViewModel();
            SelectedViewModel = SettingsMainPage;
            _gammaWatcher.StopWatch();
        }
    }
}