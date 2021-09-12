using Light.Models;
using System;
using System.Diagnostics;
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

        private readonly ScreenModel _screenModel = new();

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
    }
}
