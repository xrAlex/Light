using Light.Infrastructure;
using Light.Models;
using Light.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Services
{
    public class GammaWatcherService
    {
        private readonly ScreenModel _screenModel;
        private readonly SettingsService _settings;
        private readonly ServiceLocator _serviceLocator;
        private readonly WorkTime _workTime;
        private readonly ProcessBounds _processBounds;
        private CancellationTokenSource cts;

        public GammaWatcherService()
        {
            _screenModel = new();
            _workTime = new();
            _processBounds = new();
            _serviceLocator = ServiceLocator.Source;
            _settings = _serviceLocator.Settings;
        }

        public void StartWatch()
        {
            cts = new CancellationTokenSource();
            Task.Run(() => Cycle(cts.Token), cts.Token);
        }

        public void StopWatch() => cts.Cancel();

        private async Task Cycle(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                foreach (ScreenEntity screen in _screenModel.Screens)
                {
                    if (screen.IsActive)
                    {
                        bool isWorkTime = _workTime.IsWorkTime(screen);

                        if (isWorkTime)
                        {
                            if (_settings.CheckFullScreenApps)
                            {
                                if (!_processBounds.IsFullScreenProcessFounded(screen.Instance))
                                {
                                    _screenModel.SetUserValues(screen);
                                    break;
                                }
                            }
                            else
                            {
                                _screenModel.SetUserValues(screen);
                                break;
                            }
                        }
                        _screenModel.SetDefaultValues(screen);
                    }
                }
                await Task.Delay(1000);
            }
        }
    }
}
