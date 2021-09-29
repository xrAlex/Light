#region

using System;

#endregion

namespace Light.Services
{
    /// <summary>
    /// Класс представляет из себя хранилище объектов которые должны быть созданы в одном экземпляре
    /// </summary>
    /// <remarks> [Нужно заменить на DI контeйнер] </remarks>
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
