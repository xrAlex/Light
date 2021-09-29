using System.Windows.Input;

namespace Light.Views.Settings
{
    public partial class SettingsWindowView
    {
        public SettingsWindowView()
        {
            InitializeComponent();
        }

        private void SettingsUserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}