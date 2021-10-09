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

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Localization.LangDictionary.Eng();
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
           Localization.LangDictionary.Rus();
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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