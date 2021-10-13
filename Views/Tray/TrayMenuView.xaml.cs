namespace Light.Views.Tray
{
    public partial class TrayMenuView
    {
        private bool _isClosing;

        public TrayMenuView() => InitializeComponent();

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => _isClosing = true;

        private void Window_Closed(object sender, System.EventArgs e) => _isClosing = false;

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            if (!_isClosing)
            {
                Close();
            }
        }

#if DEBUG
        ~TrayMenuView()
        {
            DebugConsole.Print("[Window] TrayMenuView Disposed");
        }
#endif
    }
}