using Light.Commands;
using Light.Models;
using Light.Templates.Entities;
using Light.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Light.Services.Interfaces;


namespace Light.ViewModels
{
    internal sealed class SettingsMainPageViewModel : ViewModelBase
    {
        #region Methods

        private void RefreshUI()
        {
            OnPropertyChanged("DayColorTemperature");
            OnPropertyChanged("DayBrightness");
            OnPropertyChanged("NightColorTemperature");
            OnPropertyChanged("NightBrightness");
            OnPropertyChanged("HourStart");
            OnPropertyChanged("MinStart");
            OnPropertyChanged("HourEnd");
            OnPropertyChanged("MinEnd");
        }

        #endregion

        #region Fields

        public ObservableCollection<ScreenEntity> Screens { get; }
        private readonly ISettingsService _settingsService;
        private readonly ScreenModel _screenModel;
        private readonly RegistryModel _registryModel;

        #endregion

        #region Properties

        public int MinGammaRange => _registryModel.IsExtendedGammaRangeActive() ? 1000 : 4200;
        public int MinBrightnessRange => _registryModel.IsExtendedGammaRangeActive() ? 10 : 70;


        public int SelectedScreenIndex
        {
            get => _settingsService.SelectedScreen;
            set
            {
                if (value == -1) return;
                _settingsService.SelectedScreen = value;
                RefreshUI();
            }
        }

        private ScreenEntity SelectedScreen => _screenModel.GetScreen(SelectedScreenIndex);

        public int DayColorTemperature
        {
            get => SelectedScreen.ColorConfiguration.DayColorTemperature;
            set
            {
                SelectedScreen.ColorConfiguration.DayColorTemperature = value;
                _screenModel.ApplyColorConfiguration(SelectedScreen, value, -1);
                OnPropertyChanged();
            }
        }

        public double DayBrightness
        {
            get => SelectedScreen.ColorConfiguration.DayBrightness;
            set
            {
                SelectedScreen.ColorConfiguration.DayBrightness = value;
                _screenModel.ApplyColorConfiguration(SelectedScreen, -1, value);
                OnPropertyChanged();
            }
        }

        public int NightColorTemperature
        {
            get => SelectedScreen.ColorConfiguration.NightColorTemperature;
            set
            {
                SelectedScreen.ColorConfiguration.NightColorTemperature = value;
                _screenModel.ApplyColorConfiguration(SelectedScreen, value, -1);
                OnPropertyChanged();
            }
        }

        public double NightBrightness
        {
            get => SelectedScreen.ColorConfiguration.NightBrightness;
            set
            {
                SelectedScreen.ColorConfiguration.NightBrightness = value;
                _screenModel.ApplyColorConfiguration(SelectedScreen, -1, value);
                OnPropertyChanged();
            }
        }

        public int HourStart
        {
            get => _screenModel.GetStartHour(SelectedScreenIndex);
            set
            {
                _screenModel.SetWorkTimeStart(value, MinStart, SelectedScreenIndex);
                OnPropertyChanged();
            }
        }

        public int MinStart
        {
            get => _screenModel.GetStartMin(SelectedScreenIndex);
            set
            {
                _screenModel.SetWorkTimeStart(HourStart, value, SelectedScreenIndex);
                OnPropertyChanged();
            }
        }

        public int HourEnd
        {
            get => _screenModel.GetEndHour(SelectedScreenIndex);
            set
            {
                _screenModel.SetWorkTimeEnd(value, MinEnd, SelectedScreenIndex);
                OnPropertyChanged();
            }
        }

        public int MinEnd
        {
            get => _screenModel.GetEndMin(SelectedScreenIndex);
            set
            {
                _screenModel.SetWorkTimeEnd(HourEnd, value, SelectedScreenIndex);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand MonitorDoubleClickCommand { get; }

        private void OnMonitorDoubleClickCommandExecute() => _screenModel.ChangeScreenActivity(SelectedScreenIndex);

        #endregion

        public SettingsMainPageViewModel(ISettingsService settingsService)
        {
            MonitorDoubleClickCommand = new LambdaCommand(_ => OnMonitorDoubleClickCommandExecute());

            _settingsService = settingsService;
            _screenModel = new ScreenModel(settingsService);
            _registryModel = new RegistryModel();
            Screens = _settingsService.Screens;
            SelectedScreenIndex = _settingsService.SelectedScreen;
            Localization.LangDictionary.OnLocalizationChanged += (_, _) => { RefreshUI(); };
        }

#if DEBUG
        ~SettingsMainPageViewModel()
        {
            Logging.Write("[View Model] SettingsMainPageViewModel Disposed");
        }
#endif

    }
}