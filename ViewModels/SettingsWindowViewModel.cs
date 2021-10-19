using Sparky.Commands;
using Sparky.Services;
using Sparky.ViewModels.Base;
using System.Windows.Input;
using Sparky.Services.Interfaces;


namespace Sparky.ViewModels
{
    internal sealed class SettingsWindowViewModel : ViewModelBase
    {
        #region Fields
        private ViewModelBase _selectedViewModel;
        private readonly IDialogService _dialogService;
        private readonly IPeriodWatcherService _periodWatcherService;
        private readonly ISettingsService _settingsService;

        #endregion

        #region Properties

        public int SelectedLangIndex
        {
            get => _settingsService.SelectedLang;
            set
            {
                _settingsService.SelectedLang = value;
                Localization.LangDictionary.SetLang(value);
            }
        }

        public ViewModelBase SelectedViewModel
        {
            get => _selectedViewModel;
            private set => Set(ref _selectedViewModel, value);
        }

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
            _settingsService.Reset();
        }

        private void OnCloseSettingsCommandExecute()
        {
            _dialogService.CloseDialog<SettingsWindowViewModel>();
            _settingsService.Reset();
            _periodWatcherService.StartWatch();
        }

        private void OnApplySettingsCommandExecute()
        {
            _settingsService.Save();
            _dialogService.CloseDialog<SettingsWindowViewModel>();
            _periodWatcherService.StartWatch();
        }

        #endregion

        public SettingsWindowViewModel(
            ISettingsService settingsService, 
            IPeriodWatcherService periodWatcherService, 
            IDialogService dialogService)
        {
            ApplySettingsCommand = new LambdaCommand(_ => OnApplySettingsCommandExecute());
            ResetSettingsCommand = new LambdaCommand(_ => OnResetSettingsCommandExecute());
            CloseSettingsCommand = new LambdaCommand(_ => OnCloseSettingsCommandExecute());

            ToOtherSettingsPageCommand = new LambdaCommand(_ => OnToOtherSettingsPageCommandExecute());
            ToSettingsMainPageCommand = new LambdaCommand(_ => OnToSettingsMainPageCommandExecute());
            ToProcessPageCommand = new LambdaCommand(_ => OnToProcessPageCommandExecute());

            _settingsService = settingsService;
            _periodWatcherService = periodWatcherService;
            _dialogService = dialogService;
            _periodWatcherService.StopWatch();

            SelectedViewModel = ViewModelLocator.SettingsMainPageViewModel;
        }

        public SettingsWindowViewModel() {}

#if DEBUG
        ~SettingsWindowViewModel()
        {
            LoggingModule.Log.Verbose("[View Model] SettingsWindowViewModel Disposed");
        }
#endif
    }
}