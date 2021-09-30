using Light.Services;
using Light.ViewModels;
using Light.Views.Main;
using Light.Views.Tray;

namespace Light
{
    public partial class App
    {
        private App()
        {
            InitializeComponent();
            var serviceLocator = ServiceLocator.Source;
            var dialogService = serviceLocator.DialogService;

            dialogService.Register<MainWindowViewModel, MainWindowView>();
            dialogService.Register<TrayMenuViewModel, TrayMenuView>();
            dialogService.CreateDialog<MainWindowViewModel>();
            dialogService.ShowDialog<MainWindowViewModel>();
        }
    }
}