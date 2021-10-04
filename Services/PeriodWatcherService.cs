using Light.Infrastructure;
using Light.Models;
using Light.Templates.Entities;
using Light.WinApi;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Services
{
    /// <summary>
    /// Класс циклически проверяет какая цветовая конфигурация должна быть быть установлена
    /// </summary>
    public class PeriodWatcherService
    {
        private readonly ScreenModel _screenModel;
        private readonly SettingsService _settings;
        private readonly WorkTime _workTime;
        private CancellationTokenSource _cts;

        public PeriodWatcherService()
        {
            _screenModel = new ScreenModel();
            _workTime = new WorkTime();
            var serviceLocator = ServiceLocator.Source;
            _settings = serviceLocator.Settings;
        }

        public void StartWatch()
        {
            _cts = new CancellationTokenSource();
            Task.Run(() => Cycle(_cts.Token), _cts.Token).ConfigureAwait(false);
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
                    if (!screen.IsActive) continue;

                    var isWorkTime = _workTime.IsDayPeriod(screen);

                    if (isWorkTime)
                    {
                        if (_settings.CheckFullScreenApps)
                        {
                            if (_screenModel.IsFullScreenProcessFounded(screen))
                            {
                                _screenModel.SetDayPeriod(screen);
                            }
                            else
                            {
                                _screenModel.SetNightPeriod(screen);
                            }
                        }
                        else
                        {
                            _screenModel.SetNightPeriod(screen);
                        }
                    }
                    else
                    {
                        _screenModel.SetDayPeriod(screen);
                    }
                }

                await Task.Delay(1000, token);
            }
        }
    }
}