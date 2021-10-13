using Light.Models;
using Light.Services.Interfaces;
using Light.ViewModels.Base;

namespace Light.ViewModels
{
    internal sealed class OtherSettingsPageViewModel : ViewModelBase
    {
        private readonly RegistryModel _registryModel;
        private readonly ITrayNotifierService _trayNotifierService;

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
                _trayNotifierService.ShowTip(Localization.LangDictionary.GetString("Loc_RestartNotification"));
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

        public OtherSettingsPageViewModel(ITrayNotifierService trayNotifier)
        {
            _trayNotifierService = trayNotifier;
            _registryModel = new RegistryModel();
        }

#if DEBUG
        ~OtherSettingsPageViewModel()
        {
            DebugConsole.Print("[View Model] OtherSettingsPageViewModel Disposed");
        }
#endif
    }
}
