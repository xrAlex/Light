using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;


namespace Light.Views.Main
{
    public partial class MainWindowView
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

#if DEBUG
        ~MainWindowView()
        {
            DebugConsole.Print("[Window] MainWindowView Disposed");
        }
#endif
    }
}