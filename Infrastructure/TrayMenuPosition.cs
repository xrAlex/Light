using Light.Templates.Entities;
using Light.WinApi;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Light.Infrastructure
{
    internal sealed class TrayMenuPosition
    {
        private enum TaskBarLocation
        {
            Top,
            Bottom,
            Right,
            Left
        }

        /// <summary>
        /// Method gets tray menu window position on screen
        /// </summary>
        /// <returns> Window position <see cref="Point"/></returns>
        public Point GetTrayMenuPos()
        {
            var cursorPos = Cursor.Position;
            var taskBarLocation = GetTaskBarLocation();

            switch (taskBarLocation)
            {
                case TaskBarLocation.Bottom:
                    cursorPos.X += 5;
                    cursorPos.Y -= 90;
                    break;
                case TaskBarLocation.Top:
                    cursorPos.X += 5;
                    break;
                case TaskBarLocation.Left:
                    cursorPos.X += 5;
                    cursorPos.Y += 5;
                    break;
                case TaskBarLocation.Right:
                    cursorPos.X -= 100;
                    cursorPos.Y += 5;
                    break;
            }

            return cursorPos;
        }

        /// <summary>
        /// Method gets task bar location on screen
        /// </summary>
        /// <returns>Screen <see cref="TaskBarLocation"/></returns>
        private TaskBarLocation GetTaskBarLocation()
        {
            var taskBarPos = GetTaskBarDimensions();
            if (taskBarPos == Rectangle.Empty) return TaskBarLocation.Bottom;

            var taskBarLocation = TaskBarLocation.Top;
            var screen = Screen.PrimaryScreen;

            var taskBarOnTopOrBottom = taskBarPos.Width == screen.Bounds.Width;

            if (taskBarOnTopOrBottom)
            {
                if (taskBarPos.Top > 0) taskBarLocation = TaskBarLocation.Bottom;
            }
            else
            {
                taskBarLocation = taskBarPos.Left > 0 ? TaskBarLocation.Right : TaskBarLocation.Left;
            }

            return taskBarLocation;
        }

        /// <summary>
        /// Method gets task bar dimensions
        /// </summary>
        /// <returns>Task bar <see cref="Rectangle"/></returns>
        private Rectangle GetTaskBarDimensions()
        {
            const uint dwMessage = 5;
            var data = new TaskBarData();
            var shellMessage = Native.SHAppBarMessage(dwMessage, ref data);
            return shellMessage == 0
                ? Rectangle.Empty
                : new Rectangle(data.Rc.Left, data.Rc.Top, data.Rc.Right - data.Rc.Left, data.Rc.Bottom - data.Rc.Top);
        }
    }
}
