#region

using System.Collections.ObjectModel;
using System.Windows.Input;
using Light.Commands;
using Light.Models;
using Light.Services;
using Light.Templates.Entities;
using Light.ViewModels.Base;

#endregion

namespace Light.ViewModels
{
    internal sealed class SettingsMainPageViewModel : ViewModelBase
    {
        #region Methods

        private void RefreshUI()
        {
            if (SelectedScreenIndex != -1)
            {
                HourStart = _screenModel.GetStartHour(SelectedScreenIndex);
                MinStart = _screenModel.GetStartMin(SelectedScreenIndex);
                HourEnd = _screenModel.GetEndHour(SelectedScreenIndex);
                MinEnd = _screenModel.GetEndMin(SelectedScreenIndex);
                DayColorTemperature = SelectedScreen.ColorConfiguration.DayColorTemperature;
                NightColorTemperature = SelectedScreen.ColorConfiguration.NightColorTemperature;
            }
        }

        #endregion

        #region Fields

        public ObservableCollection<ScreenEntity> Screens { get; }
        private readonly SettingsService _settings;
        private readonly ScreenModel _screenModel;

        #endregion

        #region Properties

        public int SelectedScreenIndex
        {
            get => _settings.SelectedScreen;
            set
            {
                _settings.SelectedScreen = value;
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
                _screenModel.ApplyColorConfiguration(SelectedScreen, value, -1f);
                OnPropertyChanged();
            }
        }

        public float DayBrightness
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
                _screenModel.ApplyColorConfiguration(SelectedScreen, value, -1f);
                OnPropertyChanged();
            }
        }

        public float NightBrightness
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

        public SettingsMainPageViewModel()
        {
            MonitorDoubleClickCommand = new LambdaCommand(p => OnMonitorDoubleClickCommandExecute());

            _screenModel = new ScreenModel();
            var services = ServiceLocator.Source;
            _settings = services.Settings;
            Screens = _screenModel.Screens;
            SelectedScreenIndex = _settings.SelectedScreen;
            _screenModel.ForceColorTemperature();
        }
    }
}