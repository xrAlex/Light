using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Light.Infrastructure
{
    public class TrayMenuPosition
    {
        private string GetTaskBarLocation()
        {
            var taskBarLocation = "Bottom";

            foreach (var screen in Screen.AllScreens)
            {
                var taskBarOnTopOrBottom = screen.WorkingArea.Width == screen.Bounds.Width;
                if (taskBarOnTopOrBottom)
                {
                    if (screen.WorkingArea.Top > 0) taskBarLocation = "Top";
                }
                else
                {
                    taskBarLocation = screen.WorkingArea.Left > 0 ? "Left" : "Right";
                }
            }
            return taskBarLocation;
        }

        public Point GetTrayMenuPos()
        {
            var cursorPos = Cursor.Position;
            var taskBarLocation = GetTaskBarLocation();
            var position = new Point(cursorPos.X + 5, cursorPos.Y - 140);

            switch (taskBarLocation)
            {
                case "Top":
                    position.X = cursorPos.X + 10;
                    position.Y = cursorPos.Y;
                    break;
                case "Right":
                    position.X = cursorPos.X - 160;
                    position.Y = cursorPos.Y - 90;
                    break;
            }
            return position;
        }
    }
}
