using System.Windows;
using System.Windows.Media.Effects;

namespace Sparky.Views.Information
{
    /// <summary>
    /// Логика взаимодействия для InformationView.xaml
    /// </summary>
    public partial class InformationView : Window
    {
        public InformationView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var blur = new BlurEffect
            {
                Radius = 5
            };
            Owner.IsEnabled = false;
            Owner.Effect = blur;
            e.Handled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }
    }
}
