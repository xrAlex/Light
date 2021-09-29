using System.Windows.Input;

namespace Light.Views.Main
{
    public partial class MainWindowView
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        private void MainWindowUserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}