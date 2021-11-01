using System.Collections.ObjectModel;
using Sparky.Services.Interfaces;
using Sparky.Templates.Entities;
using Ninject;
using Sparky.Services;

namespace Sparky.Models
{
    internal sealed class ScreenModel
    {
        #region Fields

        private readonly ObservableCollection<ScreenEntity> _screens;

        #endregion

        #region Values

        private const int Hour = 60;

        #endregion

        #region Methods
        public void ChangeScreenActivity(int screenIndex) => _screens[screenIndex].IsActive = !_screens[screenIndex].IsActive;
        public ScreenEntity GetScreen(int screenIndex) => _screens[screenIndex];

        public void ApplyColorConfiguration(ScreenEntity screen, ColorConfiguration colorConfiguration)
        {
            GammaRegulatorService.ApplyColorConfiguration(colorConfiguration, screen.SysName);
            screen.CurrentColorConfiguration = colorConfiguration;
        }

        public static void SetDefaultColorTemperatureOnAllScreens()
        {
            var screens = App.Kernel.Get<ISettingsService>().Screens;

            foreach (var screen in screens)
            {
#if DEBUG
                LoggingModule.Log.Information($"Forced default color configuration on screen {screen.Name} [{screen.SysName}]");
#endif
                GammaRegulatorService.ApplyColorConfiguration(new ColorConfiguration(6600, 1), screen.SysName);
            }
        }

#endregion

        public ScreenModel(ISettingsService settingsService)
        {
            _screens = settingsService.Screens;
        }

#if DEBUG
        ~ScreenModel()
        {
            LoggingModule.Log.Verbose("[Model] ScreenModel Disposed");
        }
#endif
    }
}