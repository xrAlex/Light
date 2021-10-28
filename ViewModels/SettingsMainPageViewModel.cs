using Sparky.Commands;
using Sparky.Models;
using Sparky.Templates.Entities;
using Sparky.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Sparky.Services.Interfaces;


namespace Sparky.ViewModels
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
            OnPropertyChanged("NightStartHour");
            OnPropertyChanged("NightStartMinute");
            OnPropertyChanged("DayStartHour");
            OnPropertyChanged("DayStartMinute");
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

        public double DayColorTemperature
        {
            get => SelectedScreen.DayColorConfiguration.ColorTemperature;
            set
            {
                var colorConfiguration = new ColorConfiguration(value, DayBrightness);
                SelectedScreen.DayColorConfiguration = colorConfiguration;
                _screenModel.ApplyColorConfiguration(SelectedScreen, colorConfiguration);
                OnPropertyChanged();
            }
        }

        public double DayBrightness
        {
            get => SelectedScreen.DayColorConfiguration.Brightness;
            set
            {
                var colorConfiguration = new ColorConfiguration(DayColorTemperature, value);
                SelectedScreen.DayColorConfiguration = colorConfiguration;
                _screenModel.ApplyColorConfiguration(SelectedScreen, colorConfiguration);
                OnPropertyChanged();
            }
        }

        public double NightColorTemperature
        {
            get => SelectedScreen.NightColorConfiguration.ColorTemperature;
            set
            {
                var colorConfiguration = new ColorConfiguration(value, NightBrightness);
                SelectedScreen.NightColorConfiguration = colorConfiguration;
                _screenModel.ApplyColorConfiguration(SelectedScreen, colorConfiguration);
                OnPropertyChanged();
            }
        }

        public double NightBrightness
        {
            get => SelectedScreen.NightColorConfiguration.Brightness;
            set
            {
                var colorConfiguration = new ColorConfiguration(NightColorTemperature, value);
                SelectedScreen.NightColorConfiguration = colorConfiguration;
                _screenModel.ApplyColorConfiguration(SelectedScreen, colorConfiguration);
                OnPropertyChanged();
            }
        }

        public int NightStartHour
        {
            get => SelectedScreen.NightStartTime.Hour;
            set
            {
                SelectedScreen.NightStartTime = new StartTime(value, NightStartMinute);
                OnPropertyChanged();
            }
        }

        public int NightStartMinute
        {
            get => SelectedScreen.NightStartTime.Minute;
            set
            {
                SelectedScreen.NightStartTime = new StartTime(NightStartHour, value);
                OnPropertyChanged();
            }
        }

        public int DayStartHour
        {
            get => SelectedScreen.DayStartTime.Hour;
            set
            {
                SelectedScreen.DayStartTime = new StartTime(value, NightStartMinute);
                OnPropertyChanged();
            }
        }

        public int DayStartMinute
        {
            get => SelectedScreen.DayStartTime.Minute;
            set
            {
                SelectedScreen.DayStartTime = new StartTime(DayStartHour, value);
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

        public SettingsMainPageViewModel() {}

#if DEBUG
        ~SettingsMainPageViewModel()
        {
            LoggingModule.Log.Verbose("[View Model] SettingsMainPageViewModel Disposed");
        }
#endif

    }
}