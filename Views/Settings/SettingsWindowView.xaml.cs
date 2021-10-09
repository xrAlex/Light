using System.Windows.Input;

namespace Light.Views.Settings
{
    public partial class SettingsWindowView
    {
        public SettingsWindowView()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

#if DEBUG
        ~SettingsWindowView()
        {
            DebugConsole.Print("[Window] SettingsWindowView Disposed");
        }
#endif
    }
}