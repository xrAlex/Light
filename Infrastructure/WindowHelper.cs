using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Light.Native;

namespace Light.Infrastructure
{
    public static class WindowHelper
    {
        public static bool IsWindowValid(nint handler) => handler != 0 && User32.IsWindowVisible(handler);

        public static bool IsWindowOnFullScreen(Screen screen, nint handler)
        {
            var rect = new Rect();
            User32.GetWindowRect(new HandleRef(null, handler), ref rect);

            return screen.Bounds.Width == rect.Right + rect.Left && screen.Bounds.Height == rect.Bottom + rect.Top;
        }
    }
}
