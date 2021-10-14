﻿using Light.Commands;
using Light.Models;
using Light.Templates.Entities;
using Light.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Light.Services.Interfaces;
using Application = System.Windows.Application;

namespace Light.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private string _currentTime = "12:00";
        public ObservableCollection<ScreenEntity> Screens { get; }
        private readonly IDialogService _dialogService;
        private readonly ITrayNotifierService _trayNotifierService;

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
            Application.Current.Shutdown();
        }

        private void OnAppToTrayCommandExecute()
        {
            _trayNotifierService.ShowTip(Localization.LangDictionary.GetString("Loc_ToTrayNotification"));
            _dialogService.CloseDialog<MainWindowViewModel>();
        }

        private void OnOpenSettingsWindowCommandExecute()
        {
            _dialogService.ShowDialog<SettingsWindowViewModel>(typeof(MainWindowViewModel));
        }

        #endregion

        public MainWindowViewModel(
            ICurrentTimeService currentTimeService, 
            ITrayNotifierService trayNotifierService, 
            ISettingsService settingsService, 
            IDialogService dialogService)
        {
            _trayNotifierService = trayNotifierService;
            _dialogService = dialogService;
            OpenSettingsWindowCommand = new LambdaCommand(p => OnOpenSettingsWindowCommandExecute());
            CloseAppCommand = new LambdaCommand(p => OnCloseAppCommandExecute());
            AppToTrayCommand = new LambdaCommand(p => OnAppToTrayCommandExecute());

            Screens = settingsService.Screens;
            CurrentTime = currentTimeService.GetCurrentTime();

            currentTimeService.OnCurrTimeChanged += (_, args) => { CurrentTime = args.CurrentTime; };
        }

#if DEBUG
        ~MainWindowViewModel()
        {
            Logging.Write("[View Model] MainWindowViewModel Disposed");
        }
#endif
    }
}