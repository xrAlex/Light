using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Light.Commands;
using Light.Infrastructure;
using Light.Models;
using Light.Services;
using Light.Templates.Entities;
using Light.Templates.EventHandlers;
using Light.ViewModels.Base;
using Application = System.Windows.Application;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace Light.ViewModels
{
    internal sealed class TrayMenuViewModel : ViewModelBase
    {
        private readonly DialogService _dialogService;
        private readonly ScreenModel _screenModel;
        private readonly PeriodWatcherService _periodWatcherService;
        private readonly TrayNotifier _trayNotifier;

        private double _topLocation;
        private double _leftLocation;

        public double TopLocation
        {
            get => _topLocation;
            private set => Set(ref _topLocation, value);
        }

        public double LeftLocation
        {
            get => _leftLocation;
            private set => Set(ref _leftLocation, value);
        }

        public ICommand CloseTrayMenuCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand ShutdownCommand { get; }

        private void OnPauseCommandExecute()
        {
            _periodWatcherService.StopWatch();
            _screenModel.SetDefaultColorTemperatureOnAllScreens();
            _dialogService.CloseDialog<TrayMenuViewModel>();

        }

        private void OnShutdownCommandExecute()
        {
            _screenModel.SetDefaultColorTemperatureOnAllScreens();
            Application.Current.Shutdown();
        }

        private void OnCloseTrayMenuCommandExecute()
        {
            _dialogService.CloseDialog<TrayMenuViewModel>();
        }

        public TrayMenuViewModel()
        {
            PauseCommand = new LambdaCommand(p => OnPauseCommandExecute());
            ShutdownCommand = new LambdaCommand(p => OnShutdownCommandExecute());
            CloseTrayMenuCommand = new LambdaCommand(p => OnCloseTrayMenuCommandExecute());

            var serviceLocator = ServiceLocator.Source;
            _dialogService = serviceLocator.DialogService;
            _screenModel = new();
            _trayNotifier = new();
            _periodWatcherService = serviceLocator.PeriodWatcherService;

            _trayNotifier.OnLocationChanged += (sender, args) =>
            {
                LeftLocation = args.Location.X;
                TopLocation = args.Location.Y;
            };
        }
    }
}
