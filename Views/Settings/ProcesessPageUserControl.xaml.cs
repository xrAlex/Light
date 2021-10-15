namespace Light.Views.Settings
{
    public partial class ProcessesPageUserControl 
    {
        public ProcessesPageUserControl() => InitializeComponent();
#if DEBUG
        ~ProcessesPageUserControl()
        {
            LoggingModule.Log.Verbose("[UC] ProcessesPageUserControl  Disposed");
        }
#endif
    }
}