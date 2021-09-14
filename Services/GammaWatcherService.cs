using System.Threading;
using System.Threading.Tasks;
using Light.Infrastructure;
using Light.Models;

namespace Light.Services
{
    public class GammaWatcherService
    {
        private readonly ProcessBounds _processBounds;
        private readonly ScreenModel _screenModel;
        private readonly SettingsService _settings;
        private readonly WorkTime _workTime;
        private CancellationTokenSource _cts;

        public GammaWatcherService()
        {
            _screenModel = new ScreenModel();
            _workTime = new WorkTime();
            _processBounds = new ProcessBounds();
            var serviceLocator = ServiceLocator.Source;
            _settings = serviceLocator.Settings;
        }

        public void StartWatch()
        {
            _cts = new CancellationTokenSource();
            Task.Run(() => Cycle(_cts.Token), _cts.Token);
        }

        public void StopWatch()
        {
            _cts.Cancel();
        }

        private async Task Cycle(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                foreach (var screen in _screenModel.Screens)
                {
                    if (screen.IsActive)
                    {
                        var isWorkTime = _workTime.IsWorkTime(screen);

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

                await Task.Delay(1000, token);
            }
        }
    }
}