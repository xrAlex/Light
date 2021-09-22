using System.Collections.ObjectModel;
using System.Windows.Input;
using Light.Commands;
using Light.Models;
using Light.Models.Entities;
using Light.Services;
using Light.ViewModels.Base;

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
                DayColorTemperature = Screens[SelectedScreenIndex].DayColorTemperature;
                NightColorTemperature = Screens[SelectedScreenIndex].NightColorTemperature;
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

        public int DayColorTemperature
        {
            get => Screens[SelectedScreenIndex].DayColorTemperature;
            set
            {
                _screenModel.SetDayColorTemperature(value, SelectedScreenIndex);
                OnPropertyChanged();
            }
        }

        public float DayBrightness
        {
            get => Screens[SelectedScreenIndex].DayBrightness;
            set
            {
                _screenModel.SetDayBrightness(value, SelectedScreenIndex);
                OnPropertyChanged();
            }
        }

        public int NightColorTemperature
        {
            get => Screens[SelectedScreenIndex].NightColorTemperature;
            set
            {
                _screenModel.SetNightColorTemperature(value, SelectedScreenIndex);
                OnPropertyChanged();
            }
        }

        public float NightBrightness
        {
            get => Screens[SelectedScreenIndex].NightBrightness;
            set
            {
                _screenModel.SetNightBrightness(value, SelectedScreenIndex);
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