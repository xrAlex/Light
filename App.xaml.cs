using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Light.Models;
using Light.Services;
using Light.Services.Interfaces;
using Light.ViewModels;
using Light.Views.Main;
using Light.Views.Settings;
using Ninject;

namespace Light
{
    public partial class App
    {
        private readonly Mutex _mutex;
        private App()
        {
            _mutex = new Mutex(true, ResourceAssembly.GetName().Name);

            if (!_mutex.WaitOne())
            {
                Current.Shutdown();
                return;
            }

            var appSettings = Kernel.Get<ISettingsService>();
            appSettings.Load();

            var dialogService = Kernel.Get<IDialogService>();
            var periodWatcherService = Kernel.Get<IPeriodWatcherService>();

            var silentLaunch = Environment.GetCommandLineArgs().Contains("-silent");

            dialogService.Register<MainWindowViewModel, MainWindowView>();
            dialogService.Register<SettingsWindowViewModel, SettingsWindowView>();

            periodWatcherService.StartWatch();
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            InitializeComponent();

            if (!silentLaunch)
            {
                dialogService.ShowDialog<MainWindowViewModel>();
            }

        }
    }


    public partial class App
    {
        private static IKernel _kernel;

        public static IKernel Kernel { get; } = _kernel ??= ConfigureServices();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetupExceptionHandling();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Kernel.Get<IPeriodWatcherService>().StopWatch();
            ScreenModel.SetDefaultColorTemperatureOnAllScreens();
            _mutex?.ReleaseMutex();
            base.OnExit(e);
            _kernel = null;
        }

        private static IKernel ConfigureServices()
        {
            IKernel standardKernel = new StandardKernel();

            //Services
            standardKernel.Bind<IPeriodWatcherService>().To<PeriodWatcherService>().InSingletonScope();
            standardKernel.Bind<ISettingsService>().To<SettingsService>().InSingletonScope();
            standardKernel.Bind<IDialogService>().To<DialogService>().InSingletonScope();
            standardKernel.Bind<ICurrentTimeService>().To<CurrentTimeService>().InSingletonScope();

            //ViewModels
            standardKernel.Bind<MainWindowViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<OtherSettingsPageViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<ProcessPageViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<SettingsMainPageViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<SettingsWindowViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<TrayMenuViewModel>().ToSelf().InTransientScope();

            return standardKernel;
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) => 
                Logging.Write("UnhandledException", (Exception)e.ExceptionObject);

            DispatcherUnhandledException += (s, e) =>
            {
                Logging.Write("DispatcherUnhandledException", e.Exception);
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Logging.Write("UnobservedTaskException", e.Exception);
                e.SetObserved();
            };
        }
    }
}