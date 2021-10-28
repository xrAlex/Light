using Sparky.Models;
using Sparky.Services.Interfaces;
using Sparky.ViewModels.Base;

namespace Sparky.ViewModels
{
    internal sealed class OtherSettingsPageViewModel : ViewModelBase
    {
        private readonly RegistryModel _registryModel;
        private readonly ISettingsService _settingsService;
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

        public bool SmoothGammaChange
        {
            get => _settingsService.SmoothGammaChange;
            set => _settingsService.SmoothGammaChange = value;
        }

        public OtherSettingsPageViewModel(ISettingsService settingsService)
        {
            _registryModel = new RegistryModel();
            _settingsService = settingsService;
        }

        public OtherSettingsPageViewModel() {}

#if DEBUG
        ~OtherSettingsPageViewModel()
        {
            LoggingModule.Log.Verbose("[View Model] OtherSettingsPageViewModel Disposed");
        }
#endif
    }
}
