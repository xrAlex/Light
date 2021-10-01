﻿using System.Collections.Generic;
using System.Runtime.InteropServices;
using Light.Templates.Entities;
using Light.WinApi;

namespace Light.Infrastructure
{
    /// <summary>
    /// Implements methods for working with system windows
    /// </summary>
    public static class SystemWindow
    {
        /// <summary>
        /// Checks if the application window is displayed
        /// </summary>
        /// <returns> true when window can be displayed </returns>
        public static bool IsWindowValid(nint handler) => handler != 0 && Native.IsWindowVisible(handler);

        /// <summary>
        /// Checks if the window works in full screen, given a task bar
        /// </summary>
        /// <returns> true, when window expanded on full screen</returns>
        public static bool IsWindowOnFullScreen(ScreenEntity screen, nint handler)
        {
            Native.GetWindowRect(new HandleRef(null, handler), out var rect);
            return screen.Bounds["Width"] == rect.Right + rect.Left && screen.Bounds["Height"] == rect.Bottom + rect.Top;
        }

        /// <summary>
        /// Method gets all window handlers in system
        /// </summary>
        /// <returns> List of window handlers </returns>
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