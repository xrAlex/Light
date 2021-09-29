#region

using System.Windows;
using Light.Services;
using Light.ViewModels;
using Light.Views;
using Light.Views.Main;
using Light.Views.Settings;
using Light.Views.Tray;

#endregion

namespace Light
{
    /// <summary>
    ///     Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
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