namespace Light.Views.Settings
{
    public partial class SettingsMainPageUserControl
    {
        public SettingsMainPageUserControl() => InitializeComponent();
#if DEBUG
        ~SettingsMainPageUserControl()
        {
            Logging.Write("[UC] SettingsMainPageUserControl Disposed");
        }
#endif
    }
}