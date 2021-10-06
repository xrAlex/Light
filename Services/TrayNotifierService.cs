using Light.ViewModels;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Light.Services
{
    public sealed class TrayNotifierService : IDisposable
    {
        #region Fields

        private readonly NotifyIcon _notifier;
        private readonly DialogService _dialogService;

        #endregion

        #region Methods

        public void ShowTip(string tip) => _notifier.ShowBalloonTip(300, $"{AppDomain.CurrentDomain.FriendlyName}", tip, ToolTipIcon.Info);

        private void TrayIcon_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _dialogService.CloseDialog<TrayMenuViewModel>();
                _dialogService.ShowDialog<MainWindowViewModel>();
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    _dialogService.ShowDialog<TrayMenuViewModel>();
                }
            }
        }

        public void Dispose()
        {
            _notifier.MouseClick -= TrayIcon_Click;
            _notifier.Visible = false;
            _notifier.Dispose();
        }

        #endregion

        public TrayNotifierService()
        {
            _dialogService = ServiceLocator.DialogService;

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
