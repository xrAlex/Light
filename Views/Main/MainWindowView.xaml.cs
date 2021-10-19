using System.Windows.Input;


namespace Sparky.Views.Main
{
    public partial class MainWindowView
    {
        public MainWindowView() => InitializeComponent();

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

#if DEBUG
        ~MainWindowView()
        {
            LoggingModule.Log.Verbose("[Window] MainWindowView Disposed");
        }
#endif
    }
}