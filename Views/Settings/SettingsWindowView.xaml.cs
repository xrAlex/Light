#region

using System.Windows;
using System.Windows.Input;

#endregion

namespace Light.Views.Settings
{
    /// <summary>
    ///     Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindowView : Window
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