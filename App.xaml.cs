using Light.Services;
using Light.ViewModels;
using Light.Views.Main;
using Light.Views.Settings;
using Light.Views.Tray;

namespace Light
{
    public partial class App
    {
        private App()
        {
            InitializeComponent();
            var serviceLocator = ServiceLocator.Source;
            var appSettings = serviceLocator.Settings;
            var dialogService = serviceLocator.DialogService;
            appSettings.Load();

            dialogService.Register<MainWindowViewModel, MainWindowView>();
            dialogService.Register<SettingsWindowViewModel, SettingsWindowView>();
            dialogService.Register<TrayMenuViewModel, TrayMenuView>();
            dialogService.CreateDialog<MainWindowViewModel>();
            dialogService.ShowDialog<MainWindowViewModel>();
        }
    }
}