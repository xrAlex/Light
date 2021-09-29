using System;
using System.Windows;

namespace Light.Templates.EventHandlers
{
    public class TrayLocationEventArgs : EventArgs
    {
        public Point Location { get; }
        public TrayLocationEventArgs(Point location)
        {
            Location = location;
        }
    }
}