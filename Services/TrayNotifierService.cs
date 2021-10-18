using System;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Light.Services.Interfaces;
using Light.ViewModels;
using Light.Views.Tray;

namespace Light.Services
{
    internal class TrayNotifierService : IDisposable, ITrayNotifierService
    {
        private readonly TaskbarIcon _taskBarNotifier;
        private readonly IDialogService _dialogService;

        public TrayNotifierService(IDialogService dialogService)
        {
            _dialogService = dialogService;
            _taskBarNotifier = new TaskbarIcon()
            {
                PopupActivation = PopupActivationMode.RightClick,
                ToolTipText = "Light",
                TrayPopup = new TrayMenuPopup(),
                DataContext = ViewModelLocator.TrayMenuViewModel,
                Icon = Properties.Resources.Icon1,
                Visibility = Visibility.Visible
            };
            _taskBarNotifier.TrayLeftMouseDown += _taskBarNotifier_TrayLeftMouseDown;
        }

        private void _taskBarNotifier_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            _dialogService.ShowDialog<MainWindowViewModel>();
        }

        public void Dispose()
        {
            _taskBarNotifier.Dispose();
        }

        public void ShowTip(string localizationKey)
        {
            //
        }
    }
}
