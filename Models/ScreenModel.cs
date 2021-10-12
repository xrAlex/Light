using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Light.Infrastructure;
using Light.Services;
using Light.Templates.Entities;
using Light.WinApi;

namespace Light.Models
{
    internal sealed class ScreenModel
    {
        #region Fields

        public ObservableCollection<ScreenEntity> Screens { get; }
        private readonly SettingsService _settings; 

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
        public ScreenEntity GetScreen(int screenIndex) => Screens[screenIndex];

        public void SetDayPeriod(ScreenEntity screen)
        {
            screen.IsDayTimePeriod = true;
            ApplyColorConfiguration(screen);
        }

        public void SetNightPeriod(ScreenEntity screen)
        {
            screen.IsDayTimePeriod = false;
            ApplyColorConfiguration(screen);
        }

        /// <summary>
        /// Метод проверяет для устройства отображения развернуто ли окно на переднем плане во весь экран
        /// </summary>
        /// <returns> true если окно работает в полноэкранном режиме </returns>
        public bool IsFullScreenProcessFounded(ScreenEntity screen)
        {
            var handle = Native.GetForegroundWindow();
            if (!SystemWindow.IsWindowValid(handle) || !SystemWindow.IsWindowOnFullScreen(screen, handle)) return false;

            var pId = SystemProcess.GetId(handle);
            using var process = SystemProcess.TryOpenProcess(pId);
            var processPath = process?.TryGetProcessPath();
            var processFileName = Path.GetFileNameWithoutExtension(processPath);

            return _settings.IgnoredApplications.Count == 0 || _settings.IgnoredApplications.All(p => p != processFileName);
        }

        public void ApplyColorConfiguration(ScreenEntity screen)
        {
            var configuration = screen.ColorConfiguration;
            double brightness;
            int colorTemperature;
            if (screen.IsDayTimePeriod)
            {
                colorTemperature = configuration.DayColorTemperature;
                brightness = configuration.DayBrightness;
            }
            else
            {
                colorTemperature = configuration.NightColorTemperature;
                brightness = configuration.NightBrightness;
            }
            GammaRegulator.ApplyColorConfiguration(colorTemperature, brightness, screen.SysName);
            screen.ColorConfiguration.CurrentColorTemperature = colorTemperature;
            screen.ColorConfiguration.CurrentBrightness = brightness;
        }

        public void ApplyColorConfiguration(ScreenEntity screen, int colorTemperature, double brightness)
        {
            var configuration = screen.ColorConfiguration;

            colorTemperature = colorTemperature == -1 ? configuration.CurrentColorTemperature : colorTemperature;
            brightness = brightness == -1 ? configuration.CurrentBrightness : brightness;

            GammaRegulator.ApplyColorConfiguration(colorTemperature, brightness, screen.SysName);
            screen.ColorConfiguration.CurrentColorTemperature = colorTemperature;
            screen.ColorConfiguration.CurrentBrightness = brightness;
        }

        public static void SetDefaultColorTemperatureOnAllScreens()
        {
            var screens = ServiceLocator.Settings.Screens;
            foreach (var screen in screens)
            {
                GammaRegulator.ApplyColorConfiguration(6600, 1f, screen.SysName);
            }
        }

        public void ForceColorTemperature()
        {
            var workTime = new WorkTime();
            foreach (var screen in Screens)
            {
                if (!screen.IsActive) continue;

                if (workTime.IsDayPeriod(screen))
                {
                    SetNightPeriod(screen);
                }
                else
                {
                    SetDayPeriod(screen);
                }
            }
        }

        #endregion

        public ScreenModel()
        {
            _settings = ServiceLocator.Settings;
            Screens = _settings.Screens;
        }

#if DEBUG
        ~ScreenModel()
        {
            DebugConsole.Print("[Model] ScreenModel Disposed");
        }
#endif
    }
}