namespace Light.Services
{
    public interface IWindowService
    {
        public void CreateWindow(object viewModel, int H, int W);
        public void ShowWindow();
        public void HideWindow();
        public void CloseWindow();
    }
}
