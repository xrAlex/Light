using Light.Models;
using Light.Services;
using Light.ViewModels.Base;

namespace Light.ViewModels
{
    internal sealed class OtherSettingsPageViewModel : ViewModelBase
    {
        private readonly RegistryModel _registryModel;
        private readonly SettingsService _settings;
 
        public bool StartMinimized
        {
            get => _settings.StartMinimized;
            set => _settings.StartMinimized = value;
        }

        public bool LaunchOnStartup
        {
            get => _registryModel.IsAppStartupKeyFounded();
            set
            {
                if (value)
                {
                    _registryModel.AddAppStartupKey();
                }
                else
                {
                    _registryModel.DeleteAppStartupKey();
                }
            }
        }

        public bool ExtendedGammaRange
        {
            get => _registryModel.IsExtendedGammaRangeActive();
            set
            {
                if (value)
                {
                    _registryModel.SetExtendedGammaRangeKey();
                }
                else
                {
                    _registryModel.SetDefaultGammaRangeKey();
                }
            }
        }

        public OtherSettingsPageViewModel()
        {
            var serviceLocator = ServiceLocator.Source;
            _settings = serviceLocator.Settings;
            _registryModel = new();
        }

#if DEBUG
        ~OtherSettingsPageViewModel()
        {
            DebugConsole.Print("OtherSettingsPageViewModel Disposed");
        }
#endif
    }
}
