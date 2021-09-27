#region

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Markup;
using Light.Templates.Entities;
using Light.WinApi;

#endregion

namespace Light.Infrastructure
{
    /// <summary>
    /// Класс реализует методы расширяющие возможности работы с окнами
    /// </summary>
    public static class SystemWindow
    {
        /// <summary>
        /// Метод проверяет отображается ли окно приложения
        /// </summary>
        /// <returns> true если окно может быть отображено </returns>
        public static bool IsWindowValid(nint handler) => handler != 0 && Native.IsWindowVisible(handler);

        /// <summary>
        /// Метод проверяет работает ли окно на полный экран с учетом таскбара
        /// </summary>
        /// <returns> true если окно развернуто на полный экран </returns>
        public static bool IsWindowOnFullScreen(ScreenEntity screen, nint handler)
        {
            Native.GetWindowRect(new HandleRef(null, handler), out var rect);
            return screen.Bounds["Width"] == rect.Right + rect.Left && screen.Bounds["Height"] == rect.Bottom + rect.Top;
        }

        /// <summary>
        /// Метод получает все дескрипторы окон в системе
        /// </summary>
        /// <returns> List дескрипторов окон </returns>
        public static IEnumerable<nint> GetAllWindows()
        {
            var windows = new List<nint>();

            var callback = new Native.EnumWindowsProc((hWnd, lParam) =>
            {
                if (IsWindowValid(hWnd))
                {
                    windows.Add(hWnd);
                }
                return true;
            });

            Native.EnumWindows(callback, 0);
            return windows;
        }
    }
}
