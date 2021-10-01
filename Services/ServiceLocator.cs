#region

using System;

#endregion

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
        
        public SettingsService Settings => _settings ??= new();
        public PeriodWatcherService PeriodWatcherService => _periodWatcherService ??= new();
        public CurrentTimeService CurrentTimeService => _currentTimeService ??= new();
        public DialogService DialogService => _dialogService ??= new();

        private ServiceLocator() {}
        private static readonly Lazy<ServiceLocator> Lazy = new(() => new ServiceLocator());
        public static ServiceLocator Source => Lazy.Value;
    }
}
