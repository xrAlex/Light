namespace Light.Views.Settings
{
    public partial class SettingsMainPageUserControl
    {
        public SettingsMainPageUserControl() => InitializeComponent();
#if DEBUG
        ~SettingsMainPageUserControl()
        {
            DebugConsole.Print("[UC] SettingsMainPageUserControl Disposed");
        }
#endif
    }
}