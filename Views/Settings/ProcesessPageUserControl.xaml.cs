namespace Light.Views.Settings
{
    public partial class ProcessesPageUserControl 
    {
        public ProcessesPageUserControl() => InitializeComponent();
#if DEBUG
        ~ProcessesPageUserControl()
        {
            Logging.Write("[UC] ProcessesPageUserControl  Disposed");
        }
#endif
    }
}