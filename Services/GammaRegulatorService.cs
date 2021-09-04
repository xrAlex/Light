using Light.Infrastructure;
using Light.Models;
using System.Windows.Forms;

namespace Light.Services
{
    public class GammaRegulatorService
    {
        private readonly GammaRegulator _gammaRegulator;
        private readonly ServiceLocator _serviceLocator;
        private readonly SettingsService _settingsService;

        private ScreenModel GetScreen(int screenIndex) => _settingsService.Screens[screenIndex];
        private void Apply(float gamma = 0f, float blueReduce = 0f, int screenIndex = 1)
        {
            const float BlueReduceMult = 0.01f;
            const float GammaMult = 1.92f;
            var screen = GetScreen(screenIndex);
            var validatedGamma = gamma != 0f ? gamma : screen.CurrentGamma;
            var validatedBlueReduce = blueReduce != 0f ? blueReduce : screen.CurrentBlueReduce;

            _gammaRegulator.ApplyGamma(validatedGamma * GammaMult, validatedBlueReduce * BlueReduceMult, screen.SysName);
            screen.CurrentGamma = validatedGamma;
            screen.CurrentBlueReduce = validatedBlueReduce;
        }

        public void SetGamma(float Gamma, int screenIndex) => Apply(Gamma, 0f, screenIndex);

        public void SetBlueReduce(float BlueReduce, int screenIndex) => Apply(0f, BlueReduce, screenIndex);

        public void SetUserGamma(float Gamma, int screenIndex)
        {
            var screen = GetScreen(screenIndex);
            Apply(Gamma, 0f, screenIndex);
            screen.UserGamma = Gamma;
        }

        public void SetUserBlueReduce(float BlueReduce, int screenIndex)
        {
            var screen = GetScreen(screenIndex);
            Apply(0f, BlueReduce, screenIndex);
            screen.UserBlueReduce = BlueReduce;
        }

        public float GetGamma(int screenIndex)
        {
            var screen = GetScreen(screenIndex);
            return screen.CurrentGamma;
        }

        public float GetBlueReduce(int screenIndex)
        {
            var screen = GetScreen(screenIndex);
            return screen.CurrentBlueReduce;
        }

        public void SetDefaultValues(int screenIndex)
        {
            SetGamma(100f, screenIndex);
            SetBlueReduce(100f, screenIndex);
        }

        public void SetUserValues(int screenIndex)
        {
            var screen = _settingsService.Screens[screenIndex];
            SetGamma(screen.UserGamma, screenIndex);
            SetBlueReduce(screen.UserBlueReduce, screenIndex);
        }

        public void ForceUserValuesOnScreens()
        {
            var screens = _settingsService.Screens;
            for (int i = 0; i < screens.Count; i++)
            {
                SetUserValues(i);
            }
        }

        public void ForceDefaultValuesOnScreens()
        {
            var screens = _settingsService.Screens;
            for (int i = 0; i < screens.Count; i++)
            {
                SetDefaultValues(i);
            }
        }

        public void SetDefaultGammaOnAllScreens()
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                _gammaRegulator.ApplyGamma(192f, 1f, screen.DeviceName);
            }
        }

        public void ForceGamma()
        {
            for (int i = 0; i < _settingsService.Screens.Count; i++)
            {
                var workTime = new WorkTime();

                if (workTime.IsWorkTime(i))
                {
                    SetUserValues(i);
                }
                else
                {
                    SetDefaultValues(i);
                }
            }
        }

        public GammaRegulatorService()
        {
            _gammaRegulator = new();
            _serviceLocator = ServiceLocator.Source;
            _settingsService = _serviceLocator.Settings;
        }
    }
}
