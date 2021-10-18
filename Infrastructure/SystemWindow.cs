using System.Collections.Generic;
using System.Runtime.InteropServices;
using Light.Templates.Entities;
using Light.WinApi;

namespace Light.Infrastructure
{
    /// <summary>
    /// Implements methods for working with system windows
    /// </summary>
    internal static class SystemWindow
    {
        /// <summary>
        /// Checks if the application window is displayed
        /// </summary>
        /// <returns> true when window can be displayed </returns>
        public static bool IsWindowValid(nint handler) => handler != 0 && Native.IsWindowVisible(handler);

        /// <summary>
        /// Checks if the window works in full screen, given a task bar
        /// </summary>
        /// <returns> <see cref="bool"/> true, when window expanded on full screen</returns>
        public static bool IsWindowOnFullScreen(ScreenEntity screen, nint handler)
        {
            Native.GetWindowRect(new HandleRef(null, handler), out var rect);
            return screen.Width == rect.Right + rect.Left && screen.Height == rect.Bottom + rect.Top;
        }

        /// <summary>
        /// Checks foreground Window bounds
        /// </summary>
        /// <returns> <see cref="bool"/> true, if the window is maximized to full screen </returns>
        public static nint GetFullscreenForegroundWindow(ScreenEntity screen)
        {
            var handle = Native.GetForegroundWindow();
            if (!IsWindowValid(handle) || IsWindowOnFullScreen(screen, handle)) return 0;
            return handle;
        }

        /// <summary>
        /// Method gets all window handlers in system
        /// </summary>
        /// <returns> List of window handlers </returns>
        public static List<nint> GetAllWindows()
        {
            var windows = new List<nint>();

            var callback = new Native.EnumWindowsProc((hWnd, _) =>
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
