using System.Collections.ObjectModel;
using System.Windows.Forms;
using Light.Infrastructure;
using Light.Models.Entities;
using Light.Services;

namespace Light.Models
{
    public class ScreenModel
    {
        #region Fields

        public ObservableCollection<ScreenEntity> Screens { get; }

        #endregion

        #region Values

        private const int Hour = 60;

        #endregion

        #region Methods

        public void SetWorkTimeStart(int hour, int min, int screenIndex) => GetScreen(screenIndex).StartTime = hour * Hour + min;

        public void SetWorkTimeEnd(int hour, int min, int screenIndex) => GetScreen(screenIndex).EndTime = hour * Hour + min;

        public int GetStartHour(int screenIndex) => GetScreen(screenIndex).StartTime / Hour;

        public int GetEndHour(int screenIndex) => GetScreen(screenIndex).EndTime / Hour;

        public int GetStartMin(int screenIndex) => GetScreen(screenIndex).StartTime % Hour;

        public int GetEndMin(int screenIndex) => GetScreen(screenIndex).EndTime % Hour;

        public void ChangeScreenActivity(int screenIndex) => Screens[screenIndex].IsActive = !Screens[screenIndex].IsActive;

        private ScreenEntity GetScreen(int screenIndex) => Screens[screenIndex];

        public void SetDayTemperature(ScreenEntity screen) => ApplyColorConfiguration(screen.DayColorTemperature, screen.DayBrightness,screen);
        public void SetNightTemperature(ScreenEntity screen) => ApplyColorConfiguration(screen.NightColorTemperature, screen.NightBrightness, screen);

        public void SetDayColorTemperature(int colorTemperature, int screenIndex)
        {
            var screen = GetScreen(screenIndex);
            ApplyColorConfiguration(colorTemperature, screen.DayBrightness, screen);
            screen.DayColorTemperature = colorTemperature;
        }

        public void SetNightColorTemperature(int colorTemperature, int screenIndex)
        {
            var screen = GetScreen(screenIndex);
            ApplyColorConfiguration(colorTemperature, screen.NightBrightness, screen);
            screen.NightColorTemperature = colorTemperature;
        }

        public void SetDayBrightness(float brightness, int screenIndex)
        {
            var screen = GetScreen(screenIndex);
            ApplyColorConfiguration(screen.DayColorTemperature, brightness, screen);
            screen.DayBrightness = brightness;
        }

        public void SetNightBrightness(float brightness, int screenIndex)
        {
            var screen = GetScreen(screenIndex);
            ApplyColorConfiguration(screen.NightColorTemperature, brightness, screen);
            screen.NightBrightness = brightness;
        }


        private void ApplyColorConfiguration(int colorTemperature, float brightness, ScreenEntity screen)
        {
            ColorTemperatureRegulator.ApplyColorTemperature(colorTemperature, brightness, screen.SysName);
            screen.CurrentColorTemperature = colorTemperature;
            screen.CurrentBrightness = brightness;
        }


        public void ForceDayTemperatureOnScreens()
        {
            foreach (var screen in Screens)
            {
                if (screen.IsActive)
                {
                    SetDayTemperature(screen);
                }
            }
        }

        public void ForceNightTemperatureOnScreens()
        {
            foreach (var screen in Screens)
            {
                if (screen.IsActive)
                {
                    SetNightTemperature(screen);
                }
            }
        }

        public void SetDefaultColorTemperatureOnAllScreens()
        {
            foreach (var screen in Screen.AllScreens)
            {
                ColorTemperatureRegulator.ApplyColorTemperature(6600,1f, screen.DeviceName);
            }
        }

        public void ForceColorTemperature()
        {
            var workTime = new WorkTime();
            foreach (var screen in Screens)
            {
                if (!screen.IsActive) continue;

                if (workTime.IsNightTemperatureTime(screen))
                {
                    SetNightTemperature(screen);
                }
                else
                {
                    SetDayTemperature(screen);
                }
            }
        }

        #endregion

        public ScreenModel()
        {
            var serviceLocator = ServiceLocator.Source;
            var settingsService = serviceLocator.Settings;
            Screens = settingsService.Screens;
        }
    }
}