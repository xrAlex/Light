using System;
using System.Linq;
using System.Reflection;
using System.Threading;
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
        private static IKernel _kernel;
        public static IKernel Kernel { get; } = _kernel ??= ConfigureServices();

        public App()
        {
            _mutex = new Mutex(true, ResourceAssembly.GetName().Name);

            if (!_mutex.WaitOne())
            {
                Current.Shutdown();
                return;
            }
            var logs = new LoggingModule();
            logs.Initialize();

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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _ = Kernel.Get<ITrayNotifierService>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Kernel.Get<IPeriodWatcherService>().StopWatch();
            ScreenModel.SetDefaultColorTemperatureOnAllScreens();
            _mutex?.ReleaseMutex();
            Kernel.Get<ITrayNotifierService>().Dispose();
            base.OnExit(e);

            _kernel = null;
            Kernel.Dispose();
        }

        private static IKernel ConfigureServices()
        {
            IKernel standardKernel = new StandardKernel();
            standardKernel.Load(Assembly.GetExecutingAssembly());

            //Services
            standardKernel.Bind<IPeriodWatcherService>().To<PeriodWatcherService>().InSingletonScope();
            standardKernel.Bind<ISettingsService>().To<SettingsService>().InSingletonScope();
            standardKernel.Bind<IDialogService>().To<DialogService>().InSingletonScope();
            standardKernel.Bind<ICurrentTimeService>().To<CurrentTimeService>().InSingletonScope();
            standardKernel.Bind<ITrayNotifierService>().To<TrayNotifierService>().InSingletonScope();

            //ViewModels
            standardKernel.Bind<MainWindowViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<OtherSettingsPageViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<ProcessPageViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<SettingsMainPageViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<SettingsWindowViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<TrayMenuViewModel>().ToSelf().InTransientScope();
            return standardKernel;
        }
    }
}