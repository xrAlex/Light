using System;

namespace Light.Services
{
    public class ServiceLocator
    {
        // TODO: Di Container

        private ColorTemperatureWatcherService _colorTemperatureWatcher;
        private SettingsService _settings;
        private WindowService _windowService;
        private CurrentTimeService _currentTimeService;

        public SettingsService Settings => _settings ??= new();
        public ColorTemperatureWatcherService ColorTemperatureWatcher => _colorTemperatureWatcher ??= new();

        public WindowService WindowService => _windowService ??= new();

        public CurrentTimeService CurrentTimeService => _currentTimeService ??= new();

        private ServiceLocator() {}
        private static readonly Lazy<ServiceLocator> Lazy = new(() => new ServiceLocator());
        public static ServiceLocator Source => Lazy.Value;
    }
}
