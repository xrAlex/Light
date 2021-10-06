using System;
using Microsoft.Extensions.DependencyInjection;

namespace Light.Services
{
    /// <summary>
    /// Storage of objects that must be created in one instance
    /// </summary>
    public static class ServiceLocator
    {
        public static SettingsService Settings => App.ServicesHost.Services.GetRequiredService<SettingsService>();
        public static PeriodWatcherService PeriodWatcherService => App.ServicesHost.Services.GetRequiredService<PeriodWatcherService>();
        public static CurrentTimeService CurrentTimeService => App.ServicesHost.Services.GetRequiredService<CurrentTimeService>();
        public static DialogService DialogService => App.ServicesHost.Services.GetRequiredService<DialogService>();
        public static TrayNotifierService TrayNotifier => App.ServicesHost.Services.GetRequiredService<TrayNotifierService>();
    }
}
