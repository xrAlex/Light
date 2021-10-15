namespace Light.Views.Settings
{
    public partial class OtherSettingsPageUserControl
    {
        public OtherSettingsPageUserControl() => InitializeComponent();

#if DEBUG
        ~OtherSettingsPageUserControl()
        {
            LoggingModule.Log.Verbose("[UC] OtherSettingsPageUserControl Disposed");
        }
#endif
    }
}