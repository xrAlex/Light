namespace Light.Views.Settings
{
    public partial class ProcessesPageUserControl 
    {
        public ProcessesPageUserControl()
        {
            InitializeComponent();
        }
#if DEBUG
        ~ProcessesPageUserControl()
        {
            DebugConsole.Print("[UC] ProcessesPageUserControl  Disposed");
        }
#endif
    }
}