using Light.Commands;
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
        #region Values

        private WindowService _windowService;
        private ViewModelBase _selectedViewModel;
        public OtherSettingsPageViewModel OtherSettingsPage { get; private set; }
        public SettingsMainPageViewModel SettingsMainPage { get; private set; }
        public ProcessPageViewModel ProcessPage { get; private set; }

        #endregion

        public ViewModelBase SelectedViewModel
        {
            get => _selectedViewModel;
            set => Set(ref _selectedViewModel, value);
        }

        public ICommand ChangePageCommand { get; }
        public ICommand ApplySettingsCommand { get; }
        public ICommand ResetSettingsCommand { get; }
        public ICommand CloseSettingsCommand { get; }

        private void OnChangePageCommandExecute(object viewModel)
        {
            SelectedViewModel = (ViewModelBase)viewModel;
        }
        private void CreatePages()
        {
            OtherSettingsPage = new();
            SettingsMainPage = new();
            ProcessPage = new();
        }

        private void OnResetSettingsCommandExecute()
        {
            /* in progress */
        }

        private void OnCloseSettingsCommandExecute()
        {
            _windowService.CloseWindow();
        }

        private void OnApplySettingsCommandExecute()
        {
            _windowService.CloseWindow();
        }


        public SettingsWindowViewModel(WindowService windowService)
        {
            ApplySettingsCommand = new LambdaCommand(p => OnApplySettingsCommandExecute());
            ResetSettingsCommand = new LambdaCommand(p => OnResetSettingsCommandExecute());
            CloseSettingsCommand = new LambdaCommand(p => OnCloseSettingsCommandExecute());
            ChangePageCommand = new LambdaCommand(OnChangePageCommandExecute);

            CreatePages();
            SelectedViewModel = SettingsMainPage;
            _windowService = windowService;
        }
    }
}
