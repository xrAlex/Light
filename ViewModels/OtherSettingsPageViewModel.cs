using Light.Infrastructure;
using Light.Models;
using Light.ViewModels.Base;

namespace Light.ViewModels
{
    internal sealed class OtherSettingsPageViewModel : ViewModelBase
    {
        private readonly RegistryModel _registryModel;
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
                UserNotifier.ShowTip("Loc_RestartNotification");
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
            _registryModel = new RegistryModel();
        }

#if DEBUG
        ~OtherSettingsPageViewModel()
        {
            Logging.Write("[View Model] OtherSettingsPageViewModel Disposed");
        }
#endif
    }
}
