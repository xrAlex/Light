using System;
using System.Linq;
using System.Threading;
using System.Windows;
using Sparky.Models;
using Sparky.Services;
using Sparky.Services.Interfaces;
using Sparky.ViewModels;
using Sparky.Views.Main;
using Sparky.Views.Settings;
using Ninject;
using Sparky.Views.Information;

namespace Sparky
{
    public partial class App : Application
    {
        private readonly Mutex _mutex;
        public static IKernel Kernel { get; private set; }

        public App()
        {
            _mutex = new Mutex(true, ResourceAssembly.GetName().Name);
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            new LoggingModule().Initialize();

            ValidateInstance();
            ConfigureServices();
            ConfigureViewContainer();
            InitializeComponent();

            Kernel.Get<ITrayNotifierService>();
            Kernel.Get<IPeriodWatcherService>().StartWatch();

            var silentLaunch = Environment.GetCommandLineArgs().Contains("-silent");
            if (!silentLaunch)
            {
                Kernel.Get<IDialogService>().ShowDialog<MainWindowViewModel>();
            }
        }
    }


    public partial class App
    {
        protected override void OnExit(ExitEventArgs e)
        {
            Kernel.Get<IPeriodWatcherService>().StopWatch();
            ScreenModel.SetDefaultColorTemperatureOnAllScreens();
            _mutex?.ReleaseMutex();
            Kernel.Get<ITrayNotifierService>().Dispose();
            Kernel.Dispose();

            base.OnExit(e);
        }
        private void ValidateInstance()
        {
            if (!_mutex.WaitOne()) Current.Shutdown();
        }
        private void ConfigureServices()
        {
            IKernel standardKernel = new StandardKernel();

            //Services
            standardKernel.Bind<IPeriodWatcherService>().To<PeriodWatcherService>().InSingletonScope();
            standardKernel.Bind<ISettingsService>().To<SettingsService>().InSingletonScope();
            standardKernel.Bind<IDialogService>().To<DialogService>().InSingletonScope();
            standardKernel.Bind<ICurrentTimeService>().To<CurrentTimeService>().InSingletonScope();
            standardKernel.Bind<ITrayNotifierService>().To<TrayNotifierService>().InSingletonScope();
            standardKernel.Bind<ILinksService>().To<LinksService>().InTransientScope();

            //ViewModels
            standardKernel.Bind<MainWindowViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<OtherSettingsPageViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<ProcessPageViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<SettingsMainPageViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<SettingsWindowViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<TrayMenuViewModel>().ToSelf().InTransientScope();
            standardKernel.Bind<InformationViewModel>().ToSelf().InTransientScope();

            Kernel = standardKernel;
        }
        private void ConfigureViewContainer()
        {
            var dialogService = Kernel.Get<IDialogService>();
            dialogService.Register<MainWindowViewModel, MainWindowView>();
            dialogService.Register<SettingsWindowViewModel, SettingsWindowView>();
            dialogService.Register<InformationViewModel, InformationView>();
        }
    }
}