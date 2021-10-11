using System;
using System.Linq;
using System.Threading;
using System.Windows;
using Light.Services;
using Light.ViewModels;
using Light.Views.Main;
using Light.Views.Settings;
using Light.Views.Tray;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Light
{
    public partial class App
    {
        private readonly Mutex _mutex;
        private readonly TrayNotifierService _trayNotifier;
        private App()
        {
            _mutex = new Mutex(true, ResourceAssembly.GetName().Name);

            if (!_mutex.WaitOne())
            {
                Current.Shutdown();
                return;
            }

            var appSettings = ServiceLocator.Settings;
            appSettings.Load();

            InitializeComponent();

            var dialogService = ServiceLocator.DialogService;
            var periodWatcherService = ServiceLocator.PeriodWatcherService;
            var silentLaunch = Environment.GetCommandLineArgs().Contains("-silent");
            periodWatcherService.StartWatch();

            dialogService.Register<MainWindowViewModel, MainWindowView>();
            dialogService.Register<SettingsWindowViewModel, SettingsWindowView>();
            dialogService.Register<TrayMenuViewModel, TrayMenuView>();

            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _trayNotifier = ServicesHost.Services.GetRequiredService<TrayNotifierService>();

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
            var host = ServicesHost;
            base.OnStartup(e);

            await host.StartAsync().ConfigureAwait(false);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            var host = ServicesHost;
            _trayNotifier?.Dispose();
            _mutex?.ReleaseMutex();
            base.OnExit(e);

            await host.StopAsync().ConfigureAwait(false);
            _servicesHost = null;
        }

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            //Services
            services.AddScoped<PeriodWatcherService>();
            services.AddScoped<SettingsService>();
            services.AddScoped<DialogService>();
            services.AddScoped<TrayNotifierService>();
            services.AddTransient<CurrentTimeService>();

            //ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<OtherSettingsPageViewModel>();
            services.AddTransient<ProcessPageViewModel>();
            services.AddTransient<SettingsMainPageViewModel>();
            services.AddTransient<SettingsWindowViewModel>();
            services.AddTransient<TrayMenuViewModel>();
        }
    }
}