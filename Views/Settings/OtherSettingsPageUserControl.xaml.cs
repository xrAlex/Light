namespace Light.Views.Settings
{
    public partial class OtherSettingsPageUserControl
    {
        public OtherSettingsPageUserControl() => InitializeComponent();

#if DEBUG
        ~OtherSettingsPageUserControl()
        {
            Logging.Write("[UC] OtherSettingsPageUserControl Disposed");
        }
#endif
    }
}