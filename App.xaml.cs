using System;
using System.Linq;
using System.Threading;
using System.Windows;
using Light.Services;
using Light.Services.Interfaces;
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
            dialogService.Register<TrayMenuViewModel, TrayMenuView>();

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
            await ServicesHost.StartAsync().ConfigureAwait(false);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            ServicesHost.Services.GetRequiredService<IPeriodWatcherService>().StopWatch();
            ServicesHost.Services.GetRequiredService<ITrayNotifierService>().Dispose();
            _mutex?.ReleaseMutex();
            base.OnExit(e);

            await ServicesHost.StopAsync().ConfigureAwait(false);
            _servicesHost = null;
        }

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            //Services
            services.AddSingleton<IPeriodWatcherService, PeriodWatcherService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<ITrayNotifierService, TrayNotifierService>();
            services.AddTransient<ICurrentTimeService, CurrentTimeService>();

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