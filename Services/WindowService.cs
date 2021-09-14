using System.Windows;

namespace Light.Services
{
    public class WindowService : IWindowService
    {
        private Window Window { get; set; }

        public void CreateWindow(object viewModel, int H, int W)
        {
            Window = new Window()
            {
                Content = viewModel,
                Height = H,
                Width = W,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Owner = Application.Current.MainWindow,
                Topmost = true,
            };
        }

        public void CloseWindow() => Window.Close();
        public void HideWindow() => Window.Hide();
        public void ShowWindow() => Window.Show();
    }
}
