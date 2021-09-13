using Light.Models;
using Light.Services;
using System;
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
        private static extern IntPtr GetForegroundWindow();

        #endregion

        public ProcessBounds()
        {
            _screenModel = new();
            _serviceLocator = ServiceLocator.Source;
            _settings = _serviceLocator.Settings;
        }

        private readonly ScreenModel _screenModel;
        private readonly ServiceLocator _serviceLocator;
        private readonly SettingsService _settings;

        public bool IsProcessOnFullScreen(Process process)
        {
            bool fullscreen = false;
            if (process != null)
            {
                foreach (var screenEntity in _screenModel.Screens)
                {
                    if (IsFullScreen(process, screenEntity.Instance))
                    {
                        fullscreen = true;
                        break;
                    }
                }
            }
            return fullscreen;
        }

        public bool IsFullScreenProcessFounded(Screen screen)
        {
            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                if (ProcessHasWindow(process))
                {
                    if (!_settings.IgnoredProcesses.Any(p => p.Name == process.ProcessName) && IsFullScreen(process, screen))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsFullScreen(Process process, Screen screen)
        {
            var rect = new RECT();

            GetWindowRect(new HandleRef(null, process.MainWindowHandle), ref rect);

            if (screen.Bounds.Width == (rect.right + rect.left) && screen.Bounds.Height == (rect.bottom + rect.top))
            {
                return true;
            }

            return false;
        }

        private bool ProcessHasWindow(Process process) => process.MainWindowHandle != IntPtr.Zero;
    }
}
