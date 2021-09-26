#region

using System.Windows;
using Light.Services;
using Light.ViewModels;
using Light.Views;

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

            dialogService.Register<MainWindowViewModel,MainWindow>();
            dialogService.Register<SettingsWindowViewModel, SettingsWindow>();
            dialogService.ShowDialog<MainWindowViewModel>();
        }
    }
}