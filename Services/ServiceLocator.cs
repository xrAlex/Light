using System;

namespace Light.Services
{
    /// <summary>
    /// Storage of objects that must be created in one instance
    /// </summary>
    /// <remarks> [Need to replace with DI container] </remarks>
    public class ServiceLocator
    {
        private PeriodWatcherService _periodWatcherService;
        private SettingsService _settings;
        private CurrentTimeService _currentTimeService;
        private DialogService _dialogService;
        private TrayNotifierService _trayNotifier;
        
        public SettingsService Settings => _settings ??= new SettingsService();
        public PeriodWatcherService PeriodWatcherService => _periodWatcherService ??= new PeriodWatcherService();
        public CurrentTimeService CurrentTimeService => _currentTimeService ??= new CurrentTimeService();
        public DialogService DialogService => _dialogService ??= new DialogService();
        public TrayNotifierService TrayNotifier => _trayNotifier ??= new TrayNotifierService();

        private ServiceLocator() {}
        private static readonly Lazy<ServiceLocator> Lazy = new(() => new ServiceLocator());
        public static ServiceLocator Source => Lazy.Value;
    }
}
