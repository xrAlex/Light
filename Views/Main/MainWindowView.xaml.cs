﻿using System.Windows.Input;


namespace Light.Views.Main
{
    public partial class MainWindowView
    {
        public MainWindowView() => InitializeComponent();

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

#if DEBUG
        ~MainWindowView()
        {
            Logging.Write("[Window] MainWindowView Disposed");
        }
#endif
    }
}