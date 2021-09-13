using System;

namespace Light.Services
{
    public class ServiceLocator
    {
        private SettingsService _settings;
        private WindowService _windowService;
        private CurrentTimeService _currentTimeService;

        public SettingsService Settings => _settings ??= new();

        public WindowService WindowService => _windowService ??= new();

        public CurrentTimeService CurrentTimeService => _currentTimeService ??= new();

        private ServiceLocator() {}
        private static readonly Lazy<ServiceLocator> lazy = new(() => new ServiceLocator());
        public static ServiceLocator Source { get { return lazy.Value; } }
    }
}
