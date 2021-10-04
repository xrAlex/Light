using System;
using System.Linq;
using System.Threading;
using System.Windows;
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
        private readonly Mutex _mutex;
        private readonly TrayNotifierService _trayNotifier;

        protected override void OnExit(ExitEventArgs e)
        {
            _trayNotifier?.Dispose();
            _mutex?.ReleaseMutex();
            base.OnExit(e);
        }

        private App()
        {
            _mutex = new Mutex(true, ResourceAssembly.GetName().Name);

            if (!_mutex.WaitOne())
            {
                Current.Shutdown();
                return;
            }

            InitializeComponent();
            var serviceLocator = ServiceLocator.Source;
            var appSettings = serviceLocator.Settings;
            var dialogService = serviceLocator.DialogService;
            var periodWatcherService = serviceLocator.PeriodWatcherService;
            var silentLaunch = Environment.GetCommandLineArgs().Contains("-silent");
            appSettings.Load();
            periodWatcherService.StartWatch();

            dialogService.Register<MainWindowViewModel, MainWindowView>();
            dialogService.Register<SettingsWindowViewModel, SettingsWindowView>();
            dialogService.Register<TrayMenuViewModel, TrayMenuView>(true);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _trayNotifier = serviceLocator.TrayNotifier;

            if (!silentLaunch)
            {
                dialogService.ShowDialog<MainWindowViewModel>();
            }
        }
    }
}