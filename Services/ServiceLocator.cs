using System;

namespace Light.Services
{
    public class ServiceLocator
    {
        private GammaWatcherService _gammaWatcher;
        private SettingsService _settings;
        private WindowService _windowService;
        private CurrentTimeService _currentTimeService;

        public SettingsService Settings => _settings ??= new();
        public GammaWatcherService GammaWatcher => _gammaWatcher ??= new();

        public WindowService WindowService => _windowService ??= new();

        public CurrentTimeService CurrentTimeService => _currentTimeService ??= new();

        private ServiceLocator() {}
        private static readonly Lazy<ServiceLocator> Lazy = new(() => new ServiceLocator());
        public static ServiceLocator Source => Lazy.Value;
    }
}
