using System.CodeDom.Compiler;
using Light.Models;
using Light.Services;
using Light.ViewModels.Base;

namespace Light.ViewModels
{
    internal sealed class OtherSettingsPageViewModel : ViewModelBase
    {
        private readonly RegistryModel _registryModel;
        private readonly SettingsService _settings;
        private readonly TrayNotifierService _trayNotifier;

        public int SelectedLangIndex
        {
            get => _settings.SelectedLang;
            set
            {
                _settings.SelectedLang = value;
                Localization.LangDictionary.SetLang(value);
            }
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
                _trayNotifier.ShowTip(Localization.LangDictionary.GetString("Loc_RestartNotification"));
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
            _trayNotifier = ServiceLocator.TrayNotifier;
            _settings = ServiceLocator.Settings;
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
