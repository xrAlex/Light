#region

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Light.Infrastructure;
using Light.Models;
using Light.Templates.Entities;
using Light.WinApi;

#endregion

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
                    if (!screen.IsActive) continue;

                    var isWorkTime = _workTime.IsDayPeriod(screen);

                    if (isWorkTime)
                    {
                        if (_settings.CheckFullScreenApps)
                        {
                            if (IsFullScreenProcessFounded(screen))
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

        /// <summary>
        /// Метод проверяет для устройства отображения развернуто ли окно на переднем плане во весь экран
        /// </summary>
        /// <returns> true если окно работает в полноэкранном режиме </returns>
        private bool IsFullScreenProcessFounded(ScreenEntity screen)
        {
            var handle = Native.GetForegroundWindow();
            if (!SystemWindow.IsWindowValid(handle) || !SystemWindow.IsWindowOnFullScreen(screen, handle)) return false;

            var pId = SystemProcess.GetId(handle);
            using var process = SystemProcess.TryOpenProcess(pId);
            var processPath = process?.TryGetProcessPath();
            var processFileName = Path.GetFileNameWithoutExtension(processPath);

            return _settings.IgnoredProcesses.Count == 0 || _settings.IgnoredProcesses.All(p => p != processFileName);
        }
    }
}