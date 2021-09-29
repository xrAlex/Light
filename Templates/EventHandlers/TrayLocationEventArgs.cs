#region

using System;
using System.Windows;

#endregion

namespace Light.Templates.EventHandlers
{
    public class TrayLocationEventArgs : EventArgs
    {
        public Point Location { get; set; }
        public TrayLocationEventArgs(Point location)
        {
            Location = location;
        }
    }
}