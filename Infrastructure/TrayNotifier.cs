using Light.Services;
using Light.Templates.EventHandlers;
using Light.ViewModels;
using System;
using System.Windows;
using System.Windows.Forms;

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
                this.Dispose();
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
            var taskBarLocation = TaskBarLocation.Bottom;

            foreach (var screen in Screen.AllScreens)
            {
                var taskBarOnTopOrBottom = screen.WorkingArea.Width == screen.Bounds.Width;
                if (taskBarOnTopOrBottom)
                {
                    if (screen.WorkingArea.Top > 0) taskBarLocation = TaskBarLocation.Top;
                }
                else
                {
                    taskBarLocation = screen.WorkingArea.Left > 0 ? TaskBarLocation.Left : TaskBarLocation.Right;
                }
            }
            return taskBarLocation;
        }

        private void RefreshTrayMenuPos()
        {
            var cursorPos = Cursor.Position;
            var taskBarLocation = GetTaskBarLocation();
            var position = new Point(cursorPos.X + 5, cursorPos.Y - 140);

            switch (taskBarLocation)
            {
                case TaskBarLocation.Top:
                    position.X = cursorPos.X + 10;
                    position.Y = cursorPos.Y;
                    break;
                case TaskBarLocation.Right:
                    position.X = cursorPos.X - 160;
                    position.Y = cursorPos.Y - 90;
                    break;
            }

            TrayMenuLocation = position;
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
            _notifier.MouseClick += TrayIcon_Click;
        }
    }
}
