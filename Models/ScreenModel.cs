using Light.Infrastructure;
using Light.Models.Entities;
using Light.Services;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Light.Models
{
    public class ScreenModel
    {
        #region Fields

        private readonly ServiceLocator _serviceLocator;
        private readonly SettingsService _settingsService;
        private readonly GammaRegulator _gammaRegulator;
        public ObservableCollection<ScreenEntity> Screens { get; private set; }

        #endregion

        #region Values

        private const int _hour = 60;
        private const float _defaultGamma = 192f;
        private const float _defaultBlueReduce = 1f;
        private const float BlueReduceMult = 0.01f;
        private const float GammaMult = 1.92f;

        #endregion

        #region Methods
        public void SetWorkTimeStart(int hour, int min, int screenIndex) => GetScreen(screenIndex).StartTime = hour * _hour + min;
        public void SetWorkTimeEnd(int hour, int min, int screenIndex) => GetScreen(screenIndex).EndTime = hour * _hour + min;
        public int GetStartHour(int screenIndex) => GetScreen(screenIndex).StartTime / _hour;
        public int GetEndHour(int screenIndex) => GetScreen(screenIndex).EndTime / _hour;
        public int GetStartMin(int screenIndex) => GetScreen(screenIndex).StartTime % _hour;
        public int GetEndMin(int screenIndex) => GetScreen(screenIndex).EndTime % _hour;
        public void ChangeScreenActivity(int screenIndex) => Screens[screenIndex].IsActive = !Screens[screenIndex].IsActive;
        public ScreenEntity GetScreen(int screenIndex) => Screens[screenIndex];

        private void ApplyGammaValues(float gamma = 0f, float blueReduce = 0f, ScreenEntity screen = null, bool forced = false)
        {
            if (screen != null && (screen.IsActive || forced == true))
            {
                var validatedGamma = gamma != 0f ? gamma : screen.CurrentGamma;
                var validatedBlueReduce = blueReduce != 0f ? blueReduce : screen.CurrentBlueReduce;

                _gammaRegulator.ApplyGamma(validatedGamma * GammaMult, validatedBlueReduce * BlueReduceMult, screen.SysName);
                screen.CurrentGamma = validatedGamma;
                screen.CurrentBlueReduce = validatedBlueReduce;
            }
        }

        public void SetGamma(float Gamma, ScreenEntity screen) => ApplyGammaValues(Gamma, 0f, screen);

        public void SetBlueReduce(float BlueReduce, ScreenEntity screen) => ApplyGammaValues(0f, BlueReduce, screen);

        public void SetUserGamma(float gamma, int screenIndex)
        {
            var screen = GetScreen(screenIndex);
            ApplyGammaValues(gamma, 0f, screen);
            screen.UserGamma = gamma;
        }

        public void SetUserBlueReduce(float blueReduce, int screenIndex)
        {
            var screen = GetScreen(screenIndex);
            ApplyGammaValues(0f, blueReduce, screen);
            screen.UserBlueReduce = blueReduce;
        }

        public void SetDefaultValues(ScreenEntity screen)
        {
            SetGamma(100f, screen);
            SetBlueReduce(100f, screen);
        }

        public void SetUserValues(ScreenEntity screen)
        {
            SetGamma(screen.UserGamma, screen);
            SetBlueReduce(screen.UserBlueReduce, screen);
        }

        public void ForceUserValuesOnScreens()
        {
            foreach (ScreenEntity screen in Screens)
            {
                if (screen.IsActive)
                {
                    SetUserValues(screen);
                }
            }
        }

        public void ForceDefaultValuesOnScreens()
        {
            foreach (ScreenEntity screen in Screens)
            {
                if (screen.IsActive)
                {
                    SetDefaultValues(screen);
                }
            }
        }

        public void SetDefaultGammaOnAllScreens()
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                _gammaRegulator.ApplyGamma(_defaultGamma, _defaultBlueReduce, screen.DeviceName);
            }
        }

        public void ForceGamma()
        {
            var workTime = new WorkTime();
            foreach (ScreenEntity screen in Screens)
            {
                if (screen.IsActive)
                {
                    if (workTime.IsWorkTime(screen))
                    {
                        SetUserValues(screen);
                    }
                    else
                    {
                        SetDefaultValues(screen);
                    }
                }
            }
        }

        #endregion

        public ScreenModel()
        {
            _gammaRegulator = new();
            _serviceLocator = ServiceLocator.Source;
            _settingsService = _serviceLocator.Settings;
            Screens = _settingsService.Screens;
        }
    }
}
