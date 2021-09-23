namespace Light.Interfaces
{
    public interface IWindowService
    {
        public void CreateWindow(object viewModel, int h, int w);
        public void ShowWindow();
        public void HideWindow();
        public void CloseWindow();
    }
}
