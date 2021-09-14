using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Light.Models;
using Light.Services;

namespace Light.Infrastructure
{
    public class ProcessBounds
    {
        private readonly ScreenModel _screenModel;
        private readonly SettingsService _settings;

        public bool IsProcessOnFullScreen(Process process)
        {
            return process != null && _screenModel.Screens.Select(screenEntity => IsFullScreen(screenEntity.Instance, process.MainWindowHandle)).FirstOrDefault();
        }

        public bool IsFullScreenProcessFounded(Screen screen)
        {
            var handler = GetForegroundWindow();
            uint pid = 0;
            GetWindowThreadProcessId(handler, ref pid);
            var process = Process.GetProcessById(Convert.ToInt32(pid));

            return _settings.IgnoredProcesses.All(p => p.Name != process.ProcessName) && IsFullScreen(screen, handler);
        }

        private bool IsFullScreen(Screen screen, nint handle)
        {
            var rect = new Rect();
            GetWindowRect(new HandleRef(null, handle), ref rect);

            return screen.Bounds.Width == rect.right + rect.left && screen.Bounds.Height == rect.bottom + rect.top;
        }

        public ProcessBounds()
        {
            _screenModel = new ScreenModel();
            _settings = ServiceLocator.Source.Settings;
        }


        #region DLLImport

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct Rect
        {
            public readonly int left;
            public readonly int top;
            public readonly int right;
            public readonly int bottom;
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(HandleRef hWnd, [In] [Out] ref Rect rect);

        [DllImport("user32.dll")]
        private static extern nint GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(nint hwnd, ref uint pid);

        #endregion
    }
}