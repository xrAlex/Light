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

        public ObservableCollection<ScreenEntity> Screens { get; }
        private readonly ServiceLocator _services;
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
                OnPropertyChanged("SelectedScreenIndex");
                RefreshUI();
            }
        }

        public float Gamma
        {
            get => Screens[SelectedScreenIndex].CurrentGamma;
            set 
            {
                _screenModel.SetUserGamma(value, SelectedScreenIndex);
                OnPropertyChanged("Gamma");
            }

        }
        public float BlueReduce
        {
            get => Screens[SelectedScreenIndex].CurrentBlueReduce;
            set 
            {
                _screenModel.SetUserBlueReduce(value, SelectedScreenIndex);
                OnPropertyChanged("BlueReduce");
            }
        }

        public int HourStart
        {   
            get => _screenModel.GetStartHour(SelectedScreenIndex);
            set
            {
                _screenModel.SetWorkTimeStart(value, MinStart, SelectedScreenIndex);
                OnPropertyChanged("HourStart");
            }
        }
        public int MinStart
        {
            get => _screenModel.GetStartMin(SelectedScreenIndex);
            set
            {
                _screenModel.SetWorkTimeStart(HourStart, value, SelectedScreenIndex);
                OnPropertyChanged("MinStart");
            }
        }
        public int HourEnd
        {
            get => _screenModel.GetEndHour(SelectedScreenIndex);
            set
            {
                _screenModel.SetWorkTimeEnd(value, MinEnd, SelectedScreenIndex);
                OnPropertyChanged("HourEnd");
            }
        }
        public int MinEnd
        {
            get => _screenModel.GetEndMin(SelectedScreenIndex);
            set
            {
                _screenModel.SetWorkTimeEnd(HourEnd, value, SelectedScreenIndex);
                OnPropertyChanged("MinEnd");
            }
        }

        #endregion

        #region Commands
        public ICommand MonitorDoubleClickCommand { get; }

        private void OnMonitorDoubleClickCommandExecute() => _screenModel.ChangeScreenActivity(SelectedScreenIndex);

        #endregion

        #region Methods

        private void RefreshUI()
        {
            if (SelectedScreenIndex != -1)
            {
                HourStart = _screenModel.GetStartHour(SelectedScreenIndex);
                MinStart = _screenModel.GetStartMin(SelectedScreenIndex);
                HourEnd = _screenModel.GetEndHour(SelectedScreenIndex);
                MinEnd = _screenModel.GetEndMin(SelectedScreenIndex);
                Gamma = Screens[SelectedScreenIndex].UserGamma;
                BlueReduce = Screens[SelectedScreenIndex].UserBlueReduce;
            }
        }

        #endregion

        public SettingsMainPageViewModel()
        {
            MonitorDoubleClickCommand = new LambdaCommand(p => OnMonitorDoubleClickCommandExecute());

            _screenModel = new();
            _services = ServiceLocator.Source;
            _settings = _services.Settings;
            Screens = _screenModel.Screens;
            SelectedScreenIndex = _settings.SelectedScreen;
            _screenModel.ForceUserValuesOnScreens();
        }
    }
}
