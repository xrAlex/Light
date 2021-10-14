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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            var appSettings = ServicesHost.Services.GetRequiredService<ISettingsService>();
            appSettings.Load();

            var dialogService = ServicesHost.Services.GetRequiredService<IDialogService>();
            var periodWatcherService = ServicesHost.Services.GetRequiredService<IPeriodWatcherService>();

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
        public static IHost ServicesHost => _servicesHost ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        private static IHost _servicesHost;

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .UseContentRoot(Environment.CurrentDirectory)
                .ConfigureAppConfiguration((_, cfg) => cfg
                    .SetBasePath(Environment.CurrentDirectory)
                )
                .ConfigureServices(ConfigureServices);

            return hostBuilder;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetupExceptionHandling();
            await ServicesHost.StartAsync().ConfigureAwait(false);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            ServicesHost.Services.GetRequiredService<IPeriodWatcherService>().StopWatch();
            ScreenModel.SetDefaultColorTemperatureOnAllScreens();
            _mutex?.ReleaseMutex();
            base.OnExit(e);

            using (ServicesHost) await ServicesHost.StopAsync().ConfigureAwait(false);
            _servicesHost = null;
        }

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            //Services
            services.AddSingleton<IPeriodWatcherService, PeriodWatcherService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddTransient<ICurrentTimeService, CurrentTimeService>();

            //ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<OtherSettingsPageViewModel>();
            services.AddTransient<ProcessPageViewModel>();
            services.AddTransient<SettingsMainPageViewModel>();
            services.AddTransient<SettingsWindowViewModel>();
            services.AddTransient<TrayMenuViewModel>();
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