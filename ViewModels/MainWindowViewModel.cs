using Light.Commands;
using Light.Services;
using Light.ViewModels.Base;
using System.Windows.Input;

namespace Light.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {

        private readonly DialogService _dialogService;

        public ICommand AppToTrayCommand { get; }


        private void OnAppToTrayCommandExecute()
        {
            _dialogService.HideDialog<MainWindowViewModel>();
            _dialogService.CreateDialog<TrayMenuViewModel>();
        }


        public MainWindowViewModel()
        {
            AppToTrayCommand = new LambdaCommand(p => OnAppToTrayCommandExecute());

            var serviceLocator = ServiceLocator.Source;
            _dialogService = serviceLocator.DialogService;
        }

#if DEBUG
        ~MainWindowViewModel()
        {
            DebugConsole.Print("MainWindowViewModel Disposed");
        }
#endif
    }
}