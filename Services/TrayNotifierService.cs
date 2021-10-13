using Light.ViewModels;
using System;
using System.Windows.Forms;
using Light.Services.Interfaces;

namespace Light.Services
{
    /// <summary>
    /// Creates TrayIcon and gives it functionality
    /// </summary>
    internal sealed class TrayNotifierService : IDisposable, ITrayNotifierService
    {
        #region Fields

        private readonly NotifyIcon _notifier;
        private readonly IDialogService _dialogService;

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

        public TrayNotifierService(IDialogService dialogService)
        {
            _dialogService = dialogService;

            _notifier = new NotifyIcon
            {
                Icon = Properties.Resources.Icon1,
                Text = $@"{AppDomain.CurrentDomain.FriendlyName}",
                Visible = true
            };

            _notifier.MouseClick += TrayIcon_Click;
        }
#if DEBUG
        ~TrayNotifierService()
        {
            DebugConsole.Print("[Service] TrayNotifierService Disposed");
        }
#endif
    }
}
