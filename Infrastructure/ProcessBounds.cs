using Light.Models;
using Light.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Light.Infrastructure
{
    public class ProcessBounds
    {
        #region DLLImport

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(HandleRef hWnd, [In, Out] ref RECT rect);

        [DllImport("user32.dll")]
        private static extern nint GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(nint hwnd, ref uint pid);

        #endregion

        private readonly ScreenModel _screenModel;
        private readonly ServiceLocator _serviceLocator;
        private readonly SettingsService _settings;

        public bool IsProcessOnFullScreen(Process process)
        {
            if (process != null)
            {
                foreach (var screenEntity in _screenModel.Screens)
                {
                    return IsFullScreen(screenEntity.Instance, process.MainWindowHandle);
                }
            }
            return false;
        }

        public bool IsFullScreenProcessFounded(Screen screen)
        {
            nint handler = GetForegroundWindow();
            uint pid = 0;
            GetWindowThreadProcessId(handler, ref pid);
            Process process = Process.GetProcessById(Convert.ToInt32(pid));

            return !_settings.IgnoredProcesses.Any(p => p.Name == process.ProcessName) && IsFullScreen(screen, handler);
        }

        private bool IsFullScreen(Screen screen, nint handle)
        {
            var rect = new RECT();
            GetWindowRect(new HandleRef(null, handle), ref rect);

            return screen.Bounds.Width == (rect.right + rect.left) && screen.Bounds.Height == (rect.bottom + rect.top);
        }

        public ProcessBounds()
        {
            _screenModel = new();
            _serviceLocator = ServiceLocator.Source;
            _settings = _serviceLocator.Settings;
        }

    }
}
