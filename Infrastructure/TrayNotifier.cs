using Light.Services;
using Light.Templates.EventHandlers;
using Light.ViewModels;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Light.Templates.Entities;
using Light.WinApi;
using Point = System.Windows.Point;

namespace Light.Infrastructure
{
    public sealed class TrayNotifier : IDisposable
    {
        #region Fields

        public event EventHandler<TrayLocationEventArgs> OnLocationChanged;
        private readonly NotifyIcon _notifier;
        private readonly DialogService _dialogService;
        private Point _trayMenuLocation;

        private enum TaskBarLocation
        {
            Top,
            Bottom,
            Right,
            Left
        }

        #endregion

        #region Constructors

        private Point TrayMenuLocation
        {
            get => _trayMenuLocation;
            set
            {
                _trayMenuLocation = value;
                OnLocationChanged?.Invoke(this, new TrayLocationEventArgs(TrayMenuLocation));
            }
        }

        #endregion

        #region Methods

        private void TrayIcon_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _dialogService.ShowDialog<MainWindowViewModel>();
                _dialogService.CloseDialog<TrayMenuViewModel>();
                Dispose();
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    RefreshTrayMenuPos();
                    _dialogService.ShowDialog<TrayMenuViewModel>();
                }
            }
        }

        private TaskBarLocation GetTaskBarLocation()
        {
            var taskBarPos = GetTaskBarPosition();
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

        private Rectangle GetTaskBarPosition()
        {
            const uint dwMessage = 5;
            var data = new TaskBarData();
            data.CbSize = Marshal.SizeOf(data);
            var shellMessage = Native.SHAppBarMessage(dwMessage, ref data);
            return shellMessage == 0
                ? Rectangle.Empty
                : new Rectangle(data.Rc.Left, data.Rc.Top, data.Rc.Right - data.Rc.Left, data.Rc.Bottom - data.Rc.Top);
        }

        private void RefreshTrayMenuPos()
        {
            var cursorPos = Cursor.Position;
            var taskBarLocation = GetTaskBarLocation();
            var x = cursorPos.X;
            var y = cursorPos.Y;

            switch (taskBarLocation)
            {
                case TaskBarLocation.Bottom:
                    x += 10;
                    y -= 90;
                    break;
                case TaskBarLocation.Top:
                    x += 10;
                    break;
                case TaskBarLocation.Left:
                    x += 10;
                    y += 5;
                    break;
                case TaskBarLocation.Right:
                    x -= 100;
                    y += 10;
                    break;
            }

            TrayMenuLocation = new Point(x, y);
        }

        public void Dispose()
        {
            _notifier.MouseClick -= TrayIcon_Click;
            _notifier.Visible = false;
            _notifier.Dispose();
        }

        #endregion

        public TrayNotifier()
        {
            var serviceLocator = ServiceLocator.Source;
            _dialogService = serviceLocator.DialogService;

            _notifier = new NotifyIcon
            {
                Icon = Properties.Resources.Icon1,
                Text = $@"{AppDomain.CurrentDomain.FriendlyName}",
                Visible = true
            };
            _notifier.ShowBalloonTip(300, $"{AppDomain.CurrentDomain.FriendlyName}", "Приложение продолжит работу в фоновом режиме", ToolTipIcon.Info);
            _notifier.MouseClick += TrayIcon_Click;
        }
    }
}
