namespace Sparky.Views.Settings
{
    public partial class SettingsMainPageUserControl
    {
        public SettingsMainPageUserControl() => InitializeComponent();
#if DEBUG
        ~SettingsMainPageUserControl()
        {
            LoggingModule.Log.Verbose("[UC] SettingsMainPageUserControl Disposed");
        }
#endif
    }
}