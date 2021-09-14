using Light.Commands;
using Light.Services;
using Light.ViewModels.Base;
using System.Windows.Input;

namespace Light.ViewModels
{
    internal sealed class SettingsWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly WindowService _windowService;
        private readonly SettingsService _settings;
        private readonly ServiceLocator _serviceLocator;
        private readonly GammaWatcherService _gammaWatcher;
        public OtherSettingsPageViewModel OtherSettingsPage { get; private set; }
        public SettingsMainPageViewModel SettingsMainPage { get; private set; }
        public ProcessPageViewModel ProcessPage { get; private set; }

        #endregion

        #region Values

        private ViewModelBase _selectedViewModel;

        #endregion

        #region Properties

        public ViewModelBase SelectedViewModel
        {
            get => _selectedViewModel;
            set => Set(ref _selectedViewModel, value);
        }

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

            _serviceLocator = ServiceLocator.Source;
            _settings = _serviceLocator.Settings;
            _windowService = _serviceLocator.WindowService;
            _gammaWatcher = _serviceLocator.GammaWatcher;

            OtherSettingsPage = new();
            SettingsMainPage = new();
            ProcessPage = new();
            SelectedViewModel = SettingsMainPage;
            _gammaWatcher.StopWatch();
        }
    }
}
