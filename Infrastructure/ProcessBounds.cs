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
            var handler = Native.User32.GetForegroundWindow();
            if (handler == 0 || !Native.User32.IsWindowVisible(handler)) return false;

            uint pid = 0;
            Native.User32.GetWindowThreadProcessId(handler, ref pid);
            if (pid == 0) return false;

            var process = Process.GetProcessById(Convert.ToInt32(pid));
            return _settings.IgnoredProcesses.All(p => p.Name != process.ProcessName) && IsFullScreen(screen, handler);
        }

        private bool IsFullScreen(Screen screen, nint handle)
        {
            var rect = new Native.Rect();
            Native.User32.GetWindowRect(new HandleRef(null, handle), ref rect);

            return screen.Bounds.Width == rect.Right + rect.Left && screen.Bounds.Height == rect.Bottom + rect.Top;
        }

        public ProcessBounds()
        {
            _screenModel = new ScreenModel();
            _settings = ServiceLocator.Source.Settings;
        }
    }
}