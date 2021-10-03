using System;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using Light.Infrastructure;
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


            _ = serviceLocator.TrayNotifier;
            dialogService.Register<MainWindowViewModel, MainWindowView>();
            dialogService.Register<SettingsWindowViewModel, SettingsWindowView>();
            dialogService.Register<TrayMenuViewModel, TrayMenuView>(true);
            dialogService.CreateDialog<MainWindowViewModel>();

            var silentLaunch = Environment.GetCommandLineArgs().Contains("-silent");

            if (appSettings.StartMinimized)
            {   
                if (!silentLaunch)
                {
                    dialogService.ShowDialog<MainWindowViewModel>();
                }
            }
            else
            {
                dialogService.ShowDialog<MainWindowViewModel>();
            }
        }
    }
}