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

        private readonly GammaRegulator _gammaRegulator;
        public ObservableCollection<ScreenEntity> Screens { get; }

        #endregion

        #region Values

        private const int Hour = 60;
        private const float DefaultGamma = 192f;
        private const float DefaultBlueReduce = 1f;
        private const float BlueReduceMult = 0.01f;
        private const float GammaMult = 1.92f;

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


        private void ApplyGammaValues(float gamma = 0f, float blueReduce = 0f, ScreenEntity screen = null, bool forced = false)
        {
            if (screen == null || (!screen.IsActive && !forced)) return;

            var validatedGamma = gamma != 0f ? gamma : screen.CurrentGamma;
            var validatedBlueReduce = blueReduce != 0f ? blueReduce : screen.CurrentBlueReduce;

            _gammaRegulator.ApplyGamma(validatedGamma * GammaMult, validatedBlueReduce * BlueReduceMult, screen.SysName);
            screen.CurrentGamma = validatedGamma;
            screen.CurrentBlueReduce = validatedBlueReduce;
        }

        private void SetGamma(float gamma, ScreenEntity screen)
        {
            ApplyGammaValues(gamma, 0f, screen);
        }

        private void SetBlueReduce(float blueReduce, ScreenEntity screen)
        {
            ApplyGammaValues(0f, blueReduce, screen);
        }

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
            foreach (var screen in Screens)
            {
                if (screen.IsActive)
                {
                    SetUserValues(screen);
                }
            }
        }

        public void ForceDefaultValuesOnScreens()
        {
            foreach (var screen in Screens)
            {
                if (screen.IsActive)
                {
                    SetDefaultValues(screen);
                }
            }
        }

        public void SetDefaultGammaOnAllScreens()
        {
            foreach (var screen in Screen.AllScreens)
            {
                _gammaRegulator.ApplyGamma(DefaultGamma, DefaultBlueReduce, screen.DeviceName);
            }
        }

        public void ForceGamma()
        {
            var workTime = new WorkTime();
            foreach (var screen in Screens)
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
            _gammaRegulator = new GammaRegulator();
            var serviceLocator = ServiceLocator.Source;
            var settingsService = serviceLocator.Settings;
            Screens = settingsService.Screens;
        }
    }
}