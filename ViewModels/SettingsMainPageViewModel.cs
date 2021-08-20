using Light.Commands;
using Light.Infrastructure;
using Light.Models;
using Light.Services;
using Light.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Light.ViewModels
{
    internal sealed class SettingsMainPageViewModel : ViewModelBase
    {
        #region Fields

        public ObservableCollection<ScreenModel> Screens { get; private set; }
        private readonly ServiceLocator _services;
        private readonly GammaRegulatorService _gammaRegulator;
        private readonly SettingsService _settings;

        #endregion

        #region Values

        private int _selectedScreen = 0;

        #endregion

        #region Properties

        public int SelectedScreen
        {
            get => _selectedScreen;
            set
            {
                _selectedScreen = value;
                ShowMonitorSettings(value);
            }
        }

        public int HourStart
        {
            get => Screens[SelectedScreen].HourStart;
            set
            {
                Screens[SelectedScreen].HourStart = value;
                OnPropertyChanged("HourStart");
            }
        }
        public int MinStart
        {
            get => Screens[SelectedScreen].MinStart;
            set
            {
                Screens[SelectedScreen].MinStart = value;
                OnPropertyChanged("MinStart");
            }
        }
        public int HourEnd
        {
            get => Screens[SelectedScreen].HourEnd;
            set
            {
                Screens[SelectedScreen].HourEnd = value;
                OnPropertyChanged("HourEnd");
            }
        }
        public int MinEnd
        {
            get => Screens[SelectedScreen].MinEnd;
            set
            {
                Screens[SelectedScreen].MinEnd = value;
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

        private void OnMonitorDoubleClickCommandExecute(object screen)
        {
            var findIndex = new FindIndexByName();
            int screenIndex = findIndex.GetIndex(screen.ToString());

            Screens[screenIndex].IsActive = !Screens[screenIndex].IsActive;
        }
        #endregion

        #region Methods
        public void Monitors_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (args.AddedItems.Count > 0)
            {
                var findIndex = new FindIndexByName();
                int screenIndex = findIndex.GetIndex(args.AddedItems[0].ToString());

                SelectedScreen = screenIndex;
            }
        }

        private void ShowMonitorSettings(int monitorIndex)
        {
            var screen = Screens[monitorIndex];
            HourStart = screen.HourStart;
            MinStart = screen.MinStart;
            HourEnd = screen.HourEnd;
            MinEnd = screen.MinEnd;
            Gamma = screen.UserGamma;
            BlueReduce = screen.UserBlueReduce;
        }

        #endregion

        public SettingsMainPageViewModel()
        {
            MonitorDoubleClickCommand = new LambdaCommand(OnMonitorDoubleClickCommandExecute);

            _services = ServiceLocator.Source;
            _settings = _services.Settings;
            _gammaRegulator = _services.GammaRegulator;
            Screens = _settings.Screens;
        }
    }
}
