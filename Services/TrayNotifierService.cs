using System;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Sparky.Services.Interfaces;
using Sparky.ViewModels;
using Sparky.Views.Tray;

namespace Sparky.Services
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
                Icon = Properties.Resources.TrayLogo,
                Visibility = Visibility.Visible
            };
            _taskBarNotifier.TrayLeftMouseDown += OnLeftMouseDown;
        }

        private void OnLeftMouseDown(object sender, RoutedEventArgs e)
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
