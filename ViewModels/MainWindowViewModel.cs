using Sparky.Commands;
using Sparky.Templates.Entities;
using Sparky.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Sparky.Services.Interfaces;

namespace Sparky.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {

        #region Fields

        private string _currentTime = "12:00";
        public ObservableCollection<ScreenEntity> Screens { get; }
        private readonly IDialogService _dialogService;

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
            _dialogService.CloseDialog<MainWindowViewModel>();
            Application.Current.Shutdown();
        }

        private void OnAppToTrayCommandExecute()
        {
            _dialogService.CloseDialog<MainWindowViewModel>();
        }

        private void OnOpenSettingsWindowCommandExecute()
        {
            _dialogService.ShowDialog<SettingsWindowViewModel>(typeof(MainWindowViewModel));
        }

        #endregion

        public MainWindowViewModel(
            ICurrentTimeService currentTimeService,
            ISettingsService settingsService, 
            IDialogService dialogService)
        {
            _dialogService = dialogService;
            OpenSettingsWindowCommand = new LambdaCommand(p => OnOpenSettingsWindowCommandExecute());
            CloseAppCommand = new LambdaCommand(p => OnCloseAppCommandExecute());
            AppToTrayCommand = new LambdaCommand(p => OnAppToTrayCommandExecute());

            Screens = settingsService.Screens;
            CurrentTime = currentTimeService.GetCurrentTime();

            currentTimeService.OnCurrTimeChanged += (_, args) => { CurrentTime = args.CurrentTime; };
        }

        public MainWindowViewModel() {}

#if DEBUG
        ~MainWindowViewModel()
        {
            LoggingModule.Log.Verbose("[View Model] MainWindowViewModel Disposed");
        }
#endif
    }
}