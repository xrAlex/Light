#region

using System.Runtime.InteropServices;
using System.Windows.Forms;
using Light.Native;
using Light.Templates.Entities;

#endregion

namespace Light.Infrastructure
{
    /// <summary>
    /// Класс реализует методы расширяющие возможности работы с окнами
    /// </summary>
    public static class WindowHelper
    {
        /// <summary>
        /// Метод проверяет отображается ли окно приложения
        /// </summary>
        /// <returns> true если окно может быть отображено </returns>
        public static bool IsWindowValid(nint handler) => handler != 0 && User32.IsWindowVisible(handler);

        /// <summary>
        /// Метод проверяет работает ли окно на полный экран с учетом таскбара
        /// </summary>
        /// <returns> true если окно развернуто на полный экран </returns>
        public static bool IsWindowOnFullScreen(Screen screen, nint handler)
        {
            var rect = new Rect();
            User32.GetWindowRect(new HandleRef(null, handler), ref rect);

            return screen.Bounds.Width == rect.Right + rect.Left && screen.Bounds.Height == rect.Bottom + rect.Top;
        }
    }
}
