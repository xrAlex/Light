using Light.Models.Entities;
using Light.Services;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Light.Models
{
    public class ScreenModel
    {
        private readonly ServiceLocator _serviceLocator;
        private readonly SettingsService _settingsService;
        private const int _hour = 60;
        public ObservableCollection<ScreenEntity> Screens { get; set; }

        public void SetWorkTimeStart(int hour, int min, int screenIndex) => GetScreen(screenIndex).StartTime = hour * _hour + min;
        public void SetWorkTimeEnd(int hour, int min, int screenIndex) => GetScreen(screenIndex).EndTime = hour * _hour + min;
        public int GetStartHour(int screenIndex) => GetScreen(screenIndex).StartTime / _hour;
        public int GetEndHour(int screenIndex) => GetScreen(screenIndex).EndTime / _hour;
        public int GetStartMin(int screenIndex) => GetScreen(screenIndex).StartTime % _hour;
        public int GetEndMin(int screenIndex) => GetScreen(screenIndex).EndTime % _hour;
        public ScreenEntity GetScreen(int screenIndex) => Screens[screenIndex]; 

        public ScreenModel()
        {
            _serviceLocator = ServiceLocator.Source;
            _settingsService = _serviceLocator.Settings;
            Screens = _settingsService.Screens;
        }
    }
}
