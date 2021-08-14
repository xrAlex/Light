using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
