using Light.Commands;
using Light.Infrastructure;
using Light.Properties;
using Light.Services;
using Light.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Light.ViewModels
{
    internal sealed class SettingsWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly WindowService _windowService;
        private readonly SettingsService _settings;
        private readonly ServiceLocator _serviceLocator;
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
        }

        private void OnApplySettingsCommandExecute()
        {
            _settings.Save();
            _windowService.CloseWindow();
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

            OtherSettingsPage = new();
            SettingsMainPage = new();
            ProcessPage = new();
            SelectedViewModel = SettingsMainPage;
        }
    }
}
