namespace Light.Views.Settings
{
    public partial class OtherSettingsPageUserControl
    {
        public OtherSettingsPageUserControl() => InitializeComponent();

#if DEBUG
        ~OtherSettingsPageUserControl()
        {
            DebugConsole.Print("[UC] OtherSettingsPageUserControl Disposed");
        }
#endif
    }
}