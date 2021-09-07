using Light.Commands;
using Light.Models;
using Light.Models.Entities;
using Light.Services;
using Light.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Light.ViewModels
{
    internal sealed class SettingsMainPageViewModel : ViewModelBase
    {
        #region Fields

        public ObservableCollection<ScreenEntity> Screens { get; private set; }
        private readonly ServiceLocator _services;
        private readonly GammaRegulatorService _gammaRegulator;
        private readonly SettingsService _settings;
        private readonly ScreenModel _screenModel;

        #endregion

        #region Properties 

        public int SelectedScreen
        {
            get => _settings.SelectedScreen;
            set
            {
                _settings.SelectedScreen = value;
                OnPropertyChanged("SelectedScreen");
                RefreshUI();
            }
        }

        public int HourStart
        {
            get => _screenModel.GetStartHour(SelectedScreen);
            set
            {
                _screenModel.SetWorkTimeStart(value, MinStart, SelectedScreen);
                OnPropertyChanged("HourStart");
            }
        }
        public int MinStart
        {
            get => _screenModel.GetStartMin(SelectedScreen);
            set
            {
                _screenModel.SetWorkTimeStart(HourStart, value, SelectedScreen);
                OnPropertyChanged("MinStart");
            }
        }
        public int HourEnd
        {
            get => _screenModel.GetEndHour(SelectedScreen);
            set
            {
                _screenModel.SetWorkTimeEnd(value, MinEnd, SelectedScreen);
                OnPropertyChanged("HourEnd");
            }
        }
        public int MinEnd
        {
            get => _screenModel.GetEndMin(SelectedScreen);
            set
            {
                _screenModel.SetWorkTimeEnd(HourEnd, value, SelectedScreen);
                OnPropertyChanged("MinEnd");
            }
        }
        public float Gamma
        {
            get => _gammaRegulator.GetGamma(SelectedScreen);
            set
            {
                _gammaRegulator.SetUserGamma(value, SelectedScreen);
                OnPropertyChanged("Gamma");
            }
        }
        public float BlueReduce
        {
            get => _gammaRegulator.GetBlueReduce(SelectedScreen);
            set
            {
                _gammaRegulator.SetUserBlueReduce(value, SelectedScreen);
                OnPropertyChanged("BlueReduce");
            }
        }

        #endregion

        #region Commands
        public ICommand MonitorDoubleClickCommand { get; }

        private void OnMonitorDoubleClickCommandExecute(object screenIndex)
        {
            var index = (int)screenIndex;
            Screens[index].IsActive = !Screens[index].IsActive;
        }

        #endregion

        #region Methods

        private void RefreshUI()
        {
            if (SelectedScreen != -1)
            {
                HourStart = _screenModel.GetStartHour(SelectedScreen);
                MinStart = _screenModel.GetStartMin(SelectedScreen);
                HourEnd = _screenModel.GetEndHour(SelectedScreen);
                MinEnd = _screenModel.GetEndMin(SelectedScreen);
                Gamma = Screens[SelectedScreen].UserGamma;
                BlueReduce = Screens[SelectedScreen].UserBlueReduce;
            }
        }

        #endregion

        public SettingsMainPageViewModel()
        {
            MonitorDoubleClickCommand = new LambdaCommand(OnMonitorDoubleClickCommandExecute);

            _screenModel = new();
            _services = ServiceLocator.Source;
            _settings = _services.Settings;
            _gammaRegulator = _services.GammaRegulator;
            Screens = _screenModel.Screens;

            SelectedScreen = _settings.SelectedScreen;
            _gammaRegulator.ForceUserValuesOnScreens();
        }
    }
}
